﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;
using UnityEngine.UI;
using UnityEngine.Video;
using OpenCVForUnity.VideoModule;
using UnityEditor.UIElements;

public class HistoryManager : MonoBehaviour
{
    [SerializeField]
    GameObject myLoc_Icon;
    [SerializeField]
    GameObject myTime_Icon;
    [SerializeField]
    RectTransform myLoc_Panel;
    [SerializeField]
    RectTransform myTime_Panel;
    [SerializeField]
    GameObject myLocMore_Icon;
    [SerializeField]
    GameObject MyHistoryMain;
    [SerializeField]
    GameObject MyHistory_LocMain;
    [SerializeField]
    GameObject MyHistory_LocHistory;
    [SerializeField]
    GameObject MyHistory_Comment;
    [SerializeField]
    GameObject MyHistoryEditPanel;
    [SerializeField]
    GameObject MyHistoryCommentPanel;
    [SerializeField]
    GameObject MyHistoryCommentContent;
    [SerializeField]
    GameObject MyHeartIcon;
    [SerializeField]
    Text MyName_Text;
    [SerializeField]
    GameObject My_Image;
    [SerializeField]
    Text MyName_Text2;
    [SerializeField]
    GameObject My_Image2;

    [SerializeField]
    GameObject FriendLoc_Icon;
    [SerializeField]
    GameObject FriendTime_Icon;
    [SerializeField]
    RectTransform FriendLoc_Panel;
    [SerializeField]
    RectTransform FriendTime_Panel;
    [SerializeField]
    GameObject FriendLocMore_Icon;
    [SerializeField]
    GameObject FriendHistoryMain;
    [SerializeField]
    GameObject FriendHistory_LocMain;
    [SerializeField]
    GameObject FriendHistory_LocHistory;
    [SerializeField]
    Text FriendName_Text;
    [SerializeField]
    GameObject Friend_Image;
    [SerializeField]
    Text FriendName_Text2;
    [SerializeField]
    GameObject Friend_Image2;
    [SerializeField]
    GameObject MyHistory_Follow;
    [SerializeField]
    GameObject FollowContent;
    [SerializeField]
    GameObject FriendHistory_Comment;
    [SerializeField]
    GameObject FriendHistoryEditPanel;
    [SerializeField]
    GameObject FriendHistoryCommentPanel;
    [SerializeField]
    GameObject FriendHistoryCommentContent;
    [SerializeField]
    GameObject FriendHeartIcon;

    [SerializeField]
    GameObject MyPrevContent;
    [SerializeField]
    GameObject MyAllContent;
    [SerializeField]
    GameObject FriendPrevContent;
    [SerializeField]
    GameObject FreindAllContent;

    LoginManager LM;
    CaptureManager CM;

    float duration = 0.5f;
    float smoothness = 0.02f;
    bool IsSelected1 = false;
    bool IsSelected2 = false;
    int state;

    int pageNum = 0;

    public int curId = 0;

