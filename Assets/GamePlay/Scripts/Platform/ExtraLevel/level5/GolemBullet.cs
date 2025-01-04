using StateMachineNP;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class GolemBullet : MonoBehaviour
{
    [SerializeField] TrailRenderer trail;
    Vector3 target;
    float timer = 0f;
    float speed = 20f;
    float size = 2f;

    PlayerHealPoint hp;
    public void OnInit(Vector3 target, PlayerHealPoint healPoint)
    {
        this.target = target;
        hp = healPoint;

        timer = 0f;
        transform.eulerAngles = new Vector3(-90, 0, 0);

        float curSize = Random.Range(size, size * 1.5f);

        transform.localScale = Vector3.one * curSize;

        trail.startWidth = 1 * curSize * .9f;
        trail.Clear();
    }
    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > 2f)
        {
            Vector3 direction = (target - transform.position).normalized;

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * speed/4);
        }
        transform.position += transform.forward * speed * Time.deltaTime;

        if(Vector3.Distance(transform.position, target) < 1.5f)
        {
            ParticleManager.Instance.PlayFxExplode(transform.position);
            ObjectPoolDictArray.Instance.ReleaseGameObject(gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Character"))
        {
            other.GetComponent<StateMachineNP.Character>().DeleteOneBrick();
            hp.OnHit();
        }
    }
}
