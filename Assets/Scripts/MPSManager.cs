using System;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using System.Net.Http;
using UnityEngine.InputSystem;




namespace MPS
{ 

    public class MPSManager : MonoBehaviour
        {
        [SerializeField] List<SemiconRobotControl> SemiconRobotControl = new List<SemiconRobotControl>();
        [SerializeField] SemiconMPSManager GateValveDoor = new SemiconMPSManager();
        [SerializeField] SemiconMPSManager LithoDoor = new SemiconMPSManager();
        [SerializeField] SemiconMPSManager Manager = new SemiconMPSManager();
        [SerializeField] List<SemiconRobotControl> lamps = new List<SemiconRobotControl>();
        [SerializeField] List<Sensor> Sensors = new List<Sensor>();

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
            //UpdateXDevices(); 

            

        }

        /* void UpdateXDevices()
        {
            // X ����̽� �� ����
            string xDeviceValue = $"{startBtnState}" +
                                    $"{(sensors[0].isEnabled ? 1 : 0)}" +    // X0: Power On (Rising Pulse)
                                    $"{(sensors[1].isEnabled ? 1 : 0)}" +    // X1: Permission Clear (���� Ȯ�� �Ϸ�)
                                    $"{(startBtnState == 0 ? 1 : 0)}" +      // X2: Start Button Red Off (���� ���� ��ư, ������ Off)
                                    $"{(sensors[2].isEnabled ? 1 : 0)}" +    // X3: Vacuum Fail (���� ���� ������ ���� ���� ����)
                                    $"{(sensors[3].isEnabled ? 1 : 0)}" +    // X4: Arrived FOUP (FOUP ���� ����)
                                    $"{(sensors[4].isEnabled ? 1 : 0)}" +    // X5: FOUP_Fail_Or_Wafer_Run_Out (FOUP ���� ���� �Ǵ� Wafer ����)
                                    $"{(sensors[5].isEnabled ? 1 : 0)}" +    // X6: 1st Robot Arm Wafer Loading Fail (1�� �κ� �� ������ ���� ����)
                                    $"{(sensors[6].isEnabled ? 1 : 0)}" +    // X7: 1st Vacuum Gate Open Fail (1�� ������ Open ����)
                                    $"{(sensors[7].isEnabled ? 1 : 0)}" +    // X8: 1st Robot Arm Set Wafer Fail (1�� �κ� �� Load Lock�� Wafer ���� ����)
                                    $"{(sensors[8].isEnabled ? 1 : 0)}" +    // X9: Wafer Run Out in FOUP (FOUP �� Wafer ����)
                                    $"{(sensors[9].isEnabled ? 1 : 0)}" +    // X10: 3rd Vacuum Gate Close Fail (�� ��° ������ Close ����)
                                    $"{(sensors[10].isEnabled ? 1 : 0)}" +   // X0A: 1st Gate Close Or Vacuum Fail (1�� ������ Close Ȥ�� Load Lock ����ȭ ����)
                                    $"{(sensors[11].isEnabled ? 1 : 0)}" +   // X0B: 2nd Vacuum Gate Open Fail (2�� ������ Open ����)
                                    $"{(sensors[12].isEnabled ? 1 : 0)}" +   // X0C: 2nd Robot Arm Wafer Loading Fail (2�� �κ� �� ������ ���� ����)
                                    $"{(sensors[13].isEnabled ? 1 : 0)}" +   // X0D: 3rd Vacuum Gate Open Fail (3�� ������ Open ����)
                                    $"{(sensors[14].isEnabled ? 1 : 0)}" +   // X0E: 2nd Vacuum Gate Close Fail (2�� ������ Close ����)
                                    $"{(sensors[15].isEnabled ? 1 : 0)}";    // X0F: 2nd Arm Set Wafer To Align Fail (2�� �κ� �� Align ��ġ�� Wafer Set ����)

            Debug.Log("X Device Values: " + xDeviceValue);
    }*/

