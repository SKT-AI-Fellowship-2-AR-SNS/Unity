using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneManager : MonoBehaviour
{
    public void Login()
    {
        SceneManager.LoadScene("Login");
    }
    public void MainView()
    {
        SceneManager.LoadScene("MainView");
    }
    public void MyHistory()
    {
        SceneManager.LoadScene("MyHistory");
    }
}
