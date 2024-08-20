using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class NoItem : MonoBehaviour
{
    public GameObject txtNoItem;
    public Tween tweenNoItem;
    public float posTarget;

    public void OnEnable()
    {
        NoItemtxt();
    }
    public void NoItemtxt()
    {
        Sequence seq = DOTween.Sequence();
        txtNoItem.SetActive(true);
        seq.Join(tweenNoItem = txtNoItem.GetComponent<RectTransform>().DOAnchorPosY(posTarget, 1, true));
        seq.Join(tweenNoItem = txtNoItem.GetComponent<Text>().DOFade(0, 1).SetDelay(1));
        seq.Append(tweenNoItem = txtNoItem.GetComponent<Text>().DOFade(1, 0));
        seq.OnComplete(() => PoolItem.Instance.ReturnObjePool(this.txtNoItem));
        seq.Play();
    }
}
