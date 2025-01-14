using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace MPS
{
    public class SEMManager : MonoBehaviour
    {
        public static SEMManager semManager;
        UIManager uiManager;
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

        List<int> SEMxPos = new List<int> { -3, -2, -1, 0, 1, 2, 3 };
        List<int> SEMzPos = new List<int> { -3, -2, -1, 0, 1, 2, 3 };
        Vector3 SEMAxis1Origin;
        Vector3 SEMAxis2Origin;
        public int SEMCount = 0;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            SEMAxis1Origin = new Vector3(SEMAxis1.localPosition.x, SEMAxis1.localPosition.y, SEMAxis1.localPosition.z);
            SEMAxis2Origin = new Vector3(SEMAxis2.localPosition.x, SEMAxis2.localPosition.y, SEMAxis2.localPosition.z);

            //StartCoroutine(SEMStep(0,0));
            StartCoroutine(RunSEM());

        }

        public IEnumerator RunSEM()
        {
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

        }
        // Update is called once per frame
        void Update()
        {

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


                yield return new WaitForEndOfFrame();
            }
            ChipData.Add(Random.Range(0, 10));
            print("ChipData: " + ChipData[SEMCount] + " SEMCount: " + SEMCount);

            //uiManager.ChipDataRawImage[0].color = new Color(0, 255, 0);
            SEMCount++;
            yield return new WaitForSeconds(duration);
        }


    }
}

