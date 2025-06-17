using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentManager : MonoBehaviour
{
    public GameObject[] opponentCharacters; // Tableau des personnages adverses
    public GameObject playerCharacter; // R�f�rence au joueur

    public void Start()
    {
        // Assumer que le personnage joueur est d�j� assign�
        if (opponentCharacters.Length == 0)
        {
            Debug.LogError("Les adversaires ne sont pas assign�s au OpponentManager.");
            return;
        }

        ActivateRandomOpponent();
    }

    void ActivateRandomOpponent()
    {
        // Cr�er une liste filtr�e d'adversaires qui exclut le joueur
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

        // S�lectionner un adversaire al�atoire parmi la liste filtr�e
        int randomIndex = Random.Range(0, validOpponents.Count);
        GameObject selectedOpponent = validOpponents[randomIndex];

        // Activer uniquement l'adversaire s�lectionn� et d�sactiver les autres
        foreach (GameObject opponent in opponentCharacters)
        {
            opponent.SetActive(opponent == selectedOpponent);
        }
    }
}
