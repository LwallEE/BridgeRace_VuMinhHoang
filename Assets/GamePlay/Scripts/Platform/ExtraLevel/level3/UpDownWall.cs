using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpDownWall : MonoBehaviour
{
    float cooldown, timer;
    bool isMoving = false;
    private void Start()
    {
        timer = 0f;
        cooldown = Random.Range(7f, 15f);
    }
    private void Update()
    {
        if (isMoving) return;
        timer += Time.deltaTime;
        if(timer > cooldown)
        {
            timer = 0;
            Move();
        }
    }
    private void Move()
    {
        isMoving = true;
        float y = transform.position.y;
        transform.DOMoveY(transform.position.y - 5f, 1f);
        transform.DOMoveY(transform.position.y - 5f, 1f);
        transform.DOMoveY(y, 1f).SetDelay(2f).OnComplete(() =>
        {
            isMoving = false;
        });
    }
}
