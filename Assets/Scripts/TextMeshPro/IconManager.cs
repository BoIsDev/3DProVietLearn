using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class IconManager : MonoBehaviour
{
    public TMP_Text textMeshPro;
    public TMP_SpriteAsset spriteAsset;
    public Transform Content;
    public List<Transform> btnIcons;
    public List<TMP_SpriteAsset> tMP_SpriteAssets;
    private TMP_SpriteAnimator spriteAnimator;

    public void Start()
    {
        LoadDataSpriteAsset();
        LoadObjContent();
    }
    public void GetSpriteAsset(TMP_SpriteAsset spriteAsset)
    {
        spriteAnimator = GetComponentInChildren<TMP_SpriteAnimator>();
        spriteAnimator.DoSpriteAnimation(0, spriteAsset, 0, 5, 5); // Chạy animation cho ký tự tại chỉ số 0
        Debug.Log("1 ");
    }
    protected virtual void LoadDataSpriteAsset()
    {
        TMP_SpriteAsset[] spriteAssets = Resources.LoadAll<TMP_SpriteAsset>("Sprite Assets");
        foreach (TMP_SpriteAsset tmp in spriteAssets)
        {
            this.tMP_SpriteAssets.Add(tmp);
        }  
    }
    public void LoadObjContent()
    {
        if (Content == null) return;
        foreach(Transform obj in Content)
        {
            foreach(TMP_SpriteAsset tmp in tMP_SpriteAssets)
            {
                tmp.name = obj.name;
                GetSpriteAsset(tmp);
            }
        }
    }

    //public void GetTMP_Text(TMP_Text text)
    //{ 
    //   text = 
    //}



}
