using System.Collections.Generic;

using UnityEngine;

namespace MPS
{
    public class Sensor : MonoBehaviour
    {

        public static Sensor Instance;
        public List<GameObject> VacuumSensors;
        public GameObject SEMActSensor;

        public bool isVacuumOn = false;
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }
        public void Start()
        {
            SEMActSensor.GetComponent<Renderer>().material.color = new Color(255, 0, 0);
            isVacuumOn = true;
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
       
        public void OnVacuumSensor(int vs)
        {
            if (isVacuumOn)
                VacuumSensors[vs].GetComponent<Renderer>().material.color = new Color(0, 255, 0); // Green

            else if (!isVacuumOn)
                VacuumSensors[vs].GetComponent<Renderer>().material.color = new Color(0, 0, 0); // Black

        }

      
    }
}

