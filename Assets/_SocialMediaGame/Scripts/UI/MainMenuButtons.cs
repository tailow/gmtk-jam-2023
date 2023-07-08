using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FMODUnity;
using UnityEngine.UI;

public class MainMenuButtons : MonoBehaviour
{
    [SerializeField] private GameObject _tutorialPanel;
    [SerializeField] private Scrollbar volumeSlider;

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
    public void VolumeSliderOnChange()
    {
        Debug.Log(volumeSlider.value);
        FMODUnity.RuntimeManager.GetBus("bus:/").setVolume(volumeSlider.value);
    }
}
