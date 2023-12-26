using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using WebSocketSharp;

public class WsClient : MonoBehaviour
{
    WebSocket ws;

    public GameManager GM;
    public Player player;

    public float pastWeight;

    public int WsData;

    public float time;

    void Start()
    {
        pastWeight = GM.curWeight;
        //ws = new WebSocket("ws://localhost:7777");
        //ws = new WebSocket("ws://arsvivendi.io/ycs1008");
        ws = new WebSocket("ws://106.244.237.103:60006");
        //서버에서 설정한 포트를 넣어줍니다.

        ws.OnOpen += (sender, e) =>
        {
            Debug.Log("Open");
        };

        ws.OnMessage += Call;
        ws.Connect();
        //연결합니다.

        
        //이벤트 추가
        
        //위랑 같은 것
         
    }

    void Call(object sender, MessageEventArgs e)
    {
        Debug.Log("주소 :  " + ((WebSocket)sender).Url + ", 데이터 : " + e.Data);

        if (e.Data[0] == 'p')
        {
            try 
            { 
                WsData = Int32.Parse(e.Data.Substring(1, e.Data.Length-1));
                //Debug.Log("Parse Sucess "+WsData);
                //Debug.Log("PlayerY " + GM.PlayerYCalculate(WsData, 1.0f));
            }
            catch (FormatException)
            {
                Debug.Log($"Unable to parse '{e.Data}'");
            }
        }
        else
        {
            Debug.Log("Data not start p");
        }
        GM.Robotpos = WsData;
    }
    void Update()
    {
        if (ws == null)
        {
            Debug.Log("Not");
            return;
        }
        Timer();
        if (player.isDead != true)
        {

            if (time >= 1 / 60)
            {
                ws.Send("t" + (GM.curWeight * 1000).ToString());
                pastWeight = GM.curWeight;
                time = 0;
            }
        }
    }

    public void SendStart()
    {
        ws.Send("start");
    }

    public void SendEnd()
    {
        ws.Send("stop");
    }

    public void Timer()
    {
        if (player.isDead != true)
        {
            time += Time.deltaTime;
        }
    }
}
