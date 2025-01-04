using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

[RequireComponent(typeof(ObjectPoolDictArray))]
public class ParticleManager : Singleton<ParticleManager>
{
    [SerializeField] ParticleSystem[] prefabs;

    public void PlayFxCollect(Vector3 position, Color color)
    {
        ParticleSystem fx = ObjectPoolDictArray.Instance.GetParticle(prefabs[(int)ParticleType.Collect]);
        fx.transform.position = position;

        var main = fx.main;
        main.startColor = color;
    }
    public void PlayFxCollide(Vector3 position)
    {
        ParticleSystem fx = ObjectPoolDictArray.Instance.GetParticle(prefabs[(int)ParticleType.Collide]);
        fx.transform.position = position + Vector3.up * 1f;

        Vector3 dir = (Camera.main.transform.position - fx.transform.position).normalized;
        fx.transform.position += dir * 5f;
    }
    public void PlayFxExplode(Vector3 position)
    {
        ParticleSystem fx = ObjectPoolDictArray.Instance.GetParticle(prefabs[(int)ParticleType.Explode]);
        fx.transform.position = position + Vector3.up * 1f;
    }
}

public enum ParticleType
{
    Collect = 0,
    Collide = 1,
    Explode = 2,
}
