using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class Golem : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] Collider hitbox, hitbox2;
    [SerializeField] Transform warning, warning2;

    public Transform targetPos;

    const string anim_Idle = "idle";
    const string anim_Attack = "attack";
    const string anim_Hammer = "hammer";
    const string anim_Damage = "damage";
    const string anim_Down = "down";

    private string currentAnim;
    private int hitCount = 0;
    private float cooldown = 10f;
    private bool isAttacking = false, isDown = false;
    StateMachineNP.Character character;

    void Start()
    {
        Camera.main.transform.position -= Camera.main.transform.forward * 3f;
        Camera.main.fieldOfView = 90;

        character = FindAnyObjectByType<StateMachineNP.Character>();

        OnAttack();
    }
    private void Update()
    {
        if (isDown) return;

        cooldown -= Time.deltaTime;
        if (cooldown < 0)
        {
            cooldown = Random.Range(8f, 12f);

            if (hitCount >= 2)
            {
                OnAttack2();
            }
            else
            {
                OnAttack();
            }
        }
    }
    public void OnDamage()
    {
        hitCount++;
        StartAnim(anim_Damage);
        StartCoroutine(delay());
        IEnumerator delay()
        {
            if (isAttacking)
            {
                warning.DOScale(0, .5f);
                hitbox.enabled = false;
            }
            yield return new WaitForSeconds(1f);

            if(hitCount > 2)
            {
                StartAnim(anim_Down);
                yield return new WaitForSeconds(1f);
                SoundManager.Instance.PlayShotOneTime(ESound.Impact);
                yield return new WaitForSeconds(1f);
                gameObject.SetActive(false);
            }
            else
            {
                OnAttack2();
            }
        }
    }
    //---------attack--------------
    public void OnAttack()
    {
        if (isAttacking) return;
        isAttacking = true;
        transform.LookAt(new Vector3(character.transform.position.x, transform.position.y, character.transform.position.z));
        StartAnim(anim_Attack);
        warning.DOScale(1f, 1f);
    }
    public void OnHitBoxEnable()
    {
        SoundManager.Instance.PlayShotOneTime(ESound.Impact);
        hitbox.enabled = true;
        warning.localScale = Vector3.zero;
    }
    public void OnHitBoxDisable()
    {
        hitbox.enabled = false;
        Idle();
    }

    public void OnAttack2()
    {
        isAttacking = true;

        StartAnim(anim_Hammer);
        warning2.DOScale(1f, 1f);
    }
    public void OnHitBoxEnable2()
    {
        SoundManager.Instance.PlayShotOneTime(ESound.Impact);

        Camera.main.DOShakePosition(.2f, 1f);
        hitbox2.enabled = true;
        warning2.localScale = Vector3.zero;
    }
    public void OnHitBoxDisable2()
    {
        hitbox2.enabled = false;
        Idle();
    }
    //-------------------------------

    private void Idle()
    {
        transform.DORotate(new Vector3(0,180f, 0), .5f);
        isAttacking = false;
        StartAnim(anim_Idle);
    }

    private void StartAnim(string animName)
    {
        currentAnim = animName;
        animator.SetTrigger(animName);
    }
}
