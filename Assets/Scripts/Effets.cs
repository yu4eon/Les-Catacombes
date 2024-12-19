// =======================================
//     Auteur: Léon Yu
//     Automne 2023, TIM
// =======================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Utilisé pour les effets qui se détruisent
/// </summary>
public class Effets : MonoBehaviour
{

    // --- Méthodes Publiques -------------------------------------------------------

    /// <summary>
    /// Uniquement appelé par animation pour les effets
    /// afin qu'ils se détruisent à la fin de l'animation
    /// </summary>
    public void Detruire()
    {
        Destroy(gameObject);
    }


}
