using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour
{
    public Transform ItemPos;
    public List<Transform> items = new List<Transform>();
    public List<ItemSO> lstItemsSO = new List<ItemSO>();
    public List<WeaponSO> lstWeaponSO = new List<WeaponSO>();
    private static ItemManager instance;
    public static ItemManager Instance => instance;

    protected void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Only one Item Script");
        }
        else
        {
            instance = this;
        }
        LoadDataSO();
        LoadItem();
    }

    private void Start()
    {

        GetDataItem();
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetDataSO();
        }
    }
    protected virtual void LoadDataSO()
    {
        // Kiểm tra xem dữ liệu đã được lưu trước đó hay chưa
        if (ES3.KeyExists("lstItemsSO") && ES3.KeyExists("lstWeaponSO"))
        {
            // Tải dữ liệu đã lưu
            lstItemsSO = ES3.Load<List<ItemSO>>("lstItemsSO");
            lstWeaponSO = ES3.Load<List<WeaponSO>>("lstWeaponSO");
        }
        else
        {
            // Nạp dữ liệu từ Resources
            ItemSO[] allItems = Resources.LoadAll<ItemSO>("");
            foreach (ItemSO itemData in allItems)
            {
                this.lstItemsSO.Add(itemData);
            }
            WeaponSO[] allWeapons = Resources.LoadAll<WeaponSO>("");
            foreach (WeaponSO weaponData in allWeapons)
            {
                this.lstWeaponSO.Add(weaponData);
            }
            // Lưu dữ liệu sau khi nạp
            SaveDataSO();
        }
    }

    private void SaveDataSO()
    {
        ES3.Save("lstItemsSO", lstItemsSO);
        ES3.Save("lstWeaponSO", lstWeaponSO);
    }

    protected virtual void LoadItem()
    {
        foreach (Transform item in ItemPos)
        {
            if (item.childCount > 0)
            {
                this.items.Add(item.GetChild(0));
            }
        }
    }

    public virtual void GetDataItem()
    {
        foreach (Transform item in items)
        {
            ItemStone itemStone = item.GetComponent<ItemStone>();
            ItemWeapon itemWeapon = item.GetComponent<ItemWeapon>();

            if (itemStone != null)
            {
                foreach (ItemSO itemSO in lstItemsSO)
                {
                    if (item.name == itemSO.itemName)
                    {
                        itemStone.itemName = itemSO.itemName;
                        itemStone.exp = itemSO.exp;
                        itemStone.quantity = itemSO.quantity;
                        itemStone.GetComponentInChildren<Text>().text = itemSO.quantity.ToString();
                        itemStone.itemCode = itemSO.itemCode;
                        itemStone.itemImage = itemSO.itemImage;
                        Image itemImageComponent = item.GetComponent<Image>();
                        if (itemImageComponent == null)
                        {
                            itemImageComponent.sprite = itemSO.itemImage;
                            Debug.Log("Sprite updated to: " + itemSO.itemImage.name);
                        }
                        break;
                    }
                }
            }
            if (itemWeapon != null)
            {
                foreach (WeaponSO weapon in lstWeaponSO)
                {
                    if (item.name == weapon.itemName)
                    {
                        itemWeapon.itemName = weapon.itemName;
                        itemWeapon.dame = weapon.dame;
                        itemWeapon.level = weapon.level;
                        itemWeapon.GetComponentInChildren<Text>().text = "LV " + itemWeapon.level.ToString();
                        itemWeapon.itemCode = weapon.itemCode;
                        itemWeapon.itemImage = weapon.itemImage;
                        Image itemImageComponent = item.GetComponent<Image>();
                        if (itemImageComponent == null)
                        {
                            itemImageComponent.sprite = weapon.itemImage;
                            Debug.Log("Sprite updated to: " + weapon.itemImage.name);
                        }
                        break;
                    }
                }
            }
        }
    }

    public void UpdateWeaponLevel(string itemName, int newLevel)
    {
        foreach (WeaponSO weapon in lstWeaponSO)
        {
            if (weapon.itemName == itemName)
            {
                weapon.level = newLevel;
                SaveDataSO();
                break;
            }
            Debug.Log("Updated weapon level to: " + newLevel);
        }
    }

    public void UpdateItemStone(string itemName, int newQuantity)
    {
        foreach (ItemSO item in lstItemsSO)
        {
            if (item.itemName == itemName)
            {
                item.quantity = newQuantity;
                SaveDataSO();
                break;
            }
        }
    }
    public void ResetDataSO()
    {
        // Xóa các khóa dữ liệu đã lưu
        ES3.DeleteKey("lstItemsSO");
        ES3.DeleteKey("lstWeaponSO");

        // Xóa các danh sách hiện tại
        lstItemsSO.Clear();
        lstWeaponSO.Clear();

        // Nạp lại dữ liệu từ Resources
        LoadDataSO();

        // Cập nhật lại các đối tượng trong game nếu cần
        GetDataItem();
    }
}
