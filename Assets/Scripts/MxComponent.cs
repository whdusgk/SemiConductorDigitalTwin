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
            print("Simulator�� ������ �� �Ǿ����ϴ�.");
        }
        else
        {
            print($"�����ڵ带 Ȯ���� �ּ���. �����ڵ�: {hexValue}");
        }
    }
    public void OnSimDisconnectBtnClkEvent()
    {
        int returnValue = mxComponent.Close();

        string hexValue = Convert.ToString(returnValue, 16);

        if (hexValue == "0")
        {
            print("Simulator�� ������ �����Ǿ����ϴ�.");
        }
        else
        {
            print($"�����ڵ带 Ȯ���� �ּ���. �����ڵ�: {hexValue}");
        }
    }

    public void OnGetDeviceBtnClkEvent()
    {
        int value;
        int returnValue = mxComponent.GetDevice(deviceInput.text, out int Value);

        if (returnValue == 0)
        {
            print("����̽� �бⰡ �Ϸ�Ǿ����ϴ�.");
            
            deviceInput.text = Value.ToString();
        }
        else
        {
            print($"������ �߻��Ͽ����ϴ�. �����ڵ�: {returnValue}");
        }
    }

    public void OnSetDeviceBtnClkEvent()
    {

        string[] data = deviceInput.text.Split(',');

        int returnValue = mxComponent.SetDevice(data[0], int.Parse(data[1]));

        if (returnValue == 0)
        {
            print("����̽� ���Ⱑ �Ϸ�Ǿ����ϴ�.");

        }
        else
        {
            print($"������ �߻��Ͽ����ϴ�. �����ڵ�: {returnValue}");
        }
    }

    public void OnReadDeviceBtnClkEvent(int blockCnt) //2������ �ϰ� �б�
    {
        int[] data = new int[blockCnt];
        int returnValue = mxComponent.ReadDeviceBlock(deviceInput.text, blockCnt, out data[0]);
        string totalData = "";

        if (returnValue == 0)
        {
            print("����̽� ��� �бⰡ �Ϸ�Ǿ����ϴ�.");
            
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
            print($"������ �߻��Ͽ����ϴ�. �����ڵ�: {returnValue}");
        }
    } 
    public static string Reverse(string input) // ���� �����͸� 2������ ��ȯ�ϰ�, �� ���� �������� ����ϴ� ���
    {
        char[] chars = input.ToCharArray();
        Array.Reverse(chars);
        return new string(chars);
    }


    private void OnDestroy() // ���� �� ��, GX Works2 ���� ����, ������ �������� �ؾ���
    {
        OnSimDisconnectBtnClkEvent();
    }
}
