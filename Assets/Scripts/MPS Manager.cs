using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine.UI;
using System.Collections;
using System;
using TMPro;
using Unity.Android.Gradle.Manifest;
using Unity.VisualScripting;
using System.Collections.Generic;
using System.IO;


public class MPSManager : MonoBehaviour
{


    private bool isCollectingData = false;
    private DatabaseReference reference;

    public FirebaseDBManager DBManager;

    // 데이터 변수
    private float consumption;
    private float temperature;
    private float humidity;
    public string Name = "마크준수1호";

    // 데이터 생성 후 Firebase에 추가
    string firebaseCode = "";  // 상태 코드
    string statusMessage = "";  // 상태 메시지
    string path = "";

    [SerializeField] int StartButtonState = 0;
    [SerializeField] int StopButtonState = 0;
    [SerializeField] int EStopButtonState = 0;

    [SerializeField] TMP_InputField EnergyConsumption;
    [SerializeField] TMP_InputField Temperature;
    [SerializeField] TMP_InputField Humidity;
    [SerializeField] TMP_Text UpdateLogText;

    [SerializeField] GameObject[] uiObjects;

    private int currentChipIndex = 0;  // 검사할 칩의 인덱스를 추적하는 변수
    private List<int> coloredChips = new List<int>(); // 색상 변경된 객체들을 추적하는 리스트

    private int currentWaferIndex = 1;
    private bool isInspectionComplete = false;  // 검사가 완료된 상태를 추적하는 변수

