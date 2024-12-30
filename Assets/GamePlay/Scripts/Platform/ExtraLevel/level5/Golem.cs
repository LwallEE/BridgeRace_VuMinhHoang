using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] Collider hitbox;
    [SerializeField] Transform warning;

    public Transform targetPos;

    const string anim_Idle = "idle";
    const string anim_Attack = "attack";
    const string anim_Hammer = "hammer";
    const string anim_Damage = "damage";
    const string anim_Down = "down";

    private string currentAnim;
    private int hitCount = 0;
    void Start()
    {
        Camera.main.transform.position -= Camera.main.transform.forward * 3f;
        Camera.main.fieldOfView = 80;
    }

    public void OnDamage()
    {
        hitCount++;
        StartAnim(anim_Damage);
        StartCoroutine(delay());
        IEnumerator delay()
        {
            yield return new WaitForSeconds(1f);
            Idle();
            yield return new WaitForSeconds(1f);
            OnAttack();
        }
    }
    public void OnAttack()
    {
        StartAnim(anim_Attack);
        warning.DOScale(1f, 1f);
    }
    public void OnHitBoxEnable()
    {
        hitbox.enabled = true;
        warning.localScale = Vector3.zero;
    }
    public void OnHitBoxDisable()
    {
        hitbox.enabled = false;
    }
    private void Idle()
    {
        StartAnim(anim_Idle);
    }

    private void StartAnim(string animName)
    {
        currentAnim = animName;
        animator.SetTrigger(animName);
    }
}
