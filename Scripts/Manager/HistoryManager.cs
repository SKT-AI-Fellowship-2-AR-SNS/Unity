using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;
using UnityEngine.UI;
using UnityEngine.Video;
using OpenCVForUnity.VideoModule;
using System.IO;
//using UnityEditor.UIElements;

public class HistoryManager : MonoBehaviour
{
    [SerializeField]
    GameObject MyHistory_Panel;
    [SerializeField]
    GameObject MyProfile;
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
    GameObject MyHistoryProfile;
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
    GameObject FriendHistory_Panel;
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
    GameObject FriendHistory_Follow;
    [SerializeField]
    GameObject FollowContent;
    [SerializeField]
    GameObject FFollowContent;
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
    GameObject FriendAllContent;

    [SerializeField]
    GameObject MyTagContent;
    [SerializeField]
    GameObject FriendTagContent;

    [SerializeField]
    Text MyStatusText;
    [SerializeField]
    Text FriendStatusText;

    [SerializeField]
    GameObject MyHistoryEditComment;
    [SerializeField]
    GameObject FriendHistoryEditComment;

    [SerializeField]
    GameObject MyCommentInput;
    [SerializeField]
    GameObject FriendCommentInput;

    [SerializeField]
    Text MyFollowingCount;
    [SerializeField]
    Text MyFollowerCount;
    [SerializeField]
    Text FriendFollowingCount;
    [SerializeField]
    Text FriendFollowerCount;

    [SerializeField]
    GameObject MyCommentContent;
    [SerializeField]
    GameObject FriendCommentContent;

    [SerializeField]
    GameObject LoadPanel;
    [SerializeField]
    GameObject AlbumContent;
    [SerializeField]
    Text MyProfilePlaceHolder;
    [SerializeField]
    Text MyProfileText;

    [SerializeField]
    Text MyWithText;
    [SerializeField]
    Text FriendWithText;

    LoginManager LM;
    CaptureManager CM;

    float duration = 0.5f;
    float smoothness = 0.02f;
    bool IsSelected1 = false;
    bool IsSelected2 = false;
    int state;

    public int pageNum = 1;
    public int TotalPageNum;

    public string MyFollwingNum;
    public string MyFollwerNum;

    public int curId = 0;
    public string cnt;
    public bool IsWriting;
    void Awake()
    {
        LM = GameObject.Find("LoginManager").GetComponent<LoginManager>();
        LM.IsMyAlbum = true;
        CM = GameObject.Find("CaptureManager").GetComponent<CaptureManager>();
    }
    /*void Start()
    {
        //StartCoroutine("AllHistory", 1);
        //StartCoroutine("PreviewHistory", 1);
    }*/
    public void OnMyProfileStatusClick()
    {
        IsWriting = true;
    }
    public void OnMyProfileStatusUpload()
    {
        if (IsWriting)
        {
            StartCoroutine(statusUpload(MyProfileText.text));
            MyProfileText.text = "";
            IsWriting = false;
        } 
    }

