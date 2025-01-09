using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEMManager : MonoBehaviour
{
    public float SEMDisAxis1;
    public float SEMDisAxis2;
    public float SEMAngleAxis3;
    public float SEMAngleAxis4;
    public float SEMAngleAxis5;
    public float SEMAngleAxis6;

    public float xchipGap = 0.0004f;
    public float zchipGap = 0.4f;
    public float speed = 100;
    public float cycleTime;
    public float duration;

    [SerializeField] Transform SEMAxis1;
    [SerializeField] Transform SEMAxis2;
    [SerializeField] Transform SEMAxis3;
    [SerializeField] Transform SEMAxis4;
    [SerializeField] Transform SEMAxis5;
    [SerializeField] Transform SEMAxis6;

    List<int> SEMxPos = new List<int> { -3, -2, -1, 0, 1, 2, 3 };
    List<int> SEMzPos = new List<int> { -3, -2, -1, 0, 1, 2, 3 };
    Vector3 SEMAxis1Origin;
    Vector3 SEMAxis2Origin;
    public int SEMCase = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SEMAxis1Origin = new Vector3(SEMAxis1.localPosition.x, SEMAxis1.localPosition.y, SEMAxis1.localPosition.z);
        SEMAxis2Origin = new Vector3(SEMAxis2.localPosition.x, SEMAxis2.localPosition.y, SEMAxis2.localPosition.z);

        StartCoroutine(SEMStep(0,0));
        //RunSEM();
    }
    public void RunSEM()
    {
        switch (SEMCase)
        {
            case 0:
                StartCoroutine(SEMStep(-1, -3));
                SEMCase++;
                break;
            case 1:
                StartCoroutine(SEMStep(1, -3));
                SEMCase ++;
                break;
            case 2:
                StartCoroutine(SEMStep(-2, -2));
                SEMCase++;
                break;
            case 3:
                StartCoroutine(SEMStep(2, -2));
                SEMCase++;
                break;
            case 4:
                StartCoroutine(SEMStep(-3, -1));
                SEMCase++;
                break;
            case 5:
                StartCoroutine(SEMStep(3, -1));
                SEMCase++;
                break;
            case 6:
                StartCoroutine(SEMStep(-3, 0));
                SEMCase++;
                break;
            case 7:
                StartCoroutine(SEMStep(3, 0));
                SEMCase++;
                break;
            case 8:
                StartCoroutine(SEMStep(-3, 1));
                SEMCase++;
                break;
            case 9:
                StartCoroutine(SEMStep(3, 1));
                SEMCase++;
                break;
            case 10:
                StartCoroutine(SEMStep(-2, 2));
                SEMCase++;
                break;
            case 11:
                StartCoroutine(SEMStep(2, 2));
                SEMCase++;
                break;
            case 12:
                StartCoroutine(SEMStep(-1, 3));
                SEMCase++;
                break;
            case 13:
                StartCoroutine(SEMStep(1, 3));
                SEMCase++;
                break;
            case 14:
                StartCoroutine(SEMStep(0, 0));
                SEMCase = 0;
                break;

        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    
    IEnumerator SEMStep(int x, int z)
    {
        Vector3 prevAxis1Pos = new Vector3(SEMAxis1.localPosition.x, SEMAxis1.localPosition.y, SEMAxis1.localPosition.z); // Axis1: Y�� �������� �̵�
        Vector3 nextAxis1APos = new Vector3(SEMAxis1Origin.x + x * xchipGap, SEMAxis1.localPosition.y, SEMAxis1.localPosition.z);

        Vector3 prevAxis2Pos = new Vector3(SEMAxis2.localPosition.x, SEMAxis2.localPosition.y, SEMAxis2.localPosition.z); // Axis1: Y�� �������� �̵�
        Vector3 nextAxis2APos = new Vector3(SEMAxis2.localPosition.x, SEMAxis2.localPosition.y, SEMAxis2Origin.z + z * zchipGap);

        float currentTime = 0;
        while (true)
        {
            currentTime += Time.deltaTime;
            if ((currentTime / speed) > 1)
            {
                break;
            }
            SEMAxis1.localPosition = Vector3.Lerp(prevAxis1Pos, nextAxis1APos, currentTime / speed);
            SEMAxis2.localPosition = Vector3.Lerp(prevAxis2Pos, nextAxis2APos, currentTime / speed);
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(duration);
    }
}