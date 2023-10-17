using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text text_PlayTime;
    public Text text_PlayerState;

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
        text_PlayTime.text = (Mathf.FloorToInt(GM.Playtime * 100f) / 100f).ToString();
        if (player.curState != "Normal")
        {
            text_PlayerState.text = player.curState + "(" + Mathf.FloorToInt(player.time_state_cur * 100f) / 100f + " / " + player.time_state_setting + ")";
        }
        else
        {
            text_PlayerState.text = player.curState;
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
            }
        }
    }

    public void ReStart()
    {
        GM.GameStart();
        GameOverPack.SetActive(false);
        GameClearPack.SetActive(false);
    }

    public void ExitGame()
    {
        GM.Exit();
    }
}
