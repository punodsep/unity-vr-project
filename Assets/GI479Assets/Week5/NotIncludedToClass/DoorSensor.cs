using UnityEngine;
using UnityEngine.Events;

public class DoorSensor : MonoBehaviour
{
    public Transform playerTransform;
    public Animator animator;
    public float requiredDistance;

    public UnityEvent doorOpenEvent;
    public UnityEvent doorCloseEvent;

    private bool isDoorOpen;

    private void Start()
    {
        isDoorOpen = false;
    }

    private void Update()
    {
        if (!isDoorOpen && Vector3.Distance(transform.position, playerTransform.position) < requiredDistance)
        {
            animator.Play("Open");
            isDoorOpen = true;

            // if (doorOpenEvent != null)
            // {
            //     doorOpenEvent.Invoke();
            // }
            doorOpenEvent?.Invoke();
        }
        else if (isDoorOpen && Vector3.Distance(transform.position, playerTransform.position) > requiredDistance)
        {
            animator.Play("Close");
            isDoorOpen = false;
            doorCloseEvent?.Invoke();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, requiredDistance);
    }
}
