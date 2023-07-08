using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{
    [SerializeField] private GameObject _tutorialPanel;

    public void LoadMainScene()
    {
        SceneManager.LoadScene("MainScene");
    }
    public void ShowTutorial()
    {
        _tutorialPanel.SetActive(true);
    }
    public void HideTutorial()
    {
        _tutorialPanel.SetActive(false);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
