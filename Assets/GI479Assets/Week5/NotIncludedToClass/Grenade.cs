using UnityEngine;

public class Grenade : MonoBehaviour
{
    public float fuseTime;

    public GameObject explosionEffect;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartFuseCountdown()
    {
        Invoke("TriggerGrenade", fuseTime);
    }

    private void TriggerGrenade()
    {
        Instantiate(explosionEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
