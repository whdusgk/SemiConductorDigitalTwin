using TMPro;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using System.Collections;
using System.Threading.Tasks;
using System;
using UnityEngine.SceneManagement;
using System.Diagnostics;


/// <summary>
/// 1. �α���: �̸���, �н����� �Է½� ȸ������ ���ο� ���� �α����Ѵ�.
/// 2. ȸ������: �̸���, �н����� �Է� �� �̸��� ������ �Ϸ�ȴٸ� ȸ�������� �ȴ�.
/// 3. ������ �ҷ�����: ���ѿ� ���� DB�� Ư�� ������ �ҷ��´�.
/// </summary>
public class FirebaseAuthManager : MonoBehaviour
{
    [Header("�α��� UI")]
    [SerializeField] GameObject loginPanel;
    [SerializeField] GameObject iDInfoPanel;
    [SerializeField] TMP_InputField loginEmailInput;
    [SerializeField] TMP_InputField loginPWInput;
    [SerializeField] TMP_InputField emailInput;
    [SerializeField] TMP_InputField nameInput;
    [SerializeField] string dbURL;
    [SerializeField] string uID;

    [Header("ȸ������ UI")]
    [SerializeField] GameObject signUpPanel;
    [SerializeField] TMP_InputField signUpEmailInput;
    [SerializeField] TMP_InputField signUpPWInput;
    [SerializeField] TMP_InputField signUpPWCheckInput;

    FirebaseAuth auth;
    FirebaseUser user;
    bool signedIn;

    // ������ EXE ������ ���
    string exeFilePath = @"C:\Users\��5���ǽ�-12\Desktop\���ؼ�\Server.EX3\Server.EX3\bin\Debug\net8.0\Server.EX3.exe";  // ���� �ʿ�

    // EXE ������ �����ϴ� �Լ�
    public void LaunchExe()
    {
        try
        {
            // ProcessStartInfo�� ����Ͽ� EXE ������ ����
            ProcessStartInfo startInfo = new ProcessStartInfo(exeFilePath)
            {
                UseShellExecute = true,  // ���� ����Ͽ� ���� (�⺻��)
                CreateNoWindow = false  // �ܼ� â�� ������ ���� (�ʿ�� false�� ����)
            };

            // EXE ���� ����
            Process process = Process.Start(startInfo);
            print("EXE ������ ����Ǿ����ϴ�.");
        }
        catch (System.Exception e)
        {
            print("EXE ���� �� ���� �߻�: " + e.Message);
        }
    }
    void Start()
    {

        LaunchExe();

        FirebaseApp.DefaultInstance.Options.DatabaseUrl = new System.Uri(dbURL);

        auth = FirebaseAuth.DefaultInstance;

        auth.StateChanged += AuthStateChangedEvent;

        AuthStateChangedEvent(this, null);

        auth.SignOut();
    }

    void AuthStateChangedEvent(object sender, System.EventArgs eventArgs)
    {
        if (auth.CurrentUser != user)
        {
            signedIn = user != auth.CurrentUser && auth.CurrentUser != null
                && auth.CurrentUser.IsValid();
            if (!signedIn && user != null)
            {
                print("Signed out " + user.UserId);
            }
            user = auth.CurrentUser;
            if (signedIn)
            {
                print("Signed in " + user.UserId);
            }
        }
    }

    public void OnLoginBtnClkEvent()
    {
        // �α��� �Ϸ�� �ٸ� ������ �Ѿ��
        StartCoroutine(CoLogin(loginEmailInput.text, loginPWInput.text));
    }

    public void OnCancelBtnClkEvent()
    {
        Application.Quit();
    }

    IEnumerator CoLogin(string email, string password)
    {
        var logInTask = auth.SignInWithEmailAndPasswordAsync(email, password);

        logInTask.ContinueWith(t =>
        {
            if (t.IsCanceled)
            {
                print("�α��� ���");
            }
            else if (t.IsFaulted)
            {
                print("�α��� ����");
            }
            else if (t.IsCompleted)
            {
                print("�α��� Ȯ��");
            }
        });

        yield return new WaitUntil(() => logInTask.IsCompleted);

        user = logInTask.Result.User;

        // �̸��� ���� ���� �ڵ� ����

        print("�α��� �Ǿ����ϴ�.");

        uID = user.UserId;

        DownloadMyDBInfo(uID);

        loginEmailInput.text = "";
        loginPWInput.text = "";

        // �ٸ� �� �ҷ�����
        AsyncOperation oper = SceneManager.LoadSceneAsync("250113_SemiconductorMPS");

        while (!oper.isDone)
        {
            print(oper.progress + "%");

            yield return null;
        }

        yield return new WaitUntil(() => oper.isDone);

        print("Load�� �Ϸ�Ǿ����ϴ�.");
    }

