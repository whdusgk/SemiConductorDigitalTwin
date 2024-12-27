using System.Net.Sockets;
using System;
using UnityEngine;
using System.Text;
using TMPro;
using System.Collections;

public class TCPCilent : MonoBehaviour
{
    [SerializeField] TMP_InputField dataInput;
    public bool isConnected;
    public float interval;
    
    TcpClient client;
    NetworkStream stream;
    string msg;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        try
        {

        //1.서버에 연결
        client = new TcpClient("127.0.0.1", 12345);
        print("서버에 연결됨");

        //2.네트워크 스트림 얻기
        stream = client.GetStream();
        }
        catch(Exception ex)
        {
            print(ex);
            print("서버를 먼저 작동시켜 주세요.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnConnectRtnClkEvent()
    {
        msg = Request("Connect"); //connect 일 경우 ok
        
        if( msg == "CONNECTED")
        {
            isConnected = true;

            StartCoroutine(CoRequest());
        }

    }

    public void OnDisconnectBtnClkEvent()
    {
        msg = Request("DisConnect"); // disconnected 일 경우 ok

        if ( msg == "connected")
        {
            isConnected = false;
        }
    }

  

    public void Connect()
    {
        byte[] dataBytes = Encoding.UTF8.GetBytes(dataInput.text);
        stream.Write(dataBytes, 0, dataBytes.Length);

        //서버로부터 데이터 읽기
        byte[] buffer = new byte[1024];
        int bytesRead = stream.Read(buffer, 0, buffer.Length);
        string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);
        Console.WriteLine("서버: " + response);
    }

    IEnumerator CoRequest() //코루틴- 반복
    {
        while (isConnected)
        {
            //1. MPS의 X 디바이스 정보를 정수형으로 전달한다.

            //2. PLC의 Y 디바이스 정보를 2진수 형태로 받는다. 


            string data = Request("Temp");

            yield return new WaitForSeconds(interval);
        }
    }
    public void Request()//(데이터 인풋 텍스트.)
    {
        byte[] dataBytes = Encoding.UTF8.GetBytes(dataInput.text);
        stream.Write(dataBytes, 0, dataBytes.Length);

        //서버로부터 데이터 읽기
        byte[] buffer = new byte[1024];
        int bytesRead = stream.Read(buffer, 0, buffer.Length);
        string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);
        Console.WriteLine("서버: " + response);
    }
    

    public string Request(string message)//(문자열을 보내겠다.)
    {
        byte[] dataBytes = Encoding.UTF8.GetBytes(message);
        stream.Write(dataBytes, 0, dataBytes.Length);

        //서버로부터 데이터 읽기
        byte[] buffer = new byte[1024];
        int bytesRead = stream.Read(buffer, 0, buffer.Length);
        string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);
        Console.WriteLine("서버: " + response);

        return response;
    }
    private void OnDestroy()
    {
        Request("Disconnect&Quit");

        if (isConnected)
        { 
            isConnected = false;
        }
    }
}