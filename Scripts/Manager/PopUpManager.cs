using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpManager : MonoBehaviour
{
    [SerializeField]
    GameObject HoloLensFaceDetectionExample;

    [SerializeField]
    GameObject PopUp;
    [SerializeField]
    GameObject PopUpContent;
    [SerializeField]
    GameObject Alarm_Normal_Icon;
    [SerializeField]
    GameObject MyHistory_Panel;
    [SerializeField]
    GameObject FriendHistory_Panel;

    [SerializeField]
    GameObject Capture_Right_Menu;
    [SerializeField]
    GameObject CapturedImage;
    [SerializeField]
    GameObject UploadImage;
    // Start is called before the first frame update
    public void OnBellClick()
    {
        if (PopUp.activeSelf==true)
        {
            PopUp.SetActive(false);
            HoloLensFaceDetectionExample.GetComponent<HoloLensWithOpenCVForUnityExample.HoloLensFaceDetectionExample>()
            .OnPlayButtonClick();
            HoloLensFaceDetectionExample.GetComponent<HoloLensWithOpenCVForUnityExample.HoloLensFaceDetectionExample>()
                .IsCapturing = false;
        }
    }
    public void OnAnimEnd()
    {
        PopUp.SetActive(true);
        CancelInvoke("PopUpCancel");
        Invoke("PopUpCancel", 15);
    }
    void PopUpCancel()
    {
        Clear(PopUpContent);
        if (FriendHistory_Panel.activeSelf == false && MyHistory_Panel.activeSelf == false 
            && Capture_Right_Menu.activeSelf == false && CapturedImage.activeSelf == false && UploadImage.activeSelf == false)
        {
            HoloLensFaceDetectionExample.GetComponent<HoloLensWithOpenCVForUnityExample.HoloLensFaceDetectionExample>()
            .OnPlayButtonClick();
            HoloLensFaceDetectionExample.GetComponent<HoloLensWithOpenCVForUnityExample.HoloLensFaceDetectionExample>()
                .IsCapturing = false;
        }
        
        Alarm_Normal_Icon.SetActive(false);
        PopUp.SetActive(false);
    }
    void Clear(GameObject content)
    {
        Transform[] childList = content.GetComponentsInChildren<Transform>(true);
        if (childList != null)
        {
            for (int i = 1; i < childList.Length; i++)
            {
                if (childList[i] != transform)
                    Destroy(childList[i].gameObject);
            }
        }
    }
}
