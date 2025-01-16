using System;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using System.Net.Http;
using UnityEngine.InputSystem;
using Unity.VisualScripting;




namespace MPS
{ 

    public class MPSManager : MonoBehaviour
    {
        [SerializeField] List<SemiconRobotControl> SemiconRobotControl = new List<SemiconRobotControl>();
        [SerializeField] SemiconMPSManager GateValveDoor = new SemiconMPSManager();
        [SerializeField] SemiconMPSManager LithoDoor = new SemiconMPSManager();
        [SerializeField] SemiconMPSManager STRAT = new SemiconMPSManager();
        [SerializeField] Sensor Sensors = new Sensor();
        [SerializeField] SEMManager SEMManager = new SEMManager();
        [SerializeField] int startBtnState = 0;

        [SerializeField] List<SemiconRobotControl> lamps = new List<SemiconRobotControl>();

       

        //[SerializeField] private int startBtnState = 0;
        int count;

        Color redLamp;
        Color yellowLamp;
        Color greenLamp;


        // Start is called once before the first execution of Update after the MonoBehaviour is created

       /* private void Awake()
        {
            redLamp = lamps[0].material.GetColor("_BaseColor");
            yellowLamp = lamps[1].material.GetColor("_BaseColor");
            greenLamp = lamps[2].material.GetColor("_BaseColor");

            OnLampOnOffBtnClkEvent("Red", false);
            OnLampOnOffBtnClkEvent("Yellow", false);
            OnLampOnOffBtnClkEvent("Green", false);
        }*/

        void Start()
        {

        }

        void Update()
        {
            UpdateYDevices();
            UpdateXDevices(); 

            

        }

        void UpdateXDevices()
        {


            // X 디바이스 값 생성
            string xDeviceValue =
                                    $"1" +                                                       // X0: Power On (Rising Pulse)
                                    $"{(FirebaseAuthManager.instance.isPermission ? 1 : 0)}" +   // X1: isPermission (허가 확인)
                                    $"{(SemiconMPSManager.instance.isStart ? 1 : 0)}" +          // X2: Start Button (공정 가동 버튼)
                                    $"{(Sensor.instance.isFoupSensed ? 1 : 0)}";                 // X3: FOUP 감지 (isFoupSensed 상태)                     

            //$"{(startBtnState == 1 ? 1 : 0)}" +   




           

            //TCPClient.Instance.xDevices = xDeviceValue;  // "0110" -> "6", "1010101010000111100010" = "12133",  "10101010100001111000/10101010101000011110001/01010101010000111100/0101010101010000111100010" = "15,0,0,0"
                                                       
            //int xDeviceValueName = Convert.ToInt32(xDeviceValue, 2);  // 2진수를 10진수로 변환

            print("XXXXXXXXX" + xDeviceValue);
        }

