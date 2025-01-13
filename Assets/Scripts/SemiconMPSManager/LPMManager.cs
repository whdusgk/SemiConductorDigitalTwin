using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 공압이 공급되면 실린더 로드가 일정 거리만큼, 일정 속도로 전진 또는 후진한다.
/// 전진 또는 후진 시, 전후진 Limit Switch(LS)가 작동한다.
/// 속성: 실린더로드, Min-Max Range, Duration, 전후방 Limit Switch
/// </summary>
public class LPMManager : MonoBehaviour
{

    [SerializeField] Transform Foup;
    [SerializeField] Transform FoupDoor;
    [SerializeField] Transform LPMBackBase;
    
    [SerializeField] float FoupMinRange;
    [SerializeField] float FoupMaxRange;
    [SerializeField] float FoupDoorXMinRange1;
    [SerializeField] float FoupDoorXMaxRange1;
    [SerializeField] float FoupDoorXMinRange2;
    [SerializeField] float FoupDoorXMaxRange2;
    [SerializeField] float FoupDoorZMinRange;
    [SerializeField] float FoupDoorZMaxRange;
    [SerializeField] float LPMBackBaseXMinRange;
    [SerializeField] float LPMBackBaseXMaxRange;
    [SerializeField] float LPMBackBaseZMinRange;
    [SerializeField] float LPMBackBaseZMaxRange;

    [SerializeField] float duration;

    public bool isUp = false;

    public float cycleTime;

    [Header("Sensors")]
    public List<GameObject> FoupSensors;
    
    public bool isFoupForward = false;
    public bool isFoupDoorForward = false;
    public bool isFoupDoorBackward = false;
    public bool isFoupOpen = false;


    private void Start()
    {
        // ETC Sensor Caligration(Black)
        foreach (GameObject s in FoupSensors) s.GetComponent<Renderer>().material.color = new Color(0, 0, 0); // Black
        //StartCoroutine(OnLPMBtnClkEvent());
    }

    IEnumerator OnLPMBtnClkEvent()
    {
        yield return LPMStep(0);
        yield return LPMStep(1);
        yield return LPMStep(2);
        yield return LPMStep(3);

    }
    IEnumerator LPMStep(int cycleCnt)
    {
        switch (cycleCnt)
        {
            case 0:
                isFoupForward = true;
                isFoupDoorForward = false;
                isFoupDoorBackward = false;
                isFoupOpen = false;
                StartCoroutine(xMoveLPM(Foup, FoupMinRange, FoupMaxRange, duration));
                StartCoroutine(xMoveLPM(FoupDoor, FoupDoorXMinRange1, FoupDoorXMaxRange1, duration));
                FoupSensors[0].GetComponent<Renderer>().material.color = new Color(255, 0, 0); // Red
                FoupSensors[1].GetComponent<Renderer>().material.color = new Color(0, 0, 0); // Black
                FoupSensors[2].GetComponent<Renderer>().material.color = new Color(0, 0, 0); // Black
                FoupSensors[3].GetComponent<Renderer>().material.color = new Color(0, 0, 0); // Black
                                                                                             //cycleCnt++;
                yield return new WaitForSeconds(3);
                break;

            case 1:
                isFoupForward = false;
                isFoupDoorForward = true;
                isFoupDoorBackward = false;
                isFoupOpen = false;
                StartCoroutine(xMoveLPM(LPMBackBase, LPMBackBaseXMinRange, LPMBackBaseXMaxRange, duration));
                FoupSensors[0].GetComponent<Renderer>().material.color = new Color(0, 0, 0); // Black
                FoupSensors[1].GetComponent<Renderer>().material.color = new Color(1, 0.5f, 0, 1); // Orange
                FoupSensors[2].GetComponent<Renderer>().material.color = new Color(0, 0, 0); // Black
                FoupSensors[3].GetComponent<Renderer>().material.color = new Color(0, 0, 0); // Black
                                                                                             //cycleCnt++;
                yield return new WaitForSeconds(3); 
                break;

            case 2:
                isFoupForward = false;
                isFoupDoorForward = false;
                isFoupDoorBackward = true;
                isFoupOpen = false;
                StartCoroutine(xMoveLPM(LPMBackBase, LPMBackBaseXMaxRange, LPMBackBaseXMinRange, duration));
                StartCoroutine(xMoveLPM(FoupDoor, FoupDoorXMinRange2, FoupDoorXMaxRange2, duration));
                FoupSensors[0].GetComponent<Renderer>().material.color = new Color(0, 0, 0); // Black
                FoupSensors[1].GetComponent<Renderer>().material.color = new Color(0, 0, 0); // Black
                FoupSensors[2].GetComponent<Renderer>().material.color = new Color(255, 255, 0); // Yellow
                FoupSensors[3].GetComponent<Renderer>().material.color = new Color(0, 0, 0); // Black
                                                                                             //cycleCnt++;
                yield return new WaitForSeconds(3);
                break;

            case 3:
                isFoupForward = false;
                isFoupDoorForward = false;
                isFoupDoorBackward = false;
                isFoupOpen = true;
                StartCoroutine(zMoveLPM(FoupDoor, FoupDoorZMinRange, FoupDoorZMaxRange, duration));
                StartCoroutine(zMoveLPM(LPMBackBase, LPMBackBaseZMinRange, LPMBackBaseZMaxRange, duration));
                FoupSensors[0].GetComponent<Renderer>().material.color = new Color(0, 0, 0); // Black
                FoupSensors[1].GetComponent<Renderer>().material.color = new Color(0, 0, 0); // Black
                FoupSensors[2].GetComponent<Renderer>().material.color = new Color(0, 0, 0); // Black
                FoupSensors[3].GetComponent<Renderer>().material.color = new Color(0, 255, 0); // Green
                                                                                               //cycleCnt = 0;
                //yield return new WaitForSeconds(3);
                break;
        }
       
    }

    IEnumerator xMoveLPM(Transform gate, float min, float max, float duration)
    {

        Vector3 minPos = new Vector3(min, gate.transform.localPosition.y, gate.transform.localPosition.z);
        Vector3 maxPos = new Vector3(max, gate.transform.localPosition.y, gate.transform.localPosition.z);

        float currentTime = 0;

        while (currentTime <= duration)
        {
            currentTime += Time.deltaTime;

            gate.localPosition = Vector3.Lerp(minPos, maxPos, currentTime / duration);

            cycleTime += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }
    }
    IEnumerator zMoveLPM(Transform gate, float min, float max, float duration)
    {

        Vector3 minPos = new Vector3(gate.transform.localPosition.x, gate.transform.localPosition.y, min);
        Vector3 maxPos = new Vector3(gate.transform.localPosition.x, gate.transform.localPosition.y, max);

        float currentTime = 0;

        while (currentTime <= duration)
        {
            currentTime += Time.deltaTime;

            gate.localPosition = Vector3.Lerp(minPos, maxPos, currentTime / duration);

            cycleTime += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

    }
}

