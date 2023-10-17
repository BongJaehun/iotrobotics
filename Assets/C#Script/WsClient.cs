using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;

public class WsClient : MonoBehaviour
{
    WebSocket ws;

    public GameManager GM;
    public Player player;

    public string pastState;

    public string WsData;

    private void Start()
    {
        pastState = player.curState;
        ws = new WebSocket("ws://localhost:7777");
        //서버에서 설정한 포트를 넣어줍니다.


        ws.Connect();
        //연결합니다.
        ws.OnMessage += Call;
        //이벤트 추가
        /*
         위랑 같은 것
        ws.OnMessage += (sender, e) =>
        {
            Debug.Log("주소 :  "+((WebSocket)sender).Url+", 데이터 : "+e.Data);
        };
         */
    }

    void Call(object sender, MessageEventArgs e)
    {
        Debug.Log("주소 :  " + ((WebSocket)sender).Url + ", 데이터 : " + e.Data);
        WsData = e.Data;
        GM.PlayerY = float.Parse(WsData);
    }
    void Update()
    {
        if (ws == null)
        {
            return;
        }
        if (pastState!=player.curState)
        {
            //ws.Send(player.curState);
            Debug.Log("send");
            pastState = player.curState;

        }
    }
}
