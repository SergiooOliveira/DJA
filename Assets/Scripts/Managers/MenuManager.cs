using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
	int Width = 1920, Height = 1080;
	FullScreenMode fullScreenMode = FullScreenMode.ExclusiveFullScreen;

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

    public void OnValueChangedResolutionDropdown(int value)
    {
        switch (value)
        {
            // Resolution: 1920x1080
            case 0:
                Width = 1920;
                Height = 1080;
                //Debug.Log("Resolution: 1920x1080");
                break;
            // Resolution: 1600x900
            case 1:
                Width = 1600;
                Height = 900;
                //Debug.Log("Resolution: 1600x900");
                break;
        }
        Screen.SetResolution(Width, Height, fullScreenMode);
    }

    public void OnValueChangedFullScreenModeDropdown(int value)
	{
		switch (value)
        {
            // View: FullScreen
            case 0:
                fullScreenMode = FullScreenMode.ExclusiveFullScreen;
				break;
            // View: Window
            case 1:
                fullScreenMode = FullScreenMode.Windowed;
                break;
        }
        Screen.SetResolution(Width, Height, fullScreenMode);
    }

}
