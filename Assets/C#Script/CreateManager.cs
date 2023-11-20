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


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Interval();
        Create();
        CreateFinish();
        harder();

        //�����ؾ� �ϴ� input: player�� ��ġ�� �߷�(player object�� ������ �ͼ� �ǽð����� Ȯ��)
        //ȣ���ؾ� �ϴ� output: ��ֹ�, item, finish ����
        //public static T Instantiate<T>(T original, Vector2 position, Quaternion rotation) where T : Object;
        //Vector2 position::float x, float y
        // float y: float ���� ��ֹ�.position.y, float player.gravity, float delta
    }

    public void IntervalSetting_Obstacle_Reset()
    {
        IntervalSetting_Obstacle = Random.Range(IntervalSetting_Obstacle_Down, IntervalSetting_Obstacle_Upper) /10;
    }

    public void Create()
    {
        if (IntervalSetting_Obstacle > interval_Obstacle  || GM.GameClear==true || GM.Playtime> GM.FinishTime-2 || isintervalTimeResetting==true)
        {
            return;
        }

        IntervalSetting_Obstacle_Reset();

        if (player.isDead == false)
        {
            CSM.CalculationStart();
            /*
            CreateStart = Random.Range(0, 10);
            CreateNum = Random.Range(1, 11 - CreateStart);
            if (CreateNum <= 3 || (CreateStart==0 && CreateNum==10))
            {
                Create();
                return;
            }
            for (int i = 0; i < CreateNum; i++)
            {
                GameObject obstacle = Instantiate(Obstacle, CreateArea[CreateStart + i].transform.position, transform.rotation);
                ItemArea[CreateStart+i] = false;

                // ������ ������ ��ֹ��� ��ġ ������ �����ϴ� ���� ����

                Rigidbody2D rigid = obstacle.GetComponentInChildren<Rigidbody2D>();
                rigid.AddForce(Vector2.left * Force, ForceMode2D.Impulse);
            }
            */

            for(int i = 0; i < 10; i++)
            {
                if(CreateArea[i].transform.position.y < CSM.downPosition || CreateArea[i].transform.position.y > CSM.upperPosition)
                {
                    ItemArea[i] = false;
                    CreateNum++;
                }
                else
                {
                    if (RandomObstacleAreaUp > i)
                    {
                        RandomObstacleAreaUp = i;
                    }

                    if (RandomObstacleAreaDown < i)
                    {
                        RandomObstacleAreaDown = i;
                    }
                }
            }


            if (CreateNum < 8)
            {
                NotCreateStart = Random.Range(RandomObstacleAreaUp, RandomObstacleAreaDown);
                CSM.lastPassageLowerPos = CreateArea[NotCreateStart + 1].transform.position.y - 0.5f;
                for(int i = 0; i < 10; i++)
                {
                    if(ItemArea[i]==true && (i!= NotCreateStart || i != NotCreateStart + 1))
                    {
                        ItemArea[i] = false;
                    }
                }
            }

            for (int i = 0; i < CreateNum; i++)
            {
                if (ItemArea[i] == false)
                {
                    GameObject obstacle = Instantiate(Obstacle, CreateArea[i].transform.position, transform.rotation);
                    Rigidbody2D rigid = obstacle.GetComponentInChildren<Rigidbody2D>();
                    rigid.AddForce(Vector2.left * Force, ForceMode2D.Impulse);
                }
            }

            interval_Obstacle = 0;

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
        if (GM.Playtime > GM.FinishTime)
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
        }
        else if (GM.Playtime >= veryhardTime && isHarderActed_Second == false)
        {
            IntervalSetting_Obstacle_Upper -= IntervalSetting_Obstacle_Upper_Delta;
            IntervalSetting_Obstacle_Down -= IntervalSetting_Obstacle_Down_Delta;
            isintervalTimeResetting = true;
            isHarderActed_Second = true;
        }
        Invoke("isintervalTimeResetting_Reset", 2.0f);
    }

    void isintervalTimeResetting_Reset()
    {
        isintervalTimeResetting = false;
    }
}
