using System;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneHandler : MonoBehaviour
{
    private const int MENU_SCENE_INDEX = 0;
    private const int GAME_SCENE_INDEX = 1;

    #region UI Finder
    private static PauseUI[] _pauseMenu;

    private void Start() => _pauseMenu = Resources.FindObjectsOfTypeAll<PauseUI>();
    #endregion

    #region Loaders
    public void LoadGameScene()
    {
        UnFreezeTime();

        ClearSubscribers(Circle.OnPlayerDie);
        ClearSubscribers(DifficultyIncreaser.OnDifficultyIncrease);

        SceneManager.LoadScene(GAME_SCENE_INDEX);
    }

    public void LoadMenuScene() => SceneManager.LoadScene(MENU_SCENE_INDEX);
    #endregion

    #region Actions
    public static void PauseGame()
    {
        if (CanUIBeToggle())
            _pauseMenu[0].gameObject.SetActive(true);

        FreezeTime();
    }

    public static void ResumeGame()
    {
        if(CanUIBeToggle())
            _pauseMenu[0].gameObject.SetActive(false);

        UnFreezeTime();
    }

    public void ExitGame() => Application.Quit();
    #endregion

    #region Helpers
    public static void FreezeTime() => Time.timeScale = 0f;

    public static void UnFreezeTime() => Time.timeScale = 1f;

    private static bool CanUIBeToggle() => _pauseMenu.Length > 0 ? true : false;

    private void ClearSubscribers(Action action)
    {
        if (action?.GetInvocationList().Length > 0)
            foreach (Delegate @delegate in action.GetInvocationList())
                Circle.OnPlayerDie -= (Action)@delegate;
        else
            return;
    }
    #endregion
}