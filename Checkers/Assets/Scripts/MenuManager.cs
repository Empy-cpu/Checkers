using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;



    public class MenuManager : MonoBehaviour
    {
        [SerializeField] private GameObject nameInputPanel;
        [SerializeField] private Button confirmBtn;
        [SerializeField] private TMP_InputField settingsNameInputField;
        [SerializeField] private Slider volumeSlider;
        [SerializeField] private TMP_Text welcomeText;
        [SerializeField] private TMP_Text placeHolderText;
        [SerializeField] private GameObject winPanel;
        [SerializeField] private TMP_Text winText;  


    private const string usernameKey = "username";
        private const string volumeKey = "volume";

        private void Start()
        {
            // Open name input panel if first time
            if (!PlayerPrefs.HasKey(usernameKey))
                nameInputPanel.SetActive(true);

            RefreshSettings();
        }

        // Update name and slider values
        private void RefreshSettings()
        {
            string username = PlayerPrefs.GetString(usernameKey, "");
            if (!string.IsNullOrEmpty(username))
            {
                welcomeText.text = "Welcome, " + username + "!";
            }

            settingsNameInputField.text = username;
            placeHolderText.text = username;
            volumeSlider.value = PlayerPrefs.GetFloat(volumeKey, 1);
           
        }

        // Called from OK button in name input panel or settings panel
        public void SetUsername(TMP_InputField inputField)
        {
            string newUsername = inputField.text;
            if (!string.IsNullOrEmpty(newUsername))
            {
                PlayerPrefs.SetString(usernameKey, newUsername);
                RefreshSettings();
                 Debug.Log("username set");
                 settingsNameInputField.gameObject.SetActive(false);
                confirmBtn.gameObject.SetActive(false);
             }
        }

        // Dynamic float function called from volume slider
        public void SetVolume(float vol)
        {
            Debug.Log("VOLUME CHANGED");
            AudioListener.volume = vol;
            PlayerPrefs.SetFloat(volumeKey, vol);
            RefreshSettings();
        }

    public void WinPanel(int isPlayer)
    {
        winPanel.SetActive(true);
        winText.text = "Player " +isPlayer + "Wins";
    }

    // Called from multiplayer button
    public void StartGame()
        {
        // SceneManager.LoadScene(1);
          nameInputPanel.SetActive(false);
        }
    }