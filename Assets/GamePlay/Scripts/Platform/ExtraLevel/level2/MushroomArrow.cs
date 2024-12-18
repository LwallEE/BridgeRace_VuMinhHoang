using StateMachineNP;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Utilities;

public class MushroomArrow : MonoBehaviour
{
    [SerializeField] TrailRenderer trail;
    float speed = 5f, lifeTime, scaleRate;

    public void OnInit(float scaleRate, float initSpeed)
    {
        this.scaleRate = scaleRate;

        speed = initSpeed;
        transform.localScale = scaleRate * Vector3.one * .5f;

        trail.startWidth = scaleRate * .5f;
        trail.Clear();

        lifeTime = 0f;
    }

    private void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;

        lifeTime += Time.deltaTime;
        if(lifeTime > 10f)
        {
            OnDespawn();
        }
    }
    private void OnDespawn()
    {
        ObjectPoolDictArray.Instance.ReleaseGameObject(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constants.CHARACTER_TAG))
        {
            StateMachineNP.Character character = other.GetComponent<StateMachineNP.Character>();

            int damage =Mathf.FloorToInt(scaleRate / .5f);
            for (int i = 0; i < damage; i++)
            {
                character.DropOneBrick();
            }

            OnDespawn();
        }
    }
}
