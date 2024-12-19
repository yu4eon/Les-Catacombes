// =======================================
//     Auteur: Léon Yu
//     Automne 2023, TIM
// =======================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe principale du gestion de la potion récoltable
/// </summary>
public class Potion : MonoBehaviour
{
    // --- Champs --------------------------------------------------------------

    [SerializeField] private AudioClip _sonPickup; // Son joué lorsque le joueur touche la potion

    // --- Méthodes Privées -------------------------------------------------------

    /// <summary>
    /// Vérifie si le joueur entre en collision avec la potion
    /// <param name="other"> Les triggers </param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SoundManager.instance.JouerSon(_sonPickup);
            other.GetComponent<Joueur>().ObtenirPotion();
            Destroy(gameObject);
        }
    }
}
