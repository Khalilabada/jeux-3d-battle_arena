using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenu; // Menu principal
    public GameObject selectCharacterAndStageMenu; // Menu de sélection des personnages et des niveaux
    public GameObject optionMenu; // Menu des options
    public GameObject controlsMenu; // Menu des contrôles

    public AudioClip buttonClickSound; // Son pour les clics sur les boutons
    public AudioClip menuOpenSound; // Son pour l'ouverture des menus
    public AudioClip backgroundMusic; // Musique de fond pour la scène

    private AudioSource audioSource; // Composant AudioSource

    void Start()
    {
        // Initialiser l'état des menus
        mainMenu.SetActive(true);
        selectCharacterAndStageMenu.SetActive(false);
        optionMenu.SetActive(false);
        controlsMenu.SetActive(false);

        // Initialiser l'AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Jouer la musique de fond en boucle
        if (backgroundMusic != null)
        {
            audioSource.clip = backgroundMusic;
            audioSource.loop = true; // Active la boucle pour la musique
            audioSource.Play();
        }
    }

    public void PlayButtonClicked()
    {
        PlaySound(menuOpenSound); // Joue le son d'ouverture de menu
        // Activer le menu de sélection des personnages et des niveaux
        mainMenu.SetActive(false);
        selectCharacterAndStageMenu.SetActive(true);
    }

    public void OptionsButtonClicked()
    {
        PlaySound(menuOpenSound); // Joue le son d'ouverture de menu
        // Activer le menu des options
        mainMenu.SetActive(false);
        optionMenu.SetActive(true);
    }

    public void ControlsButtonClicked()
    {
        PlaySound(menuOpenSound); // Joue le son d'ouverture de menu
        // Activer le menu des contrôles
        optionMenu.SetActive(false);
        controlsMenu.SetActive(true);
    }

    public void ExitButtonClicked()
    {
        PlaySound(buttonClickSound); // Joue le son de clic sur bouton
        // Quitter l'application
        Application.Quit();
        Debug.Log("Quit Game"); // Utilisé pour tester dans l'éditeur
    }

    public void BackButtonClicked()
    {
        PlaySound(buttonClickSound); // Joue le son de clic sur bouton
        // Réactiver le menu principal et désactiver les autres
        mainMenu.SetActive(true);
        selectCharacterAndStageMenu.SetActive(false);
        optionMenu.SetActive(false);
        controlsMenu.SetActive(false);
    }

    public void SelectCharacterClicked(string sceneName)
    {
        PlaySound(menuOpenSound);
        PlaySound(buttonClickSound); // Joue le son de clic sur bouton
        // Charger la scène sélectionnée pour les personnages
        SceneManager.LoadScene(sceneName);
    }

    public void SelectStageClicked(string sceneName)

    {
        PlaySound(menuOpenSound);

        PlaySound(buttonClickSound); // Joue le son de clic sur bouton
        // Charger la scène sélectionnée pour les niveaux
        Time.timeScale = 1f; // Reprendre le temps normal (utile si le jeu était en pause)
        SceneManager.LoadScene(sceneName);
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip); // Joue le clip audio une seule fois
        }
    }
}
