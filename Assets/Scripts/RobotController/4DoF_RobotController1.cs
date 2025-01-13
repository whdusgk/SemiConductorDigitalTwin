using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Collections;
using System.IO;
using static RobotController_4DoF1;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using static UnityEditor.PlayerSettings;
using static UnityEngine.Rendering.DebugUI.Table;
using Unity.VisualScripting;

/// <summary>
/// �κ� 3D Object�� RobotController�� ��ư, ��ǲ�ʵ��� ������ �����δ�.
/// - Teach ��ư�� ������ �� Axis�� ���� Step���� ����ȴ�.
/// - SingleCycle, Cycle, Stop, E-Stop ��ư�� ������ �κ��� �����Ѵ�.
/// �ʿ�Ӽ�: �κ��� ���� ȸ�� �ӵ�(0~100), Duration, Min Angle, Max Angle
///          step(speed, duration, suction, angles)
/// </summary>
public class RobotController_4DoF1 : MonoBehaviour
{
    [Serializable]
    public class Step
    {
        public int stepNumber;
        public float speed = 100;
        public float duration;

        public float disAxis1;
        public float minDisAxis1;
        public float maxDisAxis;

        public float angleAxis2;
        public float minAngleAxis2;
        public float maxAngleAxis2;

        public float angleAxis3;
        public float minAngleAxis3;
        public float maxAngleAxis3;

        public float angleAxis4;
        public float minAngleAxis4;
        public float maxAngleAxis4;


        public Step(int _stepNumber, float _speed, float _duration)
        {
            stepNumber = _stepNumber;
            speed = _speed;
            duration = _duration;

        }
    }
    public string robotFile;
    public List<Step> steps = new List<Step>();
    [SerializeField] Step originStep;
    [SerializeField] Step eStopStep;
    public bool isRunning = false;
    [SerializeField] bool isStopped = false;
    [SerializeField] bool isEStopped = false;
    [SerializeField] bool isCycleClicked = false;
    [SerializeField] float resolution = 0.2f;

    [Header("Axis Pivots")]
    [SerializeField] Transform motorAxis1;
    [SerializeField] Transform motorAxis2;
    [SerializeField] Transform motorAxis3;
    [SerializeField] Transform motorAxis4;

    [Header("UI ����")]
    [SerializeField] TMP_Text nowStepInfoTxt;
    public int totalSteps;
    public int currentStepNumber;
    public int cycleCnt;
    public float cycleTime;
    public DateTime lastMaintenanceTime;
    public DateTime nextMaintenanceTime;
    [SerializeField] TMP_InputField stepInput;
    [SerializeField] TMP_InputField speedInput;
    [SerializeField] TMP_InputField durationInput;

    [SerializeField] TMP_InputField disAxis1Input;
    [SerializeField] TMP_InputField angleAxis2Input;
    [SerializeField] TMP_InputField angleAxis3Input;
    [SerializeField] TMP_InputField angleAxis4Input;
    [SerializeField] List<Button> buttons = new List<Button>();
    [SerializeField] TMP_InputField loadFileInput;

    Coroutine currentCoroutine;
    int cycleClkCnt = 0;

    float disAxis1;
    float angleAxis2;
    float angleAxis3;
    float angleAxis4;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        originStep = new Step(-1, 100, 0);

        nowStepInfoTxt.text = $"Total step count: {totalSteps} / Current step: {currentStepNumber}";

        stepInput.text = "0";
        speedInput.text = "100";
        durationInput.text = "0";

        disAxis1Input.text = "0.002";
        angleAxis2Input.text = "120";
        angleAxis3Input.text = "60";
        angleAxis4Input.text = "-180";

