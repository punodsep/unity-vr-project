using UnityEngine;

[System.Serializable]
public class ResettableObject
{
    public GameObject prefab;
    public Transform target;
    public Health health;
}

public class ResetZone : MonoBehaviour
{
    public ResettableObject[] objectsToReset;
    public ScoreManager currentScore;
    public GunController currentAmmo;

    private Vector3[] startPositions;
    private Quaternion[] startRotations;

    private void Start()
    {
        int count = objectsToReset.Length;
        startPositions = new Vector3[count];
        startRotations = new Quaternion[count];

        for (int i = 0; i < objectsToReset.Length; i++)
        {
            var obj = objectsToReset[i].target;
            startPositions[i] = obj.position;
            startRotations[i] = obj.rotation;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        ResetAll();
    }
    public void ResetAll()
    {
        if (currentScore != null)
            currentScore.ResetScore();

        if (currentAmmo != null)
            currentAmmo.ReloadFull();

        for (int i = 0; i < objectsToReset.Length; i++)
        {
            ResetObject(i);
        }

        Debug.Log("All objects reset via destroy/instantiate!");
    }

    private void ResetObject(int index)
    {
        var obj = objectsToReset[index];

        if (obj.target != null)
            Destroy(obj.target.gameObject);

        if (obj.prefab != null)
        {
            GameObject newObj = Instantiate(obj.prefab, startPositions[index], startRotations[index]);
            obj.target = newObj.transform;
            obj.health = newObj.GetComponent<Health>();
        }
    }
}
