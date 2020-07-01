using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.Video;

public class ContentsManager : MonoBehaviour
{
    [SerializeField]
    GameObject content;
    [SerializeField]
    GameObject LoadPanel;
    // Start is called before the first frame update
    void Start()
    {
        //LoadPanel.gameObject.SetActive(false); /*ClickImage.cs

        for (int i = 0; i < 6; i++)
        {
            string url = "http://k.kakaocdn.net/dn/baWg6c/btqzhtIkxlb/Vcg3uDMKLYCx7kXKCZXhK1/img_640x640.jpg";
            GameObject img = Instantiate(Resources.Load("RawImage")) as GameObject;
            StartCoroutine(DownloadImage(url, img));
            img.transform.parent = content.transform;
            img.GetComponent<RectTransform>().localPosition = new Vector3(img.transform.position.x, img.transform.position.y, 0);
            img.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        }
    }
    IEnumerator DownloadImage(string MediaUrl, GameObject img)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(MediaUrl);
        yield return request.SendWebRequest();
        if (request.isNetworkError || request.isHttpError)
            Debug.Log(request.error);
        else
            img.GetComponent<RawImage>().texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
    }
    void Clear()
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
    public void OnImageButton()
    {
        Clear();
        for (int i = 0; i < 6; i++)
        {
            string url = "http://k.kakaocdn.net/dn/baWg6c/btqzhtIkxlb/Vcg3uDMKLYCx7kXKCZXhK1/img_640x640.jpg";
            GameObject img = Instantiate(Resources.Load("RawImage")) as GameObject;
            StartCoroutine(DownloadImage(url, img));
            img.transform.parent = content.transform;
            img.GetComponent<RectTransform>().localPosition = new Vector3(img.transform.position.x, img.transform.position.y, 0);
            img.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        }
    }
    public void OnVideoButton()
    {
        Clear();
        string url = "https://www.learningcontainer.com/wp-content/uploads/2020/05/sample-mp4-file.mp4";
        GameObject video = Instantiate(Resources.Load("Video")) as GameObject;
        video.transform.parent = content.transform;
        video.GetComponent<RectTransform>().localPosition = new Vector3(video.transform.position.x, video.transform.position.y, 0);
        video.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        video.GetComponent<VideoPlayer>().url = url;
        RenderTexture rt = new RenderTexture(256, 256, 16, RenderTextureFormat.ARGB32);
        video.GetComponent<VideoPlayer>().targetTexture = rt;
        video.GetComponent<RawImage>().texture = rt;
        video.GetComponent<VideoPlayer>().Play();

        url = "http://techslides.com/demos/sample-videos/small.mp4";
        video = Instantiate(Resources.Load("Video")) as GameObject;
        video.transform.parent = content.transform;
        video.GetComponent<RectTransform>().localPosition = new Vector3(video.transform.position.x, video.transform.position.y, 0);
        video.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        video.GetComponent<VideoPlayer>().url = url;
        rt = new RenderTexture(256, 256, 16, RenderTextureFormat.ARGB32);
        video.GetComponent<VideoPlayer>().targetTexture = rt;
        video.GetComponent<RawImage>().texture = rt;
        video.GetComponent<VideoPlayer>().Play();
    }
    public void OnLoadBack()
    {
        LoadPanel.gameObject.SetActive(false);
    }
}