    IEnumerator statusUpload(string msg)
    {
        //print(msg);
        string url = "http://54.180.5.47:3000/users/editText";
        List<IMultipartFormSection> form = new List<IMultipartFormSection>();
        //form.Add(new MultipartFormDataSection("message", msg));
        //form.Add(new MultipartFormDataSection("id", LM.UID.ToString()));
        string jsonStr = "{\n   \"id\": \"" + LM.UID.ToString() + "\",\n" +
            "\"message\": \"" + msg + "\"" +
            "}";
        var formData = System.Text.Encoding.UTF8.GetBytes(jsonStr);
        form.Add(new MultipartFormDataSection(formData));

        UnityWebRequest request = UnityWebRequest.Post(url, form);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(formData);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();
        string result = request.downloadHandler.text;

        //print(result);
        var r = JObject.Parse(result);
        if (r["status"].ToString().Equals("200"))
        {
            string tmpMSG = r["data"].SelectToken("message").ToString();
            MyProfilePlaceHolder.text = tmpMSG;
            MyHistoryMain.transform.GetChild(0).GetChild(2).GetComponent<Text>().text = tmpMSG;
        }
    }
    public void OnMyProfileImageClick()
    {
        LoadPanel.SetActive(true);
        string path = Application.persistentDataPath;
        string[] s1 = Directory.GetFiles(path);
        for (int i = 0; i < s1.Length; i++)
        {
            GameObject img = Instantiate(Resources.Load("RawImage")) as GameObject;
            img.name = i + "";
            img.transform.SetParent(AlbumContent.transform);
            img.GetComponent<RectTransform>().localPosition = new Vector3(img.transform.position.x, img.transform.position.y, 0);
            img.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            img.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 0, 0);
            img.GetComponent<Button>().onClick.AddListener(() => OnClickImage(img.name));
            //Debug.Log(s1[i]);
            LoadImage(s1[i], img);
        }
    }
    public void OnClickImage(string ImgCnt)
    {
        cnt = ImgCnt;
    }
    public void OnSelectClick()
    {
        StartCoroutine(UploadImage());
        LoadPanel.SetActive(false);
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
    IEnumerator UploadImage()
    {
        string path = Application.persistentDataPath;
        string[] s1 = Directory.GetFiles(path);

        string url = "http://54.180.5.47:3000/users/editImg";
        List<IMultipartFormSection> form = new List<IMultipartFormSection>();
        byte[] data = File.ReadAllBytes(s1[int.Parse(cnt)]);
        form.Add(new MultipartFormFileSection("img", data, s1[int.Parse(cnt)], "image/jpg"));
        form.Add(new MultipartFormDataSection("id", LM.UID.ToString()));
        UnityWebRequest request = UnityWebRequest.Post(url, form);

        yield return request.SendWebRequest();
        string result = request.downloadHandler.text;
        //print(result);//
        var r = JObject.Parse(result);
        if (r["status"].ToString().Equals("200"))
        {
            string imgURL = r["data"].SelectToken("profileImage").ToString();
            StartCoroutine(DownloadImage(imgURL, MyHistoryProfile.transform.GetChild(0).GetChild(0).gameObject));
            StartCoroutine(DownloadImage(imgURL, MyHistoryMain.transform.GetChild(0).GetChild(0).gameObject));
            StartCoroutine(DownloadImage(imgURL, MyHistory_LocMain.transform.GetChild(0).GetChild(0).gameObject));
        }

    }
    public void OnMyProfileImageBackClick()
    {
        LoadPanel.SetActive(false);
    }
    public void OnProfileClick()
    {
        MyHistoryMain.SetActive(false);
        MyHistoryProfile.SetActive(true);
    }
    public void OnProfileBackClick()
    {
        MyHistoryMain.SetActive(true);
        MyHistoryProfile.SetActive(false);
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
        string url = "http://54.180.5.47:3000/history/like/" + LM.UID + "/" + curId;
        byte[] data = null;
        UnityWebRequest request = UnityWebRequest.Put(url, data);
        yield return request.SendWebRequest();
        string result = request.downloadHandler.text;
        var r = JObject.Parse(result);
        //print(r);
        if (r.HasValues)
        {
            if (LM.IsMyAlbum)
            {
                MyHistory_LocHistory.transform.GetChild(9).GetComponent<Text>().text = r["data"].SelectToken("likes").ToString();
            }
            else
            {
                FriendHistory_LocHistory.transform.GetChild(9).GetComponent<Text>().text = r["data"].SelectToken("likes").ToString();
            }
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
        //MyHistory_LocHistory.SetActive(false);
        //MyHistory_Comment.SetActive(true);
        Vector2 v = new Vector2(0, 0);
        StartCoroutine(LerpPosGeneral(v, MyHistory_Comment));
    }
    public void OnMyHistoryCommentBackClick()
    {
        //MyHistory_LocHistory.SetActive(true);
        MyHistory_Comment.GetComponent<RectTransform>().localPosition = new Vector2(0, -384);
        //MyHistory_Comment.SetActive(false);
    }
    public void OnFriendHistoryCommentClick()
    {
        //FriendHistory_LocHistory.SetActive(false);
        //FriendHistory_Comment.SetActive(true);
        Vector2 v = new Vector2(0, 0);
        StartCoroutine(LerpPosGeneral(v, FriendHistory_Comment));
    }
    public void OnFriendHistoryCommentBackClick()
    {
        //FriendHistory_LocHistory.SetActive(true);
        FriendHistory_Comment.GetComponent<RectTransform>().localPosition = new Vector2(0, -384);
        //FriendHistory_Comment.SetActive(false);
    }
    public void OnFollowClick()
    {
        if (LM.IsMyAlbum)
        {
            if (MyHistory_Panel.activeSelf == false) MyHistory_Panel.SetActive(true);
            MyHistoryMain.SetActive(false);
            MyHistory_Follow.SetActive(true);
        }
        else
        {
            if (FriendHistory_Panel.activeSelf == false) FriendHistory_Panel.SetActive(true);
            FriendHistoryMain.SetActive(false);
            FriendHistory_Follow.SetActive(true);
        }
        
        StartCoroutine(onFollowing(0));
    }
    public void OnFollowBackClick()
    {
        if (MyProfile.activeSelf == true)
        {
            Clear(FollowContent);
            MyHistory_Follow.SetActive(false);
            MyHistoryMain.SetActive(true);
            MyHistory_Panel.SetActive(false);
            return;
        }
        if (LM.IsMyAlbum)
        {
            Clear(FollowContent);
            MyHistory_Follow.SetActive(false);
            MyHistoryMain.SetActive(true);
        }
        else
        {
            Clear(FFollowContent);
            FriendHistory_Follow.SetActive(false);
            FriendHistoryMain.SetActive(true);
        }
        
    }
    public void OnFollowLeftClick()
    {
        if (pageNum > 1)
        {
            pageNum -= 1;
            Clear(FollowContent);
            if (MyHistory_Follow.transform.GetChild(2).GetComponent<RawImage>().color.r == 151 / 255f)
                StartCoroutine(onFollowing(0));
            if (MyHistory_Follow.transform.GetChild(3).GetComponent<RawImage>().color.g == 151 / 255f)
                StartCoroutine(onFollowing(1));
            if (MyHistory_Follow.transform.GetChild(4).GetComponent<RawImage>().color.b == 151 / 255f)
                StartCoroutine(onFollowing(2));
        }
    }
    public void OnFollowRightClick()
    {
        if (TotalPageNum > pageNum)
        {
            pageNum += 1;
            Clear(FollowContent);
            if (MyHistory_Follow.transform.GetChild(2).GetComponent<RawImage>().color.r == 151 / 255f)
                StartCoroutine(onFollowing(0));
            if (MyHistory_Follow.transform.GetChild(3).GetComponent<RawImage>().color.g == 151 / 255f)
                StartCoroutine(onFollowing(1));
            if (MyHistory_Follow.transform.GetChild(4).GetComponent<RawImage>().color.b == 151 / 255f)
                StartCoroutine(onFollowing(2));
        }   
    }
    IEnumerator onFollowing(int state)
    {
        string url = "";
        if (LM.IsMyAlbum)
        {
            if (state == 0)
            {
                url = "http://54.180.5.47:3000/users/myFollowing/" + LM.UID + "?page=" + pageNum /*+ LM.UID*/;
            }
            else if (state == 1)
            {
                url = "http://54.180.5.47:3000/users/myFollower/" + LM.UID + "?page=" + pageNum /*+ LM.UID*/;
            }
            else
            {
                url = "http://54.180.5.47:3000/users/getRecommend/" + LM.UID + "?page=" + pageNum /*+ LM.UID*/;
            }
        }
        else
        {
            if (state == 0)
            {
                url = "http://54.180.5.47:3000/users/otherFollowing/" + LM.UID + "/" + CM.UID + "?page=" + pageNum /*+ LM.UID*/;
            }
            else if (state == 1)
            {
                url = "http://54.180.5.47:3000/users/otherFollower/" + LM.UID + "/" + CM.UID + "?page=" + pageNum /*+ LM.UID*/;
            }
            else
            {
                url = "http://54.180.5.47:3000/users/getRecommend/" + LM.UID + "?page=" + pageNum /*+ LM.UID*/;
            }
        }
        
        List<IMultipartFormSection> form = new List<IMultipartFormSection>();
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();
        string result = request.downloadHandler.text;
        var r = JObject.Parse(result);
        if (r.HasValues)
        {
            if (r["status"].ToString().Equals("200"))
            {
                //print(r["data"]);
                TotalPageNum = (int.Parse(r["data"].SelectToken("count").ToString()) / 7) + 1;
                foreach (var item in r["data"].SelectToken("list"))
                {
                    //print(item);
                    GameObject g = Instantiate(Resources.Load("follow")) as GameObject;
                    g.name = item.SelectToken("id").ToString();
                    StartCoroutine(DownloadImage(item.SelectToken("profileImage").ToString(), g.transform.GetChild(0).gameObject));
                    g.transform.GetChild(1).GetComponent<Text>().text = item.SelectToken("name").ToString();
                    if (item.SelectToken("isFollowing").ToString().Equals("True")) g.transform.GetChild(2).gameObject.SetActive(true);
                    else g.transform.GetChild(3).gameObject.SetActive(true);
                    if (LM.IsMyAlbum)
                        g.transform.SetParent(FollowContent.transform);
                    else
                        g.transform.SetParent(FFollowContent.transform);
                    g.GetComponent<RectTransform>().localPosition = new Vector3(g.transform.position.x, g.transform.position.y, 0);
                    g.GetComponent<RectTransform>().localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
                    g.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);

                    g.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() => OnFollow(int.Parse(item.SelectToken("id").ToString())));
                    g.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(() => OnFollow(int.Parse(item.SelectToken("id").ToString())));
                }
            }
        }
    }
    public void OnFollow(int uid)
    {
        StartCoroutine(Follow(uid));
    }
    IEnumerator Follow(int uid)
    {
        string url = "http://54.180.5.47:3000/users/" + LM.UID + "/" + uid;
        byte[] data = null;
        UnityWebRequest request = UnityWebRequest.Put(url, data);
        yield return request.SendWebRequest();
        string result = request.downloadHandler.text;
        var r = JObject.Parse(result);
        //print(r);
        if (r.HasValues)
        {
            if (r["status"].ToString().Equals("200"))
            {
                if (LM.IsMyAlbum)
                {
                    Clear(FollowContent);
                    if (MyHistory_Follow.transform.GetChild(2).GetComponent<RawImage>().color.r == 151 / 255f)
                        StartCoroutine(onFollowing(0));
                    if (MyHistory_Follow.transform.GetChild(3).GetComponent<RawImage>().color.g == 151 / 255f)
                        StartCoroutine(onFollowing(1));
                    if (MyHistory_Follow.transform.GetChild(4).GetComponent<RawImage>().color.b == 151 / 255f)
                        StartCoroutine(onFollowing(2));
                }
                else
                {
                    Clear(FFollowContent);
                    if (FriendHistory_Follow.transform.GetChild(2).GetComponent<RawImage>().color.r == 151 / 255f)
                        StartCoroutine(onFollowing(0));
                    if (FriendHistory_Follow.transform.GetChild(3).GetComponent<RawImage>().color.g == 151 / 255f)
                        StartCoroutine(onFollowing(1));
                }
                
            }
        }
    }
    public void OnFollowingTabClick()
    {
        pageNum = 1;
        if (LM.IsMyAlbum)
        {
            Clear(FollowContent);
            MyHistory_Follow.transform.GetChild(2).GetComponent<RawImage>().color = new Color(151 / 255f, 151 / 255f, 151 / 255f);
            MyHistory_Follow.transform.GetChild(3).GetComponent<RawImage>().color = new Color(102 / 255f, 101 / 255f, 100 / 255f);
            MyHistory_Follow.transform.GetChild(4).GetComponent<RawImage>().color = new Color(102 / 255f, 101 / 255f, 100 / 255f);
        }
        else
        {
            Clear(FFollowContent);
            FriendHistory_Follow.transform.GetChild(2).GetComponent<RawImage>().color = new Color(151 / 255f, 151 / 255f, 151 / 255f);
            FriendHistory_Follow.transform.GetChild(3).GetComponent<RawImage>().color = new Color(102 / 255f, 101 / 255f, 100 / 255f);
        }
        
        StartCoroutine(onFollowing(0));
        //MyHistory_Follow.transform.GetChild(1).gameObject.SetActive(true);
        
    }
    
    public void OnFollowerTabClick()
    {
        pageNum = 1;
        if (LM.IsMyAlbum)
        {
            Clear(FollowContent);
            MyHistory_Follow.transform.GetChild(2).GetComponent<RawImage>().color = new Color(102 / 255f, 101 / 255f, 100 / 255f);
            MyHistory_Follow.transform.GetChild(3).GetComponent<RawImage>().color = new Color(151 / 255f, 151 / 255f, 151 / 255f);
            MyHistory_Follow.transform.GetChild(4).GetComponent<RawImage>().color = new Color(102 / 255f, 101 / 255f, 100 / 255f);
        }
        else
        {
            Clear(FFollowContent);
            FriendHistory_Follow.transform.GetChild(2).GetComponent<RawImage>().color = new Color(102 / 255f, 101 / 255f, 100 / 255f);
            FriendHistory_Follow.transform.GetChild(3).GetComponent<RawImage>().color = new Color(151 / 255f, 151 / 255f, 151 / 255f);
        }
        
        StartCoroutine(onFollowing(1));
        //MyHistory_Follow.transform.GetChild(1).gameObject.SetActive(true);
        
    }
    public void OnRecommendTabClick()
    {
        pageNum = 1;
        Clear(FollowContent);
        StartCoroutine(onFollowing(2));
        //MyHistory_Follow.transform.GetChild(1).gameObject.SetActive(true);
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
        //print(j["historyIdx"].ToString());
        curId = int.Parse(j["historyIdx"].ToString());
        StartCoroutine(getComment(int.Parse(g.name)));
        StartCoroutine(GetHistory(1, int.Parse(j["historyIdx"].ToString())));
        //print(j["alreadyLiked"].ToString());
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
    IEnumerator GetHistory(int uid, int hitoryIdx)
    {
        string url = "http://54.180.5.47:3000/history/" + LM.UID + "/" + hitoryIdx;
        List<IMultipartFormSection> form = new List<IMultipartFormSection>();
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();
        string result = request.downloadHandler.text;
        var r = JObject.Parse(result);
        if (r.HasValues)
        {
            var a = r["data"].SelectToken("tag");
            int t = -1;
            //print(a.ToString());
            if (!a.ToString().Equals("[]"))
            {
                //print("1");
                if (LM.IsMyAlbum)
                    MyWithText.gameObject.SetActive(true);
                else
                    FriendWithText.gameObject.SetActive(true);
                t = 0;
                foreach (var i in a)
                {
                    //GameObject g = Instantiate(Resources.Load("RawImage")) as GameObject;

                    if (LM.IsMyAlbum)
                    {
                        MyTagContent.transform.GetChild(t).GetComponent<RawImage>().color = new Color(1, 1, 1);
                        StartCoroutine(DownloadImage(i.SelectToken("profileImage").ToString(), MyTagContent.transform.GetChild(t).gameObject));
                    }
                    else
                    {
                        FriendTagContent.transform.GetChild(t).GetComponent<RawImage>().color = new Color(1, 1, 1);
                        StartCoroutine(DownloadImage(i.SelectToken("profileImage").ToString(), FriendTagContent.transform.GetChild(t).gameObject));
                    }
                    t++;
                }
            }
            else
            {
                //print("2");
                if (LM.IsMyAlbum)
                    MyWithText.gameObject.SetActive(false);
                else
                    FriendWithText.gameObject.SetActive(false);
            }
            if (t != 4)
            {
                for (int i = t + 1; i < 5; i++)
                {
                    if (LM.IsMyAlbum)
                    {
                        MyTagContent.transform.GetChild(i).gameObject.GetComponent<RawImage>().color = new Color(5 / 255f, 5 / 255f, 5 / 255f);
                    }
                    else
                    {
                        FriendTagContent.transform.GetChild(i).gameObject.GetComponent<RawImage>().color = new Color(5 / 255f, 5 / 255f, 5 / 255f);
                    }
                }
            }
        }
    }
    IEnumerator getComment(int idx)
    {
        string url = "http://54.180.5.47:3000/history/getComment/" + idx;
        List<IMultipartFormSection> form = new List<IMultipartFormSection>();
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();
        string result = request.downloadHandler.text;
        var r = JObject.Parse(result);
        if (r.HasValues)
        {
            //print(r);
            var a = r["data"];
            //print(a.HasValues);
            if (a.HasValues != false)
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
                foreach (var i in a)
                {
                    GameObject g = Instantiate(Resources.Load("Comment_Panel")) as GameObject;

                    if (LM.IsMyAlbum)
                    {
                        g.transform.SetParent(MyHistoryCommentContent.transform);
                        StartCoroutine(DownloadImage(i.SelectToken("profileImage").ToString(), g.transform.GetChild(0).gameObject));
                    }
                    else
                    {
                        g.transform.SetParent(FriendHistoryCommentContent.transform);
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
                    MyHistoryCommentPanel.transform.GetChild(0).gameObject.GetComponent<RawImage>().texture = Resources.Load("default") as Texture2D;
                    MyHistoryCommentPanel.transform.GetChild(3).GetComponent<Text>().text = "";
                    MyHistoryCommentPanel.transform.GetChild(4).GetComponent<Text>().text = "첫 댓글을 입력해주세요!";
                }
                else
                {
                    FriendHistoryCommentPanel.transform.GetChild(0).gameObject.GetComponent<RawImage>().texture = Resources.Load("default") as Texture2D;
                    FriendHistoryCommentPanel.transform.GetChild(3).GetComponent<Text>().text = "";
                    FriendHistoryCommentPanel.transform.GetChild(4).GetComponent<Text>().text = "첫 댓글을 입력해주세요!";
                }
            }
        }
    }
    public void OnCommentEditClick()
    {
        if (LM.IsMyAlbum)
        {
            //MyHistory_Comment.SetActive(false);
            MyHistoryEditComment.SetActive(true);
        }
        else
        {
            //FriendHistory_Comment.SetActive(false);
            FriendHistoryEditComment.SetActive(true);
        }
        
    }
    public void OnCommentEditBackClick()
    {
        if (LM.IsMyAlbum)
        {
            //MyHistory_Comment.SetActive(true);
            MyHistoryEditComment.SetActive(false);
        }
        else
        {
            //FriendHistory_Comment.SetActive(true);
            FriendHistoryEditComment.SetActive(false);
        }
    }
    public void OnCommentEditButtonClick()
    {
        int historyIdx = curId;
        string comment = "";
        if (LM.IsMyAlbum)
        {
            comment = MyCommentInput.GetComponent<InputField>().text;
        }
        else
        {
            comment = FriendCommentInput.GetComponent<InputField>().text;
        }
            
        StartCoroutine(EditComment(historyIdx, comment));
    }
    IEnumerator EditComment(int historyIdx,string comment)
    {
        //print(historyIdx + " " + comment);
        string url = "http://54.180.5.47:3000/history/addComment";
        List<IMultipartFormSection> form = new List<IMultipartFormSection>();
        string jsonStr = "{\n   \"userIdx\": \""+"1"+"\",\n   \"historyIdx\": \""+ historyIdx.ToString()+"\",\n" +
            "\"comment\": \""+ comment+"\"\n " +
            "}";
        var formData = System.Text.Encoding.UTF8.GetBytes(jsonStr);
        form.Add(new MultipartFormDataSection(formData));
        /*form.Add(new MultipartFormDataSection("userIdx", "1"));
        form.Add(new MultipartFormDataSection("historyIdx", historyIdx.ToString()));
        form.Add(new MultipartFormDataSection("comment", comment));*/

        UnityWebRequest request = UnityWebRequest.Post(url, form);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(formData);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();

        string result = request.downloadHandler.text;
        //print(result);
        var r = JObject.Parse(result);
        if (r.HasValues)
        {
            //print(r);
            if (LM.IsMyAlbum)
            {
                Clear(MyCommentContent);
                StartCoroutine(getComment(curId));
                MyHistoryEditComment.SetActive(false);
            }
            else
            {
                Clear(FriendCommentContent);
                StartCoroutine(getComment(curId));
                FriendHistoryEditComment.SetActive(false);
            }
        }
    }
    IEnumerator AllHistory(int[] id)
    {
        string url = "http://54.180.5.47:3000/history/getHistory/" + id[0]+"/" + id[1]+"/1/1";
        List<IMultipartFormSection> form = new List<IMultipartFormSection>();
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();
        string result = request.downloadHandler.text;
        var r = JObject.Parse(result);
        if (r.HasValues)
        {
            //print(r);
            if (r.HasValues)
            {
                if (LM.IsMyAlbum)
                {
                    MyHistory_LocMain.transform.GetChild(0).GetChild(3).GetComponent<Text>().text = r["data"].SelectToken("history").First.SelectToken("location").ToString();
                    MyName_Text2.text = r["data"].SelectToken("profile").First.SelectToken("name").ToString();
                    url = r["data"].SelectToken("profile").First.SelectToken("profileImage").ToString();
                    StartCoroutine(DownloadImage(url, My_Image2));
                }
                else
                {
                    FriendHistory_LocMain.transform.GetChild(0).GetChild(3).GetComponent<Text>().text = r["data"].SelectToken("history").First.SelectToken("location").ToString();
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
                        g.transform.SetParent(MyHistory_LocMain.transform.GetChild(2).GetChild(0).GetChild(0));
                    }
                    else
                    {
                        g.transform.SetParent(FriendHistory_LocMain.transform.GetChild(2).GetChild(0).GetChild(0));
                    }
                    g.GetComponent<RectTransform>().localPosition = new Vector3(120, -50, 0);
                    g.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                    g.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 0, 0);
                    g.transform.GetChild(0).GetComponent<Text>().text = item["datetime"].ToString() + " " + item["day"].ToString();

                    url = item["image"].ToString();
                    StartCoroutine(DownloadImage(url, g));
                    g.GetComponent<Button>().onClick.AddListener(() => OnMyHistoyLocHistoryClick(g, item));
                }
            }
        }
    }

    IEnumerator PreviewHistory(int[] id)
    {
        string url = "http://54.180.5.47:3000/history/getHistory/" + id[0] + "/" + id[1] + "/1/1";
        List<IMultipartFormSection> form = new List<IMultipartFormSection>();
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();
        string result = request.downloadHandler.text;
        var r = JObject.Parse(result);
        if (r.HasValues)
        {
            //print(result);
            //print(r["data"]);
            if (LM.IsMyAlbum)
            {
                MyFollwingNum = r["data"].SelectToken("profile").First.SelectToken("followingCount").ToString();
                MyFollwerNum = r["data"].SelectToken("profile").First.SelectToken("followerCount").ToString();

                MyName_Text.text = r["data"].SelectToken("profile").First.SelectToken("name").ToString();
                MyHistoryProfile.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = MyName_Text.text;
                MyStatusText.text = r["data"].SelectToken("profile").First.SelectToken("message").ToString();
                MyProfilePlaceHolder.text = MyStatusText.text;
                MyFollowingCount.text = MyFollwingNum;
                MyFollowerCount.text = MyFollwerNum;
                url = r["data"].SelectToken("profile").First.SelectToken("profileImage").ToString();
                StartCoroutine(DownloadImage(url, My_Image));
                StartCoroutine(DownloadImage(url, MyHistoryProfile.transform.GetChild(0).GetChild(0).gameObject));
            }
            else
            {
                FriendName_Text.text = r["data"].SelectToken("profile").First.SelectToken("name").ToString();
                FriendStatusText.text = r["data"].SelectToken("profile").First.SelectToken("message").ToString();
                FriendFollowingCount.text = r["data"].SelectToken("profile").First.SelectToken("followingCount").ToString();
                FriendFollowerCount.text = r["data"].SelectToken("profile").First.SelectToken("followerCount").ToString();
                url = r["data"].SelectToken("profile").First.SelectToken("profileImage").ToString();
                StartCoroutine(DownloadImage(url, Friend_Image));
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
                    g.transform.SetParent(MyHistoryMain.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(1).
                    GetChild(0).GetChild(0).GetChild(0));
                }
                else
                {
                    g.transform.SetParent(FriendHistoryMain.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(1).
                    GetChild(0).GetChild(0).GetChild(0));
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
    }

    IEnumerator DownloadImage(string MediaUrl, GameObject img)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(MediaUrl);
        yield return request.SendWebRequest();
        if (request.isNetworkError || request.isHttpError)
        {
            //img.GetComponent<RawImage>().texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
            // Debug.Log(request.error);
        }
        else
        {
            if(img!=null)
                img.GetComponent<RawImage>().texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
        }
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
        if (LM.IsMyAlbum)
        {
            Clear(MyPrevContent);
            Clear(MyAllContent);

            ClearTag(MyTagContent);
            Clear(MyCommentContent);
            StartCoroutine("PreviewHistory", new int[] { int.Parse(LM.UID.ToString()), int.Parse(LM.UID.ToString()) });
            StartCoroutine("AllHistory", new int[] { int.Parse(LM.UID.ToString()), int.Parse(LM.UID.ToString()) });
            MyHistory_LocMain.SetActive(true);
            MyHistory_LocHistory.SetActive(false);
        }
        else
        {
            Clear(FriendPrevContent);
            Clear(FriendAllContent);

            ClearTag(FriendTagContent);
            Clear(FriendCommentContent);
            StartCoroutine("PreviewHistory", new int[] { int.Parse(LM.UID.ToString()), int.Parse(CM.UID.ToString()) });
            StartCoroutine("AllHistory", new int[] { int.Parse(LM.UID.ToString()), int.Parse(CM.UID.ToString()) });
            FriendHistory_LocMain.SetActive(true);
            FriendHistory_LocHistory.SetActive(false);
        }
    }
    void ClearTag(GameObject content)
    {
        Transform[] childList = content.GetComponentsInChildren<Transform>(true);
        if (childList != null)
        {
            for (int i = 1; i < childList.Length; i++)
            {
                if (childList[i] != transform)
                    childList[i].gameObject.GetComponent<RawImage>().color = new Color(5 / 255f, 5 / 255f, 5 / 255f);
            }
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
