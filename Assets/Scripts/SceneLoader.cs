using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private AudioSource music;

    private void Start()
    {
        music = GameObject.Find("Music").GetComponent<AudioSource>();
    }
    public void LoadGameScene()
    {
        SceneManager.LoadScene("Game");
    }
    public void LoadMenuScene()
    {
        if (Time.timeScale == 0f)
        {
            this.gameObject.SetActive(false);
            Time.timeScale = 1f; // Se reanuda el juego
            music.UnPause();// Se reanuda la música
        }
        SceneManager.LoadScene("MainTitle");
    }
    public void ResumeGame()
    {
        this.gameObject.SetActive(false);
        Time.timeScale = 1f; // Se reanuda el juego
        music.UnPause();// Se reanuda la música
    }
    public void ExitGame()
    {
        //UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }
}