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

    public GameObject P0Canvas;
    public GameObject P1Canvas;
    public GameObject P2Canvas;
    public GameObject P3Canvas;
    public GameObject P4Canvas;
    public GameObject P5Canvas;
    public GameObject P6Canvas;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        P0Camera.gameObject.SetActive(true);
        P1Camera.gameObject.SetActive(false);
        P2Camera.gameObject.SetActive(false);
        P3Camera.gameObject.SetActive(false);
        P4Camera.gameObject.SetActive(false);
        P5Camera.gameObject.SetActive(false);
        P6Camera.gameObject.SetActive(false);

        P0Canvas.SetActive(true);
        P1Canvas.SetActive(false);
        P2Canvas.SetActive(false);
        P3Canvas.SetActive(false);
        P4Canvas.SetActive(false);
        P5Canvas.SetActive(false);
        P6Canvas.SetActive(false);
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
                P0Camera.gameObject.SetActive(false);
                P1Camera.gameObject.SetActive(true);
                P2Camera.gameObject.SetActive(false);
                P3Camera.gameObject.SetActive(false);
                P4Camera.gameObject.SetActive(false);
                P5Camera.gameObject.SetActive(false);
                P6Camera.gameObject.SetActive(false);
                P0Canvas.SetActive(false);
                P1Canvas.SetActive(true);
                P2Canvas.SetActive(false);
                P3Canvas.SetActive(false);
                P4Canvas.SetActive(false);
                P5Canvas.SetActive(false);
                P6Canvas.SetActive(false);
                break;

            case "Process2":
                P0Camera.gameObject.SetActive(false);
                P1Camera.gameObject.SetActive(false);
                P2Camera.gameObject.SetActive(true);
                P3Camera.gameObject.SetActive(false);
                P4Camera.gameObject.SetActive(false);
                P5Camera.gameObject.SetActive(false);
                P6Camera.gameObject.SetActive(false);
                P0Canvas.SetActive(false);
                P1Canvas.SetActive(false);
                P2Canvas.SetActive(true);
                P3Canvas.SetActive(false);
                P4Canvas.SetActive(false);
                P5Canvas.SetActive(false);
                P6Canvas.SetActive(false);
                break;

            case "Process3":
                P0Camera.gameObject.SetActive(false);
                P1Camera.gameObject.SetActive(false);
                P2Camera.gameObject.SetActive(false);
                P3Camera.gameObject.SetActive(true);
                P4Camera.gameObject.SetActive(false);
                P5Camera.gameObject.SetActive(false);
                P6Camera.gameObject.SetActive(false);
                P0Canvas.SetActive(false);
                P1Canvas.SetActive(false);
                P2Canvas.SetActive(false);
                P3Canvas.SetActive(true);
                P4Canvas.SetActive(false);
                P5Canvas.SetActive(false);
                P6Canvas.SetActive(false);
                break;

            case "Process4":
                P0Camera.gameObject.SetActive(false);
                P1Camera.gameObject.SetActive(false);
                P2Camera.gameObject.SetActive(false);
                P3Camera.gameObject.SetActive(false);
                P4Camera.gameObject.SetActive(true);
                P5Camera.gameObject.SetActive(false);
                P6Camera.gameObject.SetActive(false);
                P0Canvas.SetActive(false);
                P1Canvas.SetActive(false);
                P2Canvas.SetActive(false);
                P3Canvas.SetActive(false);
                P4Canvas.SetActive(true);
                P5Canvas.SetActive(false);
                P6Canvas.SetActive(false);
                break;
            case "Process5":
                P0Camera.gameObject.SetActive(false);
                P1Camera.gameObject.SetActive(false);
                P2Camera.gameObject.SetActive(false);
                P3Camera.gameObject.SetActive(false);
                P4Camera.gameObject.SetActive(false);
                P5Camera.gameObject.SetActive(true);
                P6Camera.gameObject.SetActive(false);
                P0Canvas.SetActive(false);
                P1Canvas.SetActive(false);
                P2Canvas.SetActive(false);
                P3Canvas.SetActive(false);
                P4Canvas.SetActive(false);
                P5Canvas.SetActive(true);
                P6Canvas.SetActive(false);
                break;
            case "Process6":
                P0Camera.gameObject.SetActive(false);
                P1Camera.gameObject.SetActive(false);
                P2Camera.gameObject.SetActive(false);
                P3Camera.gameObject.SetActive(false);
                P4Camera.gameObject.SetActive(false);
                P5Camera.gameObject.SetActive(false);
                P6Camera.gameObject.SetActive(true);
                P0Canvas.SetActive(false);
                P1Canvas.SetActive(false);
                P2Canvas.SetActive(false);
                P3Canvas.SetActive(false);
                P4Canvas.SetActive(false);
                P5Canvas.SetActive(false);
                P6Canvas.SetActive(true);
                break;
        }
    }

    public void OnExitBtEnterClkEvent()
    {
        P0Camera.gameObject.SetActive(true);
        P1Camera.gameObject.SetActive(false);
        P2Camera.gameObject.SetActive(false);
        P3Camera.gameObject.SetActive(false);
        P4Camera.gameObject.SetActive(false);
        P5Camera.gameObject.SetActive(false);
        P6Camera.gameObject.SetActive(false);
        P0Canvas.SetActive(true);
        P1Canvas.SetActive(false);
        P2Canvas.SetActive(false);
        P3Canvas.SetActive(false);
        P4Canvas.SetActive(false);
        P5Canvas.SetActive(false);
        P6Canvas.SetActive(false);
    }
}
