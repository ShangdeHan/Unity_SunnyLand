using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class Menu : MonoBehaviour
{
    public GameObject pause;
    public AudioMixer mixer;
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void UI_Enable()
    {
        GameObject.Find("Canvas/MainMenu/UI").SetActive(true);
    }

    public void PauseGame()
    {
        pause.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        pause.SetActive(false);
        Time.timeScale = 1f;
    }

    public void SetVolume(float value)
    {
        mixer.SetFloat("MixerVolume", value);
    }
}
