using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateManager : MonoBehaviour
{
    public GameObject Obstacle;
    public GameObject Item0;
    public GameObject Item0_hard;
    public GameObject Item0_veryhard;
    public GameObject[] CreateArea;
    public GameObject Finish;
    // public gameobject Player;
    public Player player;
    public GameManager GM;
    public CreateSubManager CSM;

    public int NotCreateStart;
    public int CreateNum;
    public bool isItem = false;
    public float Force;
    public float interval_Obstacle;
    public float IntervalSetting_Obstacle;
    public float interval_Item;
    public float IntervalSetting_Item;
    public float IntervalSetting_Obstacle_Upper;
    public float IntervalSetting_Obstacle_Down;
    public float IntervalSetting_Obstacle_Upper_Delta;
    public float IntervalSetting_Obstacle_Down_Delta;
    public int RandomObstacleAreaUp;
    public int RandomObstacleAreaDown;
    public float hardTime;
    public float veryhardTime;
    public float ProhibitedAreaDown;
    public bool[] ItemArea = {true, true, true, true, true, true, true, true, true, true};
    public int ItemCreatePos;
    public bool isFinishCreate;
    public bool isintervalTimeResetting;
    public bool isHarderActed_First;
    public bool isHarderActed_Second;
    public int BoundaryUp;
    public int BoundaryDown;
    int createNum_limit = 7;

    void Start()
    {
        CSM.lastPassageLowerPos = player.transform.position.y - 0.5f;
        IntervalSetting_Obstacle_Reset();
    }

    // Update is called once per frame
    void Update()
    {
        Interval();
        Create();
        CreateFinish();
        harder();
    }

    public void IntervalSetting_Obstacle_Reset()
    {
        IntervalSetting_Obstacle = Random.Range(IntervalSetting_Obstacle_Down, IntervalSetting_Obstacle_Upper) /10;
    }

    public void Create()
    {
        if (IntervalSetting_Obstacle > interval_Obstacle  || (GM.GameClear==true && GM.ishardlevel==false&& GM.Playtime > GM.FinishTime - 2) || isintervalTimeResetting==true || GM.GameOver==true || GM.Playtime <= 1.5f)
        {
            return;
        }

        IntervalSetting_Obstacle_Reset();

        if (player.isDead == false)
        {
            /*
            CSM.CalculationStart();
            BoundaryUp = ObstacleAreaSetting(CSM.upperPosition);
            BoundaryDown = ObstacleAreaSetting(CSM.downPosition);

            for (int i=0; i < 10; i++)
            {
                if(i < BoundaryUp || i > BoundaryDown)
                {
                    ItemArea[i] = false;
                    CreateNum++;
                }
                else
                {
                    if (RandomObstacleAreaUp > i)
                    {
                        RandomObstacleAreaUp = i;
                        CSM.lastPassageUpperPos = CreateArea[i].transform.position.y + 0.5f;
                    }

                    if (RandomObstacleAreaDown < i)
                    {
                        RandomObstacleAreaDown = i;
                        CSM.lastPassageLowerPos = CreateArea[i].transform.position.y - 0.5f;
                    }
                }
            }

            if (CreateNum < createNum_limit)
            {
                NotCreateStart = Random.Range(BoundaryUp, BoundaryDown);
                CSM.lastPassageUpperPos = CreateArea[NotCreateStart].transform.position.y + 0.5f;
                CSM.lastPassageLowerPos = CreateArea[NotCreateStart + 1].transform.position.y - 0.5f;
                for(int i = 0; i < 10; i++)
                {
                    if (GM.Playtime > veryhardTime)
                    {
                        if (i == NotCreateStart || i == NotCreateStart + 1)
                        {
                            ItemArea[i] = true;
                        }
                        else
                        {
                            ItemArea[i] = false;
                            CreateNum++;
                        }

                        if (NotCreateStart == 9)
                        {
                            ItemArea[NotCreateStart - 1] = true;
                            CreateNum--;
                            Debug.Log("Max");
                        }
                    }
                    else
                    {
                        if (i == NotCreateStart || i == NotCreateStart + 1 || i == NotCreateStart - 1)
                        {
                            ItemArea[i] = true;
                        }
                        else
                        {
                            ItemArea[i] = false;
                            CreateNum++;
                        }

                        if (NotCreateStart == 9)
                        {
                            ItemArea[NotCreateStart - 2] = true;
                            CreateNum--;
                            Debug.Log("Max");
                        }
                    }
                }
            }
            */
            int Number = Random.Range(1, 9);
            if (GM.Level == 3 && isHarderActed_Second == true)
            {
                for(int i=0; i<10; i++)
                {
                    if(i == Number-1 || i == Number)
                    {
                        ItemArea[i] = true;
                    }
                    else
                    {
                        ItemArea[i] = false;
                    }
                }
            }
            else
            {
                for (int i = 0; i < 10; i++)
                {
                    if (i == Number - 1 || i == Number || i == Number + 1)
                    {
                        ItemArea[i] = true;
                    }
                    else
                    {
                        ItemArea[i] = false;
                    }
                }
            }
            for (int i = 0; i < 10; i++)
            {
                if (ItemArea[i] == false)
                {
                    GameObject obstacle = Instantiate(Obstacle, CreateArea[i].transform.position, transform.rotation);
                    Rigidbody2D rigid = obstacle.GetComponentInChildren<Rigidbody2D>();
                    rigid.AddForce(Vector2.left * Force, ForceMode2D.Impulse);
                }
            }
            Debug.Log("CreateNum: "+CreateNum);
            CreateNum = 0;
            interval_Obstacle = 0;
            RandomObstacleAreaUp = 0;
            RandomObstacleAreaDown = 0;
            if (IntervalSetting_Item > interval_Item  || player.curState!= "Normal")
            {
                for (int i = 0; i < 10; i++)
                {
                    ItemArea[i] = true;
                }
                return;
            }
            ItemCreate();
        }
    }

    int ObstacleAreaSetting(float y)
    {
        int index = 0;
        for(int i = 0; i < 10; i++)
        {
            if(CreateArea[i].transform.position.y-0.5f<=y && CreateArea[i].transform.position.y + 0.5f > y)
            {
                index = i;
            }
        }
        return index;
    }

    void Interval()
    {
        interval_Obstacle += Time.deltaTime;
        interval_Item += Time.deltaTime;
    }

    void ItemCreate()
    {
        ItemCreatePos = Random.Range(0, 10);
        if (ItemArea[ItemCreatePos] == false)
        {
            ItemCreate();
            return;
        }

        if (GM.Playtime < hardTime)
        {
            GameObject item = Instantiate(Item0, CreateArea[ItemCreatePos].transform.position, transform.rotation);
            Rigidbody2D rigid = item.GetComponentInChildren<Rigidbody2D>();
            rigid.AddForce(Vector2.left * Force, ForceMode2D.Impulse);
        }
        else if(GM.Playtime < veryhardTime)
        {
            Debug.Log("Hard");
            GameObject item_hard = Instantiate(Item0_hard, CreateArea[ItemCreatePos].transform.position, transform.rotation);
            Rigidbody2D rigid_hard = item_hard.GetComponentInChildren<Rigidbody2D>();
            rigid_hard.AddForce(Vector2.left * Force, ForceMode2D.Impulse);
        }
        else
        {
            Debug.Log("Hard");
            GameObject item_veryhard = Instantiate(Item0_veryhard, CreateArea[ItemCreatePos].transform.position, transform.rotation);
            Rigidbody2D rigid_veryhard = item_veryhard.GetComponentInChildren<Rigidbody2D>();
            rigid_veryhard.AddForce(Vector2.left * Force, ForceMode2D.Impulse);
        }
        
        interval_Item = 0;
        for(int i = 0; i < 10; i++)
        {
            ItemArea[i] = true;
        }
    }

    public void CreateFinish()
    {
        if (GM.Playtime > GM.FinishTime && GM.ishardlevel==false)
        {
            if (isFinishCreate == false)
            {
                GM.GameClear = true;
                isFinishCreate = true;
                GameObject finish = Instantiate(Finish, CreateArea[0].transform.position, transform.rotation);
                Rigidbody2D rigid = finish.GetComponentInChildren<Rigidbody2D>();
                rigid.AddForce(Vector2.left * Force, ForceMode2D.Impulse);
            }
        }
    }

    public void harder()
    {
        if (GM.Playtime >= hardTime && isHarderActed_First == false)
        {
            IntervalSetting_Obstacle_Upper -= IntervalSetting_Obstacle_Upper_Delta;
            IntervalSetting_Obstacle_Down -= IntervalSetting_Obstacle_Down_Delta;
            isintervalTimeResetting = true;
            isHarderActed_First = true;
            //createNum_limit ++;
        }
        else if (GM.Playtime >= veryhardTime && isHarderActed_Second == false)
        {
            IntervalSetting_Obstacle_Upper --;
            //IntervalSetting_Obstacle_Down --;
            isintervalTimeResetting = true;
            isHarderActed_Second = true;
            createNum_limit++;
        }
        Invoke("isintervalTimeResetting_Reset", 2.0f);
    }

    void isintervalTimeResetting_Reset()
    {
        isintervalTimeResetting = false;
    }
}
