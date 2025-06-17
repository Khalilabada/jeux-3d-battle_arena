using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenu; // Menu principal
    public GameObject selectCharacterAndStageMenu; // Menu de s�lection des personnages et des niveaux
    public GameObject optionMenu; // Menu des options
    public GameObject controlsMenu; // Menu des contr�les

    public AudioClip buttonClickSound; // Son pour les clics sur les boutons
    public AudioClip menuOpenSound; // Son pour l'ouverture des menus
    public AudioClip backgroundMusic; // Musique de fond pour la sc�ne

    private AudioSource audioSource; // Composant AudioSource

    void Start()
    {
        // Initialiser l'�tat des menus
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
        // Activer le menu de s�lection des personnages et des niveaux
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
        // Activer le menu des contr�les
        optionMenu.SetActive(false);
        controlsMenu.SetActive(true);
    }

    public void ExitButtonClicked()
    {
        PlaySound(buttonClickSound); // Joue le son de clic sur bouton
        // Quitter l'application
        Application.Quit();
        Debug.Log("Quit Game"); // Utilis� pour tester dans l'�diteur
    }

    public void BackButtonClicked()
    {
        PlaySound(buttonClickSound); // Joue le son de clic sur bouton
        // R�activer le menu principal et d�sactiver les autres
        mainMenu.SetActive(true);
        selectCharacterAndStageMenu.SetActive(false);
        optionMenu.SetActive(false);
        controlsMenu.SetActive(false);
    }

    public void SelectCharacterClicked(string sceneName)
    {
        PlaySound(menuOpenSound);
        PlaySound(buttonClickSound); // Joue le son de clic sur bouton
        // Charger la sc�ne s�lectionn�e pour les personnages
        SceneManager.LoadScene(sceneName);
    }

    public void SelectStageClicked(string sceneName)

    {
        PlaySound(menuOpenSound);

        PlaySound(buttonClickSound); // Joue le son de clic sur bouton
        // Charger la sc�ne s�lectionn�e pour les niveaux
        Time.timeScale = 1f; // Reprendre le temps normal (utile si le jeu �tait en pause)
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
