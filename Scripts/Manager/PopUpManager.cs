using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpManager : MonoBehaviour
{
    [SerializeField]
    GameObject PopUp;
    // Start is called before the first frame update
    public void OnBellClick()
    {
        if (PopUp.activeSelf==true)
        {
            PopUp.SetActive(false);

        }
    }
}
