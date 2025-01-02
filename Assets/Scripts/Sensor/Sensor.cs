using UnityEngine;

public class Sensor : MonoBehaviour
{
    public GameObject P2VacuumSensor;
    public GameObject P3VacuumSensor;
    public GameObject P4VacuumSensor;
    public GameObject P5VacuumSensor;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        P2VacuumSensor.GetComponent<Renderer>().material.color = new Color(0, 0, 0);
        P3VacuumSensor.GetComponent<Renderer>().material.color = new Color(0, 0, 0);
        P4VacuumSensor.GetComponent<Renderer>().material.color = new Color(0, 0, 0);
        P5VacuumSensor.GetComponent<Renderer>().material.color = new Color(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnStartBtnClkEvent()
    {
        P2VacuumSensor.GetComponent<Renderer>().material.color = new Color(0, 255, 0);
        P3VacuumSensor.GetComponent<Renderer>().material.color = new Color(0, 255, 0);
        P4VacuumSensor.GetComponent<Renderer>().material.color = new Color(0, 255, 0);
        P5VacuumSensor.GetComponent<Renderer>().material.color = new Color(0, 255, 0);
    }
}
