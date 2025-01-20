using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace MPS
{
    public class SEMManager : MonoBehaviour
    {
        public static SEMManager Instance;


        public float DefectiveRate;
        public float SEMDisAxis1;
        public float SEMDisAxis2;
        public float SEMAngleAxis3;
        public float SEMAngleAxis4;
        public float SEMAngleAxis5;
        public float SEMAngleAxis6;

        public float xchipGap = 0.0004f;
        public float zchipGap = 0.4f;
        public float speed = 5000;
        public float cycleTime;
        public float duration;

        public List<float> ChipData = new List<float>();

        [SerializeField] Transform SEMAxis1;
        [SerializeField] Transform SEMAxis2;
        [SerializeField] Transform SEMAxis3;
        [SerializeField] Transform SEMAxis4;
        [SerializeField] Transform SEMAxis5;
        [SerializeField] Transform SEMAxis6;

        public GameObject SEMActSensor;
        List<int> SEMxPos = new List<int> { -3, -2, -1, 0, 1, 2, 3 };
        List<int> SEMzPos = new List<int> { -3, -2, -1, 0, 1, 2, 3 };
        Vector3 SEMAxis1Origin;
        Vector3 SEMAxis2Origin;
        public int SEMCount = 0;
        public int semActCount = 0; //
        public float GoodChipCount = 0;
        public float DefectiveChipCount = 0;

        public float GoodProductCount = 0;
        public float DefectiveProductCount = 0;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }
        void Start()
        {
            SEMAxis1Origin = new Vector3(SEMAxis1.localPosition.x, SEMAxis1.localPosition.y, SEMAxis1.localPosition.z);
            SEMAxis2Origin = new Vector3(SEMAxis2.localPosition.x, SEMAxis2.localPosition.y, SEMAxis2.localPosition.z);

           

        }
        public void RunSEMCycle()
        {
            StartCoroutine(RunSEM());

            semActCount++;
        }
        public IEnumerator RunSEM()
        {
            
            Sensor.Instance.SEMActSensor.GetComponent<Renderer>().material.color = new Color(0, 255, 0); // Green
            yield return SEMReset();

            yield return SEMStep(-1, -3); yield return SEMStep(0, -3); yield return SEMStep(1, -3);

            yield return SEMStep(-2, -2); yield return SEMStep(-1, -2); yield return SEMStep(0, -2); yield return SEMStep(1, -2); yield return SEMStep(2, -2);

            yield return SEMStep(-3, -1); yield return SEMStep(-2, -1); yield return SEMStep(-1, -1); yield return SEMStep(0, -1);
            yield return SEMStep(1, -1); yield return SEMStep(2, -1); yield return SEMStep(3, -1);

            yield return SEMStep(-3, 0); yield return SEMStep(-2, 0); yield return SEMStep(-1, 0); yield return SEMStep(0, 0);
            yield return SEMStep(1, 0); yield return SEMStep(2, 0); yield return SEMStep(3, 0);

            yield return SEMStep(-3, 1); yield return SEMStep(-2, 1); yield return SEMStep(-1, 1); yield return SEMStep(0, 1);
            yield return SEMStep(1, 1); yield return SEMStep(2, 1); yield return SEMStep(3, 1);

            yield return SEMStep(-2, 2); yield return SEMStep(-1, 2); yield return SEMStep(0, 2); yield return SEMStep(1, 2); yield return SEMStep(2, 2);

            yield return SEMStep(-1, 3); yield return SEMStep(0, 3); yield return SEMStep(1, 3);

            yield return SEMStep(0, 0);

             //

        }
        // Update is called once per frame
        void Update()
        {

        }
        public IEnumerator SEMReset()
        {
            SEMCount = 0;
            GoodChipCount = 0;
            DefectiveChipCount = 0;
            UIManager.Instance.GoodToggle.isOn = false;
            UIManager.Instance.DefectiveToggle.isOn = false;

            UIManager.Instance.DefectiveRateText.text = "Defective Rate: 00.00%";
            UIManager.Instance.GDChipText.text = "Good: 0, Defective: 0";

            UIManager.Instance.GoodProductInpuField.text = "0";
            UIManager.Instance.DefectiveProductInpuField.text = "0";
            foreach (RawImage cd in UIManager.Instance.ChipDataRawImage)
                cd.color = new Color(255, 255, 255);

            ChipData.Clear();

            UIManager.Instance.SEMSensorToggle.isOn = true;
            SEMActSensor.GetComponent<Renderer>().material.color = new Color(0, 255, 0); // Green

            yield return new WaitForEndOfFrame();
        }
        public IEnumerator SEMStep(int x, int z)
        {
            Vector3 prevAxis1Pos = new Vector3(SEMAxis1.localPosition.x, SEMAxis1.localPosition.y, SEMAxis1.localPosition.z); // Axis1: Y축 기준으로 이동
            Vector3 nextAxis1APos = new Vector3(SEMAxis1Origin.x + x * xchipGap, SEMAxis1.localPosition.y, SEMAxis1.localPosition.z);

            Vector3 prevAxis2Pos = new Vector3(SEMAxis2.localPosition.x, SEMAxis2.localPosition.y, SEMAxis2.localPosition.z); // Axis1: Y축 기준으로 이동
            Vector3 nextAxis2APos = new Vector3(SEMAxis2.localPosition.x, SEMAxis2.localPosition.y, SEMAxis2Origin.z + z * zchipGap);

            float currentTime = 0;

            while (true)
            {
                currentTime += Time.deltaTime;
                if ((currentTime / speed) > 1)
                {
                    break;
                }
                SEMAxis1.localPosition = Vector3.Lerp(prevAxis1Pos, nextAxis1APos, currentTime / speed);
                SEMAxis2.localPosition = Vector3.Lerp(prevAxis2Pos, nextAxis2APos, currentTime / speed);
                UIManager.Instance.SEMPositionInpuField.text = "( " + (SEMAxis1.localPosition.x).ToString("0.0000") + " , " + (SEMAxis2.localPosition.z).ToString("0.0000") +" )";
                yield return new WaitForEndOfFrame();
            }
            ChipData.Add(Random.Range(0, 10));
            print("ChipData: " + ChipData[SEMCount] + " SEMCount: " + SEMCount);

            // ChipData 범위 정의
            if (ChipData[SEMCount] > 2 && ChipData[SEMCount] <= 8)
            {
                UIManager.Instance.ChipDataRawImage[SEMCount].color = new Color(0, 255, 0); // Green
                UIManager.Instance.GoodToggle.isOn = true;
                UIManager.Instance.DefectiveToggle.isOn = false;
                GoodChipCount++;
            }
                

            else if (ChipData[SEMCount] <= 2)
            {
                UIManager.Instance.ChipDataRawImage[SEMCount].color = new Color(0, 0, 255); // Blue
                UIManager.Instance.GoodToggle.isOn = false;
                UIManager.Instance.DefectiveToggle.isOn = true;
                DefectiveChipCount++;
            }
                

            else if (ChipData[SEMCount] > 8)
            {
                UIManager.Instance.ChipDataRawImage[SEMCount].color = new Color(255, 0, 0); // Red
                UIManager.Instance.GoodToggle.isOn = false;
                UIManager.Instance.DefectiveToggle.isOn = true;
                DefectiveChipCount++;
            }
             DefectiveRate = (DefectiveChipCount / (GoodChipCount + DefectiveChipCount)) * 100;
            UIManager.Instance.DefectiveRateText.text = "Defective Rate: " + DefectiveRate.ToString("0.00") + "%";
            UIManager.Instance.GDChipText.text = "Good: " + GoodChipCount.ToString() + ", Defective: " + DefectiveChipCount.ToString();

            SEMCount++;
            if(SEMCount == 37)
            {
                if(DefectiveRate > 30)
                {
                    DefectiveProductCount++;
                    UIManager.Instance.DefectiveProductInpuField.text = DefectiveProductCount.ToString();
                }
                    
                else if (DefectiveRate <= 30)
                {
                    GoodProductCount++;
                    UIManager.Instance.DefectiveProductInpuField.text = GoodProductCount.ToString();
                }
                    
            }
            yield return new WaitForSeconds(duration);
        }


    }
}

