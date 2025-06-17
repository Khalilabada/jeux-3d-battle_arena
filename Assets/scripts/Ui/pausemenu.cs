using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI; // Référence au menu pause
    public AudioClip pauseSound; // Son joué lors de la mise en pause
    public AudioClip resumeSound; // Son joué lors de la reprise
    public AudioClip mainMenuSound; // Son joué lors du retour au menu principal
    private AudioSource audioSource; // Composant AudioSource
    private bool isPaused = false; // État du jeu (en pause ou non)

    void Start()
    {
        
    }

    void Update()
    {
        // Vérifie si la touche Échap est pressée
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
        Cursor.lockState = CursorLockMode.None; // Libère le curseur
        Cursor.visible = true; // Rendre le curseur visible

        // Jouer le son de pause
       
    }

    public void Resume()
    {
        isPaused = false;
        Time.timeScale = 1f; // Reprend le jeu
        pauseMenuUI.SetActive(false); // Désactive l'interface du menu pause
        Cursor.lockState = CursorLockMode.Locked; // Verrouille le curseur au centre de l'écran
        Cursor.visible = false; // Cache le curseur
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f; // Reprend le temps normal avant de changer de scène
        SceneManager.LoadScene("MainMenu"); // Charge la scène du menu principal

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
