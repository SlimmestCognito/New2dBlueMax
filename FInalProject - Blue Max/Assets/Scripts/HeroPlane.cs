using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroPlane : MonoBehaviour
{
    #region Global variables

    public Rigidbody bulletPrefab;
    public GameObject bombPrefab;

    private Animator anim;
    private Rigidbody rb;
    private Camera gameCamera;

    public RectTransform rt;
    public Canvas uicanvas;
    //float UIbarHeight;
    float _width;
    float _height;
    float _buffer;

    Vector2 _rectWorldSize;
    float _uiscreenheight;

    //float bounding_size = 10.0f;
    float mult = 1000.0f;

    float normalFireInterval = 0.2f;
    float powerupFireInterval = 0.1f;
    float bombdropInterval = 1.5f;

    float last_fire_time;
    float last_bomb_time;
    float powerupTimer = 0.0f;

    [SerializeField] private AudioClip bulletShot;
    [SerializeField] private AudioClip bombShot;
    [SerializeField] private AudioClip pickupSFX;

    [SerializeField] private GameObject bombReticle;
    private SpriteRenderer bombReticleSR;

    #endregion

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        gameCamera = FindObjectOfType<Camera>();
        bombReticle.SetActive(false);
        bombReticleSR = bombReticle.GetComponent<SpriteRenderer>();
        //UIbarHeight = rt.rect.height * uicanvas.scaleFactor;
        gameObject.transform.position = gameCamera.ScreenToWorldPoint(new Vector3(400, gameCamera.pixelHeight / 2, 10));

        _height = gameCamera.orthographicSize;
        _width = _height * gameCamera.aspect;
        _buffer = _height / 10;
        _rectWorldSize = gameCamera.ScreenToWorldPoint(rt.rect.size * uicanvas.scaleFactor);
        _uiscreenheight = gameCamera.orthographicSize + _rectWorldSize.y;
    }


    void FixedUpdate()
    {
        Vector2 translation = new Vector2(0, 0);

        //clamp player xy pos to camerabounds xy

        float hspeed = Input.GetAxisRaw("Horizontal");
        float vspeed = Input.GetAxisRaw("Vertical");

        translation += new Vector2(hspeed * mult, vspeed * mult) * Time.deltaTime;
        Vector2 t2 = new Vector2(hspeed, vspeed);
        
        //OLD COLLISION
        
        //Vector2 new_position = gameCamera.WorldToScreenPoint((Vector2)transform.position + t2 / 2);
 
        /*
        if (new_position.x + bounding_size > gameCamera.pixelWidth || new_position.x - bounding_size < 0)
        {
            stop();
        }

        else if (new_position.y + bounding_size > gameCamera.pixelHeight || new_position.y - bounding_size - UIbarHeight < 0)
        {
            stop();
        }
        
        if (transform.position.x > gameCamera.pixelWidth / (uicanvas.scaleFactor * 100))
        {
            stop();
        }

        void stop()
        {
            translation = new Vector2(0, 0);
            rb.velocity = new Vector2(0, 0);
        }
        */
        
        rb.AddForce(translation);

        //clamp player in bounds of camera
        Vector2 t = transform.position;
        t.x = Mathf.Clamp(t.x, -_width + _buffer, _width - _buffer);
        t.y = Mathf.Clamp(t.y, -_height + _uiscreenheight + _buffer, _height - _buffer);
        transform.position = t;

    }

    void Update()
    {
        last_fire_time -= Time.deltaTime;
        last_bomb_time -= Time.deltaTime;

        #region clamping
        powerupTimer = Mathf.Clamp(powerupTimer, 0.0f, 10.0f);
        last_fire_time = Mathf.Clamp(last_fire_time, 0.0f, 5.0f);
        last_bomb_time = Mathf.Clamp(last_bomb_time, 0.0f, 5.0f);
        #endregion

        bombReticle.SetActive(false);

        //movement controls
        if (!UIMenuManager.isPaused)
        {
            if (Input.GetKey(KeyCode.Space) && powerupTimer == 0 || Input.GetButton("Fire1") && powerupTimer == 0)
            {
                FireBullet(normalFireInterval);
            }
            if (Input.GetKey(KeyCode.LeftShift))
            {
                bombReticle.SetActive(true);
            }
            if (Input.GetKeyDown(KeyCode.R) && UIManager.playerBombs > 0 || Input.GetButton("Fire2") && UIManager.playerBombs > 0)
            {
                DropBomb(bombdropInterval);
            }
            if (Input.GetKeyDown(KeyCode.O))
            {
                UIManager.playerHealth = 0;
            }
           
        }

        //Bomb reticle color changer
        if (last_bomb_time > 0)
        {
            bombReticleSR.color = new Vector4 (0.5f, 0.5f,0.5f,1.0f);
        }
        else if (UIManager.playerBombs == 0)
        {
            bombReticleSR.color = Color.red;
        }
        else 
        {
            bombReticleSR.color = Color.white;
        }

        //changing player animation states
        switch (rb.velocity.x)
        {
            case < -1.0f:
                anim.SetInteger("State", 1);
                break;
            case > 1.0f:
                anim.SetInteger("State", 2);
                break;
            default:
                anim.SetInteger("State", 0);
                break;
        }

        if (Input.GetKeyDown(KeyCode.Escape) && UIMenuManager.canChangePanel)
        {
            UIMenuManager.isPaused = !UIMenuManager.isPaused;
        }

        if (UIManager.playerHealth <= 0)
        {
            Death();
        }
    }

    IEnumerator RapidFireUIToggle()
    {
        UIManager._rapidfireUIToggle = true; // One shot, will be set to false straight after
        powerupTimer += 8.0f;

        while (powerupTimer > 0)
        {
            powerupTimer -= Time.deltaTime;
            if (Input.GetKey(KeyCode.Space) || (Input.GetButton("Fire1")))
            {
                FireBullet(powerupFireInterval);
            }

            yield return null;
        }
        UIManager._rapidfireUIToggle = true; // One shot, will be set to false straight after
    }

    void FireBullet(float a)
    {
        if (last_fire_time == 0)
        {
            last_fire_time = a;
            Rigidbody bullet_instance = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            bullet_instance.AddForce(new Vector2(800, 600)); //400 300
            AudioCreator.CreateAudioSource(bulletShot, 0.3f, transform);
        }
    }
    void DropBomb(float a)
    {
        if (last_bomb_time == 0)
        {
            UIManager.playerBombs -= 1;
            last_bomb_time = a;
            GameObject bomb_instance = Instantiate(bombPrefab, transform.position, Quaternion.identity);
            bomb_instance.GetComponent<Rigidbody>().AddForce(new Vector2(10, -50));
            AudioCreator.CreateAudioSource(bombShot, 1.0f, transform);
        }
    }

    void OnTriggerEnter(Collider coll)
    {

        if (coll.gameObject.CompareTag("BulletEnemy"))
        {
            AudioCreator.CreateAudioSource(AudioCreator.damageSFXsArray[Random.Range(0, AudioCreator.damageSFXsArray.Length)], 0.4f, transform);
            ModifyPlayerHealth(-10);
        }

        if (coll.gameObject.CompareTag("EFighter"))
        {
            AudioCreator.CreateAudioSource(AudioCreator.explosionSFXsArray[Random.Range(0, AudioCreator.explosionSFXsArray.Length)], 0.4f, transform);
            UIManager.playerHealth += -20;
        }

        if (coll.gameObject.CompareTag("EBomber"))
        {
            AudioCreator.CreateAudioSource(AudioCreator.explosionSFXsArray[Random.Range(0, AudioCreator.explosionSFXsArray.Length)], 0.4f, transform);
            UIManager.playerHealth += -30;
        }

        if (coll.gameObject.CompareTag("BulletPillbox"))
        {
            AudioCreator.CreateAudioSource(AudioCreator.damageSFXsArray[Random.Range(0, AudioCreator.damageSFXsArray.Length)], 0.4f, transform);
            ModifyPlayerHealth(-5);
        }

        if (coll.gameObject.CompareTag("HealthUpgrade"))
        {
            ModifyPlayerHealth(20);
            AudioCreator.CreateAudioSource(pickupSFX, 1.0f, transform);
        }

        if (coll.gameObject.CompareTag("BombUpgrade"))
        {
            Destroy(coll.gameObject);
            UIManager.playerBombs += 3;
            AudioCreator.CreateAudioSource(pickupSFX, 1.0f, transform);
        }

        if (coll.gameObject.CompareTag("SpeedUpgrade"))
        {
            Destroy(coll.gameObject);
            StartCoroutine("RapidFireUIToggle");
            AudioCreator.CreateAudioSource(pickupSFX, 1.0f, transform);
        }

        void ModifyPlayerHealth(int a)
        {
            Destroy(coll.gameObject);
            UIManager.playerHealth += a;
        }
    }
    void Death()
    {
        GameObject UI = GameObject.Find("UI");
        ParticleSystemCreator.CreateExplosionPS(transform);
        AudioCreator.CreateAudioSource(AudioCreator.explosionSFXsArray[Random.Range(0, AudioCreator.explosionSFXsArray.Length)], 0.4f, transform);
        UI.GetComponent<UIMenuManager>().StartCoroutine("ShowEndRunMenu");
        Destroy(gameObject);
    }
}