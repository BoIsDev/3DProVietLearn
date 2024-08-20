using UnityEngine;
using UnityEngine.UI;
using Firebase.Extensions;
using Firebase;
using Firebase.Auth;
using System.Net.Mail;
using Unity.VisualScripting;
using VoxelBusters.CoreLibrary;
using VoxelBusters.EssentialKit;

public class FirebaseManager : MonoBehaviour
{
    [Header("Panel")]
    public GameObject panelLogin;
    public GameObject panelRegister;
    public GameObject panelOpen;

    [Header("Button")]
    public Button btnLogin;
    public Button btnOpenRegisterPanel;
    public Button btnRegister;
    public Button btnOpenLogin;

    [Header("InputField")]
    public InputField loginEmail;
    public InputField loginPassword;
    public InputField registerEmail;
    public InputField registerPassword;
    public InputField registerConfirmPassword;
    private FirebaseAuth auth;
    private FirebaseUser user;

    public static FirebaseManager instance;

    private void Start()
    {
        if (instance != null)
        {
            Debug.Log("Only one FirebaseManager Script");
        }
        else
        {
            instance = this;
        }

        InitializeFirebase();

        btnLogin.onClick.AddListener(Login);
        btnRegister.onClick.AddListener(Register);
        btnOpenRegisterPanel.onClick.AddListener(OpenRegister);
        btnOpenLogin.onClick.AddListener(OpenLogin);

    }

    public void OpenScene()
    {
        panelOpen.SetActive(true);
        panelLogin.SetActive(false);
        panelRegister.SetActive(false);
    }

    public void OpenLogin()
    {
        panelOpen.SetActive(false);
        panelLogin.SetActive(true);
        panelRegister.SetActive(false);
    }

    public void OpenRegister()
    {
        panelOpen.SetActive(false);
        panelLogin.SetActive(false);
        panelRegister.SetActive(true);
    }

    public void Login()
    {
        if (string.IsNullOrEmpty(loginEmail.text) || string.IsNullOrEmpty(loginPassword.text))
        {
            Debug.Log("Please enter email and password");
            return;
        }
        SignUP(loginEmail.text, loginPassword.text);
        ClearTxtLogin();
    }

    public void Register()
    {
        if (string.IsNullOrEmpty(registerEmail.text) || string.IsNullOrEmpty(registerPassword.text) || string.IsNullOrEmpty(registerConfirmPassword.text))
        {
            Debug.Log("Vui lòng nhập email và mật khẩu");
            return;
        }

        if (registerPassword.text != registerConfirmPassword.text)
        {
            Debug.Log("Mật khẩu không khớp. Vui lòng kiểm tra lại mật khẩu.");
            return;
        }

        if (!IsValidEmail(registerEmail.text))
        {
            Debug.Log("Địa chỉ email không hợp lệ");
            return;
        }

        if (registerPassword.text.Length < 6)
        {
            Debug.Log("Mật khẩu phải có ít nhất 6 ký tự");
            return;
        }

        CreateUser(registerEmail.text, registerPassword.text);
        ClearTxtRegister();
        OpenLogin();
    }

    public void ClearTxtRegister()
    {
        registerEmail.text = "";
        registerPassword.text = "";
        registerConfirmPassword.text = "";
    }

    public void ClearTxtLogin()
    {
        loginEmail.text = "";
        loginPassword.text = "";
    }

    public void CreateUser(string email, string password)
    {
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }

            Firebase.Auth.AuthResult result = task.Result;
            Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                result.User.DisplayName, result.User.UserId);
        });
    }

    private bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    public void SignUP(string email, string password)
    {
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }

            Firebase.Auth.AuthResult result = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                result.User.DisplayName, result.User.UserId);
            OpenScene();

            // Lấy và lưu DeviceToken sau khi đăng nhập thành công
            IG_Notifications.instance.OnEnable();
        });
    }


    void InitializeFirebase()
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        auth.StateChanged -= AuthStateChanged;
        auth.StateChanged += AuthStateChanged;  // Ensure the event is subscribed
        AuthStateChanged(this, null);
    }

    void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if (auth.CurrentUser != user)
        {
            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;
            if (!signedIn && user != null)
            {
                Debug.Log("Signed out " + user.UserId);
            }
            user = auth.CurrentUser;
            if (signedIn)
            {
                Debug.Log("Signed in " + user.UserId);
            }
        }
    }
}
