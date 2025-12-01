using UnityEngine;

public class NPCTrigger : MonoBehaviour
{
    public Animator anim;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // กันไม่ให้โดนอะไรแล้วติดหมด
        {
            anim.SetTrigger("Talk");
            Debug.Log("Trigger NPC Animation!");
        }
    }
}
