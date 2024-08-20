using Firebase.Database;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Firebase.Auth;
using Firebase.Extensions;
using System.Linq;
using System.Threading;
using System.Collections;

public class ChoiceCharacter : MonoBehaviour
{
    [Header("Characters")]
    [SerializeField] private GameObject[] arrCharacters;
    [SerializeField] private Transform characterPos;

    [Header("Value Character")]
    [SerializeField] private Text txtValueHP;
    [SerializeField] private Text txtValueDamage;
    [SerializeField] private Text txtValueLV;
    [SerializeField] private Text txtValueArmor;
    public Text txtUserName;
    public InputField inputUserName;
    [Header("Button")]
    [SerializeField] private Button btnNextCharacter;
    [SerializeField] private Button btnPreviousCharacter;
    [SerializeField] private Button btnSelectCharacter;
    [SerializeField] private Button btnSaveUserNam;
    [SerializeField] private Button btnAddLV;
    [SerializeField] private Button btnAvatar;
    [Header("Board Data")]
    [SerializeField] private GameObject prefabsCharacterRank;
    [SerializeField] private Transform Content;
    [SerializeField] private Dropdown dropdown;
    [SerializeField] private List<CharactorSO> lstCharacterSO = new List<CharactorSO>();
    private CharacterDataFireBase characterDataFireBase;
    private DatabaseReference dbRef;
    public string userId;
    public static ChoiceCharacter Instance => instance;
    private static ChoiceCharacter instance;
    private string useName;
    private int index = 0;

    private void Awake()
    {
        dbRef = FirebaseDatabase.DefaultInstance.RootReference;
        if(instance != null)
        {
            Debug.Log("Only one ChoiceCharacter Script");
        }
        else
        {
            instance = this;
        }
        LoadDataSO();
        userId = FirebaseAuth.DefaultInstance.CurrentUser.UserId;
    }

    void Start()
    {
        characterDataFireBase = new CharacterDataFireBase();
        btnNextCharacter.onClick.AddListener(BtnNextCharacter);
        btnPreviousCharacter.onClick.AddListener(BtnPrevCharacter);
        btnSelectCharacter.onClick.AddListener(SaveDataFirebase);
        btnSaveUserNam.onClick.AddListener(UpdateUserName);
        btnAddLV.onClick.AddListener(BtnAddLV);
        btnAvatar.onClick.AddListener(GetAvatar);
        DislayCharacter(index);
        BoardDataRank(0);

        // Đợi LoadAvatar() từ IG_MediaService
        StartCoroutine(IG_MediaService.instance.WaitLoadAvatar());

        StartCoroutine(WaitForImageAndSetAvatar());
    }
    private void LoadDataSO()
    {
        CharactorSO[] allChar = Resources.LoadAll<CharactorSO>("");

        foreach (CharactorSO charSO in allChar)
        {
            lstCharacterSO.Add(charSO);
        }
    }

    private void UpdataInFCharacterScene(CharacterData characterData)
    {
        txtValueHP.text = characterData.hp.ToString();
        txtValueLV.text = characterData.level.ToString();
        txtValueArmor.text = characterData.armor.ToString();
        txtValueDamage.text = characterData.ad.ToString();
        inputUserName.text = characterData.useName;
    }

    private void DislayCharacter(int i)
    {
        for (int j = 0; j < arrCharacters.Length; j++)
        {
            arrCharacters[j].SetActive(j == i);
        }
        LoadDataFireBase(i);
        UpdataInFCharacterScene(characterPos.GetComponent<CharacterData>());
    }

    private void UpdateUserName()
    {
        if (inputUserName.text == null || inputUserName.text == "")
        {
            Debug.Log("inputUserName.text null");
            return;
        }
        useName = inputUserName.text;
        CharacterData characterData = characterPos.GetComponent<CharacterData>();
        characterData.useName = useName;
    }

