using System.Collections.Generic;

using UnityEngine;




namespace MPS
{
   


    public class Sensor : MonoBehaviour
    {
        public static Sensor instance;

        public GameObject WaferSensor;
        public GameObject FoupPosSensor;
        public List<GameObject> SensorTower;
        public List<GameObject> VacuumSensors;
    
        public bool isWaferSensed = false;
        public bool isFoupSensed = false;
        public bool isVacuumOn = false;


        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
        }

        public void Start()
        {
            //OnSensorTower("green");
            //OnVacuumSensor(0);
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        public void OnSensorTower(string color)
        {
            switch(color)
            {
                case "red":
                    SensorTower[0].GetComponent<Renderer>().material.color = new Color(255, 0, 0); // RED
                    SensorTower[1].GetComponent<Renderer>().material.color = new Color(0, 0, 0); // Black
                    SensorTower[2].GetComponent<Renderer>().material.color = new Color(0, 0, 0); // Black
                    break;

                case "yellow":
                    SensorTower[0].GetComponent<Renderer>().material.color = new Color(0, 0, 0); // Black
                    SensorTower[1].GetComponent<Renderer>().material.color = new Color(255, 255, 0); // yellow
                    SensorTower[2].GetComponent<Renderer>().material.color = new Color(0, 0, 0); // Black
                    break;

                case "green":
                    SensorTower[0].GetComponent<Renderer>().material.color = new Color(0, 0, 0); // Black
                    SensorTower[1].GetComponent<Renderer>().material.color = new Color(0, 0, 0); // Black
                    SensorTower[2].GetComponent<Renderer>().material.color = new Color(0, 255, 0); // Green
                    break;

                case "black":
                    SensorTower[0].GetComponent<Renderer>().material.color = new Color(0, 0, 0); // Black
                    SensorTower[1].GetComponent<Renderer>().material.color = new Color(0, 0, 0); // Black
                    SensorTower[2].GetComponent<Renderer>().material.color = new Color(0, 0, 0); // Black
                    break;

            }        
        }

        /*public void OnVacuumSensor(int vs)
        {
            if (isVacuumOn)
                VacuumSensors[vs].GetComponent<Renderer>().material.color = new Color(0, 255, 0); // Green

            else if (!isVacuumOn)
                VacuumSensors[vs].GetComponent<Renderer>().material.color = new Color(0, 0, 0); // Black

        }*/

        public void OnTriggerStay(Collider other)
        {

            if (other.tag == "Wafer")
            {
                isWaferSensed = true;
                print("Wafer Sensed");
                other.transform.SetParent(transform);
            }

            if (other.tag == "Foup")
            {
                isFoupSensed = true;
                print("Foup Sensed");
                //other.transform.SetParent(transform);
            }

        
        }

        public void OnTriggerExit(Collider other)
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