// =======================================
//     Auteur: L�on Yu
//     Automne 2023, TIM
// =======================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe principale du gestion du trigger qui arrete la musique
/// et ferme la porte derri�re le joueur
/// </summary>
public class TriggerMusique : MonoBehaviour
{
    // --- Champs --------------------------------------------------------------

    [SerializeField] private PorteEnnemi _porte; // La porte qui est ferm�

    // --- M�thodes Priv�es -------------------------------------------------------

    /// <summary>
    /// V�rifie si le joueur entre dans le trigger pour arr�ter la musique
    /// et fermer la porte derri�re le joueur
    /// </summary>
    /// <param name="other"> Les triggers </param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _porte.FermerPorte();
            SoundManager.instance.ArretterMusique();
            Destroy(gameObject);
        }
    }
}