    private void SaveDataFirebase()
    {
        string userId = FirebaseAuth.DefaultInstance.CurrentUser.UserId;
        Debug.Log("Saving data for User ID: " + userId);

        CharacterDataFireBase characterDataFireBase = new CharacterDataFireBase(characterPos.GetComponent<CharacterData>());
        if (characterDataFireBase != null && characterDataFireBase.useName != null && characterDataFireBase.useName != "")
        {
            string json = JsonUtility.ToJson(characterDataFireBase);
            dbRef.Child("users").Child(userId).Child(characterDataFireBase.chacracterID.ToString()).SetRawJsonValueAsync(json).ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.LogError("Failed to save data to Firebase: " + task.Exception);
                }
                else
                {
                    Debug.Log("Data saved successfully.");
                }
            });
        }
        else
        {
            Debug.Log("characterDataFireBase null or characterDataFireBase.useName null");
        }
        BoardDataRank(characterDataFireBase.chacracterID);
    }

    public void LoadDataFireBase(int index)
    {
        dbRef.Child("users").Child(userId).Child(index.ToString()).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Failed to read data from Firebase: " + task.Exception);
            }
            else if (task.IsCompletedSuccessfully)
            {
                DataSnapshot snapshot = task.Result;
                if (snapshot.Exists)
                {
                    string json = snapshot.GetRawJsonValue();
                    characterDataFireBase = JsonUtility.FromJson<CharacterDataFireBase>(json);
                    CharacterData characterData = characterPos.GetComponent<CharacterData>();
                    if (characterDataFireBase != null && characterData != null)
                    {
                        characterData.characterID = characterDataFireBase.chacracterID;
                        characterData.characterName = characterDataFireBase.characterName;
                        characterData.hp = characterDataFireBase.hp;
                        characterData.ad = characterDataFireBase.ad;
                        characterData.level = characterDataFireBase.level;
                        characterData.armor = characterDataFireBase.armor;
                        characterData.useName = characterDataFireBase.useName;
                        UpdataInFCharacterScene(characterData);
                        Debug.Log("LoadDataFireBase success");
                    }
                    else
                    {
                        Debug.Log("characterDataFireBase null");
                    }
                }
                else
                {
                    Debug.Log("No data available in Firebase.");
                    useName = "";
                    GetDataItem(index);
                }
            }
        });

    }

    public void GetDataItem(int index)
    {
        Debug.Log("GetDataItem");
        CharacterData characterData = characterPos.GetComponent<CharacterData>();
        if (characterData != null)
        {
            foreach (CharactorSO charecterSO in lstCharacterSO)
            {
                if (charecterSO.characterName == arrCharacters[index].name)
                {
                    characterData.characterID = charecterSO.CharacterID;
                    characterData.characterName = charecterSO.characterName;
                    characterData.hp = charecterSO.hp;
                    characterData.ad = charecterSO.ad;
                    characterData.level = charecterSO.level;
                    characterData.armor = charecterSO.armor;
                    characterData.useName = useName;
                }
                UpdataInFCharacterScene(characterData);
            }
        }
        else
        {
            Debug.Log("GetDataItem Debug characterData null");

        }
    }
    public void BoardDataRank(int characterID)
    {
        foreach (Transform child in Content)
        {
            Destroy(child.gameObject);
        }

        // Truy vấn tất cả người dùng
        var dbBoardData = dbRef.Child("users").OrderByChild(characterID.ToString() + "/level").GetValueAsync().ContinueWithOnMainThread(task =>
        {

            if (task.IsFaulted)
            {
                Debug.LogError("Failed to read data from Firebase: " + task.Exception);
            }
            else if (task.IsCompletedSuccessfully)
            {
                DataSnapshot snapshot = task.Result;
                if (snapshot.Exists)
                {
                    // Tạo danh sách tạm thời để lưu trữ dữ liệu cần sắp xếp
                    List<(string useName, int level)> charactersData = new List<(string useName, int level)>();

                    foreach (DataSnapshot userSnapshot in snapshot.Children)
                    {
                        if (userSnapshot.HasChild(characterID.ToString()))
                        {
                            DataSnapshot characterSnapshot = userSnapshot.Child(characterID.ToString());
                            string useNameRank = characterSnapshot.Child("useName").Value.ToString();
                            int levelRank = int.Parse(characterSnapshot.Child("level").Value.ToString());
                            charactersData.Add((useNameRank, levelRank));
                        }
                    }
                    charactersData.Reverse();
                    foreach (var character in charactersData)
                    {
                        int level = character.level;

                        PrefabsBoardRank(character.useName.ToString(), level);
                    }
                }
                else
                {
                    Debug.LogWarning("User data not found");
                }
            }
        });
    }
    public void PrefabsBoardRank(string useName, int level)
    {
        GameObject newCharacterRank = Instantiate(prefabsCharacterRank, Content);
        newCharacterRank.SetActive(true);
        for (int i = 0; i < prefabsCharacterRank.transform.childCount; i++)
        {
            if (newCharacterRank.transform.GetChild(i).name == "txtNameCharacter")
            {
                newCharacterRank.transform.GetChild(i).GetComponent<Text>().text = useName;
            }
            if (newCharacterRank.transform.GetChild(i).name == "txtRankHP")
            {
                newCharacterRank.transform.GetChild(i).GetComponent<Text>().text = level.ToString();
            }
        }
    }

    private void BtnNextCharacter()
    {
        if (index >= arrCharacters.Length - 1)
        {
            index = 0;
        }
        else
        {
            index++;
        }
        DislayCharacter(index);
    }

    private void BtnPrevCharacter()
    {
        index--;
        if (index < 0) index = arrCharacters.Length - 1;
        DislayCharacter(index);
    }

    public void BtnAddLV()
    {
        if (useName == null || useName == "")
        {
            Debug.Log("useName null");
            return;
        }
        characterPos.GetComponent<CharacterData>().level += 1;
        UpdataInFCharacterScene(characterPos.GetComponent<CharacterData>());
    }


    public void DropDownSelectCharcterID()
    {
        int characterID = dropdown.value;
        BoardDataRank(characterID);
    }
    public Sprite ConvertTexture2DToSprite(Texture2D texture)
    {
        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }

    public void GetAvatar()
    {
        // Bắt đầu quá trình chọn ảnh từ thư viện
        IG_MediaService.instance.GetImageFromGallery();
        StartCoroutine(WaitForImageAndSetAvatar());
    }

    private IEnumerator WaitForImageAndSetAvatar()
    {
        Debug.Log("Waiting for image to be loaded...");
        while (IG_MediaService.instance.currentImage == null)
        {
            yield return null; 
        }
        btnAvatar.GetComponent<Image>().sprite = ConvertTexture2DToSprite(IG_MediaService.instance.currentImage);
    }

}

[System.Serializable]
public class CharacterDataFireBase
{
    public int chacracterID;
    public string characterName;
    public float hp;
    public float ad;
    public float level;
    public float armor;
    public string useName;
    public CharacterDataFireBase() { }

    public CharacterDataFireBase(CharacterData characterData)
    {
        this.chacracterID = characterData.characterID;
        this.characterName = characterData.characterName;
        this.hp = characterData.hp;
        this.ad = characterData.ad;
        this.level = characterData.level;
        this.armor = characterData.armor;
        this.useName = characterData.useName;
    }
}
