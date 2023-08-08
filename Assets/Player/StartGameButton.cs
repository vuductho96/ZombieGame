using UnityEngine;
using UnityEngine.UI;

public class StartGameButton2 : MonoBehaviour
{
    
    public string mainGameSceneName; // Replace this with the name of your main game scene

    private void Start()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(LoadMainGameScene);
    }

    private void LoadMainGameScene()
    {

    }
}
