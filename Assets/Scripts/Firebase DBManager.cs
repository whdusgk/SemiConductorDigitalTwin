using Firebase;
using System;
using UnityEngine;
using Firebase.Database;
using System.Threading.Tasks;
using System.Collections.Generic;
using MPS;
using Firebase.Extensions;
using static FirebaseDBManager;

public class FirebaseDBManager : MonoBehaviour
{
    [SerializeField] string dbURL;
    private DatabaseReference reference;
    private float uploadInterval = 1.0f;  // 1초마다 업로드
   
    bool isReceived = false;
    DatabaseReference dbRef;

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
        public bool loadlockSensor;
        public string robot2EndPosition;
        public int robot2ActionCount;
    }

    [Serializable]
    public class Process3
    {
        public string processPosition;
        public bool vacuumSensor;
        public bool alignSensor;
        public string alignPosition;
        public int alignActionCount;
        public bool lithoSensor;
        public string lithoPosition;
        public int lithoActionCount;
    }

    [Serializable]
    public class Process4
    {
        public string processPosition;
        public bool vacuumSensor;
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
        public float chipData;
        public float defectiveRate;
        public bool good;
        public bool defective;
    }

    [Serializable]
    public class Process5
    {
        public string processPosition;
        public bool vacuumSensor;
        public bool loadlockSensor;
        public string robot4EndPosition;
        public int robot4ActionCount;
    }

    [Serializable]
    public class Process6
    {
        public string processPosition;
        public string robot5EndPosition;
        public int robot5ActionCount;
        public bool foupSensor;
        public string goodProductsDefectiveProducts;
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

    }

    void Update()
    {
        // 일정 간격으로 데이터 업로드 처리
        uploadInterval -= Time.deltaTime;
        if (uploadInterval <= 0)
        {
            //UpdateProcessData();
            uploadInterval = 1.0f;  // 업로드 간격 재설정
        }
    }

  /*  public void UploadProcessData(ProcessData data) 
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
    }*/

    public void UpdateProcessData(string processDataJson)
    {
        dbRef.Child("processData").SetRawJsonValueAsync(processDataJson).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                print("Process data updated successfully.");
            }
            else
            {
                print("Failed to update process data.");
            }
        });
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
    }
 

}
