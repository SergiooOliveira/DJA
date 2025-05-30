using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
	int Width = 1920, Height = 1080;
	FullScreenMode fullScreenMode = FullScreenMode.ExclusiveFullScreen;
    [SerializeField] private GameObject loadingPrefab;

    private GameObject loadingPanel;

	void Awake()
    {
        loadingPanel = Instantiate(loadingPrefab, transform);
	}

    public async void StartMenu()
    {
        await SceneChanger("StartMenu");
    }

    public async void StartGame()
    {
        await SceneChanger("RoomTest");
    }

    public async void CallOptionsMenu()
    {
        await SceneChanger("OptionsMenu");
    }

    public void ExitGame()
	{
		Application.Quit();
	}

    private async Task SceneChanger(string scene)
    {
        Animator anim = loadingPanel.GetComponent<Animator>();

        // Play the loading animation
        anim.Play("LoadingAnimation");

        // Wait until the state is actually active
        while (!anim.GetCurrentAnimatorStateInfo(0).IsName("LoadingAnimation"))
            await Task.Yield();

        // Wait until animation has completed one full cycle
        while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
            await Task.Yield();

        // Now change the scene
        SceneManager.LoadScene(scene);
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
