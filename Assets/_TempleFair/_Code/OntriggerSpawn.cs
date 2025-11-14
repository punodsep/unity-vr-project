using UnityEngine;

public class OntriggerSpawn : MonoBehaviour
{
    [Header("Trigger Settings")]
    public GameObject targetObject;

    [Header("Spawn Settings")]
    public GameObject objectToSpawn;   // Prefab
    public Transform spawnPoint;       // จุดเกิด
    public Transform parentTransform;  // Parent

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == targetObject)
        {
            Spawn();
        }
    }

    void Spawn()
    {
        if (objectToSpawn != null && spawnPoint != null)
        {
            Instantiate(objectToSpawn, spawnPoint.position, objectToSpawn.transform.rotation, parentTransform);
        }
    }
}
