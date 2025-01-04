using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class GolemGun : MonoBehaviour
{
    [SerializeField] GolemBullet bulletPrefab;
    [SerializeField] BoxCollider boxCollider;
    [SerializeField] PlayerHealPoint hp;

    private float shootCooldown = 1f, shootTimer = 1f;

    private void Update()
    {
        shootTimer -= Time.deltaTime;
        if (shootTimer < 0f)
        {
            Shooting();
        }
    }
    private void Shooting()
    {
        shootTimer = shootCooldown;
        GolemBullet bullet = ObjectPoolDictArray.Instance.GetGameObject(bulletPrefab);
        bullet.transform.position = transform.position;
        bullet.OnInit(GetRandomPositionInsideBox(), hp);
    }
    public Vector3 GetRandomPositionInsideBox()
    {
        if (boxCollider == null)
        {
            return Vector3.zero;
        }

        Vector3 boxSize = boxCollider.size;
        Vector3 boxCenter = boxCollider.center;

        float randomX = Random.Range(-boxSize.x / 2, boxSize.x / 2);
        float randomY = Random.Range(-boxSize.y / 2, boxSize.y / 2);
        float randomZ = Random.Range(-boxSize.z / 2, boxSize.z / 2);

        Vector3 randomLocalPosition = new Vector3(randomX, randomY, randomZ);
        Vector3 randomWorldPosition = boxCollider.transform.TransformPoint(boxCenter + randomLocalPosition);

        return randomWorldPosition;
    }
}
