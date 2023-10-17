using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Transform tr;
    Vector2 mousePosition;
    BoxCollider2D box;
    public GameManager GM;

    public float speed;
    public float PlayerX;
    public bool isDead = false;
    public bool isPlayArea = true;
    public float LimitY;
    public bool isLimited = false;
    string[] StateID= {"Normal", "+5kg", "-5kg", "Invincibility", "Inverse"};
    public string curState;

    public float time_state_cur;
    public float time_state_setting;

    void Start()
    {
        tr = GetComponent<Transform>();
        box = GetComponent<BoxCollider2D>();
        curState = StateID[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead == false)
        {
            //mousePosition = new Vector2(0, GM.PlayerY);
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            MoveLimit(mousePosition.y);
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
        }

        if (curState != "Normal")
        {
            time_state_cur += Time.deltaTime;
        }

        if (time_state_cur >= time_state_setting)
        {
            if (curState == "+5kg" || curState == "-5kg")
            {
                GM.InitializeWeight();
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

    void MoveLimit(float mouse)
    {
        if (Mathf.Abs(tr.position.y) > LimitY && isLimited == false)
        {
            isPlayArea = false;
            isLimited = true;
        }
        else if (Mathf.Abs(mouse) < LimitY)
        {
            isPlayArea = true;
            isLimited = false;
        }
    }

    void Invincibility_On()
    {
        //Invoke("Invincibility_Off", 5.0f);
        box.enabled = false;
    }

    void Invincibility_Off()
    {
        StateInitialization();
        Debug.Log("公利 辆丰");
        box.enabled = true;
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
    }
}
