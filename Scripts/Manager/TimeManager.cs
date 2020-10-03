using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    [SerializeField]
    Text text;

    LoginManager LM;
    // Start is called before the first frame update
    void Start()
    {
        /*LM = GameObject.Find("LoginManager").GetComponent<LoginManager>();
        LM.time = DateTime.Now.ToString("yyyyMMdd");*/
        text.text = DateTime.Now.ToString("yyyy")+" / "+DateTime.Now.ToString("MM")+" / "+DateTime.Now.ToString("dd");
        text.text += "  " + GetDay(DateTime.Now);
    }
    string GetDay(DateTime dt)
    {
        string strDay = "";

        switch (dt.DayOfWeek)
        {
            case DayOfWeek.Monday:
                strDay = "MON";
                break;
            case DayOfWeek.Tuesday:
                strDay = "TUE";
                break;
            case DayOfWeek.Wednesday:
                strDay = "WED";
                break;
            case DayOfWeek.Thursday:
                strDay = "THU";
                break;
            case DayOfWeek.Friday:
                strDay = "FRI";
                break;
            case DayOfWeek.Saturday:
                strDay = "SAT";
                break;
            case DayOfWeek.Sunday:
                strDay = "SUN";
                break;
        }
        return strDay;
    }
}
