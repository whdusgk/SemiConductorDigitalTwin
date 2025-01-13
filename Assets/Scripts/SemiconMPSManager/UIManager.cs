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
    [SerializeField] TMP_Text ProductsText;

    [SerializeField] List<TMP_InputField> ProcessPositionsInpuField = new List<TMP_InputField>();
    [SerializeField] List<TMP_InputField> RobotPositionsInpuField = new List<TMP_InputField>();// 미구현
    [SerializeField] List<Toggle> FoupSensorsToggle = new List<Toggle>();
    [SerializeField] List<Toggle> VacuumSensorsToggle = new List<Toggle>();
    [SerializeField] List<Toggle> LoadlockSensorsToggle = new List<Toggle>();

    [SerializeField] Toggle AlignSensorToggle;
    [SerializeField] TMP_InputField AlignPositionInpuField;
    [SerializeField] Toggle LithoSensorToggle;
    [SerializeField] TMP_InputField LithoPositionInpuField;

    [SerializeField] Toggle SEMSensorToggle;
    [SerializeField] TMP_InputField SEMPositionInpuField;

    [SerializeField] List<RawImage> ChipDataRawImage = new List<RawImage>(); // 미구현
    public List<float> ChipData; // 미구현
    [SerializeField] TMP_Text DefectiveRateText; // 미구현
    [SerializeField] Toggle GoodToggle;
    [SerializeField] Toggle DefectiveToggle;

    [SerializeField] TMP_Text GDProductsText;

    [SerializeField] TMP_InputField GoodInpuField; // 미구현
    [SerializeField] TMP_InputField DefectiveInpuField; // 미구현

    void Start()
    {
        EnergyConsumptionInpuField.text = Random.Range(0, 10).ToString();
        TemperatureInpuField.text = Random.Range(0, 10).ToString();
        HumidityInpuField.text = Random.Range(0, 10).ToString();
        ProductsText.text = Random.Range(0, 10).ToString();

        ProcessPositionsInpuField[0].text = "0,0,0";
        ProcessPositionsInpuField[1].text = "0,0,0";
        ProcessPositionsInpuField[2].text = "0,0,0";
        ProcessPositionsInpuField[3].text = "0,0,0";
        ProcessPositionsInpuField[4].text = "0,0,0";
        ProcessPositionsInpuField[5].text = "0,0,0";

        FoupSensorsToggle[0].isOn = false;
        FoupSensorsToggle[1].isOn = false;

        VacuumSensorsToggle[0].isOn = false;
        VacuumSensorsToggle[1].isOn = false;
        VacuumSensorsToggle[2].isOn = false;
        VacuumSensorsToggle[3].isOn = false;
        VacuumSensorsToggle[4].isOn = false;
        VacuumSensorsToggle[5].isOn = false;
        VacuumSensorsToggle[6].isOn = false;

        LoadlockSensorsToggle[0].isOn = false;
        LoadlockSensorsToggle[1].isOn = false;

        AlignSensorToggle.isOn = false;
        LithoSensorToggle.isOn = false;
        AlignPositionInpuField.text = "0,0,0";
        LithoPositionInpuField.text = "0,0,0";

        SEMSensorToggle.isOn = false;
        SEMPositionInpuField.text = "0,0,0";

        GoodToggle.isOn = false;
        DefectiveToggle.isOn = false;

        GDProductsText.text = "Good/Defective Products";

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
