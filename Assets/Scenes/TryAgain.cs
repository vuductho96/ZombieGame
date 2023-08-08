using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TryAgain : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void GameOver()
    {
        SceneManager.LoadScene("MainGame");
    }
        
    public void Menu()
    {
        SceneManager.LoadScene("StartGame");
    }
    public void SaveButtonState(bool isPressed)
    {
        int buttonState = isPressed ? 1 : 0;
        PlayerPrefs.SetInt("ButtonState", buttonState);
        PlayerPrefs.Save();
    }

    public bool LoadButtonState()
    {
        if (PlayerPrefs.HasKey("ButtonState"))
        {
            int buttonState = PlayerPrefs.GetInt("ButtonState");
            return buttonState == 1;
        }
        return false; // Return a default value if the key is not found
    }

}
