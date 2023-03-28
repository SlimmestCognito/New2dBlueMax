using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    #region Global variables

    [SerializeField] private Camera gameCamera;

    //Prefabs that need to be instansiated
    [Header("PREFABS")]
    
    [Header("Enemy Prefabs")]
    [SerializeField] private Object enemyFighter;
    [SerializeField] private Object enemyBomber;
    [SerializeField] private Object enemyFortress;
    [SerializeField] private Object enemyPillbox;

    [Header("Upgrade Prefabs")]
    [SerializeField] private Object upgradeAddHealth;
    [SerializeField] private Object upgradeAddBombs;
    [SerializeField] private Object upgradeRapidFire;

    private List<Object> upgradeList; 

    [Header("Misc Prefabs")]
    [SerializeField] private Object clouds;
    [SerializeField] private Sprite[] cloudsArray;
    [SerializeField] private Object tree;

    [Header("PREFAB SPAWN TIMERS")]

    [Header("Enemy Timers")]
    public float enemyFigherTime;
    public float enemyBomberTime;
    public float enemyFortressTime;
    public float enemyPillboxTime;

    [Header("Upgrade Timers")]
    public float upgradeTime;

    [Header("Tree Timers")]
    public float cloudsTime;
    public float treeTime;

    private float _sWP;
    private float orthoWidth, orthoHeight;

    private float eFighterTimer, eBomberTimer, eFortressTimer, ePillboxTimer, uTimer, cloudTimer, treeTimer;

    public float cloudamountHIn, cloudamountHOut, cloudamountVIn, cloudamountVOut;
    private float difficultyMod;
    public int difficultyModVal = 100;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        UIManager.isPlayerAlive = true;
        cloudsArray = Resources.LoadAll<Sprite>("Textures/CloudsSpriteSheet");

        gameCamera = FindObjectOfType<Camera>();
        _sWP = ScreenWidthPercent();

        upgradeList = new List<Object>();
        upgradeList.Add(upgradeAddHealth);
        upgradeList.Add(upgradeAddBombs);
        upgradeList.Add(upgradeRapidFire);

        orthoWidth = gameCamera.orthographicSize * gameCamera.aspect;
        orthoHeight = gameCamera.orthographicSize;

        //sort out
        eFighterTimer = enemyFigherTime;
        eBomberTimer = enemyBomberTime;
        eFortressTimer = enemyFortressTime;
        ePillboxTimer = enemyPillboxTime;
        uTimer = upgradeTime;
        cloudTimer = cloudsTime;
        treeTimer = treeTime;

    }

    // Update is called once per frame
    void Update()
    {
        difficultyMod += Time.deltaTime / difficultyModVal;

        if (UIManager.isPlayerAlive)
        {
            eFighterTimer -= Time.deltaTime;
            if (eFighterTimer <= 0)
            {
                Spawn(enemyFighter, -RandomPosVector(), new Vector2(40, 30));
                eFighterTimer += (enemyFigherTime + (Random.Range(-0.5f, 0.5f))) - difficultyMod;
            }

            eBomberTimer -= Time.deltaTime;
            if (eBomberTimer <= 0)
            {
                Spawn(enemyBomber, RandomPosVector(), new Vector2(-40, -30));
                eBomberTimer += (enemyBomberTime + (Random.Range(-0.5f, 0.5f))) - difficultyMod;
            }

            eFortressTimer -= Time.deltaTime;
            if (eFortressTimer <= 0)
            {
                Spawn(enemyFortress, RandomPosVector(), new Vector2(-280f, -210.0f));
                eFortressTimer += (enemyFortressTime + (Random.Range(-0.5f, 0.5f))) - difficultyMod;
            }

            ePillboxTimer -= Time.deltaTime;
            if (ePillboxTimer <= 0)
            {
                Spawn(enemyPillbox, RandomPosVector(), new Vector2(-280.0f, -210.0f));
                ePillboxTimer += (enemyPillboxTime + (Random.Range(-0.5f, 0.5f))) - difficultyMod;
            }

            uTimer -= Time.deltaTime;
            if (uTimer <= 0)
            {
                Spawn(upgradeList[Random.Range(0, upgradeList.Count)], RandomPosVector(), new Vector2(-80, -60));
                uTimer += (upgradeTime + (Random.Range(-0.5f, 0.5f)));
            }

            cloudTimer -= Time.deltaTime;
            if (cloudTimer <= 0)
            {
                StartCoroutine("CloudSpawner", 5.0f);
                cloudTimer += (cloudsTime + (Random.Range(-0.5f, 0.5f)));
            }
        }

        treeTimer -= Time.deltaTime;
        if (treeTimer <= 0)
        {
            Spawn(tree, RandomPosVector(), new Vector2(-280.0f, -210.0f));
            treeTimer += (treeTime + (Random.Range(-0.5f, 0.1f)));
        }

    }

    IEnumerator CloudSpawner(float cloudLength = 5.0f)
    {
        Vector4 spawnarea = new Vector4(orthoWidth, orthoHeight, orthoWidth, orthoHeight);
        float probability;
        float timer = 0.0f;

        //in timer
        while (timer < 2.0f)
        {
            spawnarea.x = Mathf.Lerp(orthoWidth, -orthoWidth, timer / 2);
            spawnarea.y = Mathf.Lerp(orthoHeight, -orthoHeight, timer / 2);
            probability = Mathf.Lerp(0.0f, 1.0f, timer / 2); //probability for a cloud to spawn, starting with low probability, ending with high probability
           
            if (Random.value < probability && UIMenuManager.isPaused == false) 
            {
                SpawnCloud(spawnarea);
            }
                        
            timer += Time.deltaTime;
            yield return null;
        }

        //hold clouds
        timer = 0;

        while (timer < cloudLength)
        {
            if (UIMenuManager.isPaused == false)
            {
                SpawnCloud(spawnarea);
            }
            timer += Time.deltaTime;
            yield return null;
        }

        //out timer
        timer = 0;
        while (timer < 2.0f)
        {
            spawnarea.z = Mathf.Lerp(orthoWidth, -orthoWidth, timer / 2);
            spawnarea.w = Mathf.Lerp(orthoHeight, -orthoHeight, timer / 2);
            probability = Mathf.Lerp(1.0f, 0.0f, timer / 2); //probability for a cloud to spawn, starting with high probability, ending with low probability

            if (Random.value < probability && UIMenuManager.isPaused == false)
            {
                SpawnCloud(spawnarea);
            }

            timer += Time.deltaTime;
            yield return null;
        }
    }

    //Works out even spawn distribution along the x and y axis, this converts the horizontal screen resolution into a percentage. A higher percentage means more distribution along X axis
    float ScreenWidthPercent()
    {
        int sW = gameCamera.pixelWidth;
        int sH = gameCamera.pixelHeight;
        int sWH = sW + sH;
        float sWP = ((float)sW / (float)sWH) * 100;
        return sWP;
    }

    //Creates a random spawn point just beyond the playable area
    Vector2 RandomCloudPosVector(Vector4 spawnarea, int mod = 1 )
    {
        int i = Random.Range(0, 100); //percentage int

        if (i <= _sWP)
        {
            return new Vector2(Random.Range(spawnarea.x, spawnarea.z) + mod, orthoHeight + mod);
        }
        else
        {
            return new Vector2(orthoWidth + mod, Random.Range(spawnarea.y, spawnarea.w) + mod);
        }
    }

    //Creates a random spawn point just beyond the playable area
    Vector2 RandomPosVector(int mod = 1) 
    {
        int i = Random.Range(0, 100); //percentage int

        if (i <= _sWP)
        {
            return new Vector2(Random.Range(-orthoWidth, orthoWidth) + mod , orthoHeight + mod);
        }
        else
        {
            return new Vector2(orthoWidth + mod, Random.Range(-orthoHeight, orthoHeight) + mod);
        }
    }

    void Spawn(Object obj, Vector2 startPos, Vector2 vel)
    {
        GameObject newObj = (GameObject)Instantiate(obj, startPos, transform.rotation);
        newObj.GetComponent<Rigidbody>().AddForce(vel);
    }

    void SpawnCloud(Vector4 spawnarea) 
    {
        GameObject g = new GameObject("cloud");
        Rigidbody rB = g.AddComponent<Rigidbody>();
        rB.useGravity = false;
        g.AddComponent<DestroyOnInvisible>(); // destroy the cloud when offscreen

        SpriteRenderer sR = g.AddComponent<SpriteRenderer>();
        sR.sprite = cloudsArray[Random.Range(0, cloudsArray.Length)];
        sR.color = Color255(255,255,255,64);
        sR.sortingOrder = 4; //render infront of everything else

        g.transform.position = RandomCloudPosVector(spawnarea, 2);
        g.GetComponent<Rigidbody>().AddForce(new Vector2(-280.0f, -210.0f));
    }

    Color Color255(int r, int g, int b, int a) 
    {
        return new Color(r / 255.0f, g / 255.0f, b / 255.0f, a / 255.0f);
    }
}