    void Start()
    {
       

        InvokeRepeating("UpdateData", 2, 1); //2초후 시작, 이후 1초 간격으로 호출

        GenerateRandomData();

        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            FirebaseApp app = FirebaseApp.DefaultInstance;
            reference = FirebaseDatabase.DefaultInstance.RootReference;
        });

    }

    private float lastUpdateTime = 0f;
    public float updateInterval = 10f;

    void Update()
    {

        // 데이터를 수집하는 중이면 랜덤으로 데이터 생성
        if (Time.time - lastUpdateTime >= updateInterval)
        {
            if (isCollectingData)
            {
                lastUpdateTime = Time.time; //마지막 업데이트 시간을 현재 시간으로 갱신
                GenerateRandomData();
                UpdateUI();
            }
        }
   
    }


    // Start 버튼 클릭시 데이터 수집 시작
    // Start 버튼 클릭 시 첫 번째 웨이퍼 검사 시작 및 칩 색상 변경
    public void OnStartButtonClicked()
    {
        // 데이터 수집 상태 변경
        isCollectingData = true;
        StartButtonState = 1;
        StopButtonState = 0;

        print("1호 검사 시작");

        // 첫 번째 칩의 색상을 변경 (검사 시작)
        ChangeChipUIColorBasedOnTemperature();

        // 첫 번째 웨이퍼 검사 시작 후 2초 뒤부터 10초마다 계속해서 색상 변경 작업을 진행
        InvokeRepeating("UpdateChipInspection", 2f, 1f); // 2초 후 시작하고 5초마다 계속 호출
    }
    // Stop 버튼 클릭시 3초 후에 데이터 수집 중지
    public void OnStopButtonClicked()
    {

        StartButtonState = 1;
        StopButtonState = 0;

        if (isCollectingData)
        { 
        StartCoroutine(StopDataCollection());
        print("데이터 수집 정지");
        }
    }

    // 3초 후에 데이터 수집을 멈추는 코루틴
    
   IEnumerator StopDataCollection()
    {
        print("데이터 정보 수집을 3초 후 정지합니다");  // "데이터 정보 수집을 정지합니다" 메시지 표시
        isCollectingData = false;

        yield return new WaitForSeconds(1f);
        print("3");  // 3초 전 표시

        yield return new WaitForSeconds(1f);
        print("2");  // 2초 전 표시

        yield return new WaitForSeconds(1f);
        print("1");  // 1초 전 표시

        yield return new WaitForSeconds(1f);
        print("정지완료");  // "정지완료" 메시지 표시

        // 현재 웨이퍼 검사 완료 후 전환
        if (currentWaferIndex >= 1 && currentWaferIndex < 10)
        {
            isInspectionComplete = true;  // 현재 웨이퍼 검사 완료
            print($"웨이퍼 {currentWaferIndex}호 검사 완료");

            // 다음 웨이퍼로 전환 (1 ~ 9호에서 다음 번호로 넘어감)
            currentWaferIndex++;
            isInspectionComplete = false;  // 새로운 웨이퍼 생산 시작 시 검사 완료 상태 리셋
            print($"웨이퍼 {currentWaferIndex}호 생산 시작");
        }
        else if (currentWaferIndex == 10)
        {
            isInspectionComplete = true;  // 웨이퍼 10호 검사 완료
            print("웨이퍼 10호 검사 완료");

            // 웨이퍼 1호로 돌아가서 다시 생산 시작
            currentWaferIndex = 1;
            isInspectionComplete = false;  // 검사 완료 상태 리셋
            print("웨이퍼 1호로 돌아가서 생산 시작");
        }
    }

    // E-stop 버튼 클릭시 즉시 데이터 수집 중지
    public void OnEStopButtonClicked()
    {
        EStopButtonState = (EStopButtonState == 1) ? 0 : 1;
        StartButtonState = 0;
        StopButtonState = 0;

        isCollectingData = false;
        print("긴급 정지");
    }

    // 칩 검사 및 색상 변경을 업데이트하는 함수
    private void UpdateChipInspection()
    {
        // 데이터를 수집 중이라면 계속해서 검사 진행
        if (isCollectingData)
        {
            // 칩 검사 함수 호출
            ChangeChipUIColorBasedOnTemperature();
        }
    }

    // 랜덤 데이터 생성 함수
    public void GenerateRandomData()
    {
        consumption = UnityEngine.Random.Range(0f, 1000f); // 예: 0 ~ 1000
        temperature = UnityEngine.Random.Range(-20f, 50f);  // 예: -20 ~ 50°C
        humidity = UnityEngine.Random.Range(30f, 90f);      // 예: 30 ~ 90%
    }
    public void SetRawJsonValueAsync(string path, string json)
    {
        // Firebase에 데이터를 저장하는 코드
        FirebaseDatabase.DefaultInstance
            .RootReference
            .Child(path)
            .SetRawJsonValueAsync(json);
    }
    public void AddDataToFirebase(string json, int chipIndex)
    {
        // 온도에 따른 불량 구분 (고온, 저온)
        string folderPath = "";

        if (temperature > 45f)
        {
            // 고온 불량 발생
            folderPath = $"Real-time Information/웨이퍼 1/불량 발생/(고온)Code 001/Chip {chipIndex + 1}";
        }
        else if (temperature < -15f)
        {
            // 저온 불량 발생
            folderPath = $"Real-time Information/웨이퍼 1/불량 발생/(저온)Code 002/Chip {chipIndex + 1}";
        }
        else
        {
            // 양품 생성
            folderPath = $"Real-time Information/웨이퍼 1/양품생성/Chip {chipIndex + 1}";
        }

        // Firebase에 해당 경로로 데이터 저장
        reference.Child(folderPath).SetRawJsonValueAsync(json);
    }

    public void UpdateUI()
    {
        // TMP_InputField에 데이터를 표시
        EnergyConsumption.text = " " + consumption.ToString("F2") + "kWh";
        Temperature.text = " " + temperature.ToString("F2") + "°C";
        Humidity.text = " " + humidity.ToString("F2") + "%";

        // 패널에 정보 업데이트 표시
        string currentTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");  // 현재 시간을 "yyyy-MM-dd HH:mm:ss" 형식으로 가져옴

        string newLog = "정보 업데이트 완료\n" +
                             "EnergyConsumption : " + consumption.ToString("F2") + "kWh\n" +
                             "Temperature : " + temperature.ToString("F2") + "°C\n" +
                             "Humidity : " + humidity.ToString("F2") + "%";

        // 기존의 로그에 새로운 정보를 추가
        UpdateLogText.text += newLog;  // 기존 정보 뒤에 새로운 로그를 추가

        // 현재 시간 표시
        UpdateLogText.text = "CurrentTime\n" + currentTime;  // 텍스트 UI에 현재 시간 표시

        string json = $@"{{
                ""Data"":[
                ],
                ""consumption"":""{consumption.ToString("F2") + "kWh"}"",
                ""Temperature"":""{temperature.ToString("F2") + "°C"}"",
                ""Humidity"":""{humidity.ToString("F2") + "%"}"",
                ""검사자"":""{Name}""
            }}";


        AddDataToFirebase(json, currentChipIndex); //firebase에 데이터 추가
        // DBManager.SetRawJsonValueAsync(json);

        // 온도가 30도를 초과하면 랜덤으로 객체 색상 변경
        //ChangeRandomUIColorBasedOnTemperature();


    }
    // 칩 UI 색상 변경 함수
    private void ChangeChipUIColorBasedOnTemperature()
    {
        // 검사할 칩이 남아 있으면 순차적으로 검사
        if (currentChipIndex < 37)
        {
            // 현재 검사할 칩
            int chipIndex = currentChipIndex;

            // 선택된 UI 객체에서 RawImage 컴포넌트를 찾아서 색상을 변경
            if (uiObjects[chipIndex] != null)
            {
                RawImage rawImage = uiObjects[chipIndex].GetComponent<RawImage>();
                if (rawImage != null)
                {
                    // 온도에 따라 색상 변경
                    if (temperature > 45f)
                    {
                        rawImage.color = Color.red; // 온도가 45도를 초과하면 빨간색
                        print($"불량 발생 : Code:001 (Chip {chipIndex + 1})");
                    }
                    else if (temperature < -15f)
                    {
                        rawImage.color = Color.blue; // 온도가 -15도 미만이면 파란색
                        print($"불량 발생 : Code:002 (Chip {chipIndex + 1})");
                    }
                    else
                    {
                        rawImage.color = Color.green; // 온도가 -15~45도 사이면 초록색
                        print($"양품 생성 : Code:000 (Chip {chipIndex + 1})");
                    }

                    // 색상이 변경된 객체는 coloredChips에 추가
                    coloredChips.Add(chipIndex); // 색상 변경된 chip의 인덱스를 추가하여 추후 변경되지 않도록 함
                }
            }

            // 검사한 칩 인덱스 증가
            currentChipIndex++;

            // 모든 칩이 검사 완료되면 "검사 완료" 메시지 출력
            if (currentChipIndex >= uiObjects.Length)
            {
                print("모든 칩 검사 완료");
                isInspectionComplete = true;
            }
        }
    }


    public void UpdateData()
        {
            if (isCollectingData)
            {
                GenerateRandomData();
                UpdateUI();

                print("정보 업데이트 완료");
            }

        }
}
