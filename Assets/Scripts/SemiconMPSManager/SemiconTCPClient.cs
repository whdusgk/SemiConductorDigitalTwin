using System.Net.Sockets;
using System;
using UnityEngine;
using System.Text;
using TMPro;
using System.Collections;
using System.Threading;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;


namespace MPS
{
    public class SemiconTCPClient : MonoBehaviour
    {
        public static SemiconTCPClient Instance;
        [SerializeField] TMP_InputField dataInput;
        public bool isConnected;
        public float interval;
        public int xDeviceBlockSize;
        public int yDeviceBlockSize;
        public int dDeviceBlockSize;
        public string xDevices;
        public string yDevices;
        public string dDevices;

        [SerializeField] string dataToServer;
        [SerializeField] string dataFromServer;

        TcpClient client;
        NetworkStream stream;
        string msg;
        string totalMsg;

        // Start is called once before the first execution of Update after the MonoBehaviour is created

        private void Awake() // ���ϸ��� ����
        {
            try
            {

                //1.������ ����
                client = new TcpClient("127.0.0.1", 5000);
                print("������ �����");

                //2.��Ʈ��ũ ��Ʈ�� ���
                stream = client.GetStream();
            }
            catch (Exception ex)
            {
                print(ex);
                print("������ ���� �۵����� �ּ���.");
            }

            msg = Request("Connect"); //connect �� ��� ok

            if (msg.Contains("CONNECTED"))
            {
                isConnected = true;

                StartCoroutine(CoRequest());
            }
            for (int i = 0; i < xDeviceBlockSize; i++)
            {
                xDevices += "0000000000000000";
            }

            for (int i = 0; i < yDeviceBlockSize; i++)
            {
                yDevices += "0000000000000000";
            }

            for (int i = 0; i < dDeviceBlockSize; i++)
            {
                dDevices += "0000000000000000";
            }


            if (Instance == null)
            {
                Instance = this;
            }
        }

        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnConnectRtnClkEvent()
        {
            msg = Request("Connect"); //connect �� ��� ok

            if (msg.Contains("CONNECTED"))
            {
                isConnected = true;

                StartCoroutine(CoRequest());
            }
            for (int i = 0; i < xDeviceBlockSize; i++)
            {
                xDevices += "0000000000000000";
            }

            for (int i = 0; i < yDeviceBlockSize; i++)
            {
                yDevices += "0000000000000000";
            }

            for (int i = 0; i < dDeviceBlockSize; i++)
            {
                dDevices += "0000000000000000";
            }
        }

        public void OnDisconnectBtnClkEvent()
        {
            msg = Request("DisConnect"); // disconnected �� ��� ok

            if (msg.Contains("connected"))
            {
                isConnected = false;
            }
        }



        public void Connect()
        {
            byte[] dataBytes = Encoding.UTF8.GetBytes(dataInput.text);
            stream.Write(dataBytes, 0, dataBytes.Length);

            //�����κ��� ������ �б�
            byte[] buffer = new byte[1024];
            int bytesRead = stream.Read(buffer, 0, buffer.Length);
            string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            Console.WriteLine("����: " + response);
        }

