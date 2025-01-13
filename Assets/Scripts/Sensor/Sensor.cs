using System.Collections.Generic;
using UnityEngine;

namespace MPS
{ 

    public class Sensor : MonoBehaviour
    {
        public GameObject WaferSensor;

        public bool isWaferSensed = false;


        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {


        }

        // Update is called once per frame
        void Update()
        {
        
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.tag == "Wafer")
            {
                isWaferSensed = true;
                print("Wafer Sensed");
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (isWaferSensed)
                isWaferSensed = false;
        }
    }

}