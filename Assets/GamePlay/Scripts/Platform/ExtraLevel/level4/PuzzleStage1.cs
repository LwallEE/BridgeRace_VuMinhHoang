using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleStage1 : MonoBehaviour
{
    [SerializeField] Transform red, green, yellow;
    [SerializeField] Transform wall;

    private float speed_red = 1f, speed_green = 2f, speed_yellow = 3f;

    private float rate_red, rate_green, rate_yellow;

    private float maxRate = 5f, minRate = .1f, allowablError = .2f;

    private bool isComplete = false;

    private void Start()
    {
        SetRate(red, rate_red = Random.Range(.1f, 5f));
        SetRate(yellow, rate_yellow = Random.Range(.1f, 5f));
        SetRate(green, rate_green = Random.Range(.1f, 5f));
    }
    public void OnRedPress()
    {
        speed_red *= -1;
    }
    public void OnYellowPress()
    {
        speed_yellow *= -1;
    }
    public void OnGreenPress()
    {
        speed_green *= -1;
    }
    public void OnRedPressing()
    {
        if (isComplete) return;
        rate_red += Time.deltaTime * speed_red;

        if(rate_red >= maxRate || rate_red <= minRate)
        {
            speed_red *= -1;
        }

        rate_red = Mathf.Clamp(rate_red, .1f, 5f);

        SetRate(red, rate_red);
    }
    public void OnYellowPressing()
    {
        if (isComplete) return;

        rate_yellow += Time.deltaTime * speed_yellow;

        if (rate_yellow >= maxRate || rate_yellow <= minRate)
        {
            speed_yellow *= -1;
        }

        rate_yellow = Mathf.Clamp(rate_yellow, .1f, 5f);

        SetRate(yellow, rate_yellow);
    }
    public void OnGreenPressing()
    {
        if (isComplete) return;

        rate_green += Time.deltaTime * speed_green;

        if (rate_green >= maxRate || rate_green <= minRate)
        {
            speed_green *= -1;
        }

        rate_green = Mathf.Clamp(rate_green, .1f, 5f);

        SetRate(green, rate_green);
    }
    private void SetRate(Transform tf, float rate)
    {
        tf.localScale = new Vector3(tf.localScale.x, rate, tf.localScale.z);
    }
    public void CheckCorrect()
    {
        if(isComplete) return;
        float dis1 = Mathf.Abs(rate_red - rate_green);
        float dis2 = Mathf.Abs(rate_yellow - rate_green);
        float dis3 = Mathf.Abs(rate_red - rate_yellow);

        if (dis1 <= allowablError && dis2 <= allowablError && dis3 <= allowablError)
        {
            wall.DOLocalMoveY(-5f, .5f).SetDelay(.5f);
            isComplete = true;

            red.DOScaleY(0, .5f).OnComplete(() =>
            {
                red.gameObject.SetActive(false);
                yellow.gameObject.SetActive(false);
                green.gameObject.SetActive(false);
            });
            yellow.DOScaleY(0, .5f);
            green.DOScaleY(0, .5f);

            SoundManager.Instance.PlayShotOneTime(ESound.Correct);
        }
    }
}
