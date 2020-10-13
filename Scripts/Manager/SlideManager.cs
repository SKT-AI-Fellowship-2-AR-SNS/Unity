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
    
    public void OnMyProfileClick()
    {
        MyProfile.SetActive(true);
    }
    public void OnMyProfileBackClick()
    {
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
