using UnityEngine;

public class OnTriggerSpawn : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        SpawnData data = other.GetComponent<SpawnData>();
        if (data != null)
        {
            Spawn(data);
        }
    }

    void Spawn(SpawnData data)
    {
        Instantiate(
            data.objectToSpawn,
            data.spawnPoint.position,
            data.objectToSpawn.transform.rotation,
            data.parentTransform
        );

    }
}
