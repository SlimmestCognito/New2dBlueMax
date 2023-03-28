using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using TMPro;
using System;

public class UIMenuManager : MonoBehaviour
{
    public float sceneChangeTime;
    public List<GameObject> panels;
    public AudioClip uiSelect;

    public Slider musicSlider;
    public Slider sfxSlider;

    private Color cBlack = new Vector4(0, 0, 0, 1);
    private Color cTransparent = new Vector4(0, 0, 0, 0);

    private AudioMixer m;
    private AudioSource aS;

    public static bool canChangePanel;
    public static bool isPaused;

    [SerializeField] private TextMeshProUGUI _rankText;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _highScoreText;
    [SerializeField] private TextMeshProUGUI _statsText;


    private void Start()
    {
        m = Resources.Load<AudioMixer>("Mixer");
        aS = GetComponent<AudioSource>();
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolumeParameter", 0.5f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolumeParameter", 0.5f);
        StartCoroutine("StartTransition");
        
    }

    private void Update()
    {
        if (isPaused == true && panels.Find(x => x.name == "Background_PauseMenu"))
        {
            Time.timeScale = 0;
            panels.Find(x => x.name == "Background_PauseMenu").SetActive(true);
        }
        if (isPaused == false && panels.Find(x => x.name == "Background_PauseMenu"))
        {
            Time.timeScale = 1;
            panels.Find(x => x.name == "Background_PauseMenu").SetActive(false);
        }

    }

    public void ChangeScene(string level)
    {
        StartCoroutine("cs", level);
    }

    public void QuitGame()
    {
        StartCoroutine("qg");
    }

    public void ChangePanel(GameObject panel)
    {
        StartCoroutine("cp", panel);
    }

    public void AudioRollover() 
    {
        aS.PlayOneShot(uiSelect, 10.0f);
        aS.PlayOneShot(uiSelect, 1.0f);
    }
    public void GetLeaderboard() 
    {
        CreateLeaderboard();
    }

    public void UnPause()
    {
        isPaused = false;
    }

    IEnumerator cp(GameObject panel)
    {
        if (canChangePanel)
        {
            aS.PlayOneShot(uiSelect, 20.0f);
            canChangePanel = false;
            StartCoroutine(Camera.main.GetComponent<UITransition>().CameraColourFade(cTransparent, cBlack, sceneChangeTime));
            yield return new WaitForSeconds(sceneChangeTime);
            panels.Find(x => x.activeInHierarchy).SetActive(false); //deactivate the current panel 
            panels.Find(x => x.name == panel.name).SetActive(true); //activates the selected panel
            StartCoroutine(Camera.main.GetComponent<UITransition>().CameraColourFade(cBlack, cTransparent, sceneChangeTime));
            yield return new WaitForSeconds(sceneChangeTime);
        }

        canChangePanel = true;
    }

    IEnumerator cs(string level) 
    {
        aS.PlayOneShot(uiSelect, 20.0f);
        StartCoroutine(Camera.main.GetComponent<UITransition>().CameraColourFade(cTransparent, cBlack, sceneChangeTime));
        yield return new WaitForSeconds(sceneChangeTime);
        SceneManager.LoadScene(level);
    }

    IEnumerator qg()
    {
        aS.PlayOneShot(uiSelect, 20.0f);
        StartCoroutine(Camera.main.GetComponent<UITransition>().CameraColourFade(cTransparent, cBlack, sceneChangeTime));
        yield return new WaitForSeconds(sceneChangeTime);
        Application.Quit();
    }

    public IEnumerator om(GameObject panel) //open menu to panel
    {

        if (canChangePanel)
        {
            canChangePanel = false;
            StartCoroutine(Camera.main.GetComponent<UITransition>().CameraColourFade(cTransparent, cBlack, sceneChangeTime));
            yield return new WaitForSeconds(sceneChangeTime);
            UIManager.removeGameCanvas = true;

            panels.Find(x => x.name == panel.name).SetActive(true); //activates the selected panel
            StartCoroutine(Camera.main.GetComponent<UITransition>().CameraColourFade(cBlack, cTransparent, sceneChangeTime));
            yield return new WaitForSeconds(sceneChangeTime);
        }

        canChangePanel = true;
    }

    //settings sliders
    public void SetMusicLevel(float v)
    {
        m.SetFloat("MusicVolParam", Mathf.Log10(v) * 20);
    }
    public void SetSFXLevel(float v)
    {
        m.SetFloat("SFXVolParam", Mathf.Log10(v) * 20);
    }

    void CreateLeaderboard()
    {
        Leaderboard.GetHighScores();

        string _rt = "RANK\n"; //ranktextstring
        string _nt = "NAME\n"; //nametextstring
        string _st = "SCORE\n"; //scoretextstring

        for (int i = 0; i < Leaderboard.hiscores.Count; i++) //hiscores count should be 20
        {
            _rt += (i+1); 
            switch (i + 1) //rank suffix
            {
                case 1:
                    _rt += "st\n";
                    break;
                case 2:
                    _rt += "nd\n";
                    break;
                case 3:
                    _rt += "rd\n";
                    break;
                default:
                    _rt += "th\n";
                    break;
            }
            _nt += Leaderboard.hiscores[i].GetName() + "\n";
            _st += Leaderboard.hiscores[i].GetScore().ToString("00000000") + "\n";
        }

        _rankText.text = _rt;
        _nameText.text = _nt;
        _scoreText.text = _st;

    }
    public void SavePlayerPrefs() 
    {
        PlayerPrefs.SetFloat("MusicVolumeParameter", musicSlider.value);
        PlayerPrefs.SetFloat("SFXVolumeParameter", sfxSlider.value);
        PlayerPrefs.Save();
    }

    IEnumerator StartTransition() 
    {
        canChangePanel = false;
        StartCoroutine(Camera.main.GetComponent<UITransition>().CameraColourFade(cBlack, cTransparent, sceneChangeTime));
        yield return new WaitForSeconds(sceneChangeTime);
        canChangePanel = true;
    }

    public IEnumerator ShowEndRunMenu()
    {
        UIManager.isPlayerAlive = false;
        Leaderboard.GetHighScores();

        yield return new WaitForSeconds(2);

        //string together stats text
        string result = "";
        result += "You Survived For: \n";
        //result += " ";
        TimeSpan ts = TimeSpan.FromSeconds(UIManager.gameTime);
        result += string.Format(" {0:00}:{1:00}", ts.Minutes, ts.Seconds);
        result += "\n";
        result += "\n";
        result += "You Killed: \n";
        result += " Fighters  " + UIManager.fightersDestroyed + "\n";
        result += " Bombers   " + UIManager.bombersDestroyed + "\n";
        result += " Forts     " + UIManager.fortsDestroyed + "\n";
        result += " Pillboxes " + UIManager.pillboxDestroyed + "\n";
        result += "\n";
        result += "Final Score: " + UIManager.playerScore;
        _statsText.text = result;

        _highScoreText.text = UIManager.playerScore.ToString();

        if (UIManager.playerScore > Leaderboard.hiscores[Leaderboard.hiscores.Count -1].GetScore()) //get last score in leaderboard
        {
            yield return StartCoroutine("om", panels.Find(x => x.name == "Background_DeathScreen_HighScore"));
        }
        else 
        {
            yield return StartCoroutine("om", panels.Find(x => x.name == "Background_DeathScreen_Retry"));
        }
    }
}
