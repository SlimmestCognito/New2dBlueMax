using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Leaderboard : MonoBehaviour
{
    public struct HighscoreEntry
    {
        string _name;
        int _score;

        //constructor
        public HighscoreEntry(string name, int score)
        {
            _name = name;
            _score = score;
        }

        //getters
        public string GetName()
        {
            return _name;
        }
        public int GetScore()
        {
            return _score;
        }
    }

    public static List<HighscoreEntry> hiscores = new List<HighscoreEntry>();

    public static void AddNewScoreToLeaderboard(HighscoreEntry newscore) 
    {
        hiscores.Add(newscore);
        hiscores = hiscores.OrderByDescending(x => x.GetScore()).ToList();
        hiscores.Remove(hiscores.Last());
        SetLeaderboard();
    }

    public void AddScore() 
    {
        AddNewScoreToLeaderboard(new HighscoreEntry( PlayerPrefs.GetString("PlayerSetUsername"),UIManager.playerScore));
    }

    public static void GetHighScores() 
    {
        //erase last leaderboard data
        hiscores.Clear();

        //get player leaderboard data from playerprefs.
        for (int i = 0; i < 20 ; i++) //20 highscores should be saved.
        {
            hiscores.Add(new HighscoreEntry(PlayerPrefs.GetString("user_name_" + i), PlayerPrefs.GetInt("user_highscore_" + i)));
        }
        hiscores = hiscores.OrderByDescending(x => x.GetScore()).ToList();

    }

    static void SetLeaderboard() 
    {
        for (int i = 0; i < hiscores.Count; i++) //hiscores count should be 20
        {
            PlayerPrefs.SetString("user_name_" + i, hiscores[i].GetName());
            PlayerPrefs.SetInt("user_highscore_" + i, hiscores[i].GetScore());
        }
    }   

    //reset playerprefs values to a default state. 
    public static void WipePlayerPrefs()
    {
        PlayerPrefs.SetString("PlayerSetUsername", "AAA");

        for (int i = 0; i < 20; i++) //20 Highscore variables saved
        {
            PlayerPrefs.SetString("user_name_" + i, ""); //leave name blank
            PlayerPrefs.SetInt("user_highscore_" + i, 0); 
        }
    }

    //for debug only - populate playerprefs with temp data. Not ordered, will sort list elsewhere.
    public static void TestLeaderboardFill()
    {
        PlayerPrefs.SetString("user_name_0", "TOM");
        PlayerPrefs.SetInt("user_highscore_0", 200);

        PlayerPrefs.SetString("user_name_1", "LUK");
        PlayerPrefs.SetInt("user_highscore_1", 1200);

        PlayerPrefs.SetString("user_name_2", "MAT");
        PlayerPrefs.SetInt("user_highscore_2", 5210);

        PlayerPrefs.SetString("user_name_3", "SUE");
        PlayerPrefs.SetInt("user_highscore_3", 1550);

        PlayerPrefs.SetString("user_name_4", "RDG");
        PlayerPrefs.SetInt("user_highscore_4", 26900);

        PlayerPrefs.SetString("user_name_5", "DAN");
        PlayerPrefs.SetInt("user_highscore_5", 5040);

        PlayerPrefs.SetString("user_name_6", "JON");
        PlayerPrefs.SetInt("user_highscore_6", 2000);

        PlayerPrefs.SetString("user_name_7", "AMY");
        PlayerPrefs.SetInt("user_highscore_7", 1860);

        PlayerPrefs.SetString("user_name_8", "TIM");
        PlayerPrefs.SetInt("user_highscore_8", 550);

        PlayerPrefs.SetString("user_name_9", "FUK");
        PlayerPrefs.SetInt("user_highscore_9", 5660);

        PlayerPrefs.SetString("user_name_10", "GOD");
        PlayerPrefs.SetInt("user_highscore_10", 17500);

        PlayerPrefs.SetString("user_name_11", "DOG");
        PlayerPrefs.SetInt("user_highscore_11", 10532);

        PlayerPrefs.SetString("user_name_12", "JAC");
        PlayerPrefs.SetInt("user_highscore_12", 12005);

        PlayerPrefs.SetString("user_name_13", "ADA");
        PlayerPrefs.SetInt("user_highscore_13", 6048);

        PlayerPrefs.SetString("user_name_14", "LOK");
        PlayerPrefs.SetInt("user_highscore_14", 4860);

        PlayerPrefs.SetString("user_name_15", "BOB");
        PlayerPrefs.SetInt("user_highscore_15", 6347);

        PlayerPrefs.SetString("user_name_16", "RIP");
        PlayerPrefs.SetInt("user_highscore_16", 24684);

        PlayerPrefs.SetString("user_name_17", "NIG");
        PlayerPrefs.SetInt("user_highscore_17", 26010);

        PlayerPrefs.SetString("user_name_18", "FRA");
        PlayerPrefs.SetInt("user_highscore_18", 19807);

        PlayerPrefs.SetString("user_name_19", "SPA");
        PlayerPrefs.SetInt("user_highscore_19", 17560);
    }
}
