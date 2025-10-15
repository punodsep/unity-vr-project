using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class NumpadButton : MonoBehaviour
{
    public TMP_InputField inputField;
    public Button button;
    public char padCharacter;

    void OnValidate()
    {
        button = GetComponent<Button>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        button.onClick.AddListener(AddCharacter);
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public void AddCharacter()
    {
        inputField.text += padCharacter;
    }
}
