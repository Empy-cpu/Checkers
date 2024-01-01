using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TMP_InputField nameInputField;
    public TMP_InputField nameInputField2;
    public TMP_Text welcomeText;
    public Button confirmBtn;
    public GameObject settingsPanel;
    public Button SettingsBtn;

    private void Start()
    {
        UpdateWelcomeText();
    }

    private void UpdateWelcomeText()
    {
        if (string.IsNullOrEmpty(PlayerPrefs.GetString("UserName")))
        {
            nameInputField.gameObject.SetActive(true);
            settingsPanel.SetActive(false);
        }
        else
        {
            nameInputField.gameObject.SetActive(false);
            settingsPanel.SetActive(false); // Ensure settings panel is hidden on start
            string userName = PlayerPrefs.GetString("UserName");
            welcomeText.text = "Welcome, " + userName + "!";
        }
    }

     public void SaveUserName()
    {
        // Save the entered name in PlayerPrefs (persistent data)
        string userName = nameInputField.text;
        PlayerPrefs.SetString("UserName", userName);
        SetPlaceholder();
        welcomeText.text = "Welcome, " + userName + "!";
        nameInputField.gameObject.SetActive(false);
        confirmBtn.gameObject.SetActive(false);

    }

    public void OpenSettingsPanel()
    {
        settingsPanel.SetActive(true);
    }

    public void CloseSettingsPanel()
    {
        settingsPanel.SetActive(false);
    }

    public void UpdateUserName()
    {
        string newUserName = nameInputField2.text;
        PlayerPrefs.SetString("UserName", newUserName);
        UpdateWelcomeText();
        CloseSettingsPanel();
    }

    private void SetPlaceholder()
    {
        string existingName = PlayerPrefs.GetString("UserName");
        nameInputField.placeholder.GetComponent<TextMeshProUGUI>().text = existingName;
    }
}