        IEnumerator CoRequest()
        {
            yield return new WaitUntil(() => isConnected);

            print("CoRequest");

            if (isConnected)
            {

                Task.Run(() => RequestAsync());
            }

            while (isConnected)
            {
                try
                {
                    //string data = Request("temp"); // zGET,X0,1 / SET,X0,128


                    // SET,X0,128,GET,Y0,2,GET,D0,3 -> ������ ���� -> WriteDeviceBlock 1��, ReadDeviceBlock 1��


                    // 1. MPS�� X ����̽� ������ ���������� �����Ѵ�.
                    string returnValue = WriteDevices("X0", 2, xDevices); // SET,X0,128
                    print("ScanPLC: " + dataFromServer); // PLC��ȣ(�Ǹ��� ��ȣ(32) + ������ȣ(22)): "32,22"

                    // 2. PLC�� Y, D ����̽� ������ 2���� ���·� �޴´�.
                    yDevices = ReadDevices("Y0", 2); //  GET,Y0,2
                    dDevices = ReadDevices("D0", 1); //  GET,D0,1
                }
                // 3. ����: �������� �����͸� �ְ� ���� �� ���ϴ� �����͸� �ޱ�
                // (Unity to Server ������ ����: SET,X0,3,128,24,1/GET,Y0,2/GET,D0,3)
                // (Server to Unity ������ ����: X0,123,24/D0,23
                //Request($"SET,X0,1,{xDevices}/GET,Y0,2/GET,D0,1");
                catch (Exception ex)
                {
                    print(ex);
                }

                yield return new WaitForSeconds(interval);
            }
        }
        public void Request()//(������ ��ǲ �ؽ�Ʈ.)
        {
            byte[] dataBytes = Encoding.UTF8.GetBytes(dataInput.text);
            stream.Write(dataBytes, 0, dataBytes.Length);

            //�����κ��� ������ �б�
            byte[] buffer = new byte[1024];
            int bytesRead = stream.Read(buffer, 0, buffer.Length);
            string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            Console.WriteLine("����: " + response);
        }
        public string WriteDevices(string deviceName, int blockSize, string data)
        {
            totalMsg = "";

            int totallength = blockSize * 16;
            if (data.Length < totallength)
            {
                data = data.PadRight(totallength, '0');
            }


            // data = "1101010001000000" or "110101000100000011010100010000001101010001000000" -> 555
            int[] convertedData = new int[blockSize];

            ConvertData(data);

            void ConvertData(string data)
            {
                for (int i = 0; i < blockSize; i++)
                {
                    string splited = data.Substring(i * 16, 16);        // 1101010001000000
                    splited = Reverse(splited);                         // 0000001000101011(reversed)
                    convertedData[i] = Convert.ToInt32(splited, 2);     // 555(10���� ��ȯ)
                }
            }

            // 128,64,266
            foreach (var d in convertedData)
            {
                totalMsg += "," + d;
            }

            // Server�� ������ ����
            dataToServer = $"GET,Y0,2,SET,{deviceName},{blockSize}{totalMsg}";
            //print($"SET,{deviceName},{blockSize}{totalMsg}");
            //string returnValue = Request($"SET,{deviceName},{blockSize}{totalMsg}"); // SET,X0,3,128,64,266
            return dataToServer;
        }

        public string ReadDevices(string deviceName, int blockSize)
        {
            string returnValue = dataFromServer; // "Y0,0,0"

            int[] data = new int[blockSize];
            string totalData = "";

            if (returnValue != "�����ڵ�")
            {
                print("����̽� ��� �бⰡ �Ϸ�Ǿ����ϴ�.");

                data = returnValue.Split(',').Select(n => Convert.ToInt32(n)).ToArray();

                foreach (int d in data)
                {
                    string input = Convert.ToString(d, 2); // D ����̽�: 10����, ������ ó���� ���� ���
                                                           // (����� ������ ����, �����ؾ� �� ������ ���� ����, �ҷ��� ����)
                                                           // ������ ����: 55, 100
                                                           // X, Y ����̽�: 2����, ��Ʈ ������ ���
                                                           // ��Ʈ ���� ������ ����: 1011010, xDevice[0] = 1

                    if (!deviceName.Contains("D")) // deviceName = "Y0", "X0"
                    {
                        input = Reverse(input);

                        // x[33] = 0 -> x[3][3]
                        if (16 - input.Length > 0) // 1101010001 -> 110101000100000 
                        {
                            int countZero = 16 - input.Length;
                            for (int i = 0; i < countZero; i++)
                            {
                                input += '0';
                            }
                        }
                    }

                    totalData += input;
                }

                return totalData; // 110101000100000 / 110101000100000
            }
            else
            {
                return returnValue;
            }
        }
        public static string Reverse(string input)
        {
            char[] chars = input.ToCharArray();
            Array.Reverse(chars);
            return new string(chars);
        }

        public string Request(string message)//(���ڿ��� �����ڴ�.)
        {
            byte[] dataBytes = Encoding.UTF8.GetBytes(message);
            stream.Write(dataBytes, 0, dataBytes.Length);

            //�����κ��� ������ �б�
            byte[] buffer = new byte[1024];
            int bytesRead = stream.Read(buffer, 0, buffer.Length);
            string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            Console.WriteLine("����: " + response);

            return response;
        }
        private async Task RequestAsync()
        {
            while (isConnected)
            {
                try
                {
                    //WriteDevices("X0", 1, xDevices);

                    // GET,Y0,2,SET,X0,2,32,10
                    byte[] dataBytes = Encoding.UTF8.GetBytes(dataToServer);

                    // NetworkStream�� ������ ����
                    await stream.WriteAsync(dataBytes, 0, dataBytes.Length);

                    // ������ ����
                    byte[] buffer = new byte[1024];
                    int nBytes = await stream.ReadAsync(buffer, 0, buffer.Length);
                    dataFromServer = Encoding.UTF8.GetString(buffer, 0, nBytes);
                    print("dataFromServer: " + dataFromServer); // Y0,0,0
                }
                catch (Exception e)
                {
                    print(e.ToString());
                }
            }
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

}