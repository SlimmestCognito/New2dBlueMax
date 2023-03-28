using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    #region Global variables
    public GameObject gameCanvas;
    public static bool removeGameCanvas;

    public static int playerScore = 0;
    public static int playerHealth = 100;
    public static int playerBombs = 9;
    public static bool _rapidfireUIToggle = false;
    public static bool isPlayerAlive;

    public static float gameTime;
    public static int fightersDestroyed;
    public static int bombersDestroyed;
    public static int fortsDestroyed;
    public static int pillboxDestroyed;

    private int _ps;
    private int _ph;
    private int _pb;

    [SerializeField] private TextMeshProUGUI _scoretext;
    [SerializeField] private TextMeshProUGUI _healthtext;
    [SerializeField] private TextMeshProUGUI _bombtext;
    [SerializeField] private TextMeshProUGUI _rapidfiretext;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        //reset static variables
        removeGameCanvas = false;
        Time.timeScale = 1; 
        playerHealth = 100;
        gameTime = 0;
        fightersDestroyed = 0;
        bombersDestroyed = 0;
        fortsDestroyed = 0;
        pillboxDestroyed = 0;
        playerHealth = 100;
        playerScore = 0;
        playerBombs = 9;
        _ps = playerScore;
        _ph = playerHealth;
        _pb = playerBombs;
        _rapidfiretext.enabled = false;

        UpdateUI();

    }

    // Update is called once per frame
    void Update()
    {
        playerHealth = Mathf.Clamp(playerHealth, 0, 100);
        playerBombs = Mathf.Clamp(playerBombs, 0, 12);

        if (isPlayerAlive)
        {
            gameTime += Time.deltaTime;
        }

        if (_ps != playerScore)
        {
            UpdateScore();
            _ps = playerScore;
        }
        if (_ph != playerHealth)
        {
            UpdateHealth();
            _ph = playerHealth;
        }
        if (_pb != playerBombs)
        {
            UpdateBombs();
            _pb = playerBombs;
        }
        if (_rapidfireUIToggle == true) 
        {
            UpdateRapidfireUIToggle();
        }
        if (removeGameCanvas)
        {
            gameCanvas.SetActive(false);
        }

    }

    string convertBombsToText()
    {
        string bombstostring = "";

        if (playerBombs == 0)
        {
            bombstostring += "EMPTY!";
        }
        else
        {
            for (int i = 0; i < playerBombs; i++)
            {
                bombstostring += "|";
            }
        }
        return bombstostring;
    }

    void UpdateScore()
    {
        string s = "SCORE: ";
        s += playerScore.ToString();
        _scoretext.text = s;
    }

    void UpdateHealth() {
        string h = "HEALTH: ";
        h += playerHealth.ToString();
        _healthtext.text = h;
    }

    void UpdateBombs()
    {
        string b = "BOMBS: ";
        b += convertBombsToText();
        _bombtext.text = b;
    }

    void UpdateRapidfireUIToggle() 
    {
        _rapidfiretext.enabled = !_rapidfiretext.enabled;
        _rapidfireUIToggle = !_rapidfireUIToggle;
    }
    public void UpdateUI()
    {
        UpdateHealth();
        UpdateScore();
        UpdateBombs();
        UpdateRapidfireUIToggle();
    }
}
