using Firebase;
using System;
using UnityEngine;
using Firebase.Database;

public class FirebaseDBManager : MonoBehaviour
{

    [SerializeField] string dbURL;

    bool isReceived = false;

    void Start()
    {
        FirebaseApp.DefaultInstance.Options.DatabaseUrl = new System.Uri(dbURL);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
