using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class playerselection : MonoBehaviour
{
    public GameObject playerChracters; // Les personnages disponibles
    private GameObject[] allCharacters; // Tableau des personnages
    private int curentIndex = 0; // Index du personnage s�lectionn�
    public AudioClip backgroundMusicClip; // Musique de fond
    private AudioSource audioSource; // Composant AudioSource pour jouer la musique

    public void Start()
    {
        // Initialiser l'AudioSource et jouer la musique
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = backgroundMusicClip;
        audioSource.loop = true;
        audioSource.playOnAwake = true;
        audioSource.volume = 0.5f; // Ajuster le volume si n�cessaire
        audioSource.Play();

        // Initialiser le tableau des personnages
        allCharacters = new GameObject[playerChracters.transform.childCount];
        for (int i = 0; i < playerChracters.transform.childCount; i++)
        {
            allCharacters[i] = playerChracters.transform.GetChild(i).gameObject;
            allCharacters[i].SetActive(false);
        }

        // Charger l'index du personnage s�lectionn�
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
            character.SetActive(false); // D�sactive tous les personnages
        }
        allCharacters[curentIndex].SetActive(true); // Active le personnage s�lectionn�
    }

    public void NextCharacter()
    {
        curentIndex = (curentIndex + 1) % allCharacters.Length; // Passer au personnage suivant
        ShowCurrentCharacter();
    }

    public void PreviousCharacter()
    {
        curentIndex = (curentIndex - 1 + allCharacters.Length) % allCharacters.Length; // Passer au personnage pr�c�dent
        ShowCurrentCharacter();
    }

    public void OnYesButtonClick(string sceneName)
    {
        PlayerPrefs.SetInt("SelectedCharacterIndex", curentIndex); // Sauvegarder l'index du personnage s�lectionn�
        PlayerPrefs.Save();

        // Assigner le personnage s�lectionn� au manager global
       

        // Charger la sc�ne
        SceneManager.LoadScene(sceneName);
    }
}

// Classe statique pour g�rer la s�lection du personnage
