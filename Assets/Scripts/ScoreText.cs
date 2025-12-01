using TMPro;
using UnityEngine;

public class ScoreText : MonoBehaviour
{
    public ScoreManager scoreManager;
    public TextMeshProUGUI scoreText;

    public GunController ammo;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = $"Score : {scoreManager.currentScore} \n Ammo : {ammo.currentAmmo}/{ammo.maxAmmo}";
    }
}
