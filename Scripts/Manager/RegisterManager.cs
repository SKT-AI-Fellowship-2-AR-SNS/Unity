using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class RegisterManager : MonoBehaviour
{
    [SerializeField]
    GameObject Registerpanel;
    [SerializeField]
    GameObject LoadPanel;
    [SerializeField]
    GameObject content;
    [SerializeField]
    Text text;
    // Start is called before the first frame update
    void Start()
    {
        text.text = Application.persistentDataPath;
        Registerpanel.gameObject.SetActive(false);
        LoadPanel.gameObject.SetActive(false);
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
    void LoadImage(string path, GameObject img)
    {
        Texture2D texture = null;
        byte[] byteTexture = System.IO.File.ReadAllBytes(path);
        if (byteTexture.Length > 0)
        {
            texture = new Texture2D(0, 0);
            texture.LoadImage(byteTexture);
        }
        img.GetComponent<RawImage>().texture = texture;
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
    public void OnRegister()
    {
        Registerpanel.gameObject.SetActive(true);
    }
    public void OnRegisterBack()
    {
        Registerpanel.gameObject.SetActive(false);
    }
    public void OnLoad()
    {
        Clear();
        LoadPanel.gameObject.SetActive(true);

        string path = Application.persistentDataPath;
        string[] s1 = Directory.GetFiles(path);
        for (int i = 0; i < s1.Length; i++)
        {
            GameObject img = Instantiate(Resources.Load("RawImage")) as GameObject;
            img.transform.parent = content.transform;
            img.GetComponent<RectTransform>().localPosition = new Vector3(img.transform.position.x, img.transform.position.y, 0);
            img.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            Debug.Log(s1[i]);
            LoadImage(s1[i],img);
        }
    }
    public void OnLoadBack()
    {
        LoadPanel.gameObject.SetActive(false);
    }
    
}