        //loadFileInput.text = "robotSteps4DoF_Final.csv";
        OnLoadBtnClkEvent(robotFile);

    }

    // Teach ��ư�� ������ �� Axis�� ���� Step���� ����ȴ�.
    public void OnTeachBtnClkEvent()
    {
        //int stepNumber;
        //bool isCorrect = int.TryParse(stepInput.text, out stepNumber);
        //if (!isCorrect)
        //{
        //    print("�ùٸ� ���� �Է��� �ּ���.");
        //    return;
        //}

        float speed;
        bool isCorrect = float.TryParse(speedInput.text, out speed);
        if (!isCorrect)
        {
            print("�ùٸ� ���� �Է��� �ּ���.");
            return;
        }

        float duration;
        isCorrect = float.TryParse(durationInput.text, out duration);
        if (!isCorrect)
        {
            print("�ùٸ� ���� �Է��� �ּ���.");
            return;
        }

        float disAxis1;
        isCorrect = float.TryParse(disAxis1Input.text, out disAxis1);
        if (!isCorrect)
        {
            print("�ùٸ� ���� �Է��� �ּ���.");
            return;
        }

        float angleAxis2;
        isCorrect = float.TryParse(angleAxis2Input.text, out angleAxis2);
        if (!isCorrect)
        {
            print("�ùٸ� ���� �Է��� �ּ���.");
            return;
        }

        float angleAxis3;
        isCorrect = float.TryParse(angleAxis3Input.text, out angleAxis3);
        if (!isCorrect)
        {
            print("�ùٸ� ���� �Է��� �ּ���.");
            return;
        }

        float angleAxis4;
        isCorrect = float.TryParse(angleAxis4Input.text, out angleAxis4);
        if (!isCorrect)
        {
            print("�ùٸ� ���� �Է��� �ּ���.");
            return;
        }


        Step step = new Step(totalSteps, speed, duration);
        step.disAxis1 = disAxis1;
        step.angleAxis2 = angleAxis2;
        step.angleAxis3 = angleAxis3;
        step.angleAxis4 = angleAxis4;


        if (cycleClkCnt > 0 && steps.Count != 0)
        {
            steps.RemoveAt(steps.Count - 1);

            cycleClkCnt = 0;
        }

        steps.Add(step);

        totalSteps++;
        stepInput.text = totalSteps.ToString();
        nowStepInfoTxt.text = $"Total step count: {totalSteps} / Current step: {currentStepNumber}";
        print("Step�� �߰��Ǿ����ϴ�.");
    }

    // Clear ��ư�� ������ ��� Step���� ��������.
    public void OnClearBtnClkEvent()
    {
        steps.Clear();

        totalSteps = currentStepNumber = 0;

        nowStepInfoTxt.text = $"Total step count: {totalSteps} / Current step: {currentStepNumber}";

        stepInput.text = "0";
        speedInput.text = "100";
        durationInput.text = "0";

        disAxis1Input.text = "0.002";
        angleAxis2Input.text = "120";
        angleAxis3Input.text = "60";
        angleAxis4Input.text = "-180";
    }

    // SingleCycle, Cycle, Stop, E-Stop ��ư�� ������ �κ��� �����Ѵ�.
    public void OnSingleCycleBtnClkEvent()
    {
        if (isEStopped)
        {
            isEStopped = false;

            List<Step> newSteps = new List<Step>();
            // eStopStep -> (���� ����� ���� �ε��� + 1) -> ������ ���Ǳ��� �۵�
            // E-Stop ��ư ���� ������ ����(eStopStep) ����
            // ���� �۵����̴� ���� ���ǵ��� newSteps�� ����
            for (int i = currentStepNumber; i < steps.Count; i++)
            {
                newSteps.Add(steps[i]);
            }

            currentCoroutine = StartCoroutine(Run(newSteps));
        }
        else
        {
            if (isRunning) return;

            // �� ���ǿ� ���� �κ��� ���Ͱ� �������� �Ѵ�.
            currentCoroutine = StartCoroutine(Run());

            cycleCnt++;
        }

        SetButtonsActive(false);
    }

    public void OnCycleBtnClkEvent()
    {
        isCycleClicked = true;

        if (cycleClkCnt == 0)
            steps.Add(steps[0]);

        cycleClkCnt++;

        currentCoroutine = StartCoroutine(RunCycle());

        SetButtonsActive(false);
    }

    public void SetButtonsActive(bool active)
    {
        foreach (var btn in buttons)
        {
            btn.interactable = active;
        }
    }

    IEnumerator RunCycle()
    {


        while (isCycleClicked)
        {
            yield return Run();
        }

        if (cycleClkCnt > 0 && steps.Count != 0)
        {
            steps.RemoveAt(steps.Count - 1);

            cycleClkCnt = 0;

        }

        SetButtonsActive(true);
    }

    /// <summary>
    /// �� ����Ŭ ������ ���� -> ���� ��ġ�� ����
    /// </summary>
    public void OnStopBtnClkEvent()
    {
        isStopped = true;

        isCycleClicked = false;

    }

    /// <summary>
    /// ���� ��ġ���� �κ��� �������� ��� �����. -> �ش� ��ġ���� ���� �ٽ� ����
    /// </summary>
    public void OnEStopBtnClkEvent()
    {
        isEStopped = true;

        isCycleClicked = false;
        // ���ߴ� ���� ������ ������ ���ο� ���� ������ ����
        // -> �ٽ� �̱� ��ư Ŭ���� ���� ��ġ���� ���� ������ ���Ǳ��� ����
    }

    /// <summary>
    /// �κ��� �ֱ� ��ġ���� ���� ó�� ��ġ�� �̵�
    /// </summary>
    public void OnOriginBtnClkEvent()
    {
        if (isRunning) return;

        StartCoroutine(RunOrigin(steps[steps.Count - 1], originStep));

        cycleCnt++;
    }

    IEnumerator Run()
    {
        isRunning = true;
        if (steps.Count > 0)
        {
            for (int i = 0; i < steps.Count; i++)
            {
                currentStepNumber = i;
                nowStepInfoTxt.text = $"Total step count: {totalSteps} / Current step: {currentStepNumber}";

                if (i - 1 < 0)
                {
                    continue;
                }

                yield return RunStep(steps[i - 1], steps[i]);

                if (isEStopped)
                {
                    SetButtonsActive(true);
                    break;
                }
            }
        }

        if (!isCycleClicked)
        {
            SetButtonsActive(true);
        }

        isRunning = false;
    }

    IEnumerator Run(List<Step> stepList)
    {
        if (stepList.Count > 0)
        {
            for (int i = 0; i < stepList.Count; i++) // 2��: 0, 1, ?
            {
                currentStepNumber = i;
                nowStepInfoTxt.text = $"Total step count: {totalSteps} / Current step: {currentStepNumber}";

                if (i - 1 < 0)
                {
                    continue;
                }

                yield return RunStep(stepList[i - 1], stepList[i]);

                if (isEStopped)
                {
                    SetButtonsActive(true);
                    break;
                }
            }
        }
        isRunning = false;
    }

    IEnumerator RunStep(Step prevStep, Step nextStep)
    {
        Vector3 prevAxis1Pos = new Vector3(motorAxis1.localPosition.x, motorAxis1.localPosition.y, prevStep.disAxis1); // Axis1: Y�� �������� �̵�
        Vector3 nextAxis1APos = new Vector3(motorAxis1.localPosition.x, motorAxis1.localPosition.y, nextStep.disAxis1);

        Vector3 prevAxis2Euler = new Vector3(0, prevStep.angleAxis2, 0); // Axis2: Y�� �������� ȸ��
        Vector3 nextAxis2AEuler = new Vector3(0, nextStep.angleAxis2, 0);

        Vector3 prevAxis3Euler = new Vector3(0, prevStep.angleAxis3, 0); // Axis3: Y�� �������� ȸ��
        Vector3 nextAxis3AEuler = new Vector3(0, nextStep.angleAxis3, 0);

        Vector3 prevAxis4Euler = new Vector3(0, prevStep.angleAxis4, 0); // Axis4: Y�� �������� ȸ��
        Vector3 nextAxis4AEuler = new Vector3(0, nextStep.angleAxis4, 0);


        float currentTime = 0;
        while (!isEStopped)
        {
            currentTime += Time.deltaTime;

            if ((currentTime / (prevStep.speed * 0.01f)) > 1)
            {
                break;
            }

            //motorAxis1.localRotation = RotateAngle(prevAxis1Pos, nextAxis1APos, currentTime / (prevStep.speed * 0.01f));
            motorAxis1.localPosition = Vector3.Lerp(prevAxis1Pos, nextAxis1APos, currentTime / (prevStep.speed * 0.01f));
            motorAxis2.localRotation = RotateAngle(prevAxis2Euler, nextAxis2AEuler, currentTime / (prevStep.speed * 0.01f));
            motorAxis3.localRotation = RotateAngle(prevAxis3Euler, nextAxis3AEuler, currentTime / (prevStep.speed * 0.01f));
            motorAxis4.localRotation = RotateAngle(prevAxis4Euler, nextAxis4AEuler, currentTime / (prevStep.speed * 0.01f));

            cycleTime += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        if (isEStopped)
        {
            eStopStep = new Step(-1, prevStep.speed, prevStep.duration);
            // eStopStep.disAxis1 = motorAxis1.localRotation.eulerAngles.z;
            eStopStep.disAxis1 = motorAxis1.localPosition.y;
            eStopStep.angleAxis2 = motorAxis2.localRotation.eulerAngles.y;
            eStopStep.angleAxis3 = motorAxis3.localRotation.eulerAngles.y;
            eStopStep.angleAxis4 = motorAxis4.localRotation.eulerAngles.y;
        }
        cycleTime += Time.deltaTime;
        yield return new WaitForSeconds(prevStep.duration);
        isRunning = false;
    }
    IEnumerator RunOrigin(Step prevStep, Step nextStep)
    {
        isRunning = true;
        Vector3 prevAxis1Pos = new Vector3(motorAxis1.localPosition.x, prevStep.disAxis1, motorAxis1.localPosition.z); // Axis1: Z�� �������� ȸ��
        Vector3 nextAxis1Pos = new Vector3(motorAxis1.localPosition.x, nextStep.disAxis1, motorAxis1.localPosition.z);

        Vector3 prevAxis2Euler = new Vector3(0, prevStep.angleAxis2, 0); // Axis2: Z�� �������� ȸ��
        Vector3 nextAxis2AEuler = new Vector3(0, nextStep.angleAxis2, 0);

        Vector3 prevAxis3Euler = new Vector3(0, prevStep.angleAxis3, 0); // Axis3: Z�� �������� ȸ��
        Vector3 nextAxis3AEuler = new Vector3(0, nextStep.angleAxis3, 0);

        Vector3 prevAxis4Euler = new Vector3(0, prevStep.angleAxis4, 0); // Axis4: Z�� �������� ȸ��
        Vector3 nextAxis4AEuler = new Vector3(0, nextStep.angleAxis4, 0);


        float currentTime = 0;
        while (!isEStopped)
        {
            currentTime += Time.deltaTime;

            if ((currentTime / (prevStep.speed * 0.01f)) > 1)
            {
                break;
            }

            //motorAxis1.localRotation = RotateAngle(prevAxis1Euler, nextAxis1AEuler, currentTime / (prevStep.speed * 0.01f));
            motorAxis1.localPosition = Vector3.Lerp(prevAxis1Pos, nextAxis1Pos, currentTime / (prevStep.speed * 0.01f));
            motorAxis2.localRotation = RotateAngle(prevAxis2Euler, nextAxis2AEuler, currentTime / (prevStep.speed * 0.01f));
            motorAxis3.localRotation = RotateAngle(prevAxis3Euler, nextAxis3AEuler, currentTime / (prevStep.speed * 0.01f));
            motorAxis4.localRotation = RotateAngle(prevAxis4Euler, nextAxis4AEuler, currentTime / (prevStep.speed * 0.01f));

            yield return new WaitForEndOfFrame();
        }

        if (isEStopped)
        {
            eStopStep = new Step(-1, prevStep.speed, prevStep.duration);
            eStopStep.disAxis1 = motorAxis1.localPosition.y;
            eStopStep.angleAxis2 = motorAxis2.localRotation.eulerAngles.y;
            eStopStep.angleAxis3 = motorAxis3.localRotation.eulerAngles.y;
            eStopStep.angleAxis4 = motorAxis4.localRotation.eulerAngles.y;
        }

        yield return new WaitForSeconds(prevStep.duration);
        isRunning = false;
    }
    private Quaternion RotateAngle(Vector3 from, Vector3 to, float t)
    {
        return Quaternion.Slerp(Quaternion.Euler(from), Quaternion.Euler(to), t);
    }
    

    /// <summary>
    /// Steps ����Ʈ�� ����� step���� CSV ������������ �����Ѵ�.
    /// ����: stepNumber,speed,duration,isSuctionOn,axis1,axis2,axis3,axis4,axis5
    /// </summary>
    public void SaveCSVFile(Step step)
    {
        using (FileStream fs = new FileStream("robotSteps4DoF.csv", FileMode.Append))
        {
            using (StreamWriter sw = new StreamWriter(fs))
            {
                string line = $"{step.stepNumber},{step.speed},{step.duration}," +
                    $"{step.disAxis1},{step.angleAxis2},{step.angleAxis3},{step.angleAxis4}";

                sw.WriteLine(line);
            }
        }
    }

    int fileCnt = 0;
    public void OnSaveBtnClkEvent()
    {
        string path = $"robotSteps4DoF_{fileCnt++}.csv";
        if (File.Exists(path))
        {
            print("������ �̹� �����մϴ�. �ٽ� �ѹ� Ŭ���ϸ� ���ο� ������ �����˴ϴ�.");

            return;
        }

        using (FileStream fs = new FileStream(path, FileMode.Append))
        {
            using (StreamWriter sw = new StreamWriter(fs))
            {                                                                                                                                                                                                         
                foreach (var step in steps)
                {
                    string line = $"{step.stepNumber},{step.speed},{step.duration}," +
                    $"{step.disAxis1},{step.angleAxis2},{step.angleAxis3},{step.angleAxis4}";

                    sw.WriteLine(line);
                }
            }
        }

        print($"������ �����߽��ϴ�. {path}");
    }
    bool isSuctionOn;
    public void OnLoadBtnClkEvent(string path)
    {
        path = loadFileInput.text; // robotSteps_0.csv

        if (File.Exists(path))
        {
            List<Step> tempSteps = new List<Step>();

            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        // stepNumber,speed,duration,axis1,axis2,axis3,axis4
                        string[] splited = line.Split(",");

                        int stepNumber;
                        bool isCorrect = int.TryParse(splited[0], out stepNumber);
                        if (!isCorrect)
                        {
                            print("���� ������ �߸��Ǿ����ϴ�. ������ Ȯ�� �� �õ��� �ּ���.");

                            return;
                        }

                        float speed;
                        isCorrect = float.TryParse(splited[1], out speed);
                        if (!isCorrect)
                        {
                            print("���� ������ �߸��Ǿ����ϴ�. ������ Ȯ�� �� �õ��� �ּ���.");

                            return;
                        }

                        float duration;
                        isCorrect = float.TryParse(splited[2], out duration);
                        if (!isCorrect)
                        {
                            print("���� ������ �߸��Ǿ����ϴ�. ������ Ȯ�� �� �õ��� �ּ���.");

                            return;
                        }


                        float disAxis1;
                        isCorrect = float.TryParse(splited[3], out disAxis1);
                        if (!isCorrect)
                        {
                            print("���� ������ �߸��Ǿ����ϴ�. ������ Ȯ�� �� �õ��� �ּ���.");

                            return;
                        }

                        float angleAxis2;
                        isCorrect = float.TryParse(splited[4], out angleAxis2);
                        if (!isCorrect)
                        {
                            print("���� ������ �߸��Ǿ����ϴ�. ������ Ȯ�� �� �õ��� �ּ���.");

                            return;
                        }

                        float angleAxis3;
                        isCorrect = float.TryParse(splited[5], out angleAxis3);
                        if (!isCorrect)
                        {
                            print("���� ������ �߸��Ǿ����ϴ�. ������ Ȯ�� �� �õ��� �ּ���.");

                            return;
                        }

                        float angleAxis4;
                        isCorrect = float.TryParse(splited[6], out angleAxis4);
                        if (!isCorrect)
                        {
                            print("���� ������ �߸��Ǿ����ϴ�. ������ Ȯ�� �� �õ��� �ּ���.");

                            return;
                        }


                        Step stepLoaded = new Step(stepNumber, speed, duration);
                        stepLoaded.disAxis1 = disAxis1;
                        stepLoaded.angleAxis2 = angleAxis2;
                        stepLoaded.angleAxis3 = angleAxis3;
                        stepLoaded.angleAxis4 = angleAxis4;

                        tempSteps.Add(stepLoaded);
                    }
                }
            }

            steps.Clear();
            steps.AddRange(tempSteps);
            //steps = tempSteps.ToList();

            print("���� �бⰡ �Ϸ�Ǿ����ϴ�.");
        }
        else if(!File.Exists(path))
        {
            print("������ �������� �ʽ��ϴ�. ���� �̸��� Ȯ���� �ּ���.");
        }
    }

    bool isUpBtnPressed = false;
    bool isDownBtnPressed = false;

    /// <summary>
    /// 1. axis�� ������ ����, UI�� �ִ� Axis Input�� �� ����
    /// 2. ����Ǵ� ���� ���� motorAxis1�� ����
    /// </summary>
    public void OnUpBtEnterClkEvent(string axis)
    {
        isUpBtnPressed = true;
        StartCoroutine(OnPointerUpStay(axis));
    }

    public void OnUpBtnExitClkEvent()
    {
        isUpBtnPressed = false;

    }

    public void OnDownBtEnterClkEvent(string axis)
    {
        isDownBtnPressed = true;
        StartCoroutine(OnPointerDownStay(axis));
    }

    public void OnDownBtnExitClkEvent()
    {
        isDownBtnPressed = false;

    }


    IEnumerator OnPointerUpStay(string axis)
    {
        float currentTime = 0;
        while (isUpBtnPressed)
        {
            currentTime += Time.deltaTime;
            switch (axis)
            {
                case "Axis1":
                    disAxis1 += resolution* 0.00001f;
                    disAxis1Input.text = disAxis1.ToString();
                    motorAxis1.transform.localPosition = new Vector3(motorAxis1.localPosition.x, motorAxis1.localPosition.y, disAxis1);
                    break;

                case "Axis2":
                    angleAxis2 += resolution;
                    angleAxis2Input.text = angleAxis2.ToString();
                    motorAxis2.transform.localRotation = Quaternion.Euler(0, angleAxis2, 0);
                    break;

                case "Axis3":
                    angleAxis3 += resolution;
                    angleAxis3Input.text = angleAxis3.ToString();
                    motorAxis3.transform.localRotation = Quaternion.Euler(0, angleAxis3, 0);
                    break;

                case "Axis4":
                    angleAxis4 += resolution;
                    angleAxis4Input.text = angleAxis4.ToString();
                    motorAxis4.transform.localRotation = Quaternion.Euler(0, angleAxis4, 0);
                    break;

            }

            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator OnPointerDownStay(string axis)
    {
        float currentTime = 0;
        while (isDownBtnPressed)
        {
            currentTime += Time.deltaTime;
            switch (axis)
            {
                case "Axis1":
                    disAxis1 -= resolution* 0.00001f;
                    disAxis1Input.text = disAxis1.ToString();
                    motorAxis1.transform.localPosition = new Vector3(motorAxis1.localPosition.x, motorAxis1.localPosition.y, disAxis1);
                    break;

                case "Axis2":
                    angleAxis2 -= resolution;
                    angleAxis2Input.text = angleAxis2.ToString();
                    motorAxis2.transform.localRotation = Quaternion.Euler(0, angleAxis2, 0);
                    break;

                case "Axis3":
                    angleAxis3 -= resolution;
                    angleAxis3Input.text = angleAxis3.ToString();
                    motorAxis3.transform.localRotation = Quaternion.Euler(0, angleAxis3, 0);
                    break;

                case "Axis4":
                    angleAxis4 -= resolution;
                    angleAxis4Input.text = angleAxis4.ToString();
                    motorAxis4.transform.localRotation = Quaternion.Euler(0, angleAxis4, 0);
                    break;

            }

            yield return new WaitForEndOfFrame();
        }
    }
}