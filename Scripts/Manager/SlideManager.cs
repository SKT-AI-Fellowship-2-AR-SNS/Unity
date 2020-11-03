using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideManager : MonoBehaviour
{
    [SerializeField]
    GameObject Setting;
    [SerializeField]
    GameObject Panel;
    [SerializeField]
    GameObject MyProfile;
    [SerializeField]
    GameObject HoloLensFaceDetectionExample;
    public void OnMyProfileClick()
    {
        if (HoloLensFaceDetectionExample.gameObject.activeSelf == true)
        {
            HoloLensFaceDetectionExample.GetComponent<HoloLensWithOpenCVForUnityExample.HoloLensFaceDetectionExample>()
            .OnStopButtonClick();
        }
        MyProfile.SetActive(true);
    }
    public void OnMyProfileBackClick()
    {
        if (HoloLensFaceDetectionExample.gameObject.activeSelf == true)
        {
            HoloLensFaceDetectionExample.GetComponent<HoloLensWithOpenCVForUnityExample.HoloLensFaceDetectionExample>()
            .OnPlayButtonClick();
        }
        MyProfile.SetActive(false);
    }
    public void OnSettingClick()
    {
        Setting.SetActive(true);
    }
    public void OnPanelClick()
    {
        Setting.SetActive(false);
    }
}
