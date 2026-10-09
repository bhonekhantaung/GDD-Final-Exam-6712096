
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("Game Scenes")]
    [SerializeField] private string drivingSceneName = "DrivingGame";
    [SerializeField] private string flyingSceneName = "FlyingGame";
    [SerializeField] private string sumoSceneName = "SumoGame";

    public void PlayDrivingGame()
    {
        OpenGame(drivingSceneName);
    }

    public void PlayFlyingGame()
    {
        OpenGame(flyingSceneName);
    }

    public void PlaySumoGame()
    {
        OpenGame(sumoSceneName);
    }

    private void OpenGame(string sceneName)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }

    public void ExitGame()
    {
        Debug.Log("Exiting game...");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
