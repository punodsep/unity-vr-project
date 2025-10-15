using UnityEngine.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Intercom : MonoBehaviour
{
    public string correctPassword;
    public TMP_InputField inputField;
    public Button resetButton;
    public Button submitButton;
    public UnityEvent onSubmitCorrectPassword;
    public UnityEvent onSubmitIncorrectPassword;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        resetButton.onClick.AddListener(ResetPassword);
        submitButton.onClick.AddListener(SubmitPassword);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ResetPassword()
    {
        inputField.text = "";
    }

    public void SubmitPassword()
    {
        if (inputField.text == correctPassword)
        {
            Debug.Log("Correct password!");
            onSubmitCorrectPassword?.Invoke();
        }
        else
        {
            Debug.Log("Incorrect password!");
            onSubmitIncorrectPassword?.Invoke();
        }
    }
}
