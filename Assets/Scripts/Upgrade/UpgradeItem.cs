using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections.Generic;
public class UpgradeItem : MonoBehaviour
{
    public GameObject txtLevelUp;
    public float posTarget;
    public float init = -67;
    Vector3 poitnFade = new Vector3(206, 306, 0);
    public Tween tweenLevelUp;

    private void Start()
    {
        UpgradeItemLevel();
    }

    public void UpgradeItemLevel()
    {
        Sequence seq = DOTween.Sequence();
        txtLevelUp.SetActive(true);
        seq.Join(tweenLevelUp = txtLevelUp.GetComponent<RectTransform>().DOAnchorPosY(posTarget, 2, true));
        seq.Join(tweenLevelUp = txtLevelUp.GetComponent<Text>().DOFade(0, 1).SetDelay(1));
        seq.OnComplete((Init));
        seq.Play();
    }

    public void Init()
    {
        txtLevelUp.SetActive(false);
        tweenLevelUp = txtLevelUp.GetComponent<RectTransform>().DOAnchorPosY(init, 0, true);
        tweenLevelUp = txtLevelUp.GetComponent<Text>().DOFade(1, 0);
    }
}
