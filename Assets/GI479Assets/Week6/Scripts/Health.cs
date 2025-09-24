using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public float maxHealth = 100;

    public UnityEvent OnTakeDamage;
    public UnityEvent OnDead;

    private float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
        
    }

    public void TakeDamage(float damageAmount)
    {
        if(!IsDead())
        {
            OnTakeDamage?.Invoke();
            currentHealth -= damageAmount;

            if(IsDead())
            {
                OnDead?.Invoke();
            }
        }
    }

    public bool IsDead()
    {
        return currentHealth <= 0;
    }
}
