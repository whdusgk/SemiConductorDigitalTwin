using System;
using System.Collections;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public bool isProcessBtnPressed;
    public Camera P0Camera;
    public Camera P1Camera;
    public Camera P2Camera;
    public Camera P3Camera;
    public Camera P4Camera;
    public Camera P5Camera;
    public Camera P6Camera;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        P0Camera.enabled = true;
        P1Camera.enabled = false;
        P2Camera.enabled = false;
        P3Camera.enabled = false;
        P4Camera.enabled = false;
        P5Camera.enabled = false;
        P6Camera.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnProcessClkEvent(string process)
    {
        isProcessBtnPressed = true;
        OnProcess(process);
    }

    private void OnProcess(string process)
    {
        switch (process)
        {
            case "Process1":
                P0Camera.enabled = false;
                P1Camera.enabled = true;
                P2Camera.enabled = false;
                P3Camera.enabled = false;
                P4Camera.enabled = false;
                P5Camera.enabled = false;
                P6Camera.enabled = false;
                break;

            case "Process2":
                P0Camera.enabled = false;
                P1Camera.enabled = false;
                P2Camera.enabled = true;
                P3Camera.enabled = false;
                P4Camera.enabled = false;
                P5Camera.enabled = false;
                P6Camera.enabled = false;
                break;

            case "Process3":
                P0Camera.enabled = false;
                P1Camera.enabled = false;
                P2Camera.enabled = false;
                P3Camera.enabled = true;
                P4Camera.enabled = false;
                P5Camera.enabled = false;
                P6Camera.enabled = false;
                break;

            case "Process4":
                P0Camera.enabled = false;
                P1Camera.enabled = false;
                P2Camera.enabled = false;
                P3Camera.enabled = false;
                P4Camera.enabled = true;
                P5Camera.enabled = false;
                P6Camera.enabled = false;
                break;
            case "Process5":
                P0Camera.enabled = false;
                P1Camera.enabled = false;
                P2Camera.enabled = false;
                P3Camera.enabled = false;
                P4Camera.enabled = false;
                P5Camera.enabled = true;
                P6Camera.enabled = false;
                break;
            case "Process6":
                P0Camera.enabled = false;
                P1Camera.enabled = false;
                P2Camera.enabled = false;
                P3Camera.enabled = false;
                P4Camera.enabled = false;
                P5Camera.enabled = false;
                P6Camera.enabled = true;
                break;
        }
    }


}