    void Awake()
    {
        LM = GameObject.Find("LoginManager").GetComponent<LoginManager>();
        LM.IsMyAlbum = true;
        CM = GameObject.Find("CaptureManager").GetComponent<CaptureManager>();
    }
    void Start()
    {
        //StartCoroutine("AllHistory", 1);
        //StartCoroutine("PreviewHistory", 1);
    }
    public void OnMyHeartClick()
    {
        StartCoroutine(HeartClick());
        if (MyHeartIcon.GetComponent<RawImage>().color.a == 75 / 255f) // 좋아요 선택
        {
            MyHeartIcon.GetComponent<RawImage>().color = new Color(1, 1, 1, 1);
        }
        else // 좋아요 취소
        {
            MyHeartIcon.GetComponent<RawImage>().color = new Color(1, 1, 1, 75/255f);
        }
    }
    public void OnFriendHeartClick()
    {
        StartCoroutine(HeartClick());
        if (FriendHeartIcon.GetComponent<RawImage>().color.a == 75 / 255f) // 좋아요 선택
        {
            FriendHeartIcon.GetComponent<RawImage>().color = new Color(1, 1, 1, 1);
        }
        else // 좋아요 취소
        {
            FriendHeartIcon.GetComponent<RawImage>().color = new Color(1, 1, 1, 75 / 255f);
        }
    }
    IEnumerator HeartClick()
    {
        string url = "http://3.34.20.225:3000/history/like/1/" + curId;
        byte[] data = null;
        UnityWebRequest request = UnityWebRequest.Put(url, data);
        yield return request.SendWebRequest();
        string result = request.downloadHandler.text;
        var r = JObject.Parse(result);
        print(r);
        if (LM.IsMyAlbum)
        {
            MyHistory_LocHistory.transform.GetChild(9).GetComponent<Text>().text = r["data"].SelectToken("likes").ToString();
        }
        else
        {
            FriendHistory_LocHistory.transform.GetChild(9).GetComponent<Text>().text = r["data"].SelectToken("likes").ToString();
        }
    }
    public void OnMyEditPanelClick()
    {
        if (MyHistoryEditPanel.activeSelf == true) MyHistoryEditPanel.SetActive(false);
        else MyHistoryEditPanel.SetActive(true);
    }
    public void OnFriendEditPanelClick()
    {
        if (FriendHistoryEditPanel.activeSelf == true) FriendHistoryEditPanel.SetActive(false);
        else FriendHistoryEditPanel.SetActive(true);
    }
    public void OnMyHistoryCommentClick()
    {
        MyHistory_LocHistory.SetActive(false);
        //MyHistory_Comment.SetActive(true);
        Vector2 v = new Vector2(0, 0);
        StartCoroutine(LerpPosGeneral(v, MyHistory_Comment));
    }
    public void OnMyHistoryCommentBackClick()
    {
        MyHistory_LocHistory.SetActive(true);
        MyHistory_Comment.GetComponent<RectTransform>().localPosition = new Vector2(0, -384);
        //MyHistory_Comment.SetActive(false);
    }
    public void OnFriendHistoryCommentClick()
    {
        FriendHistory_LocHistory.SetActive(false);
        //FriendHistory_Comment.SetActive(true);
        Vector2 v = new Vector2(0, 0);
        StartCoroutine(LerpPosGeneral(v, FriendHistory_Comment));
    }
    public void OnFriendHistoryCommentBackClick()
    {
        FriendHistory_LocHistory.SetActive(true);
        FriendHistory_Comment.GetComponent<RectTransform>().localPosition = new Vector2(0, -384);
        //FriendHistory_Comment.SetActive(false);
    }
    public void OnFollowClick()
    {
        MyHistoryMain.SetActive(false);
        MyHistory_Follow.SetActive(true);
    }
    public void OnFollowBackClick()
    {
        MyHistory_Follow.SetActive(false);
        MyHistoryMain.SetActive(true);
    }
    public void OnFollowingTabClick()
    {
        Clear(FollowContent);
        StartCoroutine(onFollowing(0));
        MyHistory_Follow.transform.GetChild(1).gameObject.SetActive(true);
        MyHistory_Follow.transform.GetChild(2).GetComponent<RawImage>().color = new Color(151 / 255f, 151 / 255f, 151 / 255f);
        MyHistory_Follow.transform.GetChild(3).GetComponent<RawImage>().color = new Color(102 / 255f, 101 / 255f, 100 / 255f);
        MyHistory_Follow.transform.GetChild(4).GetComponent<RawImage>().color = new Color(102 / 255f, 101 / 255f, 100 / 255f);
    }
    IEnumerator onFollowing(int state)
    {
        string url = "";
        if (state == 0) {
            url = "http://3.34.20.225:3000/users/getFollowing/1" /*+ LM.UID*/;
        }
        else if (state == 1) {
            url = "http://3.34.20.225:3000/users/getFollower/1" /*+ LM.UID*/;
        }
        else {
            url = "http://3.34.20.225:3000/users/getRecommend/1" /*+ LM.UID*/;
        }
        List<IMultipartFormSection> form = new List<IMultipartFormSection>();
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();
        string result = request.downloadHandler.text;
        var r = JObject.Parse(result);
        if (r["status"].ToString().Equals("200"))
        {
            //print(r["data"]);
            foreach(var item in r["data"])
            {
                print(item);
                GameObject g = Instantiate(Resources.Load("follow")) as GameObject;
                g.name = item.SelectToken("id").ToString();
                StartCoroutine(DownloadImage(item.SelectToken("profileImage").ToString(), g.transform.GetChild(0).gameObject));
                g.transform.GetChild(1).GetComponent<Text>().text = item.SelectToken("name").ToString();
                if (item.SelectToken("isFollowing").ToString().Equals("True")) g.transform.GetChild(2).gameObject.SetActive(true);
                else g.transform.GetChild(3).gameObject.SetActive(true);
                g.transform.parent = FollowContent.transform;
                g.GetComponent<RectTransform>().localPosition = new Vector3(g.transform.position.x, g.transform.position.y, 0);
                g.GetComponent<RectTransform>().localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
                g.GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
            }
        }
        
    }
    public void OnFollowerTabClick()
    {
        Clear(FollowContent);
        StartCoroutine(onFollowing(1));
        MyHistory_Follow.transform.GetChild(1).gameObject.SetActive(true);
        MyHistory_Follow.transform.GetChild(2).GetComponent<RawImage>().color = new Color(102 / 255f, 101 / 255f, 100 / 255f);
        MyHistory_Follow.transform.GetChild(3).GetComponent<RawImage>().color = new Color(151 / 255f, 151 / 255f, 151 / 255f);
        MyHistory_Follow.transform.GetChild(4).GetComponent<RawImage>().color = new Color(102 / 255f, 101 / 255f, 100 / 255f);
    }
    public void OnRecommendTabClick()
    {
        Clear(FollowContent);
        StartCoroutine(onFollowing(2));
        MyHistory_Follow.transform.GetChild(1).gameObject.SetActive(true);
        MyHistory_Follow.transform.GetChild(2).GetComponent<RawImage>().color = new Color(102 / 255f, 101 / 255f, 100 / 255f);
        MyHistory_Follow.transform.GetChild(3).GetComponent<RawImage>().color = new Color(102 / 255f, 101 / 255f, 100 / 255f);
        MyHistory_Follow.transform.GetChild(4).GetComponent<RawImage>().color = new Color(151 / 255f, 151 / 255f, 151 / 255f);
    }
    public void OnMyHistoyLocClick()
    {
        if (LM.IsMyAlbum)
        {
            MyHistoryMain.SetActive(false);
            MyHistory_LocMain.SetActive(true);
        }
        else
        {
            FriendHistoryMain.SetActive(false);
            FriendHistory_LocMain.SetActive(true);
            //StartCoroutine("AllHistory", int.Parse(CM.UID.ToString()));
        }
    }
    public void OnMyHistoyLocHistoryClick(GameObject g, JToken j)
    {
        print(j["historyIdx"].ToString());
        curId = int.Parse(j["historyIdx"].ToString());
        StartCoroutine(getComment(int.Parse(g.name)));
        print(j["alreadyLiked"].ToString());
        if (LM.IsMyAlbum)
        {
            MyHistoryMain.SetActive(false);
            MyHistory_LocMain.SetActive(false);
            MyHistory_LocHistory.SetActive(true);
            if (j["contents_type"].ToString().Equals("image"))
            {
                MyHistory_LocHistory.transform.GetChild(4).GetComponent<RawImage>().texture = g.GetComponent<RawImage>().texture;
            }
            else
            {
                GameObject tmp = g;
                string url = j["video"].ToString();
                //MyHistory_LocHistory.transform.GetChild(4).GetComponent<VideoPlayer>().url = j["contents"].ToString();
                MyHistory_LocHistory.transform.GetChild(4).GetComponent<VideoPlayer>().url = url;
                RenderTexture rt = new RenderTexture(256, 256, 16, RenderTextureFormat.ARGB32);
                MyHistory_LocHistory.transform.GetChild(4).GetComponent<VideoPlayer>().targetTexture = rt;
                MyHistory_LocHistory.transform.GetChild(4).GetComponent<RawImage>().texture = rt;
            }
            MyHistory_LocHistory.transform.GetChild(9).GetComponent<Text>().text = j["likes"].ToString();
            MyHistory_LocHistory.transform.GetChild(3).GetComponent<Text>().text = j["location"].ToString();
            MyHistory_LocHistory.transform.GetChild(6).GetComponent<Text>().text = j["datetime"].ToString();
            MyHistory_LocHistory.transform.GetChild(7).GetComponent<Text>().text = j["day"].ToString();
            MyHistory_LocHistory.transform.GetChild(5).GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>().text = j["text"].ToString();
            if (j["alreadyLiked"].ToString().Equals("True"))
                MyHistory_LocHistory.transform.GetChild(8).GetComponent<RawImage>().color = new Color(1, 1, 1, 1);
            else
                MyHistory_LocHistory.transform.GetChild(8).GetComponent<RawImage>().color = new Color(1, 1, 1, 75/255f);
        }
        else
        {
            FriendHistoryMain.SetActive(false);
            FriendHistory_LocMain.SetActive(false);
            FriendHistory_LocHistory.SetActive(true);
            if (j["contents_type"].ToString().Equals("image"))
            {
                FriendHistory_LocHistory.transform.GetChild(4).GetComponent<RawImage>().texture = g.GetComponent<RawImage>().texture;
            }
            else
            {
                GameObject tmp = g;
                string url = j["video"].ToString();
                //FriendHistory_LocHistory.transform.GetChild(4).GetComponent<VideoPlayer>().url = j["contents"].ToString();
                FriendHistory_LocHistory.transform.GetChild(4).GetComponent<VideoPlayer>().url = url;
                RenderTexture rt = new RenderTexture(256, 256, 16, RenderTextureFormat.ARGB32);
                FriendHistory_LocHistory.transform.GetChild(4).GetComponent<VideoPlayer>().targetTexture = rt;
                FriendHistory_LocHistory.transform.GetChild(4).GetComponent<RawImage>().texture = rt;
            }
            FriendHistory_LocHistory.transform.GetChild(9).GetComponent<Text>().text = j["likes"].ToString();
            FriendHistory_LocHistory.transform.GetChild(3).GetComponent<Text>().text = j["location"].ToString();
            FriendHistory_LocHistory.transform.GetChild(6).GetComponent<Text>().text = j["datetime"].ToString();
            FriendHistory_LocHistory.transform.GetChild(7).GetComponent<Text>().text = j["day"].ToString();
            FriendHistory_LocHistory.transform.GetChild(5).GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>().text = j["text"].ToString();
            if (j["alreadyLiked"].ToString().Equals("True"))
                FriendHistory_LocHistory.transform.GetChild(8).GetComponent<RawImage>().color = new Color(1, 1, 1, 1);
            else
                FriendHistory_LocHistory.transform.GetChild(8).GetComponent<RawImage>().color = new Color(1, 1, 1, 75 / 255f);
        }
    }
    IEnumerator getComment(int idx)
    {
        string url = "http://3.34.20.225:3000/history/getComment/" + idx;
        List<IMultipartFormSection> form = new List<IMultipartFormSection>();
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();
        string result = request.downloadHandler.text;
        var r = JObject.Parse(result);
        //print(r);
        var a = r["data"];
        print(a);
        if (a.ToString()!=null)
        {
            if (LM.IsMyAlbum)
            {
                StartCoroutine(DownloadImage(a.First.SelectToken("profileImage").ToString(), MyHistoryCommentPanel.transform.GetChild(0).gameObject));
                MyHistoryCommentPanel.transform.GetChild(3).GetComponent<Text>().text = a.First.SelectToken("name").ToString();
                MyHistoryCommentPanel.transform.GetChild(4).GetComponent<Text>().text = a.First.SelectToken("comment").ToString();
            }
            else
            {
                StartCoroutine(DownloadImage(a.First.SelectToken("profileImage").ToString(), FriendHistoryCommentPanel.transform.GetChild(0).gameObject));
                FriendHistoryCommentPanel.transform.GetChild(3).GetComponent<Text>().text = a.First.SelectToken("name").ToString();
                FriendHistoryCommentPanel.transform.GetChild(4).GetComponent<Text>().text = a.First.SelectToken("comment").ToString();
            }
            foreach(var i in a)
            {
                GameObject g = Instantiate(Resources.Load("Comment_Panel")) as GameObject;
                
                if (LM.IsMyAlbum)
                {
                    g.transform.parent = MyHistoryCommentContent.transform;
                    StartCoroutine(DownloadImage(i.SelectToken("profileImage").ToString(), g.transform.GetChild(0).gameObject));
                }
                else
                {
                    g.transform.parent = FriendHistoryCommentContent.transform;
                    StartCoroutine(DownloadImage(i.SelectToken("profileImage").ToString(), g.transform.GetChild(0).gameObject));
                }
                g.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
                g.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                g.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 0, 0);
                g.transform.GetChild(3).GetComponent<Text>().text = i.SelectToken("name").ToString();
                g.transform.GetChild(4).GetComponent<Text>().text = i.SelectToken("comment").ToString();
            }
        }
        else
        {
            if (LM.IsMyAlbum)
            {
                MyHistoryCommentPanel.transform.GetChild(0).gameObject.GetComponent<RawImage>().texture = null;
                MyHistoryCommentPanel.transform.GetChild(3).GetComponent<Text>().text = "";
                MyHistoryCommentPanel.transform.GetChild(4).GetComponent<Text>().text = "첫 댓글을 입력해주세요!";
            }
            else
            {
                FriendHistoryCommentPanel.transform.GetChild(0).gameObject.GetComponent<RawImage>().texture = null;
                FriendHistoryCommentPanel.transform.GetChild(3).GetComponent<Text>().text = "";
                FriendHistoryCommentPanel.transform.GetChild(4).GetComponent<Text>().text = "첫 댓글을 입력해주세요!";
            }
        }
        
    }
    IEnumerator AllHistory(int[] id)
    {
        string url = "http://3.34.20.225:3000/history/getHistory/" + id[0]+"/" + id[1]+"/1/1";
        List<IMultipartFormSection> form = new List<IMultipartFormSection>();
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();
        string result = request.downloadHandler.text;
        var r = JObject.Parse(result);
        print(r);
        if (LM.IsMyAlbum)
        {
            MyName_Text2.text = r["data"].SelectToken("profile").First.SelectToken("name").ToString();
            url = r["data"].SelectToken("profile").First.SelectToken("profileImage").ToString();
            StartCoroutine(DownloadImage(url, My_Image2));
        }
        else
        {
            FriendName_Text2.text = r["data"].SelectToken("profile").First.SelectToken("name").ToString();
            url = r["data"].SelectToken("profile").First.SelectToken("profileImage").ToString();
            StartCoroutine(DownloadImage(url, Friend_Image2));
        }

        var a = r["data"].SelectToken("history");
        List<GameObject> list = new List<GameObject>();
        foreach (var item in a)
        {
            GameObject g = null;
            if (item["contents_type"].ToString().Equals("image"))
            {
                g = Instantiate(Resources.Load("History")) as GameObject;
                g.name = item["historyIdx"].ToString();
            }
            else
            {
                g = Instantiate(Resources.Load("Video")) as GameObject;
                g.name = item["historyIdx"].ToString();
                list.Add(g);
            }
            if (LM.IsMyAlbum)
            {
                g.transform.parent = MyHistory_LocMain.transform.GetChild(2).GetChild(0).GetChild(0);
            }
            else
            {
                g.transform.parent = FriendHistory_LocMain.transform.GetChild(2).GetChild(0).GetChild(0);
            }
            g.GetComponent<RectTransform>().localPosition = new Vector3(120, -50, 0);
            g.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            g.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 0, 0);
            g.transform.GetChild(0).GetComponent<Text>().text = item["datetime"].ToString()+" "+ item["day"].ToString();
            
            url = item["image"].ToString();
            StartCoroutine(DownloadImage(url, g));
            g.GetComponent<Button>().onClick.AddListener(() => OnMyHistoyLocHistoryClick(g, item));         
        }
    }

    IEnumerator PreviewHistory(int[] id)
    {
        string url = "http://3.34.20.225:3000/history/getHistory/" + id[0] + "/" + id[1] + "/1/1";
        List<IMultipartFormSection> form = new List<IMultipartFormSection>();
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();
        string result = request.downloadHandler.text;
        var r = JObject.Parse(result);
        //print(result);
        print(r["data"]);
        if (LM.IsMyAlbum)
        {
            MyName_Text.text = r["data"].SelectToken("profile").First.SelectToken("name").ToString();
            url = r["data"].SelectToken("profile").First.SelectToken("profileImage").ToString();
            StartCoroutine(DownloadImage(url, My_Image));
        }
        else
        {
            MyName_Text.text = r["data"].SelectToken("profile").First.SelectToken("name").ToString();
            url = r["data"].SelectToken("profile").First.SelectToken("profileImage").ToString();
            StartCoroutine(DownloadImage(url, My_Image));
        }
        

        var a = r["data"].SelectToken("history");
        //List<GameObject> list = new List<GameObject>();
        int check = 0;
        foreach (var item in a)
        {
            GameObject g = null;
            if (check == 3) break;
            //Debug.Log(item);
            
            if (item["contents_type"].ToString().Equals("image"))
            {
                g = Instantiate(Resources.Load("History")) as GameObject;
                g.name = item["historyIdx"].ToString();
            }
            else
            {
                g = Instantiate(Resources.Load("Video")) as GameObject;
                g.name = item["historyIdx"].ToString();
                //list.Add(g);
            }
            
            if (LM.IsMyAlbum)
            {
                g.transform.parent = MyHistoryMain.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(1).
                GetChild(0).GetChild(0).GetChild(0);
            }
            else
            {
                g.transform.parent = FriendHistoryMain.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(1).
                GetChild(0).GetChild(0).GetChild(0);
            }
            g.GetComponent<RectTransform>().localPosition = new Vector3(82, -60, 0);
            g.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            g.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 0, 0);
            g.transform.GetChild(0).GetComponent<Text>().text = item["datetime"].ToString() + " " + item["day"].ToString();

            url = item["image"].ToString();
            StartCoroutine(DownloadImage(url, g));
            g.GetComponent<Button>().onClick.AddListener(() => OnMyHistoyLocHistoryClick(g, item));
            check++;
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
    public void OnMyHistoyBackClick()
    {
        if (LM.IsMyAlbum)
        {
            MyHistoryMain.SetActive(true);
            MyHistory_LocMain.SetActive(false);
        }
        else
        {
            FriendHistoryMain.SetActive(true);
            FriendHistory_LocMain.SetActive(false);
        }
    }
    public void OnMyHistoyLocBackClick()
    {
        Clear(MyPrevContent);
        Clear(MyAllContent);
        
        if (LM.IsMyAlbum)
        {
            StartCoroutine("PreviewHistory", new int[] { 1, 1 });
            StartCoroutine("AllHistory", new int[] { 1, 1 });
            MyHistory_LocMain.SetActive(true);
            MyHistory_LocHistory.SetActive(false);
        }
        else
        {
            StartCoroutine("PreviewHistory", new int[] { 1, int.Parse(CM.UID.ToString()) });
            StartCoroutine("AllHistory", new int[] { 1, int.Parse(CM.UID.ToString()) });
            FriendHistory_LocMain.SetActive(true);
            FriendHistory_LocHistory.SetActive(false);
        }
    }
    public void OnmyLocClick()
    {
        state = 0;
        bool check = false;
        if (IsSelected1 == false)
        {
            check = true;
            IsSelected1 = true;
            Vector2 v = new Vector2(564, 145);
            StartCoroutine("LerpPos", v);
            if (LM.IsMyAlbum)
                myLocMore_Icon.SetActive(true);
            else
                FriendLocMore_Icon.SetActive(true);
        }
        if (IsSelected1 == true & check == false)
        {
            IsSelected1 = false;
            Vector2 v = new Vector2(564, 0);
            StartCoroutine("LerpPos", v);
            if (LM.IsMyAlbum)
                myLocMore_Icon.SetActive(false);
            else
                FriendLocMore_Icon.SetActive(false);
        }
    }
    public void OnmyTimeClick()
    {
        state = 1;
        bool check = false;
        if (IsSelected2 == false)
        {
            check = true;
            IsSelected2 = true;
            Vector2 v = new Vector2(564, 145);
            StartCoroutine("LerpPos", v);
        }
        if (IsSelected2 == true & check == false)
        {
            IsSelected2 = false;
            Vector2 v = new Vector2(564, 0);
            StartCoroutine("LerpPos", v);
        }
    }
    IEnumerator LerpPos(Vector2 v)
    {
        float progress = 0; //This float will serve as the 3rd parameter of the lerp function.
        float increment = smoothness / duration; //The amount of change to apply.
        while (progress < 1)
        {
            if (LM.IsMyAlbum)
            {
                if (state == 0) myLoc_Panel.sizeDelta = Vector2.Lerp(myLoc_Panel.sizeDelta, v, progress);
                else myTime_Panel.sizeDelta = Vector2.Lerp(myTime_Panel.sizeDelta, v, progress);
            }
            else
            {
                if (state == 0) FriendLoc_Panel.sizeDelta = Vector2.Lerp(FriendLoc_Panel.sizeDelta, v, progress);
                else FriendTime_Panel.sizeDelta = Vector2.Lerp(FriendLoc_Panel.sizeDelta, v, progress);
            }
            progress += increment;
            yield return new WaitForSeconds(smoothness);
        }
    }
    IEnumerator LerpPosGeneral(Vector2 v, GameObject g)
    {
        float progress = 0; //This float will serve as the 3rd parameter of the lerp function.
        float increment = smoothness / duration; //The amount of change to apply.
        while (progress < 1)
        {
            g.GetComponent<RectTransform>().localPosition = Vector2.Lerp(g.GetComponent<RectTransform>().localPosition, v, progress);
            progress += increment;
            yield return new WaitForSeconds(smoothness);
        }
    }
}
