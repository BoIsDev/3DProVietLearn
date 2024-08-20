using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TMPManager : MonoBehaviour
{
    public TMP_InputField inputField;
    public TextMeshProUGUI chatDisplay;
    public Transform Content;
    public GameObject boxIcon;
    public Button btnIcon;
    public Button btnSender;
    public Button btn1;
    public Button btn2;
    public Button btn3;
    public Button btn4;
    public Button btn5;
    public Button btn6;
    public Button btn7;
    public Button btn8;
    public Button btn9;
    public Button btn10;
    public Button btn11;
    public Button btn12;

    // Thêm các mã icon vào ?ây
    private Dictionary<Button, string> buttonIcons = new Dictionary<Button, string>();

    public void Start()
    {
        GetButton();

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SubmitMessage();
        }
    }

    public void SubmitMessage()
    {
        if (!string.IsNullOrWhiteSpace(inputField.text))
        {
           
            TextMeshProUGUI newChat = Instantiate(chatDisplay,Content);
            newChat.name = chatDisplay.name;
            newChat.gameObject.SetActive(true);
            newChat.text += "You: " + inputField.text + " ";
            inputField.text = "";
            inputField.ActivateInputField();

        }
    }

    public void AddIconToChat(Button btn)
    {
        if (buttonIcons.TryGetValue(btn, out string iconCode))
        {
            inputField.text +=  iconCode + " ";
        }
    }

    public void GetButton()
    {
        // Gán s? ki?n cho các nút
        btn1.onClick.AddListener(() => AddIconToChat(btn1));
        btn2.onClick.AddListener(() => AddIconToChat(btn2));
        btn3.onClick.AddListener(() => AddIconToChat(btn3));
        btn4.onClick.AddListener(() => AddIconToChat(btn4));
        btn5.onClick.AddListener(() => AddIconToChat(btn5));
        btn6.onClick.AddListener(() => AddIconToChat(btn6));
        btn7.onClick.AddListener(() => AddIconToChat(btn7));
        btn8.onClick.AddListener(() => AddIconToChat(btn8));
        btn9.onClick.AddListener(() => AddIconToChat(btn9));
        btn10.onClick.AddListener(() => AddIconToChat(btn10));
        btn11.onClick.AddListener(() => AddIconToChat(btn11));
        btn12.onClick.AddListener(() => AddIconToChat(btn12));
        btnSender.onClick.AddListener(SubmitMessage);
        btnIcon.onClick.AddListener(ActiveIcon);
        // Gán mã icon cho t?ng nút
        buttonIcons[btn1] = "<sprite=\"1\" anim=\"0,5,5\">";
        buttonIcons[btn2] = "<sprite=\"2\" anim=\"0,5,5\">";
        buttonIcons[btn3] = "<sprite=\"3\" anim=\"0,5,5\">";
        buttonIcons[btn4] = "<sprite=\"4\" anim=\"0,5,5\">";
        buttonIcons[btn5] = "<sprite=\"5\" anim=\"0,5,5\">";
        buttonIcons[btn6] = "<sprite=\"6\" anim=\"0,5,5\">";
        buttonIcons[btn7] = "<sprite=\"7\" anim=\"0,4,5\">";
        buttonIcons[btn8] = "<sprite=\"8\" anim=\"0,4,5\">";
        buttonIcons[btn9] = "<sprite=\"9\" anim=\"0,4,5\">";
        buttonIcons[btn10] = "<sprite=\"10\" anim=\"0,5,5\">";
        buttonIcons[btn11] = "<sprite=\"11\" anim=\"0,5,5\">";
        buttonIcons[btn12] = "<sprite=\"12\" anim=\"0,5,5\">";
    }

    public void ActiveIcon()
    {
        if(boxIcon.activeSelf != true)
        {
            boxIcon.SetActive(true);
        }
        else
        {
            boxIcon.SetActive(false);
        }
    }
}
