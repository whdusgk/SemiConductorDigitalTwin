using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MPS
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance;

        public TMP_InputField EnergyConsumptionInpuField;
        public TMP_InputField TemperatureInpuField;
        public TMP_InputField HumidityInpuField;
        public TMP_Text ProductsText;

        [SerializeField] List<TMP_InputField> ProcessPositionsInpuField = new List<TMP_InputField>();
        [SerializeField] List<Transform> ProcessPositions = new List<Transform>();
        [SerializeField] List<TMP_InputField> RobotPositionsInpuField = new List<TMP_InputField>();
        [SerializeField] List<Transform> RobotPositions = new List<Transform>();
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


        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
            }
        }
        void Start()
        {
            EnergyConsumptionInpuField.text = Random.Range(0, 10).ToString();
            TemperatureInpuField.text = Random.Range(0, 10).ToString();
            HumidityInpuField.text = Random.Range(0, 10).ToString();
            ProductsText.text = Random.Range(0, 10).ToString();


            ProcessPositionsInpuField[0].text = "(-2.816 , 0 , 0)";
            ProcessPositionsInpuField[1].text = "(-1.571 , 0 , 0)";
            ProcessPositionsInpuField[2].text = "(0 , 0 , 0)";
            ProcessPositionsInpuField[3].text = "(1.869 , 0 , 0)";
            ProcessPositionsInpuField[4].text = "(3.489 , 0 , 0)";
            ProcessPositionsInpuField[5].text = "(4.745 , 0 , 0)";

            FoupSensorsToggle[0].isOn = false;
            FoupSensorsToggle[1].isOn = false;

            VacuumSensorsToggle[0].isOn = false;
            VacuumSensorsToggle[1].isOn = false;
            VacuumSensorsToggle[2].isOn = false;
            VacuumSensorsToggle[3].isOn = false;
            /*        VacuumSensorsToggle[4].isOn = false;
                    VacuumSensorsToggle[5].isOn = false;
                    VacuumSensorsToggle[6].isOn = false;*/

            LoadlockSensorsToggle[0].isOn = false;
            LoadlockSensorsToggle[1].isOn = false;

            AlignSensorToggle.isOn = false;
            LithoSensorToggle.isOn = false;


            SEMSensorToggle.isOn = false;
            SEMPositionInpuField.text = "0,0,0";

            GoodToggle.isOn = false;
            DefectiveToggle.isOn = false;

            GDChipText.text = "Good/Defective Products";


        }

        // Update is called once per frame
        void Update()
        {
            for (int r = 0; r < 5; r++)
                RobotPositionsInpuField[r].text = RobotPositions[r].localPosition.ToString();

            //Foup Sensor Toggle 구현
            /*            if(Sensor.Instance.isFoupSensed = true)
                        {
                            FoupSensorsToggle[0].isOn = true;
                        }*/

            //Vacuum Sensor Toggle 구현
            /*            if(SemiconMPSManager.Instance.isVacuum = true)
                        {
                            foreach(Toggle v in VacuumSensorsToggle)
                                v.isOn = true;
                        }*/

            //Loadlock Sensor Toggle 구현(삭제 고려)

            //AlignLitho Sensor Toggle 구현
            /*if (SemiconMPSManager.Instance.LithoAni1.GetComponent<Animator>().enabled == true)
                AlignSensorToggle.isOn = true;
            if (SemiconMPSManager.Instance.LithoAni2.GetComponent<Animator>().enabled == true)
                LithoSensorToggle.isOn = true;*/

            AlignPositionInpuField.text = AlignLithoZig1.localPosition.ToString();
            LithoPositionInpuField.text = AlignLithoZig2.localPosition.ToString();

        }

       
    }

}
