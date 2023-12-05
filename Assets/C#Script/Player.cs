using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Transform tr;
    Vector2 mousePosition;
    CapsuleCollider2D coll;
    Animator anim;
    public GameManager GM;
    public AudioManger AM;
    public WsClient Ws;

    public float speed;
    public float PlayerX;
    public float PlayerY;
    public float deltaPlayerY;
    public bool isDead = false;
    public bool isPlayArea = true;
    public float LimitY;
    public bool isLimited = false;
    public bool isLimited_Up = false;
    public bool isLimited_Down = false;
    string[] StateID= {"Normal", "+5kg", "-5kg", "Invincibility", "Inverse"};
    public string curState;
    public float k;

    public float time_state_cur;
    public float time_state_setting;

    void Start()
    {
        tr = GetComponent<Transform>();
        coll = GetComponent<CapsuleCollider2D>();
        anim = GetComponent<Animator>();
        curState = StateID[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead == false)
        {
            /*
            Move();
            Debug.Log(PlayerY);
            mousePosition = new Vector2(0, PlayerY);
            if (isPlayArea == true)
            {
                if (GM.isInverse == false)
                {
                    tr.position = Vector2.MoveTowards(tr.position, new Vector2(PlayerX, mousePosition.y), speed * Time.deltaTime);
                }
                else
                {
                    tr.position = Vector2.MoveTowards(tr.position, new Vector2(PlayerX, -mousePosition.y), speed * Time.deltaTime);
                }
            }
            */
            //deltaPlayerY += Input.GetAxis("Mouse ScrollWheel") * k;
            PlayerY = GM.PlayerYCalculate(GM.Robotpos, k);
            //PlayerY = Input.mousePosition.y;
            //PlayerY = GM.Robotpos * k;
            if (Mathf.Abs(PlayerY) >= LimitY)
            {
                if (PlayerY > 0)
                {
                    PlayerY = LimitY;                
                }
                else
                {
                    PlayerY = -LimitY;
                }
            }

            if (GM.isInverse == false)
            {
                tr.position = Vector2.MoveTowards(tr.position, new Vector2(PlayerX, PlayerY), speed * Time.deltaTime);
            }
            else
            {
                tr.position = Vector2.MoveTowards(tr.position, new Vector2(PlayerX, -PlayerY), speed * Time.deltaTime);
            }
        }

        if (curState != "Normal")
        {
            time_state_cur += Time.deltaTime;
        }

        if (time_state_cur >= time_state_setting)
        {
            if (curState == "+5kg" || curState == "-5kg")
            {
                //GM.InitializeWeight();
                StateInitialization();
            }
            else if(curState == "Invincibility")
            {
                Invincibility_Off();
            }
            else if(curState == "Inverse")
            {
                GM.CameraInverse();
            }
            time_state_cur = 0;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if ((collision.gameObject.tag == "Obstacle" && curState != "公利") || collision.gameObject.tag == "Finish")
        {
            isDead = true;
            //AM.AudioPlay("Die");
            //Ws.SendEnd();
            if (GM.GameClear == true)
            {
                AM.AudioPlay("Finish");
            }
            else
            {
                AM.AudioPlay("Die");
            }
            time_state_cur = 0;
            if (curState == "Inverse")
            {
                GM.CameraInverse();
            }
            StateInitialization();
        }
        else if (collision.gameObject.tag == "Item")
        {
            Item item = collision.gameObject.GetComponentInChildren<Item>();
            ItemGet(StateID[item.itemID]);
        }
        else if (collision.gameObject.tag == "Item_hard")
        {
            Item_hard item = collision.gameObject.GetComponentInChildren<Item_hard>();
            ItemGet(StateID[item.itemID]);
        }
        else if (collision.gameObject.tag == "Item_veryhard")
        {
            Item_veryhard item = collision.gameObject.GetComponentInChildren<Item_veryhard>();
            ItemGet(StateID[item.itemID]);
        }
    }

    public void Initialization()
    {
        tr.position = new Vector2(PlayerX, 0);
    }
    /*
    void Move()
    {
        //PlayerY += Input.GetAxis("Mouse ScrollWheel") * k;
        //PlayerY = GM.PlayerYCalculate(Ws.RobotAngle, k, GM.RobotArmLength);
        PlayerY = Input.mousePosition.y;
        if (Mathf.Abs(PlayerY) > LimitY && isLimited == false)
        {
            isPlayArea = false;
            isLimited = true;
            if (tr.position.y > LimitY)
            {
                isLimited_Up = true;
            }
            else if (-tr.position.y < -LimitY)
            {
                isLimited_Down = true;
            }
            
        }
        else if (Mathf.Abs(PlayerY) < LimitY)
        {
            isPlayArea = true;
            isLimited = false;
            isLimited_Up = false;
            isLimited_Down = false;
        }
    }
    */
    void Invincibility_On()
    {
        //Invoke("Invincibility_Off", 5.0f);
        coll.enabled = false;
        anim.SetBool("isNormal", false);
    }

    public void Invincibility_Off()
    {
        StateInitialization();
        //Debug.Log("公利 辆丰");
        coll.enabled = true;
        anim.SetBool("isNormal", true);
    }

    public void StateInitialization()
    {
        curState = StateID[0];
    }

    void ItemGet(string ID)
    {
        switch (ID)
        {
            case "+5kg":
                Debug.Log("刘樊");
                GM.ChangeWeight_Plus();
                break;
            case "-5kg":
                Debug.Log("皑樊");
                GM.ChangeWeight_Minus();
                break;
            case "Invincibility":
                Debug.Log("公利");
                Invincibility_On();
                break;
            case "Inverse":
                Debug.Log("Inverse");
                GM.CameraInverse();
                break;
        }
        curState = ID;
        AM.AudioPlay(curState);
    }
}
