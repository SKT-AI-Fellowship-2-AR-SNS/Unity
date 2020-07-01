using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class ClickImage : MonoBehaviour
{
    [SerializeField]
    GameObject Image;
    [SerializeField]
    GameObject LoadPanel;

    void Awake()
    {
        Image = GameObject.Find("Image");
        LoadPanel = GameObject.Find("LoadPanel");
    }
    // Start is called before the first frame update
    void Start()
    {
        if(SceneManager.GetActiveScene().name == "MyHistory")
        {
            LoadPanel.gameObject.SetActive(false);
        }

        if (SceneManager.GetActiveScene().name == "Login"){
            Image = GameObject.Find("Image");
            LoadPanel = GameObject.Find("LoadPanel");
        }
        
    }
    public void OnClickImage()
    {
        if (SceneManager.GetActiveScene().name == "Login")
        {
            Image.GetComponent<RawImage>().texture = gameObject.GetComponent<RawImage>().texture;
            Image.GetComponent<RawImage>().color = new Color(1, 1, 1, 1);
            LoadPanel.gameObject.SetActive(false);
        }
        if(SceneManager.GetActiveScene().name == "MyHistory")
        {

            LoadPanel.gameObject.SetActive(true);

            Image.GetComponent<RawImage>().texture = gameObject.GetComponent<RawImage>().texture;
            Image.GetComponent<RawImage>().color = new Color(1, 1, 1, 1);
        }
    }
}
