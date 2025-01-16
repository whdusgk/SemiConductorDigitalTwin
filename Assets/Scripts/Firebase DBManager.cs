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

    private int[] robotActionCounts = new int[5]; // �κ� 1~5�� ���� Ƚ���� ������ �迭
    bool isReceived = false;

   /* public string robot1EndPosition;  // �κ��� ��ġ
    public int robot1ActionCount;  // �κ��� ���� Ƚ��*/

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
        public float energyConsumption;  // ������ �Һ�
        public float temperature;  // �µ�
        public float humidity;  // ����
        public string defectiveTotalProducts;  // �ҷ�/�� ��ǰ
    }

    [Serializable]
    public class Process1
    {
        public string processPosition;  // ���μ��� ��ġ
        public bool foupSensor;  // FOUP ����
        public int foupActionCount;  // FOUP ���� Ƚ��
        public string robot1EndPosition;  // �κ�1 �� ��ġ
        public int robot1ActionCount;  // �κ�1 ���� Ƚ��
    }

    [Serializable]
    public class Process2
    {
        public string processPosition;  // ���μ��� ��ġ
        public bool vacuumSensor;  // ���� ����
        public bool loadlockSensor;  // �ε�� ����
        public string robot2EndPosition;  // �κ�2 �� ��ġ
        public int robot2ActionCount;  // �κ�2 ���� Ƚ��
    }

    [Serializable]
    public class Process3
    {
        public string processPosition;  // ���μ��� ��ġ
        public bool vacuumSensor;  // ���� ����
        public bool alignSensor;  // ���� ����
        public string alignPosition;  // ���� ��ġ
        public int alignActionCount;  // ���� ���� Ƚ��
        public bool lithoSensor;  // ���ұ׷��� ����
        public string lithoPosition;  // ���ұ׷��� ��ġ
        public int lithoActionCount;  // ���ұ׷��� ���� Ƚ��
    }

    [Serializable]
    public class Process4
    {
        public string processPosition;  // ���μ��� ��ġ
        public bool vacuumSensor;  // ���� ����
        public string robot3EndPosition;  // �κ�3 �� ��ġ
        public int robot3ActionCount;  // �κ�3 ���� Ƚ��
        public bool semSensor;  // SEM ����
        public string semPosition;  // SEM ��ġ
        public int semActionCount;  // SEM ���� Ƚ��
        public Results results;  // ���
    }
    [Serializable]
    public class Results
    {
        public float chipData;  // Ĩ ������
        public float defectiveRate;  // �ҷ���
        public bool good;  // ��ǰ ����
        public bool defective;  // �ҷ� ����
    }

    [Serializable]
    public class Process5
    {
        public string processPosition;  // ���μ��� ��ġ
        public bool vacuumSensor;  // ���� ����
        public bool loadlockSensor;  // �ε�� ����
        public string robot4EndPosition;  // �κ�4 �� ��ġ
        public int robot4ActionCount;  // �κ�4 ���� Ƚ��
    }

    [Serializable]
    public class Process6
    {
        public string processPosition;  // ���μ��� ��ġ
        public string robot5EndPosition;  // �κ�5 �� ��ġ
        public int robot5ActionCount;  // �κ�5 ���� Ƚ��
        public bool foupSensor;  // FOUP ����
        public string goodProductsDefectiveProducts;  // ��ǰ/�ҷ� ��ǰ
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
        Vector3 robotPosition = transform.position;  // ���� ��ü�� ��ġ�� ��������
        int actionCount0 = GetRobotActionCount(0);  // �κ��� ���� Ƚ���� ������ �޼���
        int actionCount1 = GetRobotActionCount(1);  // �κ��� ���� Ƚ���� ������ �޼���
        int actionCount2 = GetRobotActionCount(2);  // �κ��� ���� Ƚ���� ������ �޼���
        int actionCount3 = GetRobotActionCount(3);  // �κ��� ���� Ƚ���� ������ �޼���
        int actionCount4 = GetRobotActionCount(4);  // �κ��� ���� Ƚ���� ������ �޼���

        UpdateProcessData();
    }

    // �κ� ���� Ƚ���� ��ȯ�ϴ� �޼���
    public int GetRobotActionCount(int robotIndex)
    {
        return robotActionCounts[robotIndex];
    } //robotactioncounts �迭�� �ε����� �ش��ϴ� �κ��� ���� Ƚ���� ��ȯ

    void SetRawJsonValueAsync()
    {
        DatabaseReference dbRef = FirebaseDatabase.DefaultInstance.RootReference;

        // JSON ���� ����
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

        dbRef.SetRawJsonValueAsync(json);  // ���⼭ Firebase DB�� �ʱ�ȭ��
    } // firebase�� ���ε��� josn �����͸� �����ϰ�, firebase�� ���ε��ϴ� �Լ�
    void SetRawJsonValueAsync(string jsonFile)
    {
        DatabaseReference dbRef = FirebaseDatabase.DefaultInstance.RootReference;

        dbRef.SetRawJsonValueAsync(jsonFile);
    } 

    public void UploadProcessData(ProcessData data) //processdata ��ü�� json �������� ��ȯ�Ͽ� firebase�� ���ε�
    {
        // �����͸� JSON���� ����ȭ
        string json = JsonUtility.ToJson(data);

        DatabaseReference dbRef = FirebaseDatabase.DefaultInstance.RootReference;

        // Firebase�� ����
        dbRef.Child("processData").SetRawJsonValueAsync(json)
                .ContinueWith(task => {
                if (task.IsCompleted)
                {
                    print("�����Ͱ� ���������� ���ε��");
                }
                else
                {
                    print("������ ���ε� ����");
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
    }// firebase���� �����͸� �������� ���� ��� / �⺻ ������ ����
   
    public void UpdateProcessData()
    {
        DatabaseReference dbRef = FirebaseDatabase.DefaultInstance.RootReference;

       
        ProcessData processData = new ProcessData()
        {
            process0 = new Process0()
            {
                energyConsumption = 100.0f,  // ���� ��
                temperature = 25.0f,  // ���� ��
                humidity = 60.0f,  // ���� ��
                defectiveTotalProducts = "10/100"  // ���� ��
            },
            process1 = new Process1()
            {
                processPosition = "Position1",  // ���� ��
                foupSensor = true,  // ���� ��
                foupActionCount = 5,  // ���� ��
                robot1EndPosition = "EndPos1",  // ���� ��
                robot1ActionCount = 10  // ���� ��
            },
            process2 = new Process2()
            {
                processPosition = "Position2",  // ���� ��
                vacuumSensor = true,  // ���� ��
                loadlockSensor = true,  // ���� ��
                robot2EndPosition = "EndPos2",  // ���� ��
                robot2ActionCount = 20  // ���� ��
            },
            process3 = new Process3()
            {
                processPosition = "Position3",  // ���� ��
                vacuumSensor = true,  // ���� ��
                alignSensor = true,  // ���� ��
                alignPosition = "AlignPos1",  // ���� ��
                alignActionCount = 30,  // ���� ��
                lithoSensor = true,  // ���� ��
                lithoPosition = "LithoPos1",  // ���� ��
                lithoActionCount = 40  // ���� ��
            },
            process4 = new Process4()
            {
                processPosition = "Position4",  // ���� ��
                vacuumSensor = true,  // ���� ��
                robot3EndPosition = "EndPos3",  // ���� ��
                robot3ActionCount = 50,  // ���� ��
                semSensor = true,  // ���� ��
                semPosition = "SEMPos1",  // ���� ��
                semActionCount = 60,  // ���� ��
                results = new Results()
                {
                    chipData = 100.0f,  // ���� ��
                    defectiveRate = 5.0f,  // ���� ��
                    good = true,  // ���� ��
                    defective = false  // ���� ��
                }
            },
            process5 = new Process5()
            {
                processPosition = "Position5",  // ���� ��
                vacuumSensor = true,  // ���� ��
                loadlockSensor = true,  // ���� ��
                robot4EndPosition = "EndPos4",  // ���� ��
                robot4ActionCount = 70  // ���� ��
            },
            process6 = new Process6()
            {
                processPosition = "Position6",  // ���� ��
                robot5EndPosition = "EndPos5",  // ���� ��
                robot5ActionCount = 80,  // ���� ��
                foupSensor = true,  // ���� ��
                goodProductsDefectiveProducts = "100/10"  // ���� ��
            }
        };
    

        // �����͸� JSON���� ����ȭ
        string json = JsonUtility.ToJson(processData);

        // Firebase�� ����
        dbRef.Child("processData").SetRawJsonValueAsync(json)
            .ContinueWith(task => {
                if (task.IsCompleted)
                {
                    print("�����Ͱ� ���������� ���ε��");
                }
                else
                {
                    print("������ ���ε� ����");
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
                    print("�α��� �ð� ��� �Ϸ�");
                }
                else
                {
                    print("�α��� �ð� ��� ����");
                }
            });
    } //�α����� �ð��� LoginInfo ��ü�� ��� Firebase�� �����մϴ�
}