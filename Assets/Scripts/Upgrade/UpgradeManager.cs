using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    public GameObject slotItem1;
    public GameObject slotItem2;
    public GameObject txtNoItem;
    public GameObject imgExpLevel;
    public GameObject txtLevelUp;
    public Button btnUpgrade;
    public GameObject particalUpLevel;
    public int expCurrent = 0;
    public int expLevel = 5;
    private void Start()
    {
        btnUpgrade.onClick.AddListener(() => UpgradeItem());
    }
    public void UpgradeItem()
    {
        if (slotItem1.transform.childCount <= 0 || slotItem2.transform.childCount <= 0)
        {
            GameObject newItem = PoolItem.Instance.GetObjItem(txtNoItem);
            newItem.SetActive(true);
            RectTransform newItemRectTransform = newItem.GetComponent<RectTransform>();
            RectTransform txtNoItemRectTransform = txtNoItem.GetComponent<RectTransform>();
            newItemRectTransform.anchoredPosition = txtNoItemRectTransform.anchoredPosition;
            newItem.transform.position = txtNoItem.transform.position;
            AudioManager.Instance.ErroItem();
        }
        else
        {
            //LoadDataWeapon
            GameObject childWeapon = slotItem1.transform.GetChild(0).gameObject;
            ItemWeapon itemWeapon = childWeapon.GetComponent<ItemWeapon>();
            //LoadDataWStone
            GameObject childStone = slotItem2.transform.GetChild(0).gameObject;
            ItemStone itemStone = childStone.GetComponent<ItemStone>();
            if (itemWeapon == null || itemStone == null) return;
            if (expCurrent == expLevel)
            {
                //Set Obj Dotween
                GameObject newItem = PoolItem.Instance.GetObjItem(txtLevelUp);
                newItem.SetActive(true);
                RectTransform newItemRectTransform = newItem.GetComponent<RectTransform>();
                RectTransform txtNoItemRectTransform = txtLevelUp.GetComponent<RectTransform>();
                newItemRectTransform.anchoredPosition = txtNoItemRectTransform.anchoredPosition;
                newItem.transform.position = txtLevelUp.transform.position;
                // Set partical
                particalUpLevel.SetActive(true);
                particalUpLevel.transform.position = slotItem1.transform.position;
                // reset data
                itemWeapon.dame += 10;
                itemWeapon.level++;
                ItemManager.Instance.UpdateWeaponLevel(itemWeapon.name, itemWeapon.level);
                childWeapon.GetComponentInChildren<Text>().text = "LV " + itemWeapon.level.ToString();
                itemStone.quantity -= 1;
                expLevel *= 2;
                expCurrent = 0;
                childStone.GetComponentInChildren<Text>().text = itemStone.quantity.ToString();
            }
            else if (expCurrent < expLevel && itemStone.quantity > 1)
            {

                ScrollbarController.Instance.updateHealthBar(expCurrent, expLevel);
                //Set Obj Dotween
                GameObject newItem = PoolItem.Instance.GetObjItem(imgExpLevel);
                newItem.SetActive(true);
                RectTransform newItemRectTransform = newItem.GetComponent<RectTransform>();
                RectTransform txtNoItemRectTransform = txtLevelUp.GetComponent<RectTransform>();
                newItemRectTransform.anchoredPosition = txtNoItemRectTransform.anchoredPosition;
                newItem.transform.position = imgExpLevel.transform.position;
                // Set partical
                particalUpLevel.SetActive(false);
                // reset data
                expCurrent += 1;
                itemStone.quantity -= 1;
                ItemManager.Instance.UpdateItemStone(itemStone.name, itemStone.quantity);
                childStone.GetComponentInChildren<Text>().text = itemStone.quantity.ToString();
                AudioManager.Instance.AddExp();
            }
            else
            {
                ItemManager.Instance.UpdateItemStone(itemStone.name, 0);
                Destroy(itemStone.gameObject);
            }
        }
    }
}





