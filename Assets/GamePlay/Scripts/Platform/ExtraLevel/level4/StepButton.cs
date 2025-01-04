using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StepButton : MonoBehaviour
{
    [SerializeField] Transform button;

    public UnityEvent startPressEvent, pressingEvent, endPressEvent;

    private bool isPressed = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constants.CHARACTER_TAG))
        {
            if (isPressed) return;

            SoundManager.Instance.PlayShotOneTime(ESound.ButtonStep);

            isPressed = true;
            button.DOLocalMoveY(-.3f, .1f);

            startPressEvent?.Invoke();
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(Constants.CHARACTER_TAG))
        {
            if (!isPressed) return;

            pressingEvent?.Invoke();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Constants.CHARACTER_TAG))
        {
            if (!isPressed) return;

            isPressed = false;
            button.DOLocalMoveY(0, .1f);

            endPressEvent?.Invoke();
        }
    }
}
