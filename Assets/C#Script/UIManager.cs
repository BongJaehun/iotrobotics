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
        switch (player.curState)
        {
            case "Normal":
                text_PlayerState.text = "";
                break;
            case "+5kg":
                text_PlayerState.text = "무게 증가" + "(" + Mathf.FloorToInt(player.time_state_cur) + " / " + player.time_state_setting + ")";
                break;
            case "-5kg":
                text_PlayerState.text = "무게 감소" + "(" + Mathf.FloorToInt(player.time_state_cur) + " / " + player.time_state_setting + ")";
                break;
            case "Invincibility":
                text_PlayerState.text = "무적 타임" + "(" + Mathf.FloorToInt(player.time_state_cur) + " / " + player.time_state_setting + ")";
                break;
            case "Inverse":
                text_PlayerState.text = "인버스" + "(" + Mathf.FloorToInt(player.time_state_cur) + " / " + player.time_state_setting + ")";
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
