using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI; // R�f�rence au menu pause
    public AudioClip pauseSound; // Son jou� lors de la mise en pause
    public AudioClip resumeSound; // Son jou� lors de la reprise
    public AudioClip mainMenuSound; // Son jou� lors du retour au menu principal
    private AudioSource audioSource; // Composant AudioSource
    private bool isPaused = false; // �tat du jeu (en pause ou non)

    void Start()
    {
        
    }

    void Update()
    {
        // V�rifie si la touche �chap est press�e
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }
    }

    public void Pause()
    {
        isPaused = true;
        Time.timeScale = 0f; // Met le jeu en pause
        pauseMenuUI.SetActive(true); // Active l'interface du menu pause
        Cursor.lockState = CursorLockMode.None; // Lib�re le curseur
        Cursor.visible = true; // Rendre le curseur visible

        // Jouer le son de pause
       
    }

    public void Resume()
    {
        isPaused = false;
        Time.timeScale = 1f; // Reprend le jeu
        pauseMenuUI.SetActive(false); // D�sactive l'interface du menu pause
        Cursor.lockState = CursorLockMode.Locked; // Verrouille le curseur au centre de l'�cran
        Cursor.visible = false; // Cache le curseur
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f; // Reprend le temps normal avant de changer de sc�ne
        SceneManager.LoadScene("MainMenu"); // Charge la sc�ne du menu principal

        // Jouer le son du menu principal
        PlaySound(mainMenuSound);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game");
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
