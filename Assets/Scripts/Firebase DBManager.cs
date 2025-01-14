using Firebase;
using System;
using UnityEngine;
using Firebase.Database;
using System.Collections.Generic;
using static FirebaseDBManager;
using System.Diagnostics;

public class FirebaseDBManager : MonoBehaviour
{

    [SerializeField] string dbURL;

    private int[] robotActionCounts = new int[5]; // 로봇 1~5의 동작 횟수를 저장할 배열
    bool isReceived = false;

   /* public string robot1EndPosition;  // 로봇의 위치
    public int robot1ActionCount;  // 로봇의 동작 횟수*/

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
        public float energyConsumption;  // 에너지 소비
        public float temperature;  // 온도
        public float humidity;  // 습도
        public string defectiveTotalProducts;  // 불량/총 제품
    }

    [Serializable]
    public class Process1
    {
        public string processPosition;  // 프로세스 위치
        public bool foupSensor;  // FOUP 센서
        public int foupActionCount;  // FOUP 동작 횟수
        public string robot1EndPosition;  // 로봇1 끝 위치
        public int robot1ActionCount;  // 로봇1 동작 횟수
    }

    [Serializable]
    public class Process2
    {
        public string processPosition;  // 프로세스 위치
        public bool vacuumSensor;  // 진공 센서
        public bool loadlockSensor;  // 로드락 센서
        public string robot2EndPosition;  // 로봇2 끝 위치
        public int robot2ActionCount;  // 로봇2 동작 횟수
    }

    [Serializable]
    public class Process3
    {
        public string processPosition;  // 프로세스 위치
        public bool vacuumSensor;  // 진공 센서
        public bool alignSensor;  // 정렬 센서
        public string alignPosition;  // 정렬 위치
        public int alignActionCount;  // 정렬 동작 횟수
        public bool lithoSensor;  // 리소그래피 센서
        public string lithoPosition;  // 리소그래피 위치
        public int lithoActionCount;  // 리소그래피 동작 횟수
    }

    [Serializable]
    public class Process4
    {
        public string processPosition;  // 프로세스 위치
        public bool vacuumSensor;  // 진공 센서
        public string robot3EndPosition;  // 로봇3 끝 위치
        public int robot3ActionCount;  // 로봇3 동작 횟수
        public bool semSensor;  // SEM 센서
        public string semPosition;  // SEM 위치
        public int semActionCount;  // SEM 동작 횟수
        public Results results;  // 결과
    }
    [Serializable]
    public class Results
    {
        public float chipData;  // 칩 데이터
        public float defectiveRate;  // 불량률
        public bool good;  // 양품 여부
        public bool defective;  // 불량 여부
    }

    [Serializable]
    public class Process5
    {
        public string processPosition;  // 프로세스 위치
        public bool vacuumSensor;  // 진공 센서
        public bool loadlockSensor;  // 로드락 센서
        public string robot4EndPosition;  // 로봇4 끝 위치
        public int robot4ActionCount;  // 로봇4 동작 횟수
    }

    [Serializable]
    public class Process6
    {
        public string processPosition;  // 프로세스 위치
        public string robot5EndPosition;  // 로봇5 끝 위치
        public int robot5ActionCount;  // 로봇5 동작 횟수
        public bool foupSensor;  // FOUP 센서
        public string goodProductsDefectiveProducts;  // 양품/불량 제품
    }

    [Serializable]
    public class LoginInfo
    {
        public string loginTime;
    }

    void Start()
    {
        FirebaseApp.DefaultInstance.Options.DatabaseUrl = new System.Uri(dbURL);
        SetRawJsonValueAsync();
        QueryJsonFile();
        SetLoginTime();
    }

  
    void Update()
    {
        Vector3 robotPosition = transform.position;  // 현재 객체의 위치를 가져오기
        int actionCount0 = GetRobotActionCount(0);  // 로봇의 동작 횟수를 얻어오는 메서드
        int actionCount1 = GetRobotActionCount(1);  // 로봇의 동작 횟수를 얻어오는 메서드
        int actionCount2 = GetRobotActionCount(2);  // 로봇의 동작 횟수를 얻어오는 메서드
        int actionCount3 = GetRobotActionCount(3);  // 로봇의 동작 횟수를 얻어오는 메서드
        int actionCount4 = GetRobotActionCount(4);  // 로봇의 동작 횟수를 얻어오는 메서드

        UpdateProcessData();
    }

    // 로봇 동작 횟수를 반환하는 메서드
    public int GetRobotActionCount(int robotIndex)
    {
        return robotActionCounts[robotIndex];
    } //robotactioncounts 배열에 인덱스에 해당하는 로봇의 동작 횟수를 반환

    void SetRawJsonValueAsync()
    {
        DatabaseReference dbRef = FirebaseDatabase.DefaultInstance.RootReference;

        // JSON 파일 포멧
        string json = @"{
            ""array"":[
                1,
                2,
                3
            ],
            ""boolean"":true,
            ""color"":""gold"",
            ""null"":null,
            ""number"":123,
            ""object"":{
                ""a"":""b"",
                ""c"":""d""
            },
            ""string"":""Hello World""
        }";

        dbRef.SetRawJsonValueAsync(json);  // 여기서 Firebase DB를 초기화함
    } // firebase에 업로드할 josn 데이터를 설정하고, firebase에 업로드하는 함수
    void SetRawJsonValueAsync(string jsonFile)
    {
        DatabaseReference dbRef = FirebaseDatabase.DefaultInstance.RootReference;

        dbRef.SetRawJsonValueAsync(jsonFile);
    } 

    public void UploadProcessData(ProcessData data) //processdata 객체를 json 형식으로 변환하여 firebase에 업로드
    {
        // 데이터를 JSON으로 직렬화
        string json = JsonUtility.ToJson(data);

        DatabaseReference dbRef = FirebaseDatabase.DefaultInstance.RootReference;

        // Firebase에 저장
        dbRef.Child("processData").SetRawJsonValueAsync(json)
                .ContinueWith(task => {
                if (task.IsCompleted)
                {
                    print("데이터가 성공적으로 업로드됨");
                }
                else
                {
                    print("데이터 업로드 실패");
                }
            });
    } 

    public void QueryJsonFile()
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
    }// firebase에서 데이터를 가져오는 형식 출력 / 기본 데이터 포맷
   
    public void UpdateProcessData()
    {
        DatabaseReference dbRef = FirebaseDatabase.DefaultInstance.RootReference;

       
        ProcessData processData = new ProcessData()
        {
            process0 = new Process0()
            {
                energyConsumption = 100.0f,  // 예시 값
                temperature = 25.0f,  // 예시 값
                humidity = 60.0f,  // 예시 값
                defectiveTotalProducts = "10/100"  // 예시 값
            },
            process1 = new Process1()
            {
                processPosition = "Position1",  // 예시 값
                foupSensor = true,  // 예시 값
                foupActionCount = 5,  // 예시 값
                robot1EndPosition = "EndPos1",  // 예시 값
                robot1ActionCount = 10  // 예시 값
            },
            process2 = new Process2()
            {
                processPosition = "Position2",  // 예시 값
                vacuumSensor = true,  // 예시 값
                loadlockSensor = true,  // 예시 값
                robot2EndPosition = "EndPos2",  // 예시 값
                robot2ActionCount = 20  // 예시 값
            },
            process3 = new Process3()
            {
                processPosition = "Position3",  // 예시 값
                vacuumSensor = true,  // 예시 값
                alignSensor = true,  // 예시 값
                alignPosition = "AlignPos1",  // 예시 값
                alignActionCount = 30,  // 예시 값
                lithoSensor = true,  // 예시 값
                lithoPosition = "LithoPos1",  // 예시 값
                lithoActionCount = 40  // 예시 값
            },
            process4 = new Process4()
            {
                processPosition = "Position4",  // 예시 값
                vacuumSensor = true,  // 예시 값
                robot3EndPosition = "EndPos3",  // 예시 값
                robot3ActionCount = 50,  // 예시 값
                semSensor = true,  // 예시 값
                semPosition = "SEMPos1",  // 예시 값
                semActionCount = 60,  // 예시 값
                results = new Results()
                {
                    chipData = 100.0f,  // 예시 값
                    defectiveRate = 5.0f,  // 예시 값
                    good = true,  // 예시 값
                    defective = false  // 예시 값
                }
            },
            process5 = new Process5()
            {
                processPosition = "Position5",  // 예시 값
                vacuumSensor = true,  // 예시 값
                loadlockSensor = true,  // 예시 값
                robot4EndPosition = "EndPos4",  // 예시 값
                robot4ActionCount = 70  // 예시 값
            },
            process6 = new Process6()
            {
                processPosition = "Position6",  // 예시 값
                robot5EndPosition = "EndPos5",  // 예시 값
                robot5ActionCount = 80,  // 예시 값
                foupSensor = true,  // 예시 값
                goodProductsDefectiveProducts = "100/10"  // 예시 값
            }
        };
    

        // 데이터를 JSON으로 직렬화
        string json = JsonUtility.ToJson(processData);

        // Firebase에 저장
        dbRef.Child("processData").SetRawJsonValueAsync(json)
            .ContinueWith(task => {
                if (task.IsCompleted)
                {
                    print("데이터가 성공적으로 업로드됨");
                }
                else
                {
                    print("데이터 업로드 실패");
                }
            });
    } 

    public void SetLoginTime()
    {
        DatabaseReference dbRef = FirebaseDatabase.DefaultInstance.RootReference;
        LoginInfo loginInfo = new LoginInfo()
        {
            loginTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
        };

        string json = JsonUtility.ToJson(loginInfo);
        dbRef.Child("loginInfo").SetRawJsonValueAsync(json)
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
    } //로그인한 시간을 LoginInfo 객체에 담아 Firebase에 저장합니다
}