using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateBlock : MonoBehaviour
{
    public bool IsRotating;
    private Vector3 vector = new Vector3(0, 0, 90f);

    private void Start()
    {
        transform.eulerAngles = Random.Range(0, 5) * vector;
    }
    public void OnRotate(bool rotateRight = true)
    {
        IsRotating = true;
        transform.DORotate(transform.eulerAngles + vector * (rotateRight ? 1 : -1), 1f).SetEase(Ease.InOutBack).OnComplete(() =>
        {
            IsRotating = false;
        });
    }
}
