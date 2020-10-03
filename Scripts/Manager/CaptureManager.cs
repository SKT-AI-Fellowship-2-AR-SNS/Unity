using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.Windows.WebCam;
using System.IO;
using UnityEngine.Networking;
using System.Collections.Generic;
using UnityEngine.UI;
using Newtonsoft.Json.Linq;
using UnityEditor;
using System.Text.RegularExpressions;
using System.Text;
using UnityEngine.Video;

public class CaptureManager : SingletonMonoBehaviour<CaptureManager>
{
    [SerializeField]
    GameObject HoloLensFaceDetectionExample;
    [SerializeField]
    GameObject PopUp;
    [SerializeField]
    Text Name_Text;
    [SerializeField]
    GameObject Left_Menu;
    [SerializeField]
    GameObject Right_Menu;
    [SerializeField]
    GameObject Capture_Right_Menu;
    [SerializeField]
    GameObject Video_Right_Menu;
    [SerializeField]
    GameObject Capture_Icon;
    [SerializeField]
    GameObject VideoCapture_Icon;
    [SerializeField]
    GameObject VideoCapture_Icon2;
    [SerializeField]
    GameObject CapturedImage;
    [SerializeField]
    GameObject UploadImage;
    [SerializeField]
    public GameObject Location_Text;
    [SerializeField]
    GameObject Time_Text;
    [SerializeField]
    GameObject InputField;

    PhotoCapture photoCaptureObject = null;
    VideoCapture m_VideoCapture = null;
    Texture2D targetTexture = null;
    RegisterManager rm;
    LoginManager lm;

    int cnt = 0;
    int capture_cnt = 0;
    int video_cnt = 0;
    string filename;
    public string filePath;

    public string UID;

    bool IsMyCapture = false;
    public bool camera;
    
