using UnityEngine;

public class LithoTest : MonoBehaviour
{
    public Animator LithoAni;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LithoAni.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnStepUpBtnClkEvent()
    {
        LithoAni.enabled = true;
    }
}
