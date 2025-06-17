using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultManagerMenu : MonoBehaviour
{
    public GameObject resultPanel; // Panneau de base (facultatif)
    public GameObject winnerPanel; // Panneau pour afficher "Winner"
    public GameObject gameOverPanel; // Panneau pour afficher "Game Over"
    public fightingcontroller[] fightingControllers; // Liste des joueurs
    public OpponentIA[] opponentIAs; // Liste des adversaires

    private bool isGameOver = false; // V�rifie si le jeu est d�j� termin�
    private bool isWinner = false;

    void Start()
    {
        // R�initialiser les panneaux et l'�tat du jeu au d�but
        isGameOver = false;
        isWinner = false;
        Time.timeScale = 1f;
        resultPanel.SetActive(false);
        winnerPanel.SetActive(false);
        gameOverPanel.SetActive(false);

        // R�initialiser la sant� des joueurs et des adversaires au d�but
        foreach (fightingcontroller fightingController in fightingControllers)
        {
            fightingController.currenthealth = 100; // Exemple de valeur initiale
            fightingController.gameObject.SetActive(true);
        }

        foreach (OpponentIA opponentIA in opponentIAs)
        {
            opponentIA.currenthealth = 100; // Exemple de valeur initiale
            opponentIA.gameObject.SetActive(true);
        }
    }

    void Update()
    {
        if (isGameOver || isWinner) return;

        // V�rifier si un joueur a perdu
        foreach (fightingcontroller fightingController in fightingControllers)
        {
            if (fightingController.gameObject.activeSelf && fightingController.currenthealth <= 0)
            {
                StartCoroutine(HandlePlayerDeath(fightingController));
                return;
            }
        }

        // V�rifier si tous les adversaires sont battus
        bool allOpponentsDefeated = true;
        foreach (OpponentIA opponentIA in opponentIAs)
        {
            if (opponentIA.gameObject.activeSelf && opponentIA.currenthealth > 0)
            {
                allOpponentsDefeated = false;
                break;
            }
        }

        if (allOpponentsDefeated)
        {
            StartCoroutine(HandleAllOpponentsDefeated());
        }
    }

    IEnumerator HandlePlayerDeath(fightingcontroller player)
    {
        isGameOver = true;

        // D�clencher l'animation de mort
        Animator animator = player.GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetTrigger("death_animation");
        }

        // Attendre la fin de l'animation
        yield return new WaitForSeconds(6f); // Ajustez cette dur�e � la longueur de l'animation

        // Afficher "Game Over" apr�s l'animation
        SetResult(false);
    }

    IEnumerator HandleAllOpponentsDefeated()
    {
        isWinner = true;

        // D�clencher les animations de mort pour les adversaires restants
        foreach (OpponentIA opponentIA in opponentIAs)
        {
            if (opponentIA.currenthealth <= 0)
            {
                Animator animator = opponentIA.GetComponent<Animator>();
                if (animator != null)
                {
                    animator.SetTrigger("death_animation");
                }
            }
        }

        // Attendre une courte dur�e avant d'afficher le panneau "Winner"
        yield return new WaitForSeconds(6f); // Ajustez cette dur�e si n�cessaire

        // Afficher "Winner" apr�s l'animation
        SetResult(true);
    }

    void SetResult(bool winner)
    {
        if (winner)
        {
            winnerPanel.SetActive(true); // Affiche le panneau "Winner"
        }
        else
        {
            gameOverPanel.SetActive(true); // Affiche le panneau "Game Over"
        }

        resultPanel.SetActive(true); // Active le panneau principal, si n�cessaire
        Time.timeScale = 0f; // Met le jeu en pause
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
