using UnityEngine;

public class Fortress : EnemyClass
{
    public Rigidbody bp;

    private void Start()
    {
        _health = 1;
        _collObjectName = "BombSplash";
        _scoreGivenOnDeath = 500;
        canCrashIntoPlayer = false;
    }

    private void Update()
    {
        if (_health == 0)
        {
            AddScore(_scoreGivenOnDeath);
            UIManager.fortsDestroyed += 1;
            Destroy();
        }
    }
}

