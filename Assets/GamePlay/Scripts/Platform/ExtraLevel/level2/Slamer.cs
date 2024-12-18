using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slamer : MonoBehaviour
{
    [SerializeField] Transform tfLeft, tfRight;
    [SerializeField] BoxCollider boxCollider;

    float cooldown = 2f, duration, distance;
    bool isAniming = false;

    private void Start()
    {
        cooldown = Random.Range(0, 2f);
        duration = Random.Range(.8f, 1.2f) * 2f;
        distance = Random.Range(3.6f, 5f);
    }
    private void Update()
    {
        if (isAniming) return;

        cooldown -= Time.deltaTime;

        if (cooldown <= 0)
        {
            cooldown = duration;
            Slam();
        }
    }
    private void Slam()
    {
        isAniming = true;
        tfLeft.DOLocalMoveX(0f, .5f).SetEase(Ease.InBack);
        tfRight.DOLocalMoveX(0f, .5f).SetEase(Ease.InBack).OnComplete(() =>
        {
            StartCoroutine(delay());
        });

    }
    private IEnumerator delay()
    {
        boxCollider.enabled = true;
        yield return new WaitForSeconds(.5f);
        boxCollider.enabled = false;
        Rewind();
    }
    private void Rewind()
    {
        tfLeft.DOLocalMoveX(-distance, .5f).SetEase(Ease.Linear);
        tfRight.DOLocalMoveX(distance, .5f).SetEase(Ease.Linear).OnComplete(() =>
        {
            isAniming = false;
        });
    }
}
