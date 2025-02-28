using UnityEngine;

public class RoomBehaviour : MonoBehaviour
{
    public GameObject[] walls;  // 0 - North ; 1 - South ; 2 - East ; 3 - West
    public GameObject[] doors;

    public bool[] testStatus;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateRoom(testStatus);
    }

    private void UpdateRoom(bool[] status) // Status says if there is a door in that direction
    {
        for (int i = 0; i < status.Length; i++)
        {
            doors[i].SetActive(status[i]);
            walls[i].SetActive(!status[i]);
        }
    }
}
