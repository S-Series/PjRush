using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using TMPro;

public class BackEnd : MonoBehaviour
{
    public static BackEnd backEnd;

    const string ver = "0.1a";

    /*const string DatabaseLink = 
        "https://docs.google.com/spreadsheets/d/1qzc_NXa0slJjoGExU37vnw0cmDaYvsT70XZK29i8n1I/export?format=tsv";*/

    const string ScriptLink =
        "https://script.google.com/macros/s/AKfycbyc3kLfHJOdIZ5klrKfoMZSDAyfGnocv24dTIB5ynV6zTVzs-U/exec";

    [SerializeField]
    GameObject main;

    [SerializeField]
    TMP_InputField IDInput;

    [SerializeField]
    TMP_InputField PassInput;

    [SerializeField]
    TextMeshPro[] AccountInfo;

    [SerializeField]
    TextMeshPro[] AccountInfoShadow;

    [SerializeField]
    Animator AnimatorAccount;

    [SerializeField]
    Animator AnimatorLogin;

    [SerializeField]
    Button topBoxButton;

    [SerializeField]
    Toggle ToggleSave;

    string id, pass;

    public bool isOnline;
    public bool isLogin;
    private bool isDown;

    private void Awake()
    {
        isLogin = false;
        isDown = false;
        if (backEnd == null) backEnd = this;
        else Destroy(this);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainScene") main.SetActive(true);
        else main.SetActive(false);
    }

    void Start()
    {
        DontDestroyOnLoad(this);

        foreach (TextMeshPro tmp in AccountInfo)
        {
            tmp.text = "----";
        }
        foreach (TextMeshPro tmp in AccountInfoShadow)
        {
            tmp.text = "----";
        }
        isOnline = CheckOnline();

        string id = PlayerPrefs.GetString("id");
        string pass = PlayerPrefs.GetString("pass");

        if (id != null && pass != null)
        {
            WWWForm loginForm = new WWWForm();
            loginForm.AddField("order", "login");
            loginForm.AddField("id", id);
            loginForm.AddField("pass", pass);
            StartCoroutine(LoginPost(loginForm, id, pass));
        }

        WWWForm form = new WWWForm();
        form.AddField("order", "version");
        form.AddField("version", ver);

        StartCoroutine(VersionPost(form));
    }

    private void Update()
    {

    }

    private bool CheckOnline()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable) return false;
        else return true;
    }

    private bool SetIDPass()
    {
        id = IDInput.text.Trim();
        pass = PassInput.text.Trim();

        if (id == "" || pass == "") return false;
        else return true;
    }

    public void Login()
    {
        if (!SetIDPass())
        {
            return;
        }

        WWWForm form = new WWWForm();
        form.AddField("order", "login");
        form.AddField("id", id);
        form.AddField("pass", pass);

        StartCoroutine(LoginPost(form, id, pass));
    }

    public void Logout()
    {
        WWWForm form = new WWWForm();
        form.AddField("order", "logout");

        //StartCoroutine(LogoutPost(form));

        isLogin = false;
        foreach (TextMeshPro tmp in AccountInfo)
        {
            tmp.text = "----";
        }
        foreach (TextMeshPro tmp in AccountInfoShadow)
        {
            tmp.text = "----";
        }
        IDInput.text = "";
        PassInput.text = "";
        LoginCheck();
    }

    public void Register()
    {
        if (!SetIDPass())
        {
            return;
        }

        WWWForm form = new WWWForm();
        form.AddField("order", "register");
        form.AddField("id", id);
        form.AddField("pass", pass);

        StartCoroutine(Post(form));
    }

    public void Account()
    {
        topBoxButton.interactable = false;
        StartCoroutine(IeAccount());
    }

    IEnumerator IeAccount()
    {

        if (isDown)
        {
            AnimatorAccount.SetTrigger("Down");

            yield return new WaitForSeconds(1.0f);

            topBoxButton.interactable = true;
            isDown = false;
        }
        else
        {
            AnimatorAccount.SetTrigger("Up");

            yield return new WaitForSeconds(1.0f);

            topBoxButton.interactable = true;
            isDown = true;
        }
    }

    IEnumerator VersionPost(WWWForm form)
    {
        using (UnityWebRequest www = UnityWebRequest.Post(ScriptLink, form))
        {
            yield return www.SendWebRequest();

            if (www.isDone)
            {
                print(www.downloadHandler.text);
                if (www.downloadHandler.text == "true") print("true");
                else StartCoroutine(SystemShutdown());
            }
            else
            {

            }
        }
    }

    IEnumerator LoginPost(WWWForm form, string id, string pass)
    {
        char seperator = ',';
        string[] textLines;
        using (UnityWebRequest www = UnityWebRequest.Post(ScriptLink, form))
        {
            yield return www.SendWebRequest();

            if (www.isDone)
            {
                if (www.downloadHandler.text == "false")
                {
                    PlayerPrefs.SetString("id", null);
                    PlayerPrefs.SetString("pass", null);
                }
                else
                {
                    textLines = www.downloadHandler.text.Split(seperator);

                    for (int i = 0; i < 5; i++)
                    {
                        AccountInfo[i].text = textLines[i];
                        AccountInfoShadow[i].text = textLines[i];
                    }

                    if (ToggleSave.isOn)
                    {
                        PlayerPrefs.SetString("id", id);
                        PlayerPrefs.SetString("pass", pass);
                    }
                    else
                    {
                        PlayerPrefs.SetString("id", null);
                        PlayerPrefs.SetString("pass", null);
                    }

                    isLogin = true;
                    //로그인 성공 -> 로그인 화면 숨김처리
                    LoginCheck();
                }
            }
            else
            {

            }
        }
    }

    private void LoginCheck()
    {
        if (isLogin) AnimatorLogin.SetTrigger("Login");
        else AnimatorLogin.SetTrigger("Logout");
    }

    IEnumerator LogoutPost(WWWForm form)
    {
        if (!isOnline) yield break;

        using (UnityWebRequest www = UnityWebRequest.Post(ScriptLink, form))
        {
            yield return www.SendWebRequest();

            if (www.isDone)
            {
                AnimatorLogin.SetTrigger("logout");
            }
            else
            {

            }
        }
    }

    IEnumerator Post(WWWForm form)
    {
        using (UnityWebRequest www = UnityWebRequest.Post(ScriptLink, form))
        {
            yield return www.SendWebRequest();

            if (www.isDone)
            {
                print(www.downloadHandler.text);
            }
            else
            {

            }
        }
    }

    IEnumerator SystemShutdown()
    {
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0))
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit(); // 어플리케이션 종료
#endif
            }
            yield return null;
        }
    }
    /*
    IEnumerator DatabaseStart()
    {
        UnityWebRequest www = UnityWebRequest.Get(DatabaseLink);
        yield return www.SendWebRequest();

        string data;
        data = www.downloadHandler.text;
        print(data);
    }*/
}
