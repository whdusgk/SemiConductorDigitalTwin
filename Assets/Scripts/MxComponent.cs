using UnityEngine;
using ActUtlType64Lib;
using System;
using TMPro;
using Unity.VisualScripting;

public class MxComponent : MonoBehaviour
{
    public enum State 
    {
        CONNECTED,
        DISCONNECTED
    }
    public static MxComponent Instance;
    public State state = State.DISCONNECTED;

    ActUtlType64 mxComponent;
    [SerializeField] TMP_InputField deviceInput;
    public string xDevices = "000000000000000";


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    void Start()
    {
        mxComponent = new ActUtlType64();

        mxComponent.ActLogicalStationNumber = 1;


    }

    public void OnSimConnectBtnClkEvent()
    {
        int returnValue = mxComponent.Open();

        string hexValue = Convert.ToString(returnValue, 16);

        if (hexValue == "0")
        {
            print("Simulator와 연결이 잘 되었습니다.");
        }
        else
        {
            print($"에러코드를 확인해 주세요. 에러코드: {hexValue}");
        }
    }
    public void OnSimDisconnectBtnClkEvent()
    {
        int returnValue = mxComponent.Close();

        string hexValue = Convert.ToString(returnValue, 16);

        if (hexValue == "0")
        {
            print("Simulator와 연결이 해제되었습니다.");
        }
        else
        {
            print($"에러코드를 확인해 주세요. 에러코드: {hexValue}");
        }
    }

    public void OnGetDeviceBtnClkEvent()
    {
        int value;
        int returnValue = mxComponent.GetDevice(deviceInput.text, out int Value);

        if (returnValue == 0)
        {
            print("디바이스 읽기가 완료되었습니다.");
            
            deviceInput.text = Value.ToString();
        }
        else
        {
            print($"에러가 발생하였습니다. 에러코드: {returnValue}");
        }
    }

    public void OnSetDeviceBtnClkEvent()
    {

        string[] data = deviceInput.text.Split(',');

        int returnValue = mxComponent.SetDevice(data[0], int.Parse(data[1]));

        if (returnValue == 0)
        {
            print("디바이스 쓰기가 완료되었습니다.");

        }
        else
        {
            print($"에러가 발생하였습니다. 에러코드: {returnValue}");
        }
    }

    public void OnReadDeviceBtnClkEvent(int blockCnt) //2진수로 일괄 읽기
    {
        int[] data = new int[blockCnt];
        int returnValue = mxComponent.ReadDeviceBlock(deviceInput.text, blockCnt, out data[0]);
        string totalData = "";

        if (returnValue == 0)
        {
            print("디바이스 블록 읽기가 완료되었습니다.");
            
            foreach(int d in data)
            { 
                string input = Convert.ToString(d, 2);
                string reversed = Reverse(input);

                if(16 - reversed.Length > 0)
                {
                    int countZero = 16 - reversed.Length;
                    for(int i = 0; i < countZero; i++)
                    {
                        totalData += '0';
                    }
                }
                print(reversed); //00011001100
                print(reversed[38]); //1 
            
            }
        }
        else
        {
            print($"에러가 발생하였습니다. 에러코드: {returnValue}");
        }
    } 
    public static string Reverse(string input) // 읽은 데이터를 2진수로 변환하고, 그 값을 역순으로 출력하는 기능
    {
        char[] chars = input.ToCharArray();
        Array.Reverse(chars);
        return new string(chars);
    }


    private void OnDestroy() // 종료 할 때, GX Works2 같이 종료, 없으면 강제종료 해야함
    {
        OnSimDisconnectBtnClkEvent();
    }
}
