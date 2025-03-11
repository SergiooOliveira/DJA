using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void StartMenu()
    {
        SceneManager.LoadScene("StartMenu");
    }

    public void StartGame()
    {
        SceneManager.LoadScene("RoomTest");
    }

    public void CallOptionsMenu()
    {
        SceneManager.LoadScene("OptionsMenu");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
