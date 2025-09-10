using UnityEngine;

namespace TheDeveloperTrain.SciFiGuns
{

    public class Bullet : MonoBehaviour
    {
        [SerializeField]
        private float speed = 10f;

        [SerializeField]
        private float autoDestroyTime = 15f;

        [SerializeField]
        private GameObject hitParticle;

        void Start()
        {
            Destroy(gameObject, 15f);
        }

        void Update()
        {
            transform.Translate(speed * Time.deltaTime * Vector3.forward, Space.Self);
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
}