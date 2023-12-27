using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text text_PlayTime;
    public Text text_PlayerState;
    public Text text_PlayTime_GameOver;
    public Text text_Weight;

    public GameObject GameOverPack;
    public GameObject GameClearPack;

    public Player player;
    public GameManager GM;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        text_PlayTime.text = (Mathf.FloorToInt(GM.Playtime * 10f) / 10f).ToString();
        text_Weight.text = ((Mathf.FloorToInt(GM.curWeight * 100f) / 100f) * 100).ToString() + "%";
        switch (player.curState)
        {
            case "Normal":
                text_PlayerState.text = "";
                break;
            case "+5kg":
                text_PlayerState.text = "Weight Increase";
                break;
            case "-5kg":
                text_PlayerState.text = "Weight Decrease";
                break;
            case "Invincibility":
                text_PlayerState.text = "Super Time" + "(" + Mathf.FloorToInt(player.time_state_cur) + " / " + player.time_state_setting + ")";
                break;
            case "Inverse":
                text_PlayerState.text = "Inverse" + "(" + Mathf.FloorToInt(player.time_state_cur) + " / " + player.time_state_setting + ")";
                break;
        }
        ReStartButton_Show();
    }

    void ReStartButton_Show()
    {
        if (GM.GameOver)
        {
            if (GM.GameClear)
            {
                GameClearPack.SetActive(true);
            }
            else
            {
                GameOverPack.SetActive(true);
                text_PlayTime_GameOver.text = "PlayTime: " + (Mathf.FloorToInt(GM.Playtime * 100f) / 100f).ToString();
            }
        }
    }

    public void ReStart(int levelnum)
    {
        GM.GameStart(levelnum);
        GameOverPack.SetActive(false);
        GameClearPack.SetActive(false);
    }

    public void ExitGame()
    {
        GM.Exit();
    }
}