    public void OnCameraClick()
    {
        if (HoloLensFaceDetectionExample.gameObject.activeSelf == true)
        {
            HoloLensFaceDetectionExample.GetComponent<HoloLensWithOpenCVForUnityExample.HoloLensFaceDetectionExample>()
            .OnStopButtonClick();
        }
        Right_Menu.SetActive(false);
        Capture_Right_Menu.SetActive(true);
    }
    public void OnVideoClick()
    {
        if (HoloLensFaceDetectionExample.gameObject.activeSelf == true)
        {
            HoloLensFaceDetectionExample.GetComponent<HoloLensWithOpenCVForUnityExample.HoloLensFaceDetectionExample>()
            .OnStopButtonClick();
        }
        Right_Menu.SetActive(false);
        Video_Right_Menu.SetActive(true);
    }
    public void OnCaptureClick()
    {
        camera = true;
        IsMyCapture = true;
        OnCapture();
    }
    public void OnVideoCaptureClick()
    {
        camera = false;
        //VideoCapture.CreateAsync(false, OnVideoCapture); //
        OnVideoCapture();
    }
    public void OnCaptureBackClick()
    {
        Capture_Right_Menu.SetActive(false);
        Left_Menu.SetActive(true);
        Right_Menu.SetActive(true);
        if (HoloLensFaceDetectionExample.gameObject.activeSelf == true)
        {
            HoloLensFaceDetectionExample.GetComponent<HoloLensWithOpenCVForUnityExample.HoloLensFaceDetectionExample>()
            .OnPlayButtonClick();
        }
    }
    public void OnVideoBackClick()
    {
        Video_Right_Menu.SetActive(false);
        Left_Menu.SetActive(true);
        Right_Menu.SetActive(true);
        if (HoloLensFaceDetectionExample.gameObject.activeSelf == true)
        {
            HoloLensFaceDetectionExample.GetComponent<HoloLensWithOpenCVForUnityExample.HoloLensFaceDetectionExample>()
            .OnPlayButtonClick();
        }
    }
    public void OnCaptureUploadClick()
    {
        Texture2D texture = new Texture2D(0, 0);
        byte[] byteTexture = System.IO.File.ReadAllBytes(filePath);
        if (byteTexture.Length > 0)
        {
            texture.LoadImage(byteTexture);
        }
        if (!camera)
        {
            RenderTexture rt = new RenderTexture(256, 256, 16, RenderTextureFormat.ARGB32);
            UploadImage.GetComponent<VideoPlayer>().targetTexture = rt;
            UploadImage.GetComponent<RawImage>().texture = UploadImage.GetComponent<VideoPlayer>().targetTexture;
            UploadImage.GetComponent<VideoPlayer>().url = filePath;
        }
        else
        {
            UploadImage.GetComponent<RawImage>().texture = texture;
        }  
        UploadImage.transform.GetChild(2).GetComponent<Text>().text = Location_Text.GetComponent<Text>().text;
        UploadImage.transform.GetChild(3).GetComponent<Text>().text = Time_Text.GetComponent<Text>().text;
        CapturedImage.SetActive(false);
        UploadImage.SetActive(true);
    }
    public void OnCaptureUploadClick2()
    {
        StartCoroutine(Upload());
        
    }
    IEnumerator Upload()
    {
        string url = "http://3.34.20.225:3000/history/addHistory";
        var formData = File.ReadAllBytes(filePath);
        List<IMultipartFormSection> form = new List<IMultipartFormSection>();
        if(camera)
            form.Add(new MultipartFormFileSection("content", formData, filePath, "image/jpg"));
        else
            form.Add(new MultipartFormFileSection("content", formData, filePath, "video/mp4"));
        form.Add(new MultipartFormDataSection("id", "1"));
        form.Add(new MultipartFormDataSection("text", InputField.GetComponent<InputField>().text));
        form.Add(new MultipartFormDataSection("location", Location_Text.GetComponent<Text>().text));
        UnityWebRequest www = UnityWebRequest.Post(url, form);

        yield return www.SendWebRequest();
        string result = www.downloadHandler.text;
        print(result);
        UploadImage.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<InputField>().text = "";
        UploadImage.SetActive(false);
        Left_Menu.SetActive(true);
        Right_Menu.SetActive(true);

        if (HoloLensFaceDetectionExample.gameObject.activeSelf == true)
        {
            HoloLensFaceDetectionExample.GetComponent<HoloLensWithOpenCVForUnityExample.HoloLensFaceDetectionExample>()
            .OnPlayButtonClick();
        }
    }
    public void OnCaptureDeleteClick()
    {
        CapturedImage.GetComponent<RectTransform>().sizeDelta = new Vector2(683, 384);
        CapturedImage.transform.GetChild(0).gameObject.SetActive(false);
        CapturedImage.transform.GetChild(0).GetComponent<RawImage>().color = new Color(1, 1, 1, 0);
        CapturedImage.transform.GetChild(1).gameObject.SetActive(false);
        CapturedImage.transform.GetChild(1).GetComponent<RawImage>().color = new Color(1, 1, 1, 0);
        CapturedImage.transform.GetChild(2).gameObject.SetActive(false);
        CapturedImage.transform.GetChild(3).gameObject.SetActive(false);
        CapturedImage.transform.GetChild(4).gameObject.SetActive(false);
        if (camera)
            Capture_Right_Menu.SetActive(true);
        else
        {
            Video_Right_Menu.SetActive(true);
            VideoCapture_Icon.SetActive(true);
            VideoCapture_Icon2.SetActive(false);
        }
            
        CapturedImage.SetActive(false);
    }
    public void OnUploadImageBack()
    {
        if (camera)
        {
            UploadImage.SetActive(false);
            Capture_Right_Menu.SetActive(true);
        }
        else
        {
            UploadImage.SetActive(false);
            VideoCapture_Icon.SetActive(true);
            VideoCapture_Icon2.SetActive(false);
            Video_Right_Menu.SetActive(true);
        }
    }
    public void OnUploadVideoBack()
    {
        UploadImage.SetActive(false);
        VideoCapture_Icon.SetActive(true);
        VideoCapture_Icon2.SetActive(false);
        Video_Right_Menu.SetActive(true);
    }
    // Use this for initialization
    public void OnCapture()
    {
        if (!IsMyCapture)
        {
            HoloLensFaceDetectionExample.GetComponent<HoloLensWithOpenCVForUnityExample.HoloLensFaceDetectionExample>()
            .IsCapturing = true;
            HoloLensFaceDetectionExample.GetComponent<HoloLensWithOpenCVForUnityExample.HoloLensFaceDetectionExample>()
                .OnStopButtonClick();
        }

        Resolution cameraResolution = PhotoCapture.SupportedResolutions.OrderByDescending((res) => res.width * res.height).First();
        targetTexture = new Texture2D(cameraResolution.width, cameraResolution.height);

        // Create a PhotoCapture object
        PhotoCapture.CreateAsync(false, delegate (PhotoCapture captureObject) {
            photoCaptureObject = captureObject;
            CameraParameters cameraParameters = new CameraParameters();
            cameraParameters.hologramOpacity = 0.0f;
            cameraParameters.cameraResolutionWidth = cameraResolution.width;
            cameraParameters.cameraResolutionHeight = cameraResolution.height;
            cameraParameters.pixelFormat = CapturePixelFormat.BGRA32;

            if (!IsMyCapture)
            {
                filename = string.Format(@"dummy_{0}.jpg", cnt++);
                filePath = Path.Combine(Application.persistentDataPath, filename);
            }
            else
            {
                DirectoryInfo di = new DirectoryInfo(Application.persistentDataPath + "/capture");
                if (!di.Exists)
                {
                    di.Create();
                }
                filename = string.Format(@"dummy_{0}.jpg", capture_cnt++);
                filePath = Path.Combine(Application.persistentDataPath + "/capture", filename);
            }

            // Activate the camera
            photoCaptureObject.StartPhotoModeAsync(cameraParameters, delegate (PhotoCapture.PhotoCaptureResult result) {
                // Take a picture
                TakePicture();
            }); 
        });
    }
    public void OnVideoCapture()
    {
        VideoCapture_Icon.SetActive(false);
        VideoCapture_Icon2.SetActive(true);
        Resolution cameraResolution = VideoCapture.SupportedResolutions.OrderByDescending((res) => res.width * res.height).First();
        VideoCapture.CreateAsync(false, delegate (VideoCapture videoCapture) {
            if (videoCapture != null)
            {
                m_VideoCapture = videoCapture;
                float cameraFramerate = VideoCapture.GetSupportedFrameRatesForResolution(cameraResolution).OrderByDescending((fps) => fps).First();

                CameraParameters cameraParameters = new CameraParameters();
                cameraParameters.hologramOpacity = 0.0f;
                cameraParameters.frameRate = cameraFramerate;
                cameraParameters.cameraResolutionWidth = cameraResolution.width;
                cameraParameters.cameraResolutionHeight = cameraResolution.height;
                cameraParameters.pixelFormat = CapturePixelFormat.BGRA32;

                m_VideoCapture.StartVideoModeAsync(cameraParameters,
                                                    VideoCapture.AudioState.MicAudio,
                                                    OnStartedVideoCaptureMode);
            }
        });
        
    }
    void OnStartedVideoCaptureMode(VideoCapture.VideoCaptureResult result)
    {
        if (result.success)
        {
            DirectoryInfo di = new DirectoryInfo(Application.persistentDataPath + "/video");
            if (!di.Exists)
            {
                di.Create();
            }
            filename = string.Format(@"dummy_{0}.jpg", video_cnt++);
            filePath = Path.Combine(Application.persistentDataPath, filename);

            m_VideoCapture.StartRecordingAsync(filePath, OnStartedRecordingVideo);
        }
    }
    void OnStartedRecordingVideo(VideoCapture.VideoCaptureResult result)
    {
        Debug.Log("Started Recording Video!");
        // We will stop the video from recording via other input such as a timer or a tap, etc.
    }
    public void OnStopRecordingVideo()
    {
        //OnStoppedRecordingVideo();
        m_VideoCapture.StopRecordingAsync(OnStoppedRecordingVideo); //
    }
    void OnStoppedRecordingVideo(VideoCapture.VideoCaptureResult result/**/)
    {
        //OnStoppedVideoCaptureMode();
        //Debug.Log("Stopped Recording Video!");
        m_VideoCapture.StopVideoModeAsync(OnStoppedVideoCaptureMode); //
    }

