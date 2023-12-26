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
    public WsClient Ws;
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
    public float MaxWeightLimit;
    public bool GameOver;
    public bool GameClear;
    public bool isWeightReset;
    public float RobotArmLength;
    public float RobotAngle;
    public float Robotpos;

    public bool isStopSend;
    public bool ishardlevel;
    public bool isOpen;

    public float[] weight_level;
    public float[] weight_Bytime_level;
    public float[] maxweight_level;
    float[] torque_level = { 0, 0, 0, 0 };

    void Start()
    {
        curWeight = 0;
        isOpen = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (player.isDead)
        {
            if (isOpen == true)
            {
                return;
            }
            Dead();
        }
        else
        {
            Timer();
            WeightIncreaseByTime();
        }
    }

    public float PlayerYCalculate(float robotpos, float k)
    {
        return k * ((robotpos / 10.0f - 50) * 4.6f / 35);
    }

    public void WeightIncreaseByTime()
    {
        if (curWeight >= 1.0f)
        {
            curWeight = 1.0f;
            return;
        }
        curWeight += MaxWeightByTime / (WeightIncreaseMaxTime *300);
    }

    void Dead()
    {
        DestroyerAll.SetActive(true);
        OSTManager.SetActive(false);
        player.Initialization();
        player.StateInitialization();
        player.Invincibility_Off();
        GameOver = true;
        //Playtime = 0;
        if (isStopSend == false)
        {
            Ws.SendEnd();
            isStopSend = true;
        }
        if (isWeightReset==false)
        {
            InitializeWeight();
            isWeightReset = true;
        }
    }

    public void ChangeWeight_Plus()
    {
        curWeight += curWeight * deltaWeightPercent / 100;
        if (curWeight > MaxWeightLimit)
        {
            curWeight = MaxWeightLimit;
        }
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
        curWeight = 0;
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
        Ws.SendStart();
        GameOver = false;
        GameClear = false;
        isWeightReset = false;
        originalWeight = weight_level[level];
        CSM.maxExpectedTorque = torque_level[level];
        MaxWeightByTime = weight_Bytime_level[level];
        MaxWeightLimit = maxweight_level[level];
        curWeight = originalWeight;
        CM.isFinishCreate = false;
        CM.isHarderActed_First = false;
        CM.isHarderActed_Second = false;
        isStopSend = false;
        CM.IntervalSetting_Obstacle_Reset();
        player.PlayerY = 0;
        Playtime = 0;
        isOpen = false;
        if (level == 3)
        {
            //Debug.Log("ishard");
            ishardlevel = true;
        }
        else
        {
            ishardlevel = false;
        }
    }

    public void Timer()
    {
        if (GameClear != true && GameOver != true)
        {
            Playtime += Time.deltaTime;
            if (LastPlaytime + 1 <= Playtime)
            {
                LastPlaytime = ((int)(Playtime));
                //WeightIncreaseByTime(LastPlaytime);
            }
        }
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void GotoMainMenu()
    {
        SceneManager.LoadScene("Opening");
    }

}
