using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    public Health health;
    public Image hpFill;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        hpFill.fillAmount = health.currentHealth / health.maxHealth;
    }
}
