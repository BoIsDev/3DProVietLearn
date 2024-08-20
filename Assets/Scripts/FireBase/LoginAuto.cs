using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Extensions;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using UnityEngine.UI;
using UnityEngine.Networking;
using Firebase;
using Firebase.Messaging;
public class LoginAuto : MonoBehaviour
{
    private FirebaseAuth auth;
    [SerializeField] private Button btnOpenLogin;
    [SerializeField] private GameObject panelOpen;
    [SerializeField] private GameObject panelLoginAuto;
    // Start is called before the first frame update
    void Start()
    {
        btnOpenLogin.onClick.AddListener(OpenScene);
        StartCoroutine(GetText());
        IG_Notifications.instance.OnEnable();
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                InitializeFirebase();
               
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + task.Result);
            }
        });
    }

    void InitializeFirebase()
    {
        auth = FirebaseAuth.DefaultInstance;
        SignInAuto();
    }
    void SignInAuto()
    {
        auth.SignInAnonymouslyAsync().ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInAnonymouslyAsync ?ã b? h?y.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("?ã x?y ra l?i khi th?c hi?n SignInAnonymouslyAsync: " + task.Exception);
                return;
            }

            Firebase.Auth.AuthResult result = task.Result;
            Debug.LogFormat("Ng??i dùng ?ã ??ng nh?p thành công: {0} ({1})",
                result.User.DisplayName, result.User.UserId);
        });
    }

    public void OpenScene()
    {
        panelOpen.SetActive(true);
        panelLoginAuto.SetActive(false);
    }

    IEnumerator GetText()
    {
        Debug.Log("Start");
        using (UnityWebRequest uwr = UnityWebRequest.Get("http://localhost/FireBaseNotification/UserGet.php"))
        {
            yield return uwr.SendWebRequest();

            if (uwr.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error: " + uwr.error);
                Debug.LogError("Response Code: " + uwr.responseCode);
                Debug.LogError("Response Text: " + uwr.downloadHandler.text);
            }
            else
            {
                // X? lý d? li?u t?i v?
                string responseText = uwr.downloadHandler.text;
                Debug.Log("Response: " + responseText);
            }
        }
    }


}
