using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LevelUp : MonoBehaviour
{
    public GameObject txtLevelUp;
    public Tween tweenLevelUp;
    public float posTarget;

    public void OnEnable()
    {
        LevelUptxt();
    }
    public void LevelUptxt()
    {
        Sequence seq = DOTween.Sequence();
        txtLevelUp.SetActive(true);
        seq.Join(tweenLevelUp = txtLevelUp.GetComponent<RectTransform>().DOAnchorPosY(posTarget, 1, true));
        seq.Join(tweenLevelUp = txtLevelUp.GetComponent<Text>().DOFade(0, 1).SetDelay(1));
        seq.Append(tweenLevelUp = txtLevelUp.GetComponent<Text>().DOFade(1, 0));
        seq.OnComplete(() => PoolItem.Instance.ReturnObjePool(this.txtLevelUp));
        seq.Play();
    }

}
