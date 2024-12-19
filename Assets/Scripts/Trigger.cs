// =======================================
//     Auteur: Léon Yu
//     Automne 2023, TIM
// =======================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe principale du gestion des triggers qui ferment
/// les portes
/// </summary>
public class Trigger : MonoBehaviour
{
    // --- Champs --------------------------------------------------------------

    [SerializeField] private PorteEnnemi[] _portes; //Les portes que le trigger ferme
    [SerializeField] private Boss _boss; // À faire un lien si c'est le trigger de la salle boss


    // --- Méthodes Privées -------------------------------------------------------

    /// <summary>
    /// Vérifie si le joueur entre dans le trigger pour fermer les portes
    /// et s'il y a lieu, activer le boss. Il se détruit par la suite
    /// </summary>
    /// <param name="other"> Les triggers </param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            for (int i = 0; i < _portes.Length; i++)
            {
                _portes[i].FermerPorte();
            }

            if (_boss != null)
            {
                _boss.MettreActif();
                SoundManager.instance.ChangerMusique(1);
            }
            Destroy(gameObject);
        }
    }
}
