using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Singleton

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
}
