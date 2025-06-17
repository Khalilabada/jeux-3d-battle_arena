using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class playerselection : MonoBehaviour
{
    public GameObject playerChracters; // Les personnages disponibles
    private GameObject[] allCharacters; // Tableau des personnages
    private int curentIndex = 0; // Index du personnage sélectionné
    public AudioClip backgroundMusicClip; // Musique de fond
    private AudioSource audioSource; // Composant AudioSource pour jouer la musique

    public void Start()
    {
        // Initialiser l'AudioSource et jouer la musique
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = backgroundMusicClip;
        audioSource.loop = true;
        audioSource.playOnAwake = true;
        audioSource.volume = 0.5f; // Ajuster le volume si nécessaire
        audioSource.Play();

        // Initialiser le tableau des personnages
        allCharacters = new GameObject[playerChracters.transform.childCount];
        for (int i = 0; i < playerChracters.transform.childCount; i++)
        {
            allCharacters[i] = playerChracters.transform.GetChild(i).gameObject;
            allCharacters[i].SetActive(false);
        }

        // Charger l'index du personnage sélectionné
        if (PlayerPrefs.HasKey("SelectedCharacterIndex"))
        {
            curentIndex = PlayerPrefs.GetInt("SelectedCharacterIndex");
        }

        ShowCurrentCharacter();
    }

    void ShowCurrentCharacter()
    {
        foreach (GameObject character in allCharacters)
        {
            character.SetActive(false); // Désactive tous les personnages
        }
        allCharacters[curentIndex].SetActive(true); // Active le personnage sélectionné
    }

    public void NextCharacter()
    {
        curentIndex = (curentIndex + 1) % allCharacters.Length; // Passer au personnage suivant
        ShowCurrentCharacter();
    }

    public void PreviousCharacter()
    {
        curentIndex = (curentIndex - 1 + allCharacters.Length) % allCharacters.Length; // Passer au personnage précédent
        ShowCurrentCharacter();
    }

    public void OnYesButtonClick(string sceneName)
    {
        PlayerPrefs.SetInt("SelectedCharacterIndex", curentIndex); // Sauvegarder l'index du personnage sélectionné
        PlayerPrefs.Save();

        // Assigner le personnage sélectionné au manager global
       

        // Charger la scène
        SceneManager.LoadScene(sceneName);
    }
}

// Classe statique pour gérer la sélection du personnage
