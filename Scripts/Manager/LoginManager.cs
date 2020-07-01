using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginManager : DontDestroy<LoginManager>
{
    public void OnKakaoLogin()
    {
        string url = "https://kauth.kakao.com/oauth/authorize?" +
            "client_id=edfe37094c314b31e73b9a24820274a7" +
            "&redirect_uri=https://www.naver.com" +
            "&response_type=code";
        Application.OpenURL(url);
    }

    protected override void OnStart()
    {
        base.OnStart();
    }

}
