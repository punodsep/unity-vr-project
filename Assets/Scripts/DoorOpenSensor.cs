using UnityEngine;
using UnityEngine.Events;

public class DoorOpenSensor : MonoBehaviour
{
    public Transform PlayerTransform;
    public Animator DoorAnimator;
    public float Distance = 3;

    public UnityEvent OnDoorOpen;
    public UnityEvent OnDoorClose;

    private bool isDoorOpen;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isDoorOpen = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDoorOpen && Vector3.Distance(transform.position, PlayerTransform.position) < Distance)
        {
            DoorAnimator.Play("Open");
            isDoorOpen = true;

            // if (OnDoorOpen != null)
            // {
            //     OnDoorOpen.Invoke();
            // }
            OnDoorOpen?.Invoke();
        }
        else if (isDoorOpen && Vector3.Distance(transform.position, PlayerTransform.position) > Distance)
        {
            DoorAnimator.Play("Close");
            isDoorOpen = false;
            OnDoorClose?.Invoke();
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Distance);
    }
}