        void UpdateYDevices()
        {
            // TCPClient�� ������� �ʾҴٸ� �Լ� ������ ����
            if (TCPClient.Instance.isConnected == false)
                return;

            // yDevices �迭�� �����Ͱ� ���ٸ� �Լ� ������ ����
            if (TCPClient.Instance.yDevices.Length == 0)
                return;

            /* // ��ȣ ���� (Y0 ~ Y1A)
             int CheckingPermission          = TCPClient.Instance.yDevices[0]  - '0';   // Y0: ���� Ȯ��
             int REDOnGreenOff               = TCPClient.Instance.yDevices[1]  - '0';   // Y1: ������ �Ѱ� �ʷϺ� ����
             int ProcessStaying              = TCPClient.Instance.yDevices[2]  - '0';   // Y2: ���� ���
             int UnitySignalWaiting          = TCPClient.Instance.yDevices[3]  - '0';   // Y3: ����Ƽ ��ȣ ���
             int StartButtonWaiting          = TCPClient.Instance.yDevices[4]  - '0';   // Y4: ���� ��ư ���
             int GreenOnRedOff               = TCPClient.Instance.yDevices[5]  - '0';   // Y5: �ʷϺ� �Ѱ� ������ ����
             int Gate2_8SectorVacuum         = TCPClient.Instance.yDevices[6]  - '0';   // Y6: Gate2-8 ���� ����
             int ProcessReset                = TCPClient.Instance.yDevices[7]  - '0';   // Y7: ���� ����
             int RepeatT0                    = TCPClient.Instance.yDevices[8]  - '0';   // Y8: T0 �ݺ�
             int Robot1MoveToFOUP            = TCPClient.Instance.yDevices[9]  - '0';   // Y9: ù ��° �κ� FOUP�� �̵�
             int T2SelfHoldingAndY9Off       = TCPClient.Instance.yDevices[10] - '0';   // Y0A: T2 ���� Ȧ�� �� Y9 ����
             int VacuumGate1Open             = TCPClient.Instance.yDevices[11] - '0';   // Y0B: ù ��° ���� ����Ʈ ����
             int T3SelfHoldingAndT2Off       = TCPClient.Instance.yDevices[12] - '0';   // Y0C: T3 ���� Ȧ�� �� T2 ����
             int DetectingWaferRunOut        = TCPClient.Instance.yDevices[13] - '0';   // Y0D: FOUP���� ������ �� ����
             int T4SelfHoldingAndT3Off       = TCPClient.Instance.yDevices[14] - '0';   // Y0E: T4 ���� Ȧ�� �� T3 ����
             int T5SelfHoldingAndT4Off       = TCPClient.Instance.yDevices[15] - '0';   // Y0F: T5 ���� Ȧ�� �� T4 ����
             int RestartRelayP1              = TCPClient.Instance.yDevices[16] - '0';   // Y10: P1 ���� ������
             int T7SelfHoldingAndT5Off       = TCPClient.Instance.yDevices[17] - '0';   // Y11: T7 ���� Ȧ�� �� T5 ����
             int VacuumGate2Open             = TCPClient.Instance.yDevices[18] - '0';   // Y12: �� ��° ���� ����Ʈ ����
             int T8SelfHoldingAndT7Off       = TCPClient.Instance.yDevices[19] - '0';   // Y13: T8 ���� Ȧ�� �� T7 ����
             int T9SelfHoldingAndT8Off       = TCPClient.Instance.yDevices[20] - '0';   // Y14: T9 ���� Ȧ�� �� T8 ����
             int VacuumGate3Open             = TCPClient.Instance.yDevices[21] - '0';   // Y15: �� ��° ���� ����Ʈ ����
             int T10SelfHoldingAndT9Off      = TCPClient.Instance.yDevices[22] - '0';   // Y16: T10 ���� Ȧ�� �� T9 ����
             int T11SelfHoldingAndT10Off     = TCPClient.Instance.yDevices[23] - '0';   // Y17: T11 ���� Ȧ�� �� T10 ����
             int T12SelfHoldingAndT11Off     = TCPClient.Instance.yDevices[24] - '0';   // Y18: T12 ���� Ȧ�� �� T11 ����
             int T13SelfHoldingAndT12Off     = TCPClient.Instance.yDevices[25] - '0';   // Y19: T13 ���� Ȧ�� �� T12 ����
             int VacuumGate3Close            = TCPClient.Instance.yDevices[26] - '0';   // Y1A: �� ��° ���� ����Ʈ �ݱ�*/

            int CheckingPermissionComplete      = TCPClient.Instance.yDevices[0]    - '0';   // Y0: ���� ���� ���� Ȯ�� ��
            int REDOnGreenOff                   = TCPClient.Instance.yDevices[1]    - '0';   // Y1: ������ �Ѱ� �ʷϺ� ����
            int ProcessStaying                  = TCPClient.Instance.yDevices[2]    - '0';   // Y2: ���� ���
            int UnitySignalWaiting              = TCPClient.Instance.yDevices[3]    - '0';   // Y3: ����Ƽ ���� ���� ��ȣ ���
            int StartButtonWaiting              = TCPClient.Instance.yDevices[4]    - '0';   // Y4: ���� ���� ��ư ���
            int GreenOnRedOff                  = TCPClient.Instance.yDevices[5]    - '0';   // Y5: ���� ���� �۵� ����
            int Gate2_8SectorVacuum            = TCPClient.Instance.yDevices[6]    - '0';   // Y6: 2���� ~ 8���� ���� ���� ó��
            int ProcessReset                   = TCPClient.Instance.yDevices[7]    - '0';   // Y7: ���� �ʱ�ȭ
            int RepeatT0                        = TCPClient.Instance.yDevices[8]    - '0';   // Y8: FOUP ���� ���� �� T0 �ݺ�
            int FirstRobotMoveToFOUP           = TCPClient.Instance.yDevices[9]    - '0';   // Y9: 1�� �κ��� FOUP���� �̵�
            int FOUPOpening                     = TCPClient.Instance.yDevices[10]   - '0';   // YA: FOUP ����
            int LoadingWaferOn1stRobotArm      = TCPClient.Instance.yDevices[11]   - '0';   // YB: 1�� �κ��ȿ� ������ ����
            int FirstVacuumGateOpen            = TCPClient.Instance.yDevices[12]   - '0';   // YC: 1�� ������ ���� 1
            int FirstVacuumGateClose           = TCPClient.Instance.yDevices[13]   - '0';   // YD: 1�� ������ �ݱ� 1
            int AligningComplete               = TCPClient.Instance.yDevices[14]   - '0';   // YE: Aligning �Ϸ�
            int RotationingWafer               = TCPClient.Instance.yDevices[15]   - '0';   // YF: Wafer ȸ�� ���� ��
            int LithographyComplete            = TCPClient.Instance.yDevices[16]   - '0';   // Y10: Lithography �Ϸ�
            int SecondVacuumGateOpen          = TCPClient.Instance.yDevices[17]   - '0';   // Y11: 2�� ������ ���� 2
            int SecondVacuumGateClose         = TCPClient.Instance.yDevices[18]   - '0';   // Y12: 2�� ������ �ݱ� 2
            int ThirdVacuumGateOpen           = TCPClient.Instance.yDevices[19]   - '0';   // Y13: 3�� ������ ����  01 
            int ThirdVacuumGateClose          = TCPClient.Instance.yDevices[20]   - '0';   // Y14: 3�� ������ �ݱ�  001 
            int FourthVacuumGateOpen          = TCPClient.Instance.yDevices[21]   - '0';   // Y15: 4�� ������ ����  02
            int FourthVacuumGateClose         = TCPClient.Instance.yDevices[22]   - '0';   // Y16: 4�� ������ �ݱ�  002
            int FifthVacuumGateOpen           = TCPClient.Instance.yDevices[23]   - '0';   // Y17: 5�� ������ ���� 3
            int FifthVacuumGateClose          = TCPClient.Instance.yDevices[24]   - '0';   // Y18: 5�� ������ �ݱ� 3
            int SixthVacuumGateOpen           = TCPClient.Instance.yDevices[25]   - '0';   // Y19: 6�� ������ ���� 4
            int SixthVacuumGateClose          = TCPClient.Instance.yDevices[26]   - '0';   // Y1A: 6�� ������ �ݱ� 4
            int SeventhVacuumGateOpen         = TCPClient.Instance.yDevices[27]   - '0';   // Y1B: 7�� ������ ���� 5
            int SeventhVacuumGateClose        = TCPClient.Instance.yDevices[28]   - '0';   // Y1C: 7�� ������ �ݱ� 5
            int EighthVacuumGateOpen          = TCPClient.Instance.yDevices[29]   - '0';   // Y1D: 8�� ������ ���� 6
            int EighthVacuumGateClose         = TCPClient.Instance.yDevices[30]   - '0';   // Y1E: 8�� ������ �ݱ� 6
            int LoadLockVacuumRelease         = TCPClient.Instance.yDevices[31]   - '0';   // Y1F: Load Lock ���� ����
            int FifthRobotArmMoveToLoadLock   = TCPClient.Instance.yDevices[32]   - '0';   // Y20: 2�� �κ��� Load Lock���� �̵�
            int FifthRobotArmSetWaferInFOUP   = TCPClient.Instance.yDevices[33]   - '0';   // Y21: 2�� �κ��� FOUP�� ������ ����
            int ProcessCycleEnd               = TCPClient.Instance.yDevices[34]   - '0';   // Y22: ���� ����
            int FifthRobotArmComeback         = TCPClient.Instance.yDevices[35]   - '0';   // Y23: 2�� �κ��� ����
            int SixthRobotArmMoveToLoadLock   = TCPClient.Instance.yDevices[36]   - '0';   // Y24: 3�� �κ��� Load Lock���� �̵�
            int SixthRobotArmSetWaferInFOUP   = TCPClient.Instance.yDevices[37]   - '0';   // Y25: 3�� �κ��� FOUP�� ������ ����
            int SixthRobotArmComeback         = TCPClient.Instance.yDevices[38]   - '0';   // Y26: 3�� �κ��� ����
            int SeventhRobotArmMoveToLoadLock = TCPClient.Instance.yDevices[39]   - '0';   // Y27: 4�� �κ��� Load Lock���� �̵�
            int SeventhRobotArmSetWaferInFOUP = TCPClient.Instance.yDevices[40]   - '0';   // Y28: 4�� �κ��� FOUP�� ������ ����
            int SeventhRobotArmComeback       = TCPClient.Instance.yDevices[41]   - '0';   // Y29: 4�� �κ��� ����
            int EighthRobotArmMoveToLoadLock  = TCPClient.Instance.yDevices[42]   - '0';   // Y2A: 5�� �κ��� Load Lock���� �̵�
            int EighthRobotArmSetWaferInFOUP  = TCPClient.Instance.yDevices[43]   - '0';   // Y2B: 5�� �κ��� FOUP�� ������ ����
            int EighthRobotArmComeback        = TCPClient.Instance.yDevices[44]   - '0';   // Y2C: 5�� �κ��� ����
            int EmergencyStop                 = TCPClient.Instance.yDevices[45]   - '0';   // Y2D: ��� ����
            int SystemReady                   = TCPClient.Instance.yDevices[46]   - '0';   // Y2E: �ý��� �غ� �Ϸ�
            int CleanMode                     = TCPClient.Instance.yDevices[47]   - '0';   // Y2F: Ŭ�� ���
            int StandbyMode                   = TCPClient.Instance.yDevices[48]   - '0';   // Y30: ��� ���
            int CalibrationComplete           = TCPClient.Instance.yDevices[49]   - '0';   // Y31: ���� �Ϸ�
            int FaultDetected                 = TCPClient.Instance.yDevices[50]   - '0';   // Y32: ���� ����
            int MaintenanceRequired           = TCPClient.Instance.yDevices[51]   - '0';   // Y33: �������� �ʿ�
            int SystemShutdown                = TCPClient.Instance.yDevices[52]   - '0';   // Y34: �ý��� ����




            string output =
            $"CheckingPermissionComplete: {CheckingPermissionComplete}, " +
            $"REDOnGreenOff: {REDOnGreenOff}, " +
            $"ProcessStaying: {ProcessStaying}, " +
            $"UnitySignalWaiting: {UnitySignalWaiting}, " +
            $"StartButtonWaiting: {StartButtonWaiting}, " +
            $"GreenOnRedOff: {GreenOnRedOff}, " +
            $"Gate2_8SectorVacuum: {Gate2_8SectorVacuum}, " +
            $"ProcessReset: {ProcessReset}, " +
            $"RepeatT0: {RepeatT0}, " +
            $"FirstRobotMoveToFOUP: {FirstRobotMoveToFOUP}, " +
            $"FOUPOpening: {FOUPOpening}, " +
            $"LoadingWaferOn1stRobotArm: {LoadingWaferOn1stRobotArm}, " +
            $"FirstVacuumGateOpen: {FirstVacuumGateOpen}, " +
            $"FirstVacuumGateClose: {FirstVacuumGateClose}, " +
            $"AligningComplete: {AligningComplete}, " +
            $"RotationingWafer: {RotationingWafer}, " +
            $"LithographyComplete: {LithographyComplete}, " +
            $"SecondVacuumGateOpen: {SecondVacuumGateOpen}, " +
            $"SecondVacuumGateClose: {SecondVacuumGateClose}, " +
            $"ThirdVacuumGateOpen: {ThirdVacuumGateOpen}, " +
            $"ThirdVacuumGateClose: {ThirdVacuumGateClose}, " +
            $"FourthVacuumGateOpen: {FourthVacuumGateOpen}, " +
            $"FourthVacuumGateClose: {FourthVacuumGateClose}, " +
            $"FifthVacuumGateOpen: {FifthVacuumGateOpen}, " +
            $"FifthVacuumGateClose: {FifthVacuumGateClose}, " +
            $"SixthVacuumGateOpen: {SixthVacuumGateOpen}, " +
            $"SixthVacuumGateClose: {SixthVacuumGateClose}, " +
            $"SeventhVacuumGateOpen: {SeventhVacuumGateOpen}, " +
            $"SeventhVacuumGateClose: {SeventhVacuumGateClose}, " +
            $"EighthVacuumGateOpen: {EighthVacuumGateOpen}, " +
            $"EighthVacuumGateClose: {EighthVacuumGateClose}, " +
            $"LoadLockVacuumRelease: {LoadLockVacuumRelease}, " +
            $"FifthRobotArmMoveToLoadLock: {FifthRobotArmMoveToLoadLock}, " +
            $"FifthRobotArmSetWaferInFOUP: {FifthRobotArmSetWaferInFOUP}, " +
            $"ProcessCycleEnd: {ProcessCycleEnd}, " +
            $"FifthRobotArmComeback: {FifthRobotArmComeback}, " +
            $"SixthRobotArmMoveToLoadLock: {SixthRobotArmMoveToLoadLock}, " +
            $"SixthRobotArmSetWaferInFOUP: {SixthRobotArmSetWaferInFOUP}, " +
            $"SixthRobotArmComeback: {SixthRobotArmComeback}, " +
            $"SeventhRobotArmMoveToLoadLock: {SeventhRobotArmMoveToLoadLock}, " +
            $"SeventhRobotArmSetWaferInFOUP: {SeventhRobotArmSetWaferInFOUP}, " +
            $"SeventhRobotArmComeback: {SeventhRobotArmComeback}, " +
            $"EighthRobotArmMoveToLoadLock: {EighthRobotArmMoveToLoadLock}, " +
            $"EighthRobotArmSetWaferInFOUP: {EighthRobotArmSetWaferInFOUP}, " +
            $"EighthRobotArmComeback: {EighthRobotArmComeback}, " +
            $"EmergencyStop: {EmergencyStop}, " +
            $"SystemReady: {SystemReady}, " +
            $"CleanMode: {CleanMode}, " +
            $"StandbyMode: {StandbyMode}, " +
            $"CalibrationComplete: {CalibrationComplete}, " +
            $"FaultDetected: {FaultDetected}, " +
            $"MaintenanceRequired: {MaintenanceRequired}, " +
            $"SystemShutdown: {SystemShutdown}";        

            print(output);



            if (StartButtonWaiting == 1)
            {
                Manager.OnStartBtnClkEvent();
                StartButtonWaiting = 0;
            }

            if (FirstRobotMoveToFOUP == 1) SemiconRobotControl[0].OnSingleCycleBtnClkEvent(); //Y9
            else if (FirstRobotMoveToFOUP == 0) SemiconRobotControl[0].OnStopBtnClkEvent();

            if (FifthRobotArmMoveToLoadLock == 1) SemiconRobotControl[1].OnSingleCycleBtnClkEvent(); //Y20
            else if (FifthRobotArmMoveToLoadLock == 0) SemiconRobotControl[1].OnStopBtnClkEvent();

            if (SixthRobotArmMoveToLoadLock == 1) SemiconRobotControl[2].OnSingleCycleBtnClkEvent(); //Y24
            else if (SixthRobotArmMoveToLoadLock == 0) SemiconRobotControl[2].OnStopBtnClkEvent();

            if (SeventhRobotArmMoveToLoadLock == 1) SemiconRobotControl[3].OnSingleCycleBtnClkEvent(); //Y27
            else if (SeventhRobotArmMoveToLoadLock == 0) SemiconRobotControl[3].OnStopBtnClkEvent();

            if (EighthRobotArmMoveToLoadLock == 1) SemiconRobotControl[4].OnSingleCycleBtnClkEvent(); //Y2A
            else if (EighthRobotArmMoveToLoadLock == 0) SemiconRobotControl[4].OnStopBtnClkEvent();



            //GateValveDoor OPEN
            if (FirstVacuumGateOpen == 1) GateValveDoor.OnGVUpBtnClkEvent(0);  //YC
            if (SecondVacuumGateOpen == 1) GateValveDoor.OnGVUpBtnClkEvent(1); //Y11
            if (FifthVacuumGateOpen == 1) GateValveDoor.OnGVUpBtnClkEvent(2); //Y17 
            if (SixthVacuumGateOpen == 1) GateValveDoor.OnGVUpBtnClkEvent(3); //Y19
            if (SeventhVacuumGateOpen == 1) GateValveDoor.OnGVUpBtnClkEvent(4); //Y1B
            if (EighthVacuumGateOpen == 1) GateValveDoor.OnGVUpBtnClkEvent(5); //Y1D

            //GateValveDoor Close
            if (FirstVacuumGateClose == 1) GateValveDoor.OnGVDownBtnClkEvent(0); //YD
            if (SecondVacuumGateClose == 1) GateValveDoor.OnGVDownBtnClkEvent(1); //Y12
            if (FifthVacuumGateClose == 1) GateValveDoor.OnGVDownBtnClkEvent(2); //Y18
            if (SixthVacuumGateClose == 1) GateValveDoor.OnGVDownBtnClkEvent(3); //Y1A
            if (SeventhVacuumGateClose == 1) GateValveDoor.OnGVDownBtnClkEvent(4); //Y1C
            if (EighthVacuumGateClose == 1) GateValveDoor.OnGVDownBtnClkEvent(5); //Y1E

            //LITHO_Door Open
             if (ThirdVacuumGateOpen == 1) LithoDoor.OnLithoUpBtnClkEvent(0); //Y13
             if (FourthVacuumGateOpen == 1) LithoDoor.OnLithoUpBtnClkEvent(1); //Y15

            //LITHO_Door close
             if (ThirdVacuumGateClose == 1) LithoDoor.OnLithoDownBtnClkEvent(0); //Y14
             if (FourthVacuumGateClose == 1) LithoDoor.OnLithoDownBtnClkEvent(1); //Y16

             





            /*  //lamps 
              if (�������� == 1) OnLampOnOffBtnClkEvent("Red", true);
              else OnLampOnOffBtnClkEvent("Red", false);

              if (������� == 1) OnLampOnOffBtnClkEvent("Yellow", true);
              else OnLampOnOffBtnClkEvent("Yellow", false);

              if (�ʷϷ��� == 1) OnLampOnOffBtnClkEvent("Green", true);
              else OnLampOnOffBtnClkEvent("Green", false);*/

        }


    }




}