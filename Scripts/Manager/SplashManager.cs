using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SplashManager : MonoBehaviour
{
    [SerializeField]
    GameObject Splash;
    [SerializeField]
    GameObject SplashPanel;
    [SerializeField]
    GameObject LoginButton;

    float duration = 2f;
    float smoothness = 0.02f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LerpPos());
    }
    IEnumerator LerpPos()
    {
        float progress = 0; //This float will serve as the 3rd parameter of the lerp function.
        float increment = smoothness / duration; //The amount of change to apply.
        while (progress < 1)
        {
            Splash.GetComponent<RawImage>().color =
                Color.Lerp(Splash.GetComponent<RawImage>().color, new Color(1, 1, 1), progress);
            progress += increment;
            yield return new WaitForSeconds(smoothness);
        }
        LoginButton.SetActive(true);
    }
    public void OnLoginClick()
    {
        SplashPanel.SetActive(false);
    }
}
