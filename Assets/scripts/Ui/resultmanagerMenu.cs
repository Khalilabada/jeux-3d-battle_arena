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

    private bool isGameOver = false; // Vérifie si le jeu est déjà terminé
    private bool isWinner = false;

    void Start()
    {
        // Réinitialiser les panneaux et l'état du jeu au début
        isGameOver = false;
        isWinner = false;
        Time.timeScale = 1f;
        resultPanel.SetActive(false);
        winnerPanel.SetActive(false);
        gameOverPanel.SetActive(false);

        // Réinitialiser la santé des joueurs et des adversaires au début
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

        // Vérifier si un joueur a perdu
        foreach (fightingcontroller fightingController in fightingControllers)
        {
            if (fightingController.gameObject.activeSelf && fightingController.currenthealth <= 0)
            {
                StartCoroutine(HandlePlayerDeath(fightingController));
                return;
            }
        }

        // Vérifier si tous les adversaires sont battus
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

        // Déclencher l'animation de mort
        Animator animator = player.GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetTrigger("death_animation");
        }

        // Attendre la fin de l'animation
        yield return new WaitForSeconds(6f); // Ajustez cette durée à la longueur de l'animation

        // Afficher "Game Over" après l'animation
        SetResult(false);
    }

    IEnumerator HandleAllOpponentsDefeated()
    {
        isWinner = true;

        // Déclencher les animations de mort pour les adversaires restants
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

        // Attendre une courte durée avant d'afficher le panneau "Winner"
        yield return new WaitForSeconds(6f); // Ajustez cette durée si nécessaire

        // Afficher "Winner" après l'animation
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

        resultPanel.SetActive(true); // Active le panneau principal, si nécessaire
        Time.timeScale = 0f; // Met le jeu en pause
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
