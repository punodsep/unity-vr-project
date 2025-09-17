using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    [SerializeField]
    private Rigidbody rb;

    [SerializeField]
    private float speed = 10f;

    [SerializeField]
    private float autoDestroyTime = 15f;

    [SerializeField]
    private GameObject hitParticle;

    private void OnValidate()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        Destroy(gameObject, 15f);
        rb.linearVelocity = speed * transform.forward;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (hitParticle != null && collision.contactCount > 0)
        {
            var contact = collision.GetContact(0);
            var hitPos = contact.point;
            var hitRot = Quaternion.LookRotation(contact.normal);

            Instantiate(hitParticle, hitPos, hitRot);
            Destroy(gameObject);
        }
    }
}
