using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject settingsPanel;

    AudioSource audio;
    public AudioClip Chose;
    public AudioClip HoverMouseSound;

    void Start()
    {
        audio = GetComponent<AudioSource>();
        if (audio == null)
        {
            audio = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseSettingsPanel();
        }
    }

    public void StartMenu()
    {
        audio.PlayOneShot(Chose);
        SceneManager.LoadScene("LoadingScen");
    }
    public void Setting()
    {
        audio.PlayOneShot(Chose);
        settingsPanel.SetActive(true);
        

    }
    public void CloseSettingsPanel()
    {
        settingsPanel.SetActive(false);
    }
    public void QuitMenu()
    {
        audio.PlayOneShot(Chose);
        Application.Quit();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        audio.PlayOneShot(HoverMouseSound);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Stop playing the hover sound if necessary
    }
   

    public void OnSelect()
    {
        StartMenu();
    }

    public void OnCancel()
    {
        QuitMenu();
    }
}
