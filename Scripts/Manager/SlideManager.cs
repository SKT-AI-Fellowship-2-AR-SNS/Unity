using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideManager : MonoBehaviour
{
    [SerializeField]
    GameObject Setting;
    [SerializeField]
    GameObject Panel;
    public void OnSettingClick()
    {
        Setting.SetActive(true);
    }
    public void OnPanelClick()
    {
        Setting.SetActive(false);
    }
}
