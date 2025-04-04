using UnityEngine;

public class RoomBehaviour : MonoBehaviour
{
    public GameObject[] walls;  // 0 - North ; 1 - South ; 2 - East ; 3 - West
    public GameObject[] doors;

    
    public void UpdateRoom(bool[] status) // Status says if there is a door in that direction
    {
        for (int i = 0; i < status.Length; i++)
        {
            doors[i].SetActive(status[i]);
            walls[i].SetActive(!status[i]);
        }
    }
}
