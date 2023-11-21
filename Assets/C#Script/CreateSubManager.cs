using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateSubManager : MonoBehaviour
{
    public GameManager GM;
    public CreateManager CM;
    
    public float lastPassageLowerPos; //������ ����� ���� ��ġ
    public float lastPassageUpperPos;
    public float maxExpectedTorque; //�ִ� ���� ��ũ
    public float twentiesAvgTorque; //20�� ���߹��� ��� ��ũ
    public float duration; //��� �ð�
    public float k;
    public float J; //���� ���Ʈ J
    public float g; //�߷� ���ӵ� g
    public float upperPosition; //���� ��ġ
    public float downPosition; //���� ��ġ

    void Start()
    {
        
    }

    public void CalculationStart()
    {
        duration = CM.IntervalSetting_Obstacle;
        upperPosition = UpperPos(lastPassageLowerPos, maxExpectedTorque, twentiesAvgTorque, duration, k * Mathf.PI, J);
        downPosition = DownPos(lastPassageUpperPos, duration, g);
    }
    static float UpperPos(float lastPassageLowerPos, float maxExpectedTorque, float twentiesAvgTorque, float duration, float k, float J)
    {
        float angleAcc = (twentiesAvgTorque - maxExpectedTorque) / J;
        float acc = k * angleAcc;
        float deltaDownPos = 0.5f * acc * duration * duration;
        float upperPos = lastPassageLowerPos + deltaDownPos;
        if (upperPos > 4.5f)
        {
            upperPos = 4.5f;
        }
        return upperPos;
    }

    static float DownPos(float lastPassageUpperPos, float duration, float g)
    {
        float deltaDownPos = 0.5f * g * duration * duration;
        float downPos = lastPassageUpperPos + deltaDownPos;
        if (downPos < -4.5f)
        {
            downPos = -4.5f;
        }
        return downPos;
    }

}
