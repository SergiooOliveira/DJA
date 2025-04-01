using UnityEngine;

public class WallColision : MonoBehaviour
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, .01f);

        foreach (Collider collider in colliders)
        {
            if (collider.tag == "Wall" || collider.tag == "Door")
            {
                Destroy(gameObject);
                return;
            }
        }

        GetComponent<Collider>().enabled = true;
    }
}
