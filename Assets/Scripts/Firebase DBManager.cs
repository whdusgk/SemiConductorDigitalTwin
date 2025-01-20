using Firebase;
using System;
using UnityEngine;
using Firebase.Database;
using System.Threading.Tasks;
using System.Collections.Generic;
using MPS;
using Firebase.Extensions;
using static FirebaseDBManager;
using static FirebaseAuthManager;
using Firebase.Auth;

public class FirebaseDBManager : MonoBehaviour
{
    [SerializeField] string dbURL;
    public DatabaseReference reference;
    private float uploadInterval = 1f;  // 1초마다 업로드
    private float lastUploadTime = 0f;  // 마지막 업로드 시간을 기록
    bool isReceived = false;
    public DatabaseReference dbRef;
    int robot1ActionCount = 0;
    int robot2ActionCount = 0;
    int robot3ActionCount = 0;
    int robot4ActionCount = 0;
    int robot5ActionCount = 0;
    int foupActionCount = 0;
    int lithoActionCount = 0;
    int semActionCount = 0;
    FirebaseAuth auth;
    FirebaseUser user;

    public List<GameObject> canvas = new List<GameObject>();

    // ProcessData 객체 생성
    public ProcessData processData = new ProcessData();


    [Serializable]
    public class ProcessData
    {
        public LoginInfo loginInfo;
        public Process0 process0;
        public Process1 process1;
        public Process2 process2;
        public Process3 process3;
        public Process4 process4;
        public Process5 process5;
        public Process6 process6;
    }

    [Serializable]
    public class Process0
    {
        public float energyConsumption;
        public float temperature;
        public float humidity;
        public string defectiveTotalProducts;
    }

    [Serializable]
    public class Process1
    {
        public string processPosition;
        public bool foupSensor;
        public int foupActionCount;
        public string robot1EndPosition;
        public int robot1ActionCount;
    }

    [Serializable]
    public class Process2
    {
        public string processPosition;
        public bool vacuumSensor;
        public bool GateValve000;
        public bool GateValve001;
        public string robot2EndPosition;
        public int robot2ActionCount;
    }

    [Serializable]
    public class Process3
    {
        public string processPosition;
        public bool vacuumSensor;
        public bool LithoGate000;
        public bool LithoGate001;
        public string alignPosition;
        public string lithoPosition;
        public int lithoActionCount;
    }

    [Serializable]
    public class Process4
    {
        public string processPosition;
        public bool vacuumSensor;
        public bool GateValve002;
        public bool GateValve003;
        public string robot3EndPosition;
        public int robot3ActionCount;
        public bool semSensor;
        public string semPosition;
        public int semActionCount;
        public Results results;
    }

    [Serializable]
    public class Results
    {
        public Chips chips;
        public float defectiveRate;

    }

    [Serializable]
    public class Chips
    {
        public List<float> chipData = new List<float>();
    }
    [Serializable]
    public class Process5
    {
        public string processPosition;
        public bool vacuumSensor;
        public bool GateValve002;
        public bool GateValve003;
        public string robot4EndPosition;
        public int robot4ActionCount;
    }

    [Serializable]
    public class Process6
    {
        public string processPosition;
        public string robot5EndPosition;
        public int robot5ActionCount;
        public string goodProducts;
        public string DefectiveProducts;
    }

    [Serializable]
    public class LoginInfo
    {
        public string loginTime;
    }


    private void Awake()
    {
        dbRef = FirebaseDatabase.DefaultInstance.RootReference;
    }

    void Start()
    {
        FirebaseApp.DefaultInstance.Options.DatabaseUrl = new Uri(dbURL);
        dbRef = FirebaseDatabase.DefaultInstance.RootReference;
        SetLoginTime();
    }

