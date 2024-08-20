using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExpLevel : MonoBehaviour
{
    public GameObject imgExpLevel;
    public Tween tweenExpLevel;
    public float posTarget;
    public void OnEnable()
    {
        ExpLevelIMG();
    }
    public void ExpLevelIMG()
    {
        Sequence seq = DOTween.Sequence();
        imgExpLevel.SetActive(true);
        seq.Join(tweenExpLevel = imgExpLevel.GetComponent<RectTransform>().DOAnchorPosY(posTarget, 1, true));
        seq.Join(tweenExpLevel = imgExpLevel.GetComponent<Image>().DOFade(0, 1).SetDelay(1));
        seq.Append(tweenExpLevel = imgExpLevel.GetComponent<Image>().DOFade(1, 0));
        seq.OnComplete(() => PoolItem.Instance.ReturnObjePool(this.imgExpLevel));
        seq.Play();
    }
}