        void UpdateYDevices()
        {
            // TCPClient가 연결되지 않았다면 함수 실행을 종료
            if (TCPClient.Instance.isConnected == false)
                return;

            // yDevices 배열에 데이터가 없다면 함수 실행을 종료
            if (TCPClient.Instance.yDevices.Length == 0)
                return;

            int StartButton                    = TCPClient.Instance.yDevices[4] - '0';   // Y4: Start 버튼
            int SemiconRobotARM1Action         = TCPClient.Instance.yDevices[9] - '0';   // Y9: 세미콘 로봇 ARM1 동작
            int SemiconRobotARM2Action         = TCPClient.Instance.yDevices[13] - '0';  // Y13: 세미콘 로봇 ARM2 동작
            int SemiconRobotARM3Action         = TCPClient.Instance.yDevices[22] - '0';  // Y22: 세미콘 로봇 ARM3 동작
            int SemiconRobotARM4Action         = TCPClient.Instance.yDevices[44] - '0';  // Y2C: 세미콘 로봇 ARM4 동작 
            int SemiconRobotARM5Action         = TCPClient.Instance.yDevices[51] - '0';  // Y33: 세미콘 로봇 ARM5 동작 
            int GateValveDoor000Open           = TCPClient.Instance.yDevices[11] - '0';  // Y11: GateValveDoor 000 열기
            int GateValveDoor001Open           = TCPClient.Instance.yDevices[18] - '0';  // Y12: GateValveDoor 001 열기 
            int GateValveDoor002Open           = TCPClient.Instance.yDevices[33] - '0';  // Y21: GateValveDoor 002 열기 
            int GateValveDoor003Open           = TCPClient.Instance.yDevices[40] - '0';  // Y28: GateValveDoor 003 열기 
            int GateValveDoor004Open           = TCPClient.Instance.yDevices[43] - '0';  // Y2B: GateValveDoor 004 열기 
            int GateValveDoor005Open           = TCPClient.Instance.yDevices[50] - '0';  // Y32: GateValveDoor 005 열기 
            int GateValveDoor000Close          = TCPClient.Instance.yDevices[17] - '0';  // Y17: GateValveDoor 000 닫기
            int GateValveDoor001Close          = TCPClient.Instance.yDevices[23] - '0';  // Y23: GateValveDoor 001 닫기
            int GateValveDoor002Close          = TCPClient.Instance.yDevices[38] - '0';  // Y26: GateValveDoor 002 닫기 
            int GateValveDoor003Close          = TCPClient.Instance.yDevices[45] - '0';  // Y2D: GateValveDoor 003 닫기 
            int GateValveDoor004Close          = TCPClient.Instance.yDevices[48] - '0';  // Y30: GateValveDoor 004 닫기 
            int GateValveDoor005Close          = TCPClient.Instance.yDevices[55] - '0';  // Y37: GateValveDoor 005 닫기 
            int LithoDoor1Open                 = TCPClient.Instance.yDevices[21] - '0';  // Y15: LITHO_Door.1 열기 
            int LithoDoor6Open                 = TCPClient.Instance.yDevices[30] - '0';  // Y1E: LITHO_Door.6 열기 
            int LithoDoor1Close                = TCPClient.Instance.yDevices[26] - '0';  // Y1A: LITHO_Door.1 닫기 
            int LithoDoor6Close                = TCPClient.Instance.yDevices[35] - '0';  // Y23: LITHO_Door.6 닫기 
            int SEMBodyAction                  = TCPClient.Instance.yDevices[39] - '0';  // Y27: SEMBody 동작 
            int LPMFoupOpen                    = TCPClient.Instance.yDevices[16] - '0';  // Y10: LPM (Foup 열기) 
            int SensorTowerRedOn               = TCPClient.Instance.yDevices[1] - '0';   // Y1: Sensor Tower 빨간불 켜기
            int SensorTowerGreenOnRedOff       = TCPClient.Instance.yDevices[5] - '0';   // Y5: 초록불 켜고 빨간불 끄기
            int ProcessStaying                 = TCPClient.Instance.yDevices[2] - '0';   // Y2: 공정 대기
            int UnitySignalWaiting             = TCPClient.Instance.yDevices[3] - '0';   // Y3: 유니티 신호 대기
            int Gate2_8SectorVacuum            = TCPClient.Instance.yDevices[6] - '0';   // Y6: Gate2-8 섹터 진공
            int ProcessReset                   = TCPClient.Instance.yDevices[7] - '0';   // Y7: 공정 리셋
            int RepeatT0                       = TCPClient.Instance.yDevices[8] - '0';   // Y8: Repeat T0




            string output = $"StartButton: {StartButton}, " +
                $"SemiconRobotARM1Action: {SemiconRobotARM1Action}, " +
                $"SemiconRobotARM2Action: {SemiconRobotARM2Action}, " +
                $"SemiconRobotARM3Action: {SemiconRobotARM3Action}, " +
                $"SemiconRobotARM4Action: {SemiconRobotARM4Action}, " +
                $"SemiconRobotARM5Action: {SemiconRobotARM5Action}, " +
                $"GateValveDoor000Open: {GateValveDoor000Open}, " +
                $"GateValveDoor001Open: {GateValveDoor001Open}, " +
                $"GateValveDoor002Open: {GateValveDoor002Open}, " +
                $"GateValveDoor003Open: {GateValveDoor003Open}, " +
                $"GateValveDoor004Open: {GateValveDoor004Open}, " +
                $"GateValveDoor005Open: {GateValveDoor005Open}, " +
                $"GateValveDoor000Close: {GateValveDoor000Close}, " +
                $"GateValveDoor001Close: {GateValveDoor001Close}, " +
                $"GateValveDoor002Close: {GateValveDoor002Close}, " +
                $"GateValveDoor003Close: {GateValveDoor003Close}, " +
                $"GateValveDoor004Close: {GateValveDoor004Close}, " +
                $"GateValveDoor005Close: {GateValveDoor005Close}, " +
                $"LithoDoor1Open: {LithoDoor1Open}, " +
                $"LithoDoor6Open: {LithoDoor6Open}, " +
                $"LithoDoor1Close: {LithoDoor1Close}, " +
                $"LithoDoor6Close: {LithoDoor6Close}, " +
                $"SEMBodyAction: {SEMBodyAction}, " +
                $"LPMFoupOpen: {LPMFoupOpen}, " +
                $"SensorTowerRedOn: {SensorTowerRedOn}, " +
                $"SensorTowerGreenOnRedOff: {SensorTowerGreenOnRedOff}, " +
                $"ProcessStaying: {ProcessStaying}, " +
                $"UnitySignalWaiting: {UnitySignalWaiting}, " +
                $"Gate2_8SectorVacuum: {Gate2_8SectorVacuum}, " +
                $"ProcessReset: {ProcessReset}, " +
                $"RepeatT0: {RepeatT0}";

                print(output);



            if (StartButton == 1) STRAT.OnStartBtnClkEvent();
       

            if (SemiconRobotARM1Action == 1) SemiconRobotControl[0].OnSingleCycleBtnClkEvent(); 
            else if (SemiconRobotARM1Action == 0) SemiconRobotControl[0].OnStopBtnClkEvent();

            if (SemiconRobotARM2Action == 1) SemiconRobotControl[1].OnSingleCycleBtnClkEvent(); 
            else if (SemiconRobotARM2Action == 0) SemiconRobotControl[1].OnStopBtnClkEvent();

            if (SemiconRobotARM3Action == 1) SemiconRobotControl[2].OnSingleCycleBtnClkEvent(); 
            else if (SemiconRobotARM3Action == 0) SemiconRobotControl[2].OnStopBtnClkEvent();

            if (SemiconRobotARM4Action == 1) SemiconRobotControl[3].OnSingleCycleBtnClkEvent(); 
            else if (SemiconRobotARM4Action == 0) SemiconRobotControl[3].OnStopBtnClkEvent();

            if (SemiconRobotARM5Action == 1) SemiconRobotControl[4].OnSingleCycleBtnClkEvent(); 
            else if (SemiconRobotARM5Action == 0) SemiconRobotControl[4].OnStopBtnClkEvent();


            //GateValveDoor OPEN
            if (GateValveDoor000Open == 1) GateValveDoor.OnGVUpBtnClkEvent(0);  
            if (GateValveDoor001Open == 1) GateValveDoor.OnGVUpBtnClkEvent(1); 
            if (GateValveDoor002Open == 1) GateValveDoor.OnGVUpBtnClkEvent(2); 
            if (GateValveDoor003Open == 1) GateValveDoor.OnGVUpBtnClkEvent(3); 
            if (GateValveDoor004Open == 1) GateValveDoor.OnGVUpBtnClkEvent(4); 
            if (GateValveDoor005Open == 1) GateValveDoor.OnGVUpBtnClkEvent(5); 

            //GateValveDoor Close
            if (GateValveDoor000Close == 1) GateValveDoor.OnGVDownBtnClkEvent(0); 
            if (GateValveDoor001Close == 1) GateValveDoor.OnGVDownBtnClkEvent(1); 
            if (GateValveDoor002Close == 1) GateValveDoor.OnGVDownBtnClkEvent(2);
            if (GateValveDoor003Close == 1) GateValveDoor.OnGVDownBtnClkEvent(3); 
            if (GateValveDoor004Close == 1) GateValveDoor.OnGVDownBtnClkEvent(4); 
            if (GateValveDoor005Close == 1) GateValveDoor.OnGVDownBtnClkEvent(5); 

            //LITHO_Door Open
             if (LithoDoor1Open == 1) LithoDoor.OnLithoUpBtnClkEvent(0); 
             if (LithoDoor6Open == 1) LithoDoor.OnLithoUpBtnClkEvent(1); 

            //LITHO_Door close
             if (LithoDoor1Close == 1) LithoDoor.OnLithoDownBtnClkEvent(0); 
             if (LithoDoor6Close == 1) LithoDoor.OnLithoDownBtnClkEvent(1);

            //SEMBody Action
            //if (SEMBodyAction == 1) StartCoroutine (SEMManager.RunSEM());
            //if (LPMFoupOpen == 1) StartCoroutine(SEMManager.OnLPMBtnClkEvent());

            //Sensor 
            if (SensorTowerRedOn == 1) Sensors.OnSensorTower("red");
            if (SensorTowerGreenOnRedOff == 1) Sensors.OnSensorTower("green");

            ProcessStaying = 1;
            UnitySignalWaiting = 1;
            Gate2_8SectorVacuum = 1;
            ProcessReset = 0;
            RepeatT0 = 0;





            /*  //lamps 
              if (빨강램프 == 1) OnLampOnOffBtnClkEvent("Red", true);
              else OnLampOnOffBtnClkEvent("Red", false);

              if (노랑램프 == 1) OnLampOnOffBtnClkEvent("Yellow", true);
              else OnLampOnOffBtnClkEvent("Yellow", false);

              if (초록램프 == 1) OnLampOnOffBtnClkEvent("Green", true);
              else OnLampOnOffBtnClkEvent("Green", false);*/

        }
    }

}