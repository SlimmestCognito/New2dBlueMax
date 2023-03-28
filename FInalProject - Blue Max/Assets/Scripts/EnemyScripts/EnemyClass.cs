using UnityEngine;

public class EnemyClass : MonoBehaviour
{
    protected sbyte _health;
    protected float _bulletTimer;
    protected Rigidbody bulletPrefab;
    protected string _collObjectName;
    protected int _scoreGivenOnDeath;
    protected Vector2 _shotDir;
    protected AudioClip shot;
    protected float volume;
    protected bool canCrashIntoPlayer;

    public virtual void FireProjectile(Vector2 a)
    {
        if (UIManager.isPlayerAlive)
        {
            Rigidbody laser_instance = Instantiate(bulletPrefab, transform.position, Quaternion.identity) as Rigidbody;
            laser_instance.AddForce(a);
            AudioCreator.CreateAudioSource(shot, volume, transform);
        }
    }

    public virtual void Destroy()
    {
        if (canCrashIntoPlayer)
        {
            AudioCreator.CreateAudioSource(AudioCreator.explosionSFXsArray[Random.Range(0, AudioCreator.explosionSFXsArray.Length)], 0.4f, transform);
            ParticleSystemCreator.CreateExplosionPS(transform); 
        }
        Destroy(gameObject);
    }

    public virtual void AddScore(int a)
    {
        UIManager.playerScore += a;
    }
    
    private void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.CompareTag(_collObjectName) && _health >= 0)
        {
            _health -= 1;
            AudioCreator.CreateAudioSource(AudioCreator.damageSFXsArray[Random.Range(0,AudioCreator.damageSFXsArray.Length)], 0.4f, transform);
            if (canCrashIntoPlayer)
            { 
                Destroy(coll.gameObject); 
            }
        }

        if (coll.gameObject.CompareTag("Player") && canCrashIntoPlayer)
        {
            _health = 0;
        }
    }
}
