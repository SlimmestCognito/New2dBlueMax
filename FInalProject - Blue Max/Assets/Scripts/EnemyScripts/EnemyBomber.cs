using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBomber : EnemyClass
{
    public Rigidbody bp;
    [SerializeField] private AudioClip _shot;

    private void Start()
    {
        _health = 6;
        _bulletTimer = 3.5f;
        bulletPrefab = bp;
        _shotDir = new Vector2(-400, -300);
        _collObjectName = "BulletPlayer";
        _scoreGivenOnDeath = 250;
        shot = _shot;
        volume = 0.4f;
        canCrashIntoPlayer = true;
    }

    private void Update()
    {
        _bulletTimer -= Time.deltaTime;
        if (_bulletTimer <= 0)
        {
            FireProjectile(_shotDir);
            _bulletTimer += (3.2f + Random.Range(-0.5f, 0.5f));
        }

        if (_health > 10)
        {
            print(_health);
            Debug.Break();
        }

        if (_health == 0)
        {
            AddScore(_scoreGivenOnDeath);
            UIManager.bombersDestroyed += 1;
            Destroy();
        }
    }
}
