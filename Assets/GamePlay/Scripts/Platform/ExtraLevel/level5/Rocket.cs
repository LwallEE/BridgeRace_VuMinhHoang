using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    [SerializeField] GameObject obj;
    [SerializeField] ParticleSystem vfx;
    public Golem golem; 
    public float speed = 5f; 
    public float rotateSpeed = 2f;

    private bool isDespawn, isActive;
    public bool IsActive => isActive;

    float timer;

    void Update()
    {
        if (golem == null || isDespawn || !isActive) return;

        timer += Time.deltaTime;
        float curSpeed = Mathf.Clamp01(timer / 2f) * speed;
        if (timer > 2f)
        {
            Vector3 direction = (golem.targetPos.position - transform.position).normalized;

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotateSpeed);
        }

        transform.position += transform.forward * Time.deltaTime * curSpeed;

        if(Vector2.Distance(transform.position, golem.targetPos.position) < 2.5f)
        {
            Despawn();
            golem.OnDamage();
        }
    }
    public void OnActive()
    {
        isActive = true;
    }
    private void Despawn()
    {
        isDespawn = true;
        obj.SetActive(false);
        vfx.Play();
    }
}