    void OnStoppedVideoCaptureMode(VideoCapture.VideoCaptureResult result/**/)
    {
        m_VideoCapture.Dispose(); //
        m_VideoCapture = null; //
        VideoCapture_Icon2.SetActive(false);
        VideoCapture_Icon.SetActive(true);
        Video_Right_Menu.SetActive(false);
        CapturedImage.SetActive(true);
        RenderTexture rt = new RenderTexture(256, 256, 16, RenderTextureFormat.ARGB32);
        CapturedImage.GetComponent<VideoPlayer>().targetTexture = rt;
        CapturedImage.GetComponent<RawImage>().texture = rt;
        //filePath = "C:/Users/qotna/OneDrive/문서/카카오톡 받은 파일/KakaoTalk_20200827_155228433.mp4";
        CapturedImage.GetComponent<VideoPlayer>().url = filePath;
        
        Invoke("CaptureLerp", 1f);
    }
    public void OnTrue()
    {
        HoloLensFaceDetectionExample.GetComponent<HoloLensWithOpenCVForUnityExample.HoloLensFaceDetectionExample>()
            .OnPlayButtonClick();
    }
    public void Onfalse()
    {
        HoloLensFaceDetectionExample.GetComponent<HoloLensWithOpenCVForUnityExample.HoloLensFaceDetectionExample>()
            .OnStopButtonClick();
    }
    void TakePicture()
    {
        photoCaptureObject.TakePhotoAsync(filePath, PhotoCaptureFileOutputFormat.JPG, OnCapturedPhotoToMemory);
        
    }
    void OnCapturedPhotoToMemory(PhotoCapture.PhotoCaptureResult result)
    {
        photoCaptureObject.StopPhotoModeAsync(OnStoppedPhotoMode);
    }
    void OnStoppedPhotoMode(PhotoCapture.PhotoCaptureResult result)
    {
        if (!IsMyCapture)
        {
            lm = GameObject.Find("LoginManager").GetComponent<LoginManager>();
            StartCoroutine("Upload2", lm.token);
        }
        
        // Shutdown our photo capture resource
        photoCaptureObject.Dispose();
        photoCaptureObject = null;
        
        if (!IsMyCapture)
        {
            HoloLensFaceDetectionExample.GetComponent<HoloLensWithOpenCVForUnityExample.HoloLensFaceDetectionExample>()
            .OnPlayButtonClick();
            HoloLensFaceDetectionExample.GetComponent<HoloLensWithOpenCVForUnityExample.HoloLensFaceDetectionExample>()
                .IsCapturing = false;
        }
        else
        {
            IsMyCapture = false;
            Capture_Right_Menu.SetActive(false);
            CapturedImage.SetActive(true);
            Texture2D texture = new Texture2D(0,0);
            byte[] byteTexture = File.ReadAllBytes(filePath);
            if (byteTexture.Length > 0)
            {
                texture.LoadImage(byteTexture);
            }
            CapturedImage.GetComponent<RawImage>().texture = texture;
            Invoke("CaptureLerp", 1f);
        }
    }
    void CaptureLerp()
    {
        Vector2 v = new Vector2(228, 128);
        StartCoroutine(LerpPos(v,0));  
        Left_Menu.SetActive(false);
    }
    IEnumerator Upload2(string token)
    {
        var formData = File.ReadAllBytes(filePath);
        List<IMultipartFormSection> form = new List<IMultipartFormSection>();
        form.Add(new MultipartFormDataSection(formData));

        UnityWebRequest www = UnityWebRequest.Post("https://api.sirv.com/v2/files/upload?filename=%2F"+filename, form);
        www.uploadHandler = (UploadHandler)new UploadHandlerRaw(formData);
        www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        www.SetRequestHeader("authorization", "Bearer " + token);
        www.SetRequestHeader("Content-Type", "image/jpeg");
        yield return www.SendWebRequest();
        //Debug.Log(filename);
        ///////////////////
        string url = "https://unkidgen.sirv.com/"+filename+"?crop.type=face&info";
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();
        string result = request.downloadHandler.text;
        var r = JObject.Parse(result);
        int cnt = 0;
        try
        {
            var a = r.SelectToken("processingSettings").SelectToken("crop").SelectToken("faces").SelectToken("faces");
            foreach (var item in a)
            {
                cnt++;
            }
            //Debug.Log(cnt);
        }
        catch
        {
            //Debug.Log(cnt);
        }
        ///////////////////
        for (int i = 0; i < cnt; i++)
        {
            url = "https://unkidgen.sirv.com/"+filename+"?crop.type=face&crop.face=" + i;
            request = UnityWebRequest.Get(url);
            yield return request.SendWebRequest();
            var data = request.downloadHandler.data;

            form = new List<IMultipartFormSection>();
            form.Add(new MultipartFormFileSection("image", data, url, "image/jpg"));

            www = UnityWebRequest.Post("https://stg-va.sktnugu.com/api/v1/face/recognize", form);
            www.SetRequestHeader("app-id", "FHJEF7O455");
            www.SetRequestHeader("group-id", "SMB2NA4ND0");
            yield return www.SendWebRequest();


            result = www.downloadHandler.text;
            Debug.Log(result);
            var j = JObject.Parse(result);
            //print(j["subject_name"]);
            if (j != null)
            {
                if (j["subject_name"].ToString() != null)
                {
                    UID = j["subject_name"].ToString();
                    PopUp.SetActive(true);
                    CancelInvoke("PopUpCancel");
                    Invoke("PopUpCancel", 15);

                    string uid = j["subject_name"].ToString();
                    //lm.UIDInfo.Add()
                    url = "http://3.34.20.225:3000/main/getPersonName/" + uid;
                    form = new List<IMultipartFormSection>();

                    www = UnityWebRequest.Get(url);
                    yield return www.SendWebRequest();
                    result = www.downloadHandler.text;
                    r = JObject.Parse(result);
                    string name = r["data"].First().SelectToken("name").ToString();
                    print(name);
                    Name_Text.text = name;
                }
            }
            
        }


    }
    float duration = 0.5f;
    float smoothness = 0.02f;
    IEnumerator LerpPos(Vector2 v, int state)
    {
        float progress = 0; //This float will serve as the 3rd parameter of the lerp function.
        float increment = smoothness / duration; //The amount of change to apply.
        
        if (state == 0)
        {
            while (progress < 1)
            {
                CapturedImage.GetComponent<RectTransform>().sizeDelta = Vector2.Lerp(CapturedImage.GetComponent<RectTransform>().sizeDelta, v, progress);
                progress += increment;
                yield return new WaitForSeconds(smoothness);
            }
            CapturedImage.transform.GetChild(3).GetComponent<Text>().text = Location_Text.GetComponent<Text>().text;
            CapturedImage.transform.GetChild(4).GetComponent<Text>().text = Time_Text.GetComponent<Text>().text;
            CapturedImage.transform.GetChild(2).gameObject.SetActive(true);
            CapturedImage.transform.GetChild(3).gameObject.SetActive(true);
            CapturedImage.transform.GetChild(4).gameObject.SetActive(true);
            
            StartCoroutine(LerpPos(v, 1));
        }
        else
        {
            CapturedImage.transform.GetChild(0).gameObject.SetActive(true);
            CapturedImage.transform.GetChild(1).gameObject.SetActive(true);
            while (progress < 1)
            {
                CapturedImage.transform.GetChild(0).GetComponent<RawImage>().color = 
                    Color.Lerp(CapturedImage.transform.GetChild(1).GetComponent<RawImage>().color, new Color(1,1,1,1), progress);
                CapturedImage.transform.GetChild(1).GetComponent<RawImage>().color =
                    Color.Lerp(CapturedImage.transform.GetChild(1).GetComponent<RawImage>().color, new Color(1,1,1,1), progress);
                progress += increment;
                yield return new WaitForSeconds(smoothness);
            }
        }
    }
    void PopUpCancel()
    {
        PopUp.SetActive(false);
    }
    override protected void OnAwake()
    {
        base.OnAwake();
    }
}
