using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;
using System.IO;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LoginManager : DontDestroy<LoginManager>
{
    [SerializeField]
    public string face_id;
    [SerializeField]
    public string token;
    [SerializeField]
    public string time;
    [SerializeField]
    public bool IsMyAlbum;

    [SerializeField]
    public int UID = 1;

    public void OnKakaoLogin()
    {
        string url = "http://3.34.20.225:3000/users/kakao";
            /*"https://kauth.kakao.com/oauth/authorize?" +
            "client_id=edfe37094c314b31e73b9a24820274a7" +
            "&redirect_uri=https://www.naver.com" +
            "&response_type=code";*/
        Application.OpenURL(url);
        //StartCoroutine("LoadLogin");
        InvokeRepeating("Login", 2, 2);
    }
    
    protected override void OnStart()
    {
        base.OnStart();
        StartCoroutine("GetSirvToken");
    }

    IEnumerator GetSirvToken()
    {
        string jsonStr = "{\n " +
            "\"clientId\": \"FK4MFqmZQb09gnNRClocIYQfXvX\",\n " +
            "\"clientSecret\": \"hQl3ARwvb6ZZ8JOf2wPGE6X3CCKYWGUWBLPTygT/LwRQLB58Aa7b+19WYaTyZd1Gg/pvVhm8nhq/jiybWtp9xg==\"\n" +
            "}";
        var formData = System.Text.Encoding.UTF8.GetBytes(jsonStr);
        List<IMultipartFormSection> form = new List<IMultipartFormSection>();
        form.Add(new MultipartFormDataSection(formData));

        UnityWebRequest www = UnityWebRequest.Post("https://api.sirv.com/v2/token", form);
        www.uploadHandler = (UploadHandler)new UploadHandlerRaw(formData);
        www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");
        yield return www.SendWebRequest();

        string result = www.downloadHandler.text;
        string[] tmp = result.Split(new char[] { ',' });
        string tmp1 = tmp[0];
        token = tmp1.Substring(14, tmp1.Length - 15);
    }
}
