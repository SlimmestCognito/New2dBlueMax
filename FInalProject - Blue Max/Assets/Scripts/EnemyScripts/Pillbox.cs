using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pillbox : EnemyClass
{
    public Rigidbody bp;
    [SerializeField] private AudioClip _shot;

    private void Start()
    {
        _health = 1;
        _bulletTimer = 1.3f;
        bulletPrefab = bp;
        _shotDir = new Vector2(-300, 300);
        _collObjectName = "BombSplash";
        _scoreGivenOnDeath = 300;
        shot = _shot;
        volume = 0.4f;
        canCrashIntoPlayer = false;
    }

    private void Update()
    {
        _bulletTimer -= Time.deltaTime;
        if (_bulletTimer <= 0)
        {
            FireProjectile(_shotDir);
            _bulletTimer += (1.3f + Random.Range(-0.5f, 0.5f));
        }

        if (_health == 0)
        {
            AddScore(_scoreGivenOnDeath);
            UIManager.pillboxDestroyed += 1;
            Destroy();
        }
    }
}