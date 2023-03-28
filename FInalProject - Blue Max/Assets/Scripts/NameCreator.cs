using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NameCreator : MonoBehaviour
{
    public List<TextMeshProUGUI> letterList;
    [SerializeField]
    private Transform arrows;
    List<char> characters = new List<char>();    
    int letterindex = 0;
    int selectedLetter = 0;

    bool colourtoggle = false;

    private void Awake()
    {
        characters.AddRange("ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray());
        char[] playername = PlayerPrefs.GetString("PlayerSetUsername").ToCharArray();

        for (int i = 0; i < playername.Length; i++)
        {
            letterList[i].text = playername[i].ToString();
        }   

        letterList[selectedLetter].color = Color.white;

        char currentletter = letterList[selectedLetter].text[0];
        letterindex = characters.FindIndex(x => x == currentletter);
        arrows.position = letterList[selectedLetter].transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        colourtoggle = !colourtoggle;
        if (colourtoggle)
        {
            letterList[selectedLetter].color = Color.white;
        }
        else
        {
            letterList[selectedLetter].color = Color.red;
        }

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            ChangeletterValue(-1);
        }
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            ChangeletterValue(1);
        }
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ChangeSelectedLetter(-1);
        }
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            ChangeSelectedLetter(1);
        }
    }
    
    //LEFT RIGHT to change
    void ChangeSelectedLetter(int i) 
    {
        letterList[selectedLetter].color = Color.red;

        selectedLetter += i;

        if (selectedLetter == -1)
        {
            selectedLetter = letterList.Count - 1;
        }
        if (selectedLetter == letterList.Count)
        {
            selectedLetter = 0;
        }
        
        //start iterating from the current letter in the field. 
        char currentletter = letterList[selectedLetter].text[0];
        letterindex = characters.FindIndex(x => x == currentletter);
        arrows.position = letterList[selectedLetter].transform.position;
        
        //letterList[selectedLetter].color = Color.white;

    }

    //UP DOWN to change to
    void ChangeletterValue(int i) 
    {
        letterindex += i;

        if (letterindex == -1) 
        {
            letterindex = characters.Count - 1;
        }
        if (letterindex == characters.Count)
        {
            letterindex = 0;
        }

        letterList[selectedLetter].text = characters[letterindex].ToString();

    }

    public void SaveName() 
    {
        string newName = "";

        for (int i = 0; i < letterList.Count; i++)
        {
            newName += letterList[i].text.ToString();
        }

        PlayerPrefs.SetString("PlayerSetUsername", newName);
    }
}
