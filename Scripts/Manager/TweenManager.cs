using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
public class TweenManager : MonoBehaviour
{

    public void OnTween()
    {
        if (!DOTween.IsTweening(EventSystem.current.currentSelectedGameObject))
            EventSystem.current.currentSelectedGameObject.transform.DOScale(1.06f, 0.1f).SetLoops(2, LoopType.Yoyo);
    }
}
