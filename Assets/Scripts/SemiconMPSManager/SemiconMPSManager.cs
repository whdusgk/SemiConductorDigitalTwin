/*using UnityEngine;

public class SemiconMPSManager : MonoBehaviour
{
    [SerializeField] Transform cylinderRod;
    [SerializeField] float maxRange;
    [SerializeField] float minRange;
    [SerializeField] float duration;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnForwardBtnClkEvent()
    {
        if (isForward || isRodMoving) return;

        cycleCnt++;

        StartCoroutine(MoveCylinder(cylinderRod, minRange, maxRange, duration));
    }

    public void OnBackwardBtnClkEvent()
    {
        if (!isForward || isRodMoving) return;

        StartCoroutine(MoveCylinder(cylinderRod, maxRange, minRange, duration));
    }

    IEnumerator MoveCylinder(Transform rod, float min, float max, float duration)
    {
        isRodMoving = true;

        //Vector3 minPos = new Vector3(rod.transform.localPosition.x, min, rod.transform.localPosition.z);
        //Vector3 maxPos = new Vector3(rod.transform.localPosition.x, max, rod.transform.localPosition.z);
        Vector3 minPos = new Vector3(min, rod.transform.localPosition.y, rod.transform.localPosition.z);
        Vector3 maxPos = new Vector3(max, rod.transform.localPosition.y, rod.transform.localPosition.z);

        float currentTime = 0;

        while (currentTime <= duration)
        {
            currentTime += Time.deltaTime;

            rod.localPosition = Vector3.Lerp(minPos, maxPos, currentTime / duration);

            cycleTime += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        isRodMoving = false;
        isForward = !isForward;

        UpdateLimitSwitch(isForward);
    }
}
*/