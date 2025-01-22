using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace MPS
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance;

        public TMP_InputField EnergyConsumptionInpuField;
        public TMP_InputField TemperatureInpuField;
        public TMP_InputField HumidityInpuField;
        public TMP_Text ProductsText;

        public List<TMP_InputField> ProcessPositionsInpuField = new List<TMP_InputField>(); //public 변경
        [SerializeField] List<Transform> ProcessPositions = new List<Transform>(); 
        [SerializeField] List<TMP_InputField> RobotPositionsInpuField = new List<TMP_InputField>();
        public List<Transform> RobotPositions = new List<Transform>(); //public 변경
        [SerializeField] List<Toggle> FoupSensorsToggle = new List<Toggle>();
        [SerializeField] List<Toggle> VacuumSensorsToggle = new List<Toggle>();
        [SerializeField] List<Toggle> LoadlockSensorsToggle = new List<Toggle>();

        [SerializeField] Toggle AlignSensorToggle;
        [SerializeField] TMP_InputField AlignPositionInpuField;
        public Transform AlignLithoZig1;

        [SerializeField] Toggle LithoSensorToggle;
        [SerializeField] TMP_InputField LithoPositionInpuField;
        public Transform AlignLithoZig2;

        public Toggle SEMSensorToggle;
        public TMP_InputField SEMPositionInpuField;

        public List<RawImage> ChipDataRawImage = new List<RawImage>(); 

        public TMP_Text DefectiveRateText;
        public Toggle GoodToggle;
        public Toggle DefectiveToggle;

        public TMP_Text GDChipText;

        public TMP_InputField GoodProductInpuField; 
        public TMP_InputField DefectiveProductInpuField;

        public VideoPlayer SEMVideo;
        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
            }
        }
        void Start()
        {
            EnergyConsumptionInpuField.text = Random.Range(45, 70).ToString() + "MWh";
            TemperatureInpuField.text = Random.Range(20, 22).ToString() + "°C";
            HumidityInpuField.text = Random.Range(40, 45).ToString() + "% RH";
            ProductsText.text = "Defective:0 / Total:0";


            ProcessPositionsInpuField[0].text = "(-2.816 , 0 , 0)";
            ProcessPositionsInpuField[1].text = "(-1.571 , 0 , 0)";
            ProcessPositionsInpuField[2].text = "(0 , 0 , 0)";
            ProcessPositionsInpuField[3].text = "(1.869 , 0 , 0)";
            ProcessPositionsInpuField[4].text = "(3.489 , 0 , 0)";
            ProcessPositionsInpuField[5].text = "(4.745 , 0 , 0)";

            FoupSensorsToggle[0].isOn = true;
            FoupSensorsToggle[1].isOn = true;

            VacuumSensorsToggle[0].isOn = true;
            VacuumSensorsToggle[1].isOn = true;
            VacuumSensorsToggle[2].isOn = true;
            VacuumSensorsToggle[3].isOn = true;
            /*        VacuumSensorsToggle[4].isOn = false;
                    VacuumSensorsToggle[5].isOn = false;
                    VacuumSensorsToggle[6].isOn = false;*/

            LoadlockSensorsToggle[0].isOn = true;
            LoadlockSensorsToggle[1].isOn = true;

            AlignSensorToggle.isOn = true;
            LithoSensorToggle.isOn = true;


            SEMSensorToggle.isOn = true;
            SEMPositionInpuField.text = "0,0,0";

            GoodToggle.isOn = false;
            DefectiveToggle.isOn = false;

            GDChipText.text = "Good/Defective Products";


        }

        // Update is called once per frame
        void Update()
        {
            
            RobotPositionsInpuField[0].text = RobotPositions[0].localPosition.ToString();
            RobotPositionsInpuField[1].text = RobotPositions[1].localPosition.ToString();
            RobotPositionsInpuField[2].text = RobotPositions[2].localPosition.ToString();
            RobotPositionsInpuField[3].text = RobotPositions[3].localPosition.ToString();
            RobotPositionsInpuField[4].text = RobotPositions[4].localPosition.ToString();


            ProductsText.text = "Defective:" + SEMManager.Instance.DefectiveProductCount + " / Total:" + (SEMManager.Instance.GoodProductCount + SEMManager.Instance.DefectiveProductCount); 
/*            //Foup Sensor Toggle 구현
            if (WaferSensorManager.Instance.isFoupSensed == true)
            {
                FoupSensorsToggle[0].isOn = true;
            }

            //Vacuum Sensor Toggle 구현
            if (Sensor.Instance.isVacuumOn == true)
            {
                foreach (Toggle v in VacuumSensorsToggle)
                    v.isOn = true;
            }

            //Loadlock Sensor Toggle 구현(삭제 고려)

            //AlignLitho Sensor Toggle 구현
            if (SemiconMPSManagerTCP.instance.LithoAni1.GetComponent<Animator>().enabled == true)
                AlignSensorToggle.isOn = true;
            if (SemiconMPSManagerTCP.instance.LithoAni2.GetComponent<Animator>().enabled == true)
                LithoSensorToggle.isOn = true;*/

            AlignPositionInpuField.text = AlignLithoZig1.localPosition.ToString();
            LithoPositionInpuField.text = AlignLithoZig2.localPosition.ToString();

        }

       
    }

}
