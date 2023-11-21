using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateSubManager : MonoBehaviour
{
    public GameManager GM;
    public CreateManager CM;
    
    public float lastPassageLowerPos; //마지막 통과의 하한 위치
    public float lastPassageUpperPos;
    public float maxExpectedTorque; //최대 예상 토크
    public float twentiesAvgTorque; //20대 초중반의 평균 토크
    public float duration; //통과 시간
    public float k;
    public float J; //관성 모멘트 J
    public float g; //중력 가속도 g
    public float upperPosition; //상한 위치
    public float downPosition; //하한 위치

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
