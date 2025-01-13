using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] TMP_InputField EnergyConsumptionInpuField;
    [SerializeField] TMP_InputField TemperatureInpuField;
    [SerializeField] TMP_InputField HumidityInpuField;
    [SerializeField] TMP_InputField ProductsInpuField;

    [SerializeField] List<TMP_InputField> ProcessPositionsInpuField = new List<TMP_InputField>();
    [SerializeField] List<TMP_InputField> RobotPositionsInpuField = new List<TMP_InputField>();
    [SerializeField] List<Toggle> FoupSensorsToggle = new List<Toggle>();
    [SerializeField] List<Toggle> VacuumSensorsToggle = new List<Toggle>();
    [SerializeField] List<Toggle> LoadlockSensorsToggle = new List<Toggle>();

    [SerializeField] Toggle AlignSensorToggle;
    [SerializeField] TMP_InputField AlignPositionInpuField;
    [SerializeField] Toggle LithoSensorToggle;
    [SerializeField] TMP_InputField LithoPositionInpuField;

    [SerializeField] Toggle SEMSensorToggle;
    [SerializeField] TMP_InputField SEMPositionInpuField;

    [SerializeField] List<RawImage> ChipData = new List<RawImage>();
    [SerializeField] TMP_InputField DefectiveRateInpuField;
    [SerializeField] Toggle GoodToggle;
    [SerializeField] Toggle DefectiveToggle;

    [SerializeField] TMP_InputField GDProductsInpuField;



    void Start()
    {
        EnergyConsumptionInpuField.text = Random.Range(0, 10).ToString();
        TemperatureInpuField.text = Random.Range(0, 10).ToString();
        HumidityInpuField.text = Random.Range(0, 10).ToString();
        ProductsInpuField.text = Random.Range(0, 10).ToString();

        ProcessPositionsInpuField[0].text = "0,0,0";
        ProcessPositionsInpuField[1].text = "0,0,0";
        ProcessPositionsInpuField[2].text = "0,0,0";
        ProcessPositionsInpuField[3].text = "0,0,0";
        ProcessPositionsInpuField[4].text = "0,0,0";
        ProcessPositionsInpuField[5].text = "0,0,0";

        FoupSensorsToggle[0].isOn = false;
        FoupSensorsToggle[1].isOn = false;

        VacuumSensorsToggle[0].isOn = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
