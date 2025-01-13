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
            // X 디바이스 값 생성
            string xDeviceValue = $"{startBtnState}" +
                                    $"{(sensors[0].isEnabled ? 1 : 0)}" +    // X0: Power On (Rising Pulse)
                                    $"{(sensors[1].isEnabled ? 1 : 0)}" +    // X1: Permission Clear (권한 확인 완료)
                                    $"{(startBtnState == 0 ? 1 : 0)}" +      // X2: Start Button Red Off (공정 가동 버튼, 적색등 Off)
                                    $"{(sensors[2].isEnabled ? 1 : 0)}" +    // X3: Vacuum Fail (진공 공정 구간의 진공 상태 감지)
                                    $"{(sensors[3].isEnabled ? 1 : 0)}" +    // X4: Arrived FOUP (FOUP 도착 감지)
                                    $"{(sensors[4].isEnabled ? 1 : 0)}" +    // X5: FOUP_Fail_Or_Wafer_Run_Out (FOUP 개폐 실패 또는 Wafer 소진)
                                    $"{(sensors[5].isEnabled ? 1 : 0)}" +    // X6: 1st Robot Arm Wafer Loading Fail (1번 로봇 팔 웨이퍼 적재 실패)
                                    $"{(sensors[6].isEnabled ? 1 : 0)}" +    // X7: 1st Vacuum Gate Open Fail (1번 진공문 Open 실패)
                                    $"{(sensors[7].isEnabled ? 1 : 0)}" +    // X8: 1st Robot Arm Set Wafer Fail (1번 로봇 팔 Load Lock에 Wafer 적재 실패)
                                    $"{(sensors[8].isEnabled ? 1 : 0)}" +    // X9: Wafer Run Out in FOUP (FOUP 내 Wafer 소진)
                                    $"{(sensors[9].isEnabled ? 1 : 0)}" +    // X10: 3rd Vacuum Gate Close Fail (세 번째 진공문 Close 실패)
                                    $"{(sensors[10].isEnabled ? 1 : 0)}" +   // X0A: 1st Gate Close Or Vacuum Fail (1번 진공문 Close 혹은 Load Lock 진공화 실패)
                                    $"{(sensors[11].isEnabled ? 1 : 0)}" +   // X0B: 2nd Vacuum Gate Open Fail (2번 진공문 Open 실패)
                                    $"{(sensors[12].isEnabled ? 1 : 0)}" +   // X0C: 2nd Robot Arm Wafer Loading Fail (2번 로봇 팔 웨이퍼 적재 실패)
                                    $"{(sensors[13].isEnabled ? 1 : 0)}" +   // X0D: 3rd Vacuum Gate Open Fail (3번 진공문 Open 실패)
                                    $"{(sensors[14].isEnabled ? 1 : 0)}" +   // X0E: 2nd Vacuum Gate Close Fail (2번 진공문 Close 실패)
                                    $"{(sensors[15].isEnabled ? 1 : 0)}";    // X0F: 2nd Arm Set Wafer To Align Fail (2번 로봇 팔 Align 위치에 Wafer Set 실패)

            Debug.Log("X Device Values: " + xDeviceValue);
    }*/

        void UpdateYDevices()
        {
            // TCPClient가 연결되지 않았다면 함수 실행을 종료
            if (TCPClient.Instance.isConnected == false)
                return;

            // yDevices 배열에 데이터가 없다면 함수 실행을 종료
            if (TCPClient.Instance.yDevices.Length == 0)
                return;

            /* // 신호 선언 (Y0 ~ Y1A)
             int CheckingPermission          = TCPClient.Instance.yDevices[0]  - '0';   // Y0: 권한 확인
             int REDOnGreenOff               = TCPClient.Instance.yDevices[1]  - '0';   // Y1: 빨간불 켜고 초록불 끄기
             int ProcessStaying              = TCPClient.Instance.yDevices[2]  - '0';   // Y2: 공정 대기
             int UnitySignalWaiting          = TCPClient.Instance.yDevices[3]  - '0';   // Y3: 유니티 신호 대기
             int StartButtonWaiting          = TCPClient.Instance.yDevices[4]  - '0';   // Y4: 시작 버튼 대기
             int GreenOnRedOff               = TCPClient.Instance.yDevices[5]  - '0';   // Y5: 초록불 켜고 빨간불 끄기
             int Gate2_8SectorVacuum         = TCPClient.Instance.yDevices[6]  - '0';   // Y6: Gate2-8 섹터 진공
             int ProcessReset                = TCPClient.Instance.yDevices[7]  - '0';   // Y7: 공정 리셋
             int RepeatT0                    = TCPClient.Instance.yDevices[8]  - '0';   // Y8: T0 반복
             int Robot1MoveToFOUP            = TCPClient.Instance.yDevices[9]  - '0';   // Y9: 첫 번째 로봇 FOUP로 이동
             int T2SelfHoldingAndY9Off       = TCPClient.Instance.yDevices[10] - '0';   // Y0A: T2 셀프 홀딩 및 Y9 꺼짐
             int VacuumGate1Open             = TCPClient.Instance.yDevices[11] - '0';   // Y0B: 첫 번째 진공 게이트 열기
             int T3SelfHoldingAndT2Off       = TCPClient.Instance.yDevices[12] - '0';   // Y0C: T3 셀프 홀딩 및 T2 꺼짐
             int DetectingWaferRunOut        = TCPClient.Instance.yDevices[13] - '0';   // Y0D: FOUP에서 웨이퍼 고갈 감지
             int T4SelfHoldingAndT3Off       = TCPClient.Instance.yDevices[14] - '0';   // Y0E: T4 셀프 홀딩 및 T3 꺼짐
             int T5SelfHoldingAndT4Off       = TCPClient.Instance.yDevices[15] - '0';   // Y0F: T5 셀프 홀딩 및 T4 꺼짐
             int RestartRelayP1              = TCPClient.Instance.yDevices[16] - '0';   // Y10: P1 리셋 릴레이
             int T7SelfHoldingAndT5Off       = TCPClient.Instance.yDevices[17] - '0';   // Y11: T7 셀프 홀딩 및 T5 꺼짐
             int VacuumGate2Open             = TCPClient.Instance.yDevices[18] - '0';   // Y12: 두 번째 진공 게이트 열기
             int T8SelfHoldingAndT7Off       = TCPClient.Instance.yDevices[19] - '0';   // Y13: T8 셀프 홀딩 및 T7 꺼짐
             int T9SelfHoldingAndT8Off       = TCPClient.Instance.yDevices[20] - '0';   // Y14: T9 셀프 홀딩 및 T8 꺼짐
             int VacuumGate3Open             = TCPClient.Instance.yDevices[21] - '0';   // Y15: 세 번째 진공 게이트 열기
             int T10SelfHoldingAndT9Off      = TCPClient.Instance.yDevices[22] - '0';   // Y16: T10 셀프 홀딩 및 T9 꺼짐
             int T11SelfHoldingAndT10Off     = TCPClient.Instance.yDevices[23] - '0';   // Y17: T11 셀프 홀딩 및 T10 꺼짐
             int T12SelfHoldingAndT11Off     = TCPClient.Instance.yDevices[24] - '0';   // Y18: T12 셀프 홀딩 및 T11 꺼짐
             int T13SelfHoldingAndT12Off     = TCPClient.Instance.yDevices[25] - '0';   // Y19: T13 셀프 홀딩 및 T12 꺼짐
             int VacuumGate3Close            = TCPClient.Instance.yDevices[26] - '0';   // Y1A: 세 번째 진공 게이트 닫기*/

            int CheckingPermissionComplete      = TCPClient.Instance.yDevices[0]    - '0';   // Y0: 공정 가동 권한 확인 중
            int REDOnGreenOff                   = TCPClient.Instance.yDevices[1]    - '0';   // Y1: 빨간불 켜고 초록불 끄기
            int ProcessStaying                  = TCPClient.Instance.yDevices[2]    - '0';   // Y2: 공정 대기
            int UnitySignalWaiting              = TCPClient.Instance.yDevices[3]    - '0';   // Y3: 유니티 측의 연결 신호 대기
            int StartButtonWaiting              = TCPClient.Instance.yDevices[4]    - '0';   // Y4: 공정 시작 버튼 대기
            int GreenOnRedOff                  = TCPClient.Instance.yDevices[5]    - '0';   // Y5: 공정 정상 작동 상태
            int Gate2_8SectorVacuum            = TCPClient.Instance.yDevices[6]    - '0';   // Y6: 2번문 ~ 8번문 사이 진공 처리
            int ProcessReset                   = TCPClient.Instance.yDevices[7]    - '0';   // Y7: 공정 초기화
            int RepeatT0                        = TCPClient.Instance.yDevices[8]    - '0';   // Y8: FOUP 감지 실패 시 T0 반복
            int FirstRobotMoveToFOUP           = TCPClient.Instance.yDevices[9]    - '0';   // Y9: 1번 로봇팔 FOUP으로 이동
            int FOUPOpening                     = TCPClient.Instance.yDevices[10]   - '0';   // YA: FOUP 개방
            int LoadingWaferOn1stRobotArm      = TCPClient.Instance.yDevices[11]   - '0';   // YB: 1번 로봇팔에 웨이퍼 적재
            int FirstVacuumGateOpen            = TCPClient.Instance.yDevices[12]   - '0';   // YC: 1번 진공문 열기 1
            int FirstVacuumGateClose           = TCPClient.Instance.yDevices[13]   - '0';   // YD: 1번 진공문 닫기 1
            int AligningComplete               = TCPClient.Instance.yDevices[14]   - '0';   // YE: Aligning 완료
            int RotationingWafer               = TCPClient.Instance.yDevices[15]   - '0';   // YF: Wafer 회전 진행 중
            int LithographyComplete            = TCPClient.Instance.yDevices[16]   - '0';   // Y10: Lithography 완료
            int SecondVacuumGateOpen          = TCPClient.Instance.yDevices[17]   - '0';   // Y11: 2번 진공문 열기 2
            int SecondVacuumGateClose         = TCPClient.Instance.yDevices[18]   - '0';   // Y12: 2번 진공문 닫기 2
            int ThirdVacuumGateOpen           = TCPClient.Instance.yDevices[19]   - '0';   // Y13: 3번 진공문 열기  01 
            int ThirdVacuumGateClose          = TCPClient.Instance.yDevices[20]   - '0';   // Y14: 3번 진공문 닫기  001 
            int FourthVacuumGateOpen          = TCPClient.Instance.yDevices[21]   - '0';   // Y15: 4번 진공문 열기  02
            int FourthVacuumGateClose         = TCPClient.Instance.yDevices[22]   - '0';   // Y16: 4번 진공문 닫기  002
            int FifthVacuumGateOpen           = TCPClient.Instance.yDevices[23]   - '0';   // Y17: 5번 진공문 열기 3
            int FifthVacuumGateClose          = TCPClient.Instance.yDevices[24]   - '0';   // Y18: 5번 진공문 닫기 3
            int SixthVacuumGateOpen           = TCPClient.Instance.yDevices[25]   - '0';   // Y19: 6번 진공문 열기 4
            int SixthVacuumGateClose          = TCPClient.Instance.yDevices[26]   - '0';   // Y1A: 6번 진공문 닫기 4
            int SeventhVacuumGateOpen         = TCPClient.Instance.yDevices[27]   - '0';   // Y1B: 7번 진공문 열기 5
            int SeventhVacuumGateClose        = TCPClient.Instance.yDevices[28]   - '0';   // Y1C: 7번 진공문 닫기 5
            int EighthVacuumGateOpen          = TCPClient.Instance.yDevices[29]   - '0';   // Y1D: 8번 진공문 열기 6
            int EighthVacuumGateClose         = TCPClient.Instance.yDevices[30]   - '0';   // Y1E: 8번 진공문 닫기 6
            int LoadLockVacuumRelease         = TCPClient.Instance.yDevices[31]   - '0';   // Y1F: Load Lock 진공 해제
            int FifthRobotArmMoveToLoadLock   = TCPClient.Instance.yDevices[32]   - '0';   // Y20: 2번 로봇팔 Load Lock으로 이동
            int FifthRobotArmSetWaferInFOUP   = TCPClient.Instance.yDevices[33]   - '0';   // Y21: 2번 로봇팔 FOUP에 웨이퍼 적재
            int ProcessCycleEnd               = TCPClient.Instance.yDevices[34]   - '0';   // Y22: 공정 종료
            int FifthRobotArmComeback         = TCPClient.Instance.yDevices[35]   - '0';   // Y23: 2번 로봇팔 복귀
            int SixthRobotArmMoveToLoadLock   = TCPClient.Instance.yDevices[36]   - '0';   // Y24: 3번 로봇팔 Load Lock으로 이동
            int SixthRobotArmSetWaferInFOUP   = TCPClient.Instance.yDevices[37]   - '0';   // Y25: 3번 로봇팔 FOUP에 웨이퍼 적재
            int SixthRobotArmComeback         = TCPClient.Instance.yDevices[38]   - '0';   // Y26: 3번 로봇팔 복귀
            int SeventhRobotArmMoveToLoadLock = TCPClient.Instance.yDevices[39]   - '0';   // Y27: 4번 로봇팔 Load Lock으로 이동
            int SeventhRobotArmSetWaferInFOUP = TCPClient.Instance.yDevices[40]   - '0';   // Y28: 4번 로봇팔 FOUP에 웨이퍼 적재
            int SeventhRobotArmComeback       = TCPClient.Instance.yDevices[41]   - '0';   // Y29: 4번 로봇팔 복귀
            int EighthRobotArmMoveToLoadLock  = TCPClient.Instance.yDevices[42]   - '0';   // Y2A: 5번 로봇팔 Load Lock으로 이동
            int EighthRobotArmSetWaferInFOUP  = TCPClient.Instance.yDevices[43]   - '0';   // Y2B: 5번 로봇팔 FOUP에 웨이퍼 적재
            int EighthRobotArmComeback        = TCPClient.Instance.yDevices[44]   - '0';   // Y2C: 5번 로봇팔 복귀
            int EmergencyStop                 = TCPClient.Instance.yDevices[45]   - '0';   // Y2D: 비상 정지
            int SystemReady                   = TCPClient.Instance.yDevices[46]   - '0';   // Y2E: 시스템 준비 완료
            int CleanMode                     = TCPClient.Instance.yDevices[47]   - '0';   // Y2F: 클린 모드
            int StandbyMode                   = TCPClient.Instance.yDevices[48]   - '0';   // Y30: 대기 모드
            int CalibrationComplete           = TCPClient.Instance.yDevices[49]   - '0';   // Y31: 교정 완료
            int FaultDetected                 = TCPClient.Instance.yDevices[50]   - '0';   // Y32: 고장 감지
            int MaintenanceRequired           = TCPClient.Instance.yDevices[51]   - '0';   // Y33: 유지보수 필요
            int SystemShutdown                = TCPClient.Instance.yDevices[52]   - '0';   // Y34: 시스템 종료




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
              if (빨강램프 == 1) OnLampOnOffBtnClkEvent("Red", true);
              else OnLampOnOffBtnClkEvent("Red", false);

              if (노랑램프 == 1) OnLampOnOffBtnClkEvent("Yellow", true);
              else OnLampOnOffBtnClkEvent("Yellow", false);

              if (초록램프 == 1) OnLampOnOffBtnClkEvent("Green", true);
              else OnLampOnOffBtnClkEvent("Green", false);*/

        }


    }




}