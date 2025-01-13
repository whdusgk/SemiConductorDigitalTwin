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
public class Backup_SemiconMPSManager : MonoBehaviour
{
    [SerializeField] List<Transform> LithoDoor;
    [SerializeField] List<Transform> gateValveDoor;
    [SerializeField] float LithoMaxRange;
    [SerializeField] float LithoMinRange;
    [SerializeField] float GateValveMaxRange;
    [SerializeField] float GateValveMinRange;
    [SerializeField] float duration;

    public bool isStart = false;
    public bool isUp = false;
    float currentTime = 0;
    public int cycleCnt;
    public float cycleTime;

    [Header("Sensors")]
    //public List<GameObject> Foup1Sensors;
    public List<GameObject> Foup2Sensors;
    //public List<GameObject> RobotActSensors;
    public List<GameObject> VacuumSensors;

    public List<GameObject> GateValveUpSensors;
    public List<GameObject> GateValveDownSensors;
    public List<GameObject> LithoGateUpSensors;
    public List<GameObject> LithoGateDownSensors;

    public List<GameObject> LithoActSensors;

    public GameObject SEMActSensor;

    public bool isFoupForward = false;
    public bool isFoupDoorForward = false;
    public bool isFoupDoorBackward = false;
    public bool isFoupOpen = false;

    public bool isRobotAct = false;
    public bool isVacuum = false;
    public bool isGateValveUp = false;
    public bool isLithoGateUp = false;
    public bool isLithoWafer = false;
    public bool isSEMAct = false;

    public GameObject LithoAni1;
    public GameObject LithoAni2;
    public void OnStartBtnClkEvent()
    {
        isStart = true;

        // ETC Sensor Caligration(Black)
        //foreach (GameObject s in Foup1Sensors) s.GetComponent<Renderer>().material.color = new Color(0, 0, 0); // Black
        foreach (GameObject s in Foup2Sensors) s.GetComponent<Renderer>().material.color = new Color(0, 0, 0); // Black       
        foreach (GameObject s in VacuumSensors) s.GetComponent<Renderer>().material.color = new Color(0, 0, 0); // Black       
        foreach (GameObject s in GateValveUpSensors) s.GetComponent<Renderer>().material.color = new Color(0, 0, 0); // Black
        foreach (GameObject s in LithoGateUpSensors) s.GetComponent<Renderer>().material.color = new Color(0, 0, 0); // Black 
        foreach (GameObject s in LithoActSensors) s.GetComponent<Renderer>().material.color = new Color(0, 0, 0); // Black
        SEMActSensor.GetComponent<Renderer>().material.color = new Color(0, 0, 0); // Black

        // GateDownSensor Caligration(Green)
        foreach (GameObject s in GateValveDownSensors) s.GetComponent<Renderer>().material.color = new Color(255, 0,  0); // RED
        foreach (GameObject s in LithoGateDownSensors) s.GetComponent<Renderer>().material.color = new Color(255, 0,  0); // RED

        OnGVUpBtnClkEvent(1);
        OnLithoUpBtnClkEvent(0);
        
    }

    private void Update()
    {
        currentTime += Time.deltaTime;
        StartCoroutine(SetAnimator());
    }
    IEnumerator SetAnimator()
    {
        if (currentTime < 7.8f)
        {
            LithoAni1.GetComponent<Animator>().enabled = true;
            LithoAni2.GetComponent<Animator>().enabled = false;
        }
        else
        {
            LithoAni2.GetComponent<Animator>().enabled = true;
        }
        yield return new WaitForEndOfFrame();
    }
   
    
    public void OnGVUpBtnClkEvent(int gv)
    {
        if (isUp) return;


        StartCoroutine(MoveGate(gateValveDoor[gv], GateValveMinRange, GateValveMaxRange, duration));
        GateValveUpSensors[gv].GetComponent<Renderer>().material.color = new Color(0, 255, 0); // Green
        GateValveDownSensors[gv].GetComponent<Renderer>().material.color = new Color(0, 0, 0); // Black

    }
    public void OnGVDownBtnClkEvent(int gv)
    {
        if (!isUp) return;

        StartCoroutine(MoveGate(gateValveDoor[gv], GateValveMaxRange, GateValveMinRange, duration));
        GateValveUpSensors[gv].GetComponent<Renderer>().material.color = new Color(0, 0, 0); // Black
        GateValveDownSensors[gv].GetComponent<Renderer>().material.color = new Color(255, 0, 0); // Red

    }
    public void OnLithoUpBtnClkEvent(int litho)
    {
        if (isUp) return;

        StartCoroutine(MoveGate(LithoDoor[litho], LithoMinRange, LithoMaxRange, duration));
        LithoGateUpSensors[litho].GetComponent<Renderer>().material.color = new Color(0, 255, 0); // Green
        LithoGateDownSensors[litho].GetComponent<Renderer>().material.color = new Color(0, 0, 0); // Black    

    }
    public void OnLithoDownBtnClkEvent(int litho)
    {
        if (!isUp) return;

        StartCoroutine(MoveGate(LithoDoor[litho], LithoMaxRange, LithoMinRange, duration));
        LithoGateUpSensors[litho].GetComponent<Renderer>().material.color = new Color(0, 0, 0); // Black
        LithoGateDownSensors[litho].GetComponent<Renderer>().material.color = new Color(255, 0, 0); // Red
    
       
    }

    IEnumerator MoveGate(Transform gate, float min, float max, float duration)
    {

        Vector3 minPos = new Vector3(gate.transform.localPosition.x, min, gate.transform.localPosition.z);
        Vector3 maxPos = new Vector3(gate.transform.localPosition.x, max, gate.transform.localPosition.z);

        currentTime = 0;

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

