using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    // Method to load a specific minigame scene
    public void LoadMinigame(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // Method to exit the game
    public void ExitGame()
    {
#if UNITY_EDITOR
        // Stops play mode in the Unity Editor
        UnityEditor.EditorApplication.isPlaying = false; 
#else
            // Closes the application in a built version
            Application.Quit(); 
#endif
    }
}