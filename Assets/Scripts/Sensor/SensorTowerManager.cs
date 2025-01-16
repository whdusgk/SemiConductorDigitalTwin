using System.Collections.Generic;

using UnityEngine;

namespace MPS
{
    public class SensorTowerManager : MonoBehaviour
    {

        public static SensorTowerManager Instance;
        public List<GameObject> SensorTower;

        public void Start()
        {
            SensorTower[0].GetComponent<Renderer>().material.color = new Color(0, 0, 0); // Black
            //SensorTower[1].GetComponent<Renderer>().material.color = new Color(0, 0, 0); // Black
            //SensorTower[2].GetComponent<Renderer>().material.color = new Color(0, 0, 0); // Black
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        public void OnSensorTower(string color)
        {
            switch (color)
            {
                case "red":
                    SensorTower[0].GetComponent<Renderer>().material.color = new Color(255, 0, 0); // RED
                    //SensorTower[1].GetComponent<Renderer>().material.color = new Color(0, 0, 0); // Black
                    //SensorTower[2].GetComponent<Renderer>().material.color = new Color(0, 0, 0); // Black
                    break;

                case "yellow":
                    //SensorTower[0].GetComponent<Renderer>().material.color = new Color(0, 0, 0); // Black
                    SensorTower[0].GetComponent<Renderer>().material.color = new Color(255, 255, 0); // yellow
                    //SensorTower[2].GetComponent<Renderer>().material.color = new Color(0, 0, 0); // Black
                    break;

                case "green":
                    //SensorTower[0].GetComponent<Renderer>().material.color = new Color(0, 0, 0); // Black
                    //SensorTower[1].GetComponent<Renderer>().material.color = new Color(0, 0, 0); // Black
                    SensorTower[0].GetComponent<Renderer>().material.color = new Color(0, 255, 0); // Green
                    break;

                case "black":
                    SensorTower[0].GetComponent<Renderer>().material.color = new Color(0, 0, 0); // Black
                    //SensorTower[1].GetComponent<Renderer>().material.color = new Color(0, 0, 0); // Black
                    //SensorTower[2].GetComponent<Renderer>().material.color = new Color(0, 0, 0); // Black
                    break;

            }
        }

    }
}

