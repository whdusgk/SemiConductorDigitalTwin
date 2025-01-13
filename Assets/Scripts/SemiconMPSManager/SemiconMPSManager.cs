using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 공압이 공급되면 실린더 로드가 일정 거리만큼, 일정 속도로 전진 또는 후진한다.
/// 전진 또는 후진 시, 전후진 Limit Switch(LS)가 작동한다.
/// 속성: 실린더로드, Min-Max Range, Duration, 전후방 Limit Switch
/// </summary>

namespace MPS
{
    [Serializable]
    public class SemiconMPSManager : MonoBehaviour
    {
      
        [SerializeField] List<Transform> LithoDoor;
        [SerializeField] List<Transform> gateValveDoor;
        [SerializeField] float LithoMaxRange;
        [SerializeField] float LithoMinRange;
        [SerializeField] float GateValveMaxRange;
        [SerializeField] float GateValveMinRange;
        [SerializeField] float duration;

        public bool isUp = false;

        public int cycleCnt;
        public float cycleTime;

        [Header("Sensors")]
        //public List<GameObject> Foup1Sensors;
        public List<GameObject> Foup2Sensors;
        //public List<GameObject> RobotActSensors;
        public List<GameObject> VacuumSensors;

        public List<GameObject> GateValveUpSensors;
        public List<GameObject> GateValveDownSensors;
        public List<GameObject> LithoGateUpSensors;
        public List<GameObject> LithoGateDownSensors;

        public List<GameObject> LithoActSensors;

        public GameObject SEMActSensor;

        public bool isFoupForward = false;
        public bool isFoupDoorForward = false;
        public bool isFoupDoorBackward = false;
        public bool isFoupOpen = false;

        public bool isRobotAct = false;
        public bool isVacuum = false;
        public bool isGateValveUp = false;
        public bool isLithoGateUp = false;
        public bool isLithoWafer = false;
        public bool isSEMAct = false;

        private void Start()
        {
            // ETC Sensor Caligration(Black)
            //foreach (GameObject s in Foup1Sensors) s.GetComponent<Renderer>().material.color = new Color(0, 0, 0); // Black
            foreach (GameObject s in Foup2Sensors) s.GetComponent<Renderer>().material.color = new Color(0, 0, 0); // Black       
            foreach (GameObject s in VacuumSensors) s.GetComponent<Renderer>().material.color = new Color(0, 0, 0); // Black       
            foreach (GameObject s in GateValveUpSensors) s.GetComponent<Renderer>().material.color = new Color(0, 0, 0); // Black
            foreach (GameObject s in LithoGateUpSensors) s.GetComponent<Renderer>().material.color = new Color(0, 0, 0); // Black 
            foreach (GameObject s in LithoActSensors) s.GetComponent<Renderer>().material.color = new Color(0, 0, 0); // Black
            SEMActSensor.GetComponent<Renderer>().material.color = new Color(0, 0, 0); // Black

            // GateDownSensor Caligration(Green)
            foreach (GameObject s in GateValveDownSensors) s.GetComponent<Renderer>().material.color = new Color(255, 0,  0); // RED
            foreach (GameObject s in LithoGateDownSensors) s.GetComponent<Renderer>().material.color = new Color(255, 0,  0); // RED
        }

        public void OnUpBtnClkEvent()
        {
            if (isUp) return;

            cycleCnt++;

            foreach (Transform l in LithoDoor)
            {
                StartCoroutine(MoveGate(l, LithoMinRange, LithoMaxRange, duration));
                LithoGateUpSensors[0].GetComponent<Renderer>().material.color = new Color(0, 255, 0); // Green
                LithoGateDownSensors[0].GetComponent<Renderer>().material.color = new Color(0, 0, 0); // Black
            }

            foreach (Transform gv in gateValveDoor)
            {
                StartCoroutine(MoveGate(gv, GateValveMinRange, GateValveMaxRange, duration));
                GateValveUpSensors[0].GetComponent<Renderer>().material.color = new Color(0, 255, 0); // Green
                GateValveDownSensors[0].GetComponent<Renderer>().material.color = new Color(0, 0, 0); // Black
            }

        }

        public void OnDownBtnClkEvent()
        {
            if (!isUp) return;

            foreach (Transform l in LithoDoor)
            {
                StartCoroutine(MoveGate(l, LithoMaxRange, LithoMinRange, duration));
                LithoGateUpSensors[0].GetComponent<Renderer>().material.color = new Color(0, 0, 0); // Black
                LithoGateDownSensors[0].GetComponent<Renderer>().material.color = new Color(255, 0, 0); // Red
            }

            foreach (Transform gv in gateValveDoor)
            {
                StartCoroutine(MoveGate(gv, GateValveMaxRange, GateValveMinRange, duration));
                GateValveUpSensors[0].GetComponent<Renderer>().material.color = new Color(0, 0, 0); // Black
                GateValveDownSensors[0].GetComponent<Renderer>().material.color = new Color(255, 0, 0); // Red
            }
        }

        IEnumerator MoveGate(Transform gate, float min, float max, float duration)
        {

            Vector3 minPos = new Vector3(gate.transform.localPosition.x, min, gate.transform.localPosition.z);
            Vector3 maxPos = new Vector3(gate.transform.localPosition.x, max, gate.transform.localPosition.z);

            float currentTime = 0;

            while (currentTime <= duration)
            {
                currentTime += Time.deltaTime;

                gate.localPosition = Vector3.Lerp(minPos, maxPos, currentTime / duration);

                cycleTime += Time.deltaTime;

                yield return new WaitForEndOfFrame();
            }

            isUp = !isUp;

        }

    }
}

