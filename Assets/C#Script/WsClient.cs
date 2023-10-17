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
        //�������� ������ ��Ʈ�� �־��ݴϴ�.


        ws.Connect();
        //�����մϴ�.
        ws.OnMessage += Call;
        //�̺�Ʈ �߰�
        /*
         ���� ���� ��
        ws.OnMessage += (sender, e) =>
        {
            Debug.Log("�ּ� :  "+((WebSocket)sender).Url+", ������ : "+e.Data);
        };
         */
    }

    void Call(object sender, MessageEventArgs e)
    {
        Debug.Log("�ּ� :  " + ((WebSocket)sender).Url + ", ������ : " + e.Data);
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