    void Update()
    {
        // 1초마다 Firebase에 데이터를 보내는 로직
        if (Time.time - lastUploadTime >= uploadInterval)
        {
            SendDataToFirebase();
            lastUploadTime = Time.time;  // 마지막 업로드 시간을 업데이트
        }
    }
    /// <summary>
    /// semiconTCPClient의 yDevices의 형식을 내가 원하는 대로 바꿔서 업로드한다.
    /// </summary>
    /// <param name="processDataJson"></param>
    public void UpdateProcessData(string processDataJson)
    {
        dbRef.Child(FirebaseAuthManager.instance.uID).Child("processData").SetRawJsonValueAsync(processDataJson).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                print("성공적으로 업데이트되었습니다.");
            }
            else
            {
                print("업데이트 실패.");
            }
        });
    }

    public void InitializeProcessData()
    {
        // 로그인 정보 설정
        processData.loginInfo = new LoginInfo { loginTime = System.DateTime.Now.ToString() };

        // Process0 정보 설정
        processData.process0 = new Process0
        {
            energyConsumption = float.Parse(UIManager.Instance.EnergyConsumptionInpuField.text),
            temperature = float.Parse(UIManager.Instance.TemperatureInpuField.text),
            humidity = float.Parse(UIManager.Instance.HumidityInpuField.text),
            defectiveTotalProducts = (UIManager.Instance.ProductsText.text),
        };

        // Process1 정보 설정
        processData.process1 = new Process1
        {

            processPosition = UIManager.Instance.ProcessPositionsInpuField[0].text.ToString(), //xyz 
            foupSensor = (SemiconTCPClient.Instance.xDevices[4] - '0') == 1 ? true : false,
            foupActionCount = LPMManager.Instance.LPMcount,
            robot1EndPosition = UIManager.Instance.RobotPositions[0].transform.position.ToString(),
            robot1ActionCount = SemiconMPSManagerTCP.instance.RobotArm[0].cycleCnt
        };

        // Process2 정보 설정
        processData.process2 = new Process2
        {
            processPosition = UIManager.Instance.ProcessPositionsInpuField[1].text.ToString(),
            vacuumSensor = Sensor.Instance.isVacuumOn == true ? true : false,
            GateValve000 = SemiconMPSManagerTCP.instance.isGateValveUp == true ? true : false,
            GateValve001 = SemiconMPSManagerTCP.instance.isGateValveUp == true ? true : false,
            robot2EndPosition = UIManager.Instance.RobotPositions[1].transform.position.ToString(),
            robot2ActionCount = SemiconMPSManagerTCP.instance.RobotArm[1].cycleCnt
        };

        // Process3 정보 설정
        processData.process3 = new Process3
        {
            processPosition = UIManager.Instance.ProcessPositionsInpuField[2].text.ToString(),
            vacuumSensor = Sensor.Instance.isVacuumOn == true ? true : false,
            LithoGate000 = SemiconMPSManagerTCP.instance == true ? true : false,
            LithoGate001 = SemiconMPSManagerTCP.instance == true ? true : false,
            alignPosition = UIManager.Instance.AlignLithoZig1.transform.position.ToString(),
            lithoPosition = UIManager.Instance.AlignLithoZig2.transform.position.ToString(),
            lithoActionCount = SemiconMPSManagerTCP.instance.lithoActionCount,
        };

        // Process4 정보 설정
        processData.process4 = new Process4
        {
            processPosition = UIManager.Instance.ProcessPositionsInpuField[3].text.ToString(),
            vacuumSensor = Sensor.Instance.isVacuumOn == true ? true : false,
            GateValve002 = SemiconMPSManagerTCP.instance == true ? true : false,
            GateValve003 = SemiconMPSManagerTCP.instance == true ? true : false,
            robot3EndPosition = UIManager.Instance.RobotPositions[2].transform.position.ToString(),
            robot3ActionCount = SemiconMPSManagerTCP.instance.RobotArm[2].cycleCnt,
            semSensor = SemiconMPSManagerTCP.instance == true ? true : false,
            semPosition = UIManager.Instance.SEMPositionInpuField.transform.position.ToString(),
            semActionCount = SEMManager.Instance.semActCount,
            results = new Results
            {
                defectiveRate = SEMManager.Instance.DefectiveProductCount,

                chips = new Chips
                {
                    chipData = SEMManager.Instance.ChipData,
                }

            }
        };

        // Process5 정보 설정
        processData.process5 = new Process5
        {
            processPosition = UIManager.Instance.ProcessPositionsInpuField[4].text.ToString(),
            vacuumSensor = Sensor.Instance.isVacuumOn == true ? true : false,
            GateValve002 = SemiconMPSManagerTCP.instance == true ? true : false,
            GateValve003 = SemiconMPSManagerTCP.instance == true ? true : false,
            robot4EndPosition = UIManager.Instance.RobotPositions[3].transform.position.ToString(),
            robot4ActionCount = SemiconMPSManagerTCP.instance.RobotArm[3].cycleCnt
        };

        // Process6 정보 설정
        processData.process6 = new Process6
        {
            processPosition = UIManager.Instance.ProcessPositionsInpuField[5].transform.position.ToString(),
            robot5EndPosition = UIManager.Instance.RobotPositions[4].transform.position.ToString(),
            robot5ActionCount = SemiconMPSManagerTCP.instance.RobotArm[4].cycleCnt,
            goodProducts = UIManager.Instance.GoodProductInpuField.text.ToString(),
            DefectiveProducts = UIManager.Instance.DefectiveProductInpuField.text.ToString()
        };

    }
    public void UpdateData()
    {
        // 로그인
        processData.loginInfo.loginTime = System.DateTime.Now.ToString();

        // Process0
        processData.process0.energyConsumption = float.Parse(UIManager.Instance.EnergyConsumptionInpuField.text);
        processData.process0.temperature = float.Parse(UIManager.Instance.TemperatureInpuField.text);
        processData.process0.humidity = float.Parse(UIManager.Instance.HumidityInpuField.text);
        processData.process0.defectiveTotalProducts = (UIManager.Instance.ProductsText.text);

        // Process1
        //if (canvas[1].active == true)
        processData.process1.processPosition = UIManager.Instance.ProcessPositionsInpuField[0].text.ToString();
        processData.process1.foupSensor = (SemiconTCPClient.Instance.xDevices[4] - '0') == 1 ? true : false;
        processData.process1.foupActionCount = LPMManager.Instance.LPMcount; //y16
        processData.process1.robot1EndPosition = UIManager.Instance.RobotPositions[0].transform.position.ToString();
        processData.process1.robot1ActionCount = SemiconMPSManagerTCP.instance.RobotArm[0].cycleCnt;


        // Process2
        processData.process2.processPosition = UIManager.Instance.ProcessPositionsInpuField[1].text.ToString();
        processData.process2.vacuumSensor = Sensor.Instance.isVacuumOn == true ? true : false;
        processData.process2.GateValve000 = SemiconMPSManagerTCP.instance.isGateValveUp == true ? true : false;
        processData.process2.GateValve001 = SemiconMPSManagerTCP.instance.isGateValveUp == true ? true : false;
        processData.process2.robot2EndPosition = UIManager.Instance.RobotPositions[1].transform.position.ToString();
        processData.process2.robot2ActionCount = SemiconMPSManagerTCP.instance.RobotArm[1].cycleCnt;

        // Process3 
        processData.process3.processPosition = UIManager.Instance.ProcessPositionsInpuField[2].text.ToString();
        processData.process3.vacuumSensor = Sensor.Instance.isVacuumOn == true ? true : false;
        processData.process3.LithoGate000 = SemiconMPSManagerTCP.instance == true ? true : false;
        processData.process3.LithoGate001 = SemiconMPSManagerTCP.instance == true ? true : false;
        processData.process3.alignPosition = UIManager.Instance.AlignLithoZig1.transform.position.ToString();
        processData.process3.lithoPosition = UIManager.Instance.AlignLithoZig2.transform.position.ToString();
        processData.process3.lithoActionCount = SemiconMPSManagerTCP.instance.lithoActionCount; //y12

        // Process4
        processData.process4.processPosition = UIManager.Instance.ProcessPositionsInpuField[3].text.ToString();
        processData.process4.vacuumSensor = Sensor.Instance.isVacuumOn == true ? true : false;
        processData.process4.GateValve002 = SemiconMPSManagerTCP.instance == true ? true : false;
        processData.process4.GateValve003 = SemiconMPSManagerTCP.instance == true ? true : false;
        processData.process4.robot3EndPosition = UIManager.Instance.RobotPositions[2].transform.position.ToString();
        processData.process4.robot3ActionCount = SemiconMPSManagerTCP.instance.RobotArm[2].cycleCnt;
        processData.process4.semSensor = SemiconMPSManagerTCP.instance == true ? true : false;
        processData.process4.semPosition = UIManager.Instance.SEMPositionInpuField.transform.position.ToString();
        processData.process4.semActionCount = SEMManager.Instance.semActCount; //y15
        processData.process4.results.defectiveRate = SEMManager.Instance.DefectiveProductCount;
        //processData.process4.results.chips.chipData[0] = 0.0f;
        for (int i = 0; i < 37; i++)
        {
            if (SEMManager.Instance.ChipData != null)
                processData.process4.results.chips.chipData[i] = 0.0f;//SEMManager.Instance.ChipData[i];
        }

        // Process5 정보 설정
        processData.process5.processPosition = UIManager.Instance.ProcessPositionsInpuField[4].text.ToString();
        processData.process5.vacuumSensor = Sensor.Instance.isVacuumOn == true ? true : false;
        processData.process5.GateValve002 = SemiconMPSManagerTCP.instance == true ? true : false;
        processData.process5.GateValve003 = SemiconMPSManagerTCP.instance == true ? true : false;
        processData.process5.robot4EndPosition = UIManager.Instance.RobotPositions[3].transform.position.ToString();
        processData.process5.robot4ActionCount = SemiconMPSManagerTCP.instance.RobotArm[3].cycleCnt;

        // Process6 정보 설정
        processData.process6.processPosition = UIManager.Instance.ProcessPositionsInpuField[5].text.ToString();
        processData.process6.robot5EndPosition = UIManager.Instance.RobotPositions[4].transform.position.ToString();
        processData.process6.robot5ActionCount = SemiconMPSManagerTCP.instance.RobotArm[4].cycleCnt;
        processData.process6.goodProducts = UIManager.Instance.GoodProductInpuField.text.ToString();
        processData.process6.DefectiveProducts = UIManager.Instance.DefectiveProductInpuField.text.ToString();
    }
    public void SendDataToFirebase()
    {
        UpdateData();
        // processData를 JSON 문자열로 변환
        string processDataJson = JsonUtility.ToJson(processData);

        // Firebase에 JSON 데이터를 전송
        UpdateProcessData(processDataJson); // Firebase로 JSON 데이터를 보내는 메서드 호출
    }

    /* public void QueryJsonFile()
     {
         string ProcessData = @"{

                           ""process0"": {
                            ""energyConsumption"": 0.0,
                            ""temperature"": 0.0,
                            ""humidity"": 0.0,
                            ""defectiveTotalProducts"": ""0/0""
                          },
                          ""process1"": {
                            ""processPosition"": """",
                            ""foupSensor"": true,
                            ""foupActionCount"": 0,
                            ""robot1EndPosition"": """",
                            ""robot1ActionCount"": 0
                          },
                          ""process2"": {
                            ""processPosition"": """",
                            ""vacuumSensor"": true,
                            ""loadlockSensor"": true,
                            ""robot2EndPosition"": """",
                            ""robot2ActionCount"": 0
                          },
                          ""process3"": {
                            ""processPosition"": """",
                            ""vacuumSensor"": true,
                            ""alignSensor"": true,
                            ""alignPosition"": """",
                            ""alignActionCount"": 0,
                            ""lithoSensor"": true,
                            ""lithoPosition"": """",
                            ""lithoActionCount"": 0
                          },
                          ""process4"": {
                            ""processPosition"": """",
                            ""vacuumSensor"": true,
                            ""robot3EndPosition"": """",
                            ""robot3ActionCount"": 0,
                            ""semSensor"": true,
                            ""semPosition"": """",
                            ""semActionCount"": 0,
                            ""results"": {
                              ""chipData"": 0.0,
                              ""defectiveRate"": 0.0,
                              ""good"": true,
                              ""defective"": true
                            }
                          },
                          ""process5"": {
                            ""processPosition"": """",
                            ""vacuumSensor"": true,
                            ""loadlockSensor"": true,
                            ""robot4EndPosition"": """",
                            ""robot4ActionCount"": 0
                          },
                          ""process6"": {
                            ""processPosition"": """",
                            ""robot5EndPosition"": """",
                            ""robot5ActionCount"": 0,
                            ""foupSensor"": true,
                            ""goodProductsDefectiveProducts"": ""0/0""
                          }
                      }";


         print(ProcessData);
     }// firebase에서 데이터를 가져오는 형식 출력 / 기본 데이터 포맷*/

    public void SetLoginTime()
    {

        LoginInfo loginInfo = new LoginInfo()
        {
            loginTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
        };



        string json = JsonUtility.ToJson(loginInfo);
        dbRef.Child(FirebaseAuthManager.instance.uID).Child("loginInfo").SetRawJsonValueAsync(json)
            .ContinueWith(task => {
                if (task.IsCompleted)
                {
                    print("로그인 시간 기록 완료");
                }
                else
                {
                    print("로그인 시간 기록 실패");
                }
            });
       
    }
}
