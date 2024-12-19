// =======================================
//     Auteur: L�on Yu
//     Automne 2023, TIM
// =======================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Utilis� pour les effets qui se d�truisent
/// </summary>
public class Effets : MonoBehaviour
{

    // --- M�thodes Publiques -------------------------------------------------------

    /// <summary>
    /// Uniquement appel� par animation pour les effets
    /// afin qu'ils se d�truisent � la fin de l'animation
    /// </summary>
    public void Detruire()
    {
        Destroy(gameObject);
    }


}
