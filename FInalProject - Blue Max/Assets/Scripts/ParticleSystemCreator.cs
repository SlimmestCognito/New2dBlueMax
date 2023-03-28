using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemCreator : MonoBehaviour
{
    public static ParticleSystem explosionPS;

    private void Start()
    {
        explosionPS = Resources.Load<ParticleSystem>("Prefabs/PrefabParticleSystems/ParticleSystem Explosion");
    }

    public static void CreateExplosionPS(Transform t)
    {
        ParticleSystem ps = Instantiate(explosionPS, t.position, Quaternion.identity);
        Destroy(ps.gameObject, ps.main.duration);
    }
}
