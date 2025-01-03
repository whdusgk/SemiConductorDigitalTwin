using System.Collections.Generic;
using UnityEngine;

public class Sensor : MonoBehaviour
{

    public List<GameObject> Foup1Sensors;
    public List<GameObject> Foup2Sensors;
    public List <GameObject> RobotActSensors;
    public List <GameObject> RobotWaferSensors;
    public List <GameObject> VacuumSensors;

    public List<GameObject> GateValveUpSensors;
    public List<GameObject> GateValveDownSensors;
    public List<GameObject> LithoGateUpSensors;
    public List<GameObject> LithoGateDownSensors;

    public List<GameObject> LithoWaferSensors;
    public List<GameObject> LithoActSensors;

    public List<GameObject> LoadlockWaferSensors;

    public GameObject SEMWaferSensor;
    public GameObject SEMActSensor;

    public bool isFoupForward = false;
    public bool isFoupDoorForward = false;
    public bool isFoupDoorBackward = false;
    public bool isFoupOpen = false;

    public bool isRobotAct = false;
    public bool isRobotWafer = false;
    public bool isVacuum = false;
    public bool isGateValveUp = false;
    public bool isLithoGateUp = false;  
    public bool isLithoWafer = false;  
    public bool isLoadlockWafer = false;
    public bool isSEMWafer = false;
    public bool isSEMAct = false;
    public bool isEnabled = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // ETC Sensor Caligration(Black)
        foreach(GameObject s in Foup1Sensors) s.GetComponent<Renderer>().material.color = new Color(0, 0, 0); // Black
        foreach (GameObject s in Foup1Sensors) s.GetComponent<Renderer>().material.color = new Color(0, 0, 0); // Black       
        foreach (GameObject s in RobotActSensors) s.GetComponent<Renderer>().material.color = new Color(0, 0, 0); // Black          
        foreach (GameObject s in VacuumSensors) s.GetComponent<Renderer>().material.color = new Color(0, 0, 0); // Black       
        foreach (GameObject s in GateValveUpSensors) s.GetComponent<Renderer>().material.color = new Color(0, 0, 0); // Black
        foreach (GameObject s in LithoGateUpSensors) s.GetComponent<Renderer>().material.color = new Color(0, 0, 0); // Black 
        foreach (GameObject s in LithoActSensors) s.GetComponent<Renderer>().material.color = new Color(0, 0, 0); // Black
        SEMActSensor.GetComponent<Renderer>().material.color = new Color(0, 0, 0); // Black
        
        // GateDownSensor Caligration(Green)
        foreach (GameObject s in GateValveDownSensors) s.GetComponent<Renderer>().material.color = new Color(0, 255, 0); // Green
        foreach (GameObject s in LithoGateDownSensors) s.GetComponent<Renderer>().material.color = new Color(0, 255, 0); // Green

    }

    // Update is called once per frame
    void Update()
    {
        if(isGateValveUp)
        {
            GateValveUpSensors[0].GetComponent<Renderer>().material.color = new Color(0, 255, 0); // Green

        }
        else if(!isGateValveUp)
        {
            GateValveUpSensors[0].GetComponent<Renderer>().material.color = new Color(0, 0, 0); // Black
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Wafer")
        {
            isRobotWafer = true;
            print("Wafer Sensed");
        }
    }

/*    private void OnTriggerExit(Collider other)
    {
        if (isRobotWafer)
            isRobotWafer = false;
    }*/
}
