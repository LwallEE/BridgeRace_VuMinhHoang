using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UINetworkBase : MonoBehaviour
{
   public  RectTransform _rect;
   
   public virtual void Open()
   {
      gameObject.SetActive(true);
   }

   public bool IsOpen()
   {
      return gameObject.activeSelf;
   }

   public virtual void Close()
   {
      gameObject.SetActive(false);
   }

   public void ScaleUp(float timeScale)
   {
      if (_rect == null) return;
      StopAllCoroutines();
      StartCoroutine(ScaleCoroutine(timeScale, true));
   }

   public void ScaleDown(float timeScale)
   {
      if (_rect == null) return;
      StopAllCoroutines();
      StartCoroutine(ScaleCoroutine(timeScale, false));
   }

   private IEnumerator ScaleCoroutine(float timeScale, bool isScaleUp)
   {

      Vector3 initialScale = isScaleUp ? Vector3.zero : _rect.localScale;
      Vector3 goalScale = isScaleUp ? Vector3.one : Vector3.zero;
      _rect.localScale = initialScale;
      int interation = 100;
      float sign = isScaleUp ? 1f : -1f;
      float speed = 1f / interation * sign;
      WaitForSeconds wait = new WaitForSeconds(timeScale *1f / interation);
      while (Vector3.Distance(_rect.localScale, goalScale) > 0.01)
      {
         _rect.localScale = _rect.localScale + Vector3.one * speed;
         yield return wait;
      }
      if(!isScaleUp) gameObject.SetActive(false);
   }

}
