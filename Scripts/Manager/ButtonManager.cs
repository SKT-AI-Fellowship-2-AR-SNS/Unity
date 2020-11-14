using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ButtonManager : MonoBehaviour
{
    [SerializeField]
    GameObject MyHistory_Panel;
    [SerializeField]
    GameObject FriendHistory_Panel;
    [SerializeField]
    GameObject Back_Icon;
    [SerializeField]
    GameObject HoloLensFaceDetectionExample;
    [SerializeField]
    GameObject content1;
    [SerializeField]
    GameObject content2;
    [SerializeField]
    GameObject content3;
    [SerializeField]
    GameObject content4;
    [SerializeField]
    GameObject content5;
    [SerializeField]
    GameObject content6;
    [SerializeField]
    GameObject MyCommentContent;
    [SerializeField]
    GameObject FriendCommentContent;

    LoginManager LM;
    HistoryManager HM;
    CaptureManager CM;

    void Start()
    {
        LM = GameObject.Find("LoginManager").GetComponent<LoginManager>();
        HM = GameObject.Find("HistoryManager").GetComponent<HistoryManager>();
        CM = GameObject.Find("CaptureManager").GetComponent<CaptureManager>();
    }
    public void OnMyHistoryClick()
    {
        LM.IsMyAlbum = true;
        if (HoloLensFaceDetectionExample.gameObject.activeSelf == true)
        {
            HoloLensFaceDetectionExample.GetComponent<HoloLensWithOpenCVForUnityExample.HoloLensFaceDetectionExample>()
            .OnStopButtonClick();
        }
        HM.StartCoroutine("PreviewHistory", new int[] { int.Parse(LM.UID.ToString()), int.Parse(LM.UID.ToString()) });
        HM.StartCoroutine("AllHistory", new int[] { int.Parse(LM.UID.ToString()), int.Parse(LM.UID.ToString()) });
        MyHistory_Panel.SetActive(true);
    }
    public void OnMyHistoryBackClick()
    {
        //content1.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
        content2.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
        Clear(content1);
        Clear(content2);
        Clear(content5);
        Clear(MyCommentContent);
        MyHistory_Panel.SetActive(false);
        if (HoloLensFaceDetectionExample.gameObject.activeSelf == true)
        {
            HoloLensFaceDetectionExample.GetComponent<HoloLensWithOpenCVForUnityExample.HoloLensFaceDetectionExample>()
            .OnPlayButtonClick();
            HoloLensFaceDetectionExample.GetComponent<HoloLensWithOpenCVForUnityExample.HoloLensFaceDetectionExample>()
                .IsCapturing = false;
        }

    }
    public void OnfriendHistoryClick(string uid = "")
    {
        LM.IsMyAlbum = false;
        if (HoloLensFaceDetectionExample.gameObject.activeSelf == true)
        {
            HoloLensFaceDetectionExample.GetComponent<HoloLensWithOpenCVForUnityExample.HoloLensFaceDetectionExample>()
            .OnStopButtonClick();
        }
        if (uid.Length == 0)
        {
            HM.StartCoroutine("PreviewHistory", new int[] { int.Parse(LM.UID.ToString()), int.Parse(CM.UID.ToString()) });
            HM.StartCoroutine("AllHistory", new int[] { int.Parse(LM.UID.ToString()), int.Parse(CM.UID.ToString()) });
        }
        else
        {
            CM.UID = uid;
            HM.StartCoroutine("PreviewHistory", new int[] { int.Parse(LM.UID.ToString()), int.Parse(CM.UID.ToString()) });
            HM.StartCoroutine("AllHistory", new int[] { int.Parse(LM.UID.ToString()), int.Parse(CM.UID.ToString()) });
        }
        FriendHistory_Panel.SetActive(true);
    }
    public void OnfriendHistoryBackClick()
    {
        //content3.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
        content4.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
        Clear(content3);
        Clear(content4);
        Clear(content6);
        Clear(FriendCommentContent);
        FriendHistory_Panel.SetActive(false);
        if (HoloLensFaceDetectionExample.gameObject.activeSelf == true)
        {
            HoloLensFaceDetectionExample.GetComponent<HoloLensWithOpenCVForUnityExample.HoloLensFaceDetectionExample>()
            .OnPlayButtonClick();
            HoloLensFaceDetectionExample.GetComponent<HoloLensWithOpenCVForUnityExample.HoloLensFaceDetectionExample>()
                .IsCapturing = false;
        }
    }
    public void OnfriendHistoryLocClick(string uid = "")
    {
        LM.IsMyAlbum = false;
        if (HoloLensFaceDetectionExample.gameObject.activeSelf == true)
        {
            HoloLensFaceDetectionExample.GetComponent<HoloLensWithOpenCVForUnityExample.HoloLensFaceDetectionExample>()
            .OnStopButtonClick();
        }
        if (uid.Length == 0)
        {
            HM.StartCoroutine("PreviewHistory", new int[] { int.Parse(LM.UID.ToString()), int.Parse(CM.UID.ToString()) });
            HM.StartCoroutine("AllHistory", new int[] { int.Parse(LM.UID.ToString()), int.Parse(CM.UID.ToString()) });
        }
        else
        {
            CM.UID = uid;
            HM.StartCoroutine("PreviewHistory", new int[] { int.Parse(LM.UID.ToString()), int.Parse(CM.UID.ToString()) });
            HM.StartCoroutine("AllHistory", new int[] { int.Parse(LM.UID.ToString()), int.Parse(CM.UID.ToString()) });
        }
        
        FriendHistory_Panel.SetActive(true);
        FriendHistory_Panel.transform.GetChild(0).gameObject.SetActive(false);
        FriendHistory_Panel.transform.GetChild(1).gameObject.SetActive(true);
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
