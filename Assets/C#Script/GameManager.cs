using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Player player;
    public GameObject camera0;
    public CreateManager CM;
    public CreateSubManager CSM;
    public GameObject DestroyerAll;
    public GameObject OSTManager;
    public GameObject LevelPage;

    public float curWeight;
    public float originalWeight;
    public float deltaWeightPercent;
    public bool isInverse=false;
    public int LastPlaytime = 0;
    public float Playtime = 0;
    public int FinishTime;
    public int WeightIncreaseMaxTime;
    public float MaxWeightByTime;
    public bool GameOver;
    public bool GameClear;
    public bool isWeightReset;
    public float RobotArmLength;
    
    float[] weight_level = { 0, 10, 20, 30 };
    float[] torque_level = { 0, 0, 0, 0 };

    void Start()
    {
        
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

    public float PlayerYCalculate(float RobotAngle, float k)
    {
        return k * RobotArmLength * RobotAngle * Mathf.PI / 180;
    }

    public void WeightIncreaseByTime(int time)
    {
        if (WeightIncreaseMaxTime < time)
        {
            return;
        }
        curWeight += MaxWeightByTime / WeightIncreaseMaxTime;
    }

    void Dead()
    {
        DestroyerAll.SetActive(true);
        OSTManager.SetActive(false);
        player.Initialization();
        player.StateInitialization();
        player.Invincibility_Off();
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
        curWeight += curWeight * deltaWeightPercent / 100;
        Debug.Log(curWeight);
        //Invoke("InitializeWeight", 3.0f);
    }
    public void ChangeWeight_Minus()
    {
        curWeight -= curWeight * deltaWeightPercent / 100;
        Debug.Log(curWeight);
        //Invoke("InitializeWeight", 3.0f);
    }

    public void InitializeWeight()
    {
        curWeight = originalWeight;
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

    public void LevelSetting()
    {
        LevelPage.SetActive(true);
    }

    public void GameStart(int level)
    {
        DestroyerAll.SetActive(false);
        LevelPage.SetActive(false);
        OSTManager.SetActive(true);
        player.isDead = false;
        GameOver = false;
        GameClear = false;
        isWeightReset = false;
        originalWeight = weight_level[level];
        CSM.maxExpectedTorque = torque_level[level];
        curWeight = originalWeight;
        CM.isFinishCreate = false;
        CM.isHarderActed_First = false;
        CM.isHarderActed_Second = false;
        CM.IntervalSetting_Obstacle_Reset();
        player.PlayerY = 0;
    }

    public void Timer()
    {
        if (GameClear != true)
        {
            Playtime += Time.deltaTime;
            if (LastPlaytime + 1 <= Playtime)
            {
                LastPlaytime = ((int)(Playtime));
                WeightIncreaseByTime(LastPlaytime);
            }
        }
    }

    public void Exit()
    {
        Application.Quit();
    }

}
