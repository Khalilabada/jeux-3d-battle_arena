using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class selectstage : MonoBehaviour
{
    public AudioClip backgroundMusicClip; // Musique de fond
    private AudioSource audioSource; // Composant AudioSource pour jouer la musique
    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = backgroundMusicClip;
        audioSource.loop = true;
        audioSource.playOnAwake = true;
        audioSource.volume = 0.5f; // Ajuster le volume si nécessaire
        audioSource.Play();
    }
    public void OnselctstageButtonClick(string sceneName)
    {
        Debug.Log("Bouton cliqué, chargement de la scène : " + sceneName);
        SceneManager.LoadScene(sceneName);
    }
}
