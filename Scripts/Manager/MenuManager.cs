using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    RectTransform slider;
    //384
    float duration = 0.5f; 
    float smoothness = 0.02f;
    bool IsSelected = false;
    public void OnMenuClick()
    {
        bool check = false;
        if (IsSelected==false)
        {
            check = true;
            IsSelected = true;
            Vector2 v = new Vector2(0, 0);
            StartCoroutine("LerpPos", v);
        }
        if (IsSelected == true & check == false)
        {
            IsSelected = false;
            Vector2 v = new Vector2(0, 384);
            StartCoroutine("LerpPos", v);
        }
    }
    IEnumerator LerpPos(Vector2 v)
    {
        float progress = 0; //This float will serve as the 3rd parameter of the lerp function.
        float increment = smoothness / duration; //The amount of change to apply.
        while (progress < 1)
        {
            slider.anchoredPosition = Vector2.Lerp(slider.anchoredPosition, v, progress);
            progress += increment;
            yield return new WaitForSeconds(smoothness);
        }
    }
}
