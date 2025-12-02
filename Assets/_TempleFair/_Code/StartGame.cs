using UnityEngine;

public class StartGame : MonoBehaviour
{
    public Transform teleportDestination;
    public GameObject[] objectsToDestroy;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.position = teleportDestination.position;

            foreach (GameObject obj in objectsToDestroy)
            {
                if (obj != null)
                    Destroy(obj);
            }
        }
    }
}
