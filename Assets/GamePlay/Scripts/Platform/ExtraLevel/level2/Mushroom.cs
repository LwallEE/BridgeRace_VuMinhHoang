using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class Mushroom : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] Transform spawnPos;
    [SerializeField] MushroomArrow arrowPrefab;

    float scaleRate = 1, speed = 5f;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        scaleRate = Random.Range(.5f, 2f);
        animator.speed = 1 / scaleRate * 1.5f;

        transform.localScale = Vector3.one * 3f * scaleRate;
        speed = Random.Range(.75f, 1.25f) * 5f;
    }

    public void Shot()
    {
        MushroomArrow arrow = ObjectPoolDictArray.Instance.GetGameObject(arrowPrefab, spawnPos.position, transform.rotation);
        arrow.OnInit(scaleRate, speed);
    }
}
