using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TryAgain2 : MonoBehaviour
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
    public void Save()
    {

    }

}