    [Serializable]
    public class User
    {
        public string email;
        public string name;
    }
    [SerializeField] User userInfo;
    bool isDataDownloaded = false;
    private void DownloadMyDBInfo(string uID)
    {
        DatabaseReference dbRef = FirebaseDatabase.DefaultInstance.GetReference("users");

        dbRef.Child(uID).GetValueAsync().ContinueWith(task =>
        {
            DataSnapshot snapshot = task.Result;

            string json = snapshot.GetRawJsonValue();

            print(json);

            userInfo = JsonUtility.FromJson<User>(json);

            // Refactoring: �ڵ�����
            if (task.IsCanceled)
            {
                print("������ �ٿ�ε� ���");
            }
            else if (task.IsFaulted)
            {
                print("������ �ٿ�ε� ����");
            }
            else if (task.IsCompleted)
            {
                isDataDownloaded = true;

                print("������ �ٿ�ε� �Ϸ�");
            }
        });

        StartCoroutine("CoUpdateIDInfo");
    }

    IEnumerator CoUpdateIDInfo()
    {
        yield return new WaitUntil(() => isDataDownloaded);

        emailInput.text = userInfo.email;
        nameInput.text = userInfo.name;

        loginPanel.SetActive(false);
        iDInfoPanel.SetActive(true);
    }

    public void OnEditBtnClkEvent()
    {
        userInfo.email = emailInput.text;
        userInfo.name = nameInput.text;

        UploadMyDBInfo(uID, userInfo);
    }

    bool isTaskDone = false;
    private void UploadMyDBInfo(string uID, User info)
    {
        DatabaseReference dbRef = FirebaseDatabase.DefaultInstance.GetReference("users");

        string json = JsonUtility.ToJson(info);
        dbRef.Child(uID).SetRawJsonValueAsync(json).ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                isTaskDone = true;
            }
        });

        StartCoroutine("CoEditDone");
    }

    IEnumerator CoEditDone()
    {
        yield return new WaitUntil(() => isTaskDone);

        print("���� �Ϸ�");
    }

    public void OnSignupBtnClkEvent()
    {
        loginPanel.SetActive(false);
        signUpPanel.SetActive(true);

        loginEmailInput.text = "";
        loginPWInput.text = "";
    }

    public void OnSignupOKBtnClkEvent()
    {
        StartCoroutine(CoSignUp(signUpEmailInput.text, signUpPWInput.text, signUpPWCheckInput.text));
        loginPanel.SetActive(true);
        signUpPanel.SetActive(false);
    }

    public void OnSignupCancelBtnClkEvent()
    {
        loginPanel.SetActive(true);
        signUpPanel.SetActive(false);

        signUpEmailInput.text = "";
        signUpPWInput.text = "";
        signUpPWCheckInput.text = "";
    }

    IEnumerator CoSignUp(string email, string password, string passwordCheck)
    {
        if (email == "" || password == "" || passwordCheck == "")
        {
            print("�̸��� �Ǵ� �н����带 �Է��� �ּ���.");

            yield break;
        }

        if (password != passwordCheck)
        {
            print("��й�ȣ�� Ȯ�κ�й�ȣ�� ���� �ʽ��ϴ�. �Է��� Ȯ���� �ּ���.");

            yield break;
        }

        Task t = auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            FirebaseException exception = task.Exception.GetBaseException() as FirebaseException;

            AuthError authError = (AuthError)exception.ErrorCode;

            switch (authError)
            {
                case AuthError.InvalidEmail:
                    print("��ȿ���� ���� �̸��� �����Դϴ�.");
                    break;
                case AuthError.WeakPassword:
                    print("��й�ȣ�� ����մϴ�.");
                    break;
                case AuthError.EmailAlreadyInUse:
                    print("�̹� ������� �̸��� �Դϴ�.");
                    break;
                default:
                    print(authError);
                    break;
            }

            if (task.IsCanceled)
            {
                print("���� ���� ���");
            }
            else if (task.IsFaulted)
            {
                print("���� ���� ����");
            }
            else if (task.IsCompletedSuccessfully)
            {
                print("ȸ�������� �Ϸ�Ǿ����ϴ�.");
            }
            else if (task.IsCompleted)
            {
                print("ȸ�������� �Ϸ�Ǿ����ϴ�.");
            }
        });

        // �̸��� ���� ���� �ڵ� ����
    }
}
