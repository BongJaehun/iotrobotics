using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Player player;
    public GameObject camera0;
    public CreateManager CM;
    public GameObject DestroyerAll;
    public GameObject AudioManager;

    public float curWeight;
    public float originalWeight;
    public float deltaWeight;
    public bool isInverse=false;
    public float Playtime = 0;
    public int FinishTime;
    public bool GameOver;
    public bool GameClear;
    public bool isWeightReset;

    public float PlayerY;

    void Start()
    {
        curWeight = originalWeight;
    }

    // Update is called once per frame
    void Update()
    {
        if (player.isDead)
        {
            Dead();
        }
        else
        {
            Timer();
        }
    }

    void Dead()
    {
        DestroyerAll.SetActive(true);
        AudioManager.SetActive(false);
        player.Initialization();
        GameOver = true;
        Playtime = 0;
        if (isWeightReset==false)
        {
            InitializeWeight();
            isWeightReset = true;
        }
    }

    public void ChangeWeight_Plus()
    {
        curWeight += deltaWeight;
        Debug.Log(curWeight);
        //Invoke("InitializeWeight", 3.0f);
    }
    public void ChangeWeight_Minus()
    {
        curWeight -= deltaWeight;
        Debug.Log(curWeight);
        //Invoke("InitializeWeight", 3.0f);
    }

    public void InitializeWeight()
    {
        curWeight = originalWeight;
        Debug.Log(curWeight);
        player.StateInitialization();
    }

    public void CameraInverse()
    {
        if (isInverse == true)
        {
            isInverse = false;
            camera0.transform.rotation = Quaternion.Euler(0, 0, 0);
            //player.StateInitialization();
        }
        else
        {
            isInverse = true;
            camera0.transform.rotation = Quaternion.Euler(0, 0, 180);
            //Invoke("CameraInverse", 5.0f);
        }
    }

    public void GameStart()
    {
        DestroyerAll.SetActive(false);
        AudioManager.SetActive(true);
        player.isDead = false;
        GameOver = false;
        GameClear = false;
        isWeightReset = false;
        CM.isFinishCreate = false;
        CM.isHarderActed_First = false;
        CM.isHarderActed_Second = false;
        CM.IntervalSetting_Obstacle = CM.IntervalSetting_Obstacle_Original;
    }

    public void Timer()
    {
        if (GameClear != true)
        {
            Playtime += Time.deltaTime;
        }
    }

    public void Exit()
    {
        Application.Quit();
    }

}
