using UnityEngine;

public class EnemyFighter : EnemyClass 
{ 
    public Rigidbody bp;
    [SerializeField] private AudioClip _shot;

    private void Start()
    {
        _health = 2;
        _bulletTimer = 3.2f;
        bulletPrefab = bp;
        _shotDir = new Vector2(400, 300);
        _collObjectName = "BulletPlayer";
        _scoreGivenOnDeath = 100;
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
            _bulletTimer += (1.8f + Random.Range(-0.5f, 0.5f));
        }

        if (_health <= 0)
        {
            AddScore(_scoreGivenOnDeath);
            UIManager.fightersDestroyed += 1;
            Destroy();
        }
    }
}
