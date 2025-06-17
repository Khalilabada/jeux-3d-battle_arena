using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentManager : MonoBehaviour
{
    public GameObject[] opponentCharacters; // Tableau des personnages adverses
    public GameObject playerCharacter; // Référence au joueur

    public void Start()
    {
        // Assumer que le personnage joueur est déjà assigné
        if (opponentCharacters.Length == 0)
        {
            Debug.LogError("Les adversaires ne sont pas assignés au OpponentManager.");
            return;
        }

        ActivateRandomOpponent();
    }

    void ActivateRandomOpponent()
    {
        // Créer une liste filtrée d'adversaires qui exclut le joueur
        List<GameObject> validOpponents = new List<GameObject>();

        foreach (GameObject opponent in opponentCharacters)
        {
            if (opponent != playerCharacter)
            {
                validOpponents.Add(opponent);
            }
        }

        if (validOpponents.Count == 0)
        {
            Debug.LogError("Aucun adversaire valide disponible.");
            return;
        }

        // Sélectionner un adversaire aléatoire parmi la liste filtrée
        int randomIndex = Random.Range(0, validOpponents.Count);
        GameObject selectedOpponent = validOpponents[randomIndex];

        // Activer uniquement l'adversaire sélectionné et désactiver les autres
        foreach (GameObject opponent in opponentCharacters)
        {
            opponent.SetActive(opponent == selectedOpponent);
        }
    }
}
