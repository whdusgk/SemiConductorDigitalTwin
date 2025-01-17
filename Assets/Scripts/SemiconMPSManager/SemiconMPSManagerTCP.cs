using MPS;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Sockets;
using Unity.VisualScripting;
using UnityEngine;


/// <summary>
/// 공압이 공급되면 실린더 로드가 일정 거리만큼, 일정 속도로 전진 또는 후진한다.
/// 전진 또는 후진 시, 전후진 Limit Switch(LS)가 작동한다.
/// 속성: 실린더로드, Min-Max Range, Duration, 전후방 Limit Switch
/// </summary>
public class SemiconMPSManagerTCP : MonoBehaviour
{

    public static SemiconMPSManagerTCP instance;

    [SerializeField] List<Transform> LithoDoor;
    [SerializeField] List<Transform> gateValveDoor;
    public List<SemiconRobotControl> RobotArm = new List<SemiconRobotControl>();
    [SerializeField] List<SensorTowerManager> sensortowers = new List<SensorTowerManager>();
    [SerializeField] WaferSensorManager foupSensor;

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

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void OnStartBtnClkEvent()
    {
        isStart = true;

        // ETC Sensor Caligration(Black)
        //foreach (GameObject s in Foup1Sensors) s.GetComponent<Renderer>().material.color = new Color(0, 0, 0); // Black
/*        foreach (GameObject s in Foup2Sensors) s.GetComponent<Renderer>().material.color = new Color(0, 0, 0); // Black       
        foreach (GameObject s in VacuumSensors) s.GetComponent<Renderer>().material.color = new Color(0, 0, 0); // Black       
        foreach (GameObject s in GateValveUpSensors) s.GetComponent<Renderer>().material.color = new Color(0, 0, 0); // Black
        foreach (GameObject s in LithoGateUpSensors) s.GetComponent<Renderer>().material.color = new Color(0, 0, 0); // Black 
        foreach (GameObject s in LithoActSensors) s.GetComponent<Renderer>().material.color = new Color(0, 0, 0); // Black
        SEMActSensor.GetComponent<Renderer>().material.color = new Color(0, 0, 0); // Black

        // GateDownSensor Caligration(Green)
        foreach (GameObject s in GateValveDownSensors) s.GetComponent<Renderer>().material.color = new Color(255, 0,  0); // RED
        foreach (GameObject s in LithoGateDownSensors) s.GetComponent<Renderer>().material.color = new Color(255, 0,  0); // RED

        SensorTowerManager.Instance.OnSensorTower("green");*/
    }

