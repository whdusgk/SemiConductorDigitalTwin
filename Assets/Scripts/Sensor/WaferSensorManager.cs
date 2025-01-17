using System.Collections.Generic;

using UnityEngine;

namespace MPS
{
    public class WaferSensorManager : MonoBehaviour
    {

        public static WaferSensorManager Instance;
        public GameObject WaferSensor;
        public GameObject FoupPosSensor;

        public bool isWaferSensed = false;
        public bool isFoupSensed = false;
        public void Start()
        {
            
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
       


        private void OnTriggerStay(Collider other)
        {

            if (other.tag == "Wafer")
            {
                isWaferSensed = true;
                print("Wafer Sensed");
                other.transform.SetParent(transform);
            }
            
            if (other.tag == "Foup")
            {
                if (FoupPosSensor == null) return;
                isFoupSensed = true;
                print("Foup Sensed");
                //other.transform.SetParent(transform);
            }


        }

        private void OnTriggerExit(Collider other)
        {
            if (isWaferSensed)
            {
                isWaferSensed = false;
                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).SetParent(null);
                }
            }

            if (isFoupSensed)
                isFoupSensed = false;
        }
    }
}

