using UnityEngine;

public class HotSpotQuest : MonoBehaviour
{
    public GameObject imageUI;

    private void Start()
    {
        if (imageUI != null)
            imageUI.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            imageUI.SetActive(true);
        }
    }

}
