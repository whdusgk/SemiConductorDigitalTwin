using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 공압이 공급되면 실린더 로드가 일정 거리만큼, 일정 속도로 전진 또는 후진한다.
/// 전진 또는 후진 시, 전후진 Limit Switch(LS)가 작동한다.
/// 속성: 실린더로드, Min-Max Range, Duration, 전후방 Limit Switch
/// </summary>
public class SemiconMPSManager : MonoBehaviour
{
    [SerializeField] List<Transform> LithoDoor;
    [SerializeField] List<Transform> gateValveDoor;
    [SerializeField] float LithoMaxRange;
    [SerializeField] float LithoMinRange;
    [SerializeField] float GateValveMaxRange;
    [SerializeField] float GateValveMinRange;
    [SerializeField] float duration;

    public bool isUp = false;

    public int cycleCnt;
    public float cycleTime;

    private void Start()
    {

    }

    public void OnUpBtnClkEvent()
    {
        if (isUp) return;

        cycleCnt++;

        foreach (Transform l in LithoDoor)
        {
            StartCoroutine(MoveGate(l, LithoMinRange, LithoMaxRange, duration));
        }

        foreach (Transform gv in gateValveDoor)
        {
            StartCoroutine(MoveGate(gv, GateValveMinRange, GateValveMaxRange, duration));
        }

    }

    public void OnDownBtnClkEvent()
    {
        if (!isUp) return;

        foreach (Transform l in LithoDoor)
        {
            StartCoroutine(MoveGate(l, LithoMaxRange, LithoMinRange, duration));
        }

        foreach (Transform gv in gateValveDoor)
        {
            StartCoroutine(MoveGate(gv, GateValveMaxRange, GateValveMinRange, duration));
        }

    }

    IEnumerator MoveGate(Transform gate, float min, float max, float duration)
    {

        Vector3 minPos = new Vector3(gate.transform.localPosition.x, min, gate.transform.localPosition.z);
        Vector3 maxPos = new Vector3(gate.transform.localPosition.x, max, gate.transform.localPosition.z);

        float currentTime = 0;

        while (currentTime <= duration)
        {
            currentTime += Time.deltaTime;

            gate.localPosition = Vector3.Lerp(minPos, maxPos, currentTime / duration);

            cycleTime += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        isUp = !isUp;

    }

}