    private void Update()
    {
        currentTime += Time.deltaTime;
        LithoAni1.GetComponent<Animator>().enabled = false;
        LithoAni2.GetComponent<Animator>().enabled = false;
        // StartCoroutine(SetAnimator());

        UpdateYDevices();
        UpdateXDevices();
        UpdateDDevices();

        void UpdateYDevices()
        {
            if (SemiconTCPClient.Instance.isConnected == false) return;
            if (SemiconTCPClient.Instance.yDevices.Length == 0) return;

            int startbtn = SemiconTCPClient.Instance.yDevices[0] - '0'; // Y0
            int Robot1Act = SemiconTCPClient.Instance.yDevices[1] - '0'; // Y0
            int Robot2Act = SemiconTCPClient.Instance.yDevices[2] - '0'; // Y1
            int Robot3Act = SemiconTCPClient.Instance.yDevices[3] - '0'; // Y2
            int Robot4Act = SemiconTCPClient.Instance.yDevices[4] - '0'; // Y3
            int Robot5Act = SemiconTCPClient.Instance.yDevices[5] - '0'; // Y4

            int GVGate1Up = SemiconTCPClient.Instance.yDevices[6] - '0'; 
            int GVGate1Down = SemiconTCPClient.Instance.yDevices[7] - '0';

            int GVGate2Up = SemiconTCPClient.Instance.yDevices[8] - '0';
            int GVGate2Down = SemiconTCPClient.Instance.yDevices[9] - '0';

            int GVGate3Up = SemiconTCPClient.Instance.yDevices[10] - '0';
            int GVGate3Down = SemiconTCPClient.Instance.yDevices[11] - '0';

            int GVGate4Up = SemiconTCPClient.Instance.yDevices[12] - '0';
            int GVGate4Down = SemiconTCPClient.Instance.yDevices[13] - '0';

            int GVGate5Up = SemiconTCPClient.Instance.yDevices[14] - '0';
            int GVGate5Down = SemiconTCPClient.Instance.yDevices[15] - '0';

            int GVGate6Up = SemiconTCPClient.Instance.yDevices[16] - '0';
            int GVGate6Down = SemiconTCPClient.Instance.yDevices[17] - '0';

            int Litho1Up = SemiconTCPClient.Instance.yDevices[18] - '0';
            int Litho1Down = SemiconTCPClient.Instance.yDevices[19] - '0';

            int Litho2Up = SemiconTCPClient.Instance.yDevices[20] - '0';
            int Litho2Down = SemiconTCPClient.Instance.yDevices[21] - '0';

            int SEMAction = SemiconTCPClient.Instance.yDevices[22] - '0';

            int FoupOpen = SemiconTCPClient.Instance.yDevices[23] - '0';

            int SensorTowerRedOn = SemiconTCPClient.Instance.yDevices[24] - '0';

            int ProcessStaying = SemiconTCPClient.Instance.yDevices[25] - '0';
            int UnitySignalWaiting = SemiconTCPClient.Instance.yDevices[26] - '0';
            int GateVacuumOn = SemiconTCPClient.Instance.yDevices[27] - '0';

            if (startbtn == 1) OnStartBtnClkEvent();

            if (Robot1Act == 1) RobotArm[0].OnSingleCycleBtnClkEvent();
            else if (Robot1Act == 0) RobotArm[0].OnStopBtnClkEvent();

            if (Robot2Act == 1) RobotArm[1].OnSingleCycleBtnClkEvent();
            else if (Robot2Act == 0) RobotArm[1].OnStopBtnClkEvent();

            if (Robot3Act == 1) RobotArm[2].OnSingleCycleBtnClkEvent();
            else if (Robot3Act == 0) RobotArm[2].OnStopBtnClkEvent();

            if (Robot4Act == 1) RobotArm[3].OnSingleCycleBtnClkEvent();
            else if (Robot4Act == 0) RobotArm[3].OnStopBtnClkEvent();

            if (Robot5Act == 1) RobotArm[4].OnSingleCycleBtnClkEvent();
            else if (Robot5Act == 0) RobotArm[4].OnStopBtnClkEvent();

            if (GVGate1Up == 1) OnGVUpBtnClkEvent(0);
            if (GVGate1Down == 1) OnGVDownBtnClkEvent(0);

            if (GVGate2Up == 1) OnGVUpBtnClkEvent(1);
            if (GVGate2Down == 1) OnGVDownBtnClkEvent(1);

            if (GVGate3Up == 1) OnGVUpBtnClkEvent(2);
            if (GVGate3Down == 1) OnGVDownBtnClkEvent(2);

            if (GVGate4Up == 1) OnGVUpBtnClkEvent(3);
            if (GVGate4Down == 1) OnGVDownBtnClkEvent(3);

            if (GVGate5Up == 1) OnGVUpBtnClkEvent(4);
            if (GVGate5Down == 1) OnGVDownBtnClkEvent(4);

            if (GVGate6Up == 1) OnGVUpBtnClkEvent(5);
            if (GVGate6Down == 1) OnGVDownBtnClkEvent(5);

            if (Litho1Up == 1) OnLithoUpBtnClkEvent(0);
            if (Litho1Down == 1) OnLithoDownBtnClkEvent(0);

            if (Litho2Up == 1) OnLithoUpBtnClkEvent(1);
            if (Litho2Down == 1) OnLithoDownBtnClkEvent(1);

            if (SEMAction == 1) SEMManager.Instance.RunSEMCycle();

            if (FoupOpen == 1) LPMManager.Instance.RunLPMCycle();

            if (SensorTowerRedOn == 1) sensortowers[0].OnSensorTower("red");

            ProcessStaying = 1;
            UnitySignalWaiting = 1;
            if (GateVacuumOn == 1) for (int i = 0; i < 7; i++) Sensor.Instance.OnVacuumSensor(i);
        }

        void UpdateXDevices()
        {
            string xDeviceValue = $"{(foupSensor.isFoupSensed == true ? 1 : 0)}" 
                                    + "0000000000000000000000000000000";
            SemiconTCPClient.Instance.xDevices = xDeviceValue;
        }

        void UpdateDDevices()
        {
            if (SemiconTCPClient.Instance.isConnected == false) return;

            if (SemiconTCPClient.Instance.dDevices.Length == 0) return;

            print(SemiconTCPClient.Instance.dDevices);
        }
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

