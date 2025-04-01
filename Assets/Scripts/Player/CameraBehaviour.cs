using UnityEngine;
using static UnityEditor.PlayerSettings;

public class CameraBehaviour : MonoBehaviour
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            Vector3 pos = player.transform.position;

            Camera.main.transform.position = new Vector3(pos.x, pos.y, pos.z);
        }
    }
}
