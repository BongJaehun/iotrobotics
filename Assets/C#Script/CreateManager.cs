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
    public Player player;
    public GameManager GM;

    public int CreateStart;
    public int CreateNum;
    public bool isItem = false;
    public float Force;
    public float interval_Obstacle;
    public float IntervalSetting_Obstacle;
    public float interval_Item;
    public float IntervalSetting_Item;
    public float IntervalSetting_Obstacle_Original;
    public bool[] ItemArea = {true, true, true, true, true, true, true, true, true, true};
    public int ItemCreatePos;
    public bool isFinishCreate;
    public bool isintervalTimeResetting;
    public bool isHarderActed_First;
    public bool isHarderActed_Second;

    void Start()
    {
        IntervalSetting_Obstacle_Original = IntervalSetting_Obstacle;
    }

    // Update is called once per frame
    void Update()
    {
        Interval();
        Create();
        CreateFinish();
        harder();
    }

    public void Create()
    {
        if (IntervalSetting_Obstacle > interval_Obstacle  || GM.GameClear==true || GM.Playtime> GM.FinishTime-2 || isintervalTimeResetting==true)
        {
            return;
        }
        
        if (player.isDead == false)
        {
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
                Rigidbody2D rigid = obstacle.GetComponentInChildren<Rigidbody2D>();
                rigid.AddForce(Vector2.left * Force, ForceMode2D.Impulse);
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

        if (GM.Playtime < 50)
        {
            GameObject item = Instantiate(Item0, CreateArea[ItemCreatePos].transform.position, transform.rotation);
            Rigidbody2D rigid = item.GetComponentInChildren<Rigidbody2D>();
            rigid.AddForce(Vector2.left * Force, ForceMode2D.Impulse);
        }
        else if(GM.Playtime<95)
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
        if (GM.Playtime >= 50 && isHarderActed_First == false)
        {
            IntervalSetting_Obstacle -= 0.5f;
            isintervalTimeResetting = true;
            isHarderActed_First = true;
        }
        else if (GM.Playtime >= 95 && isHarderActed_Second == false)
        {
            IntervalSetting_Obstacle -= 0.5f;
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
