// =======================================
//     Auteur: L�on Yu
//     Automne 2023, TIM
// =======================================


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe principale du gestion du bouclier, qui
/// bloque les projectiles du joueur
/// </summary>
public class Bouclier : MonoBehaviour
{
    // --- Champs --------------------------------------------------------------

    [SerializeField] private GameObject _boss;
    private float _vitesseRotationBouclier = 50f;

    // --- M�thodes Priv�es -------------------------------------------------------

    /// <summary>
    /// S'ex�cute selon un temps fixte, pour les physiques.
    /// Permet au bouclier de tourner autour du boss
    /// </summary>
    private void FixedUpdate()
    {
        transform.position = _boss.transform.position;
        transform.Rotate(new Vector3(0, 0, -1 * _vitesseRotationBouclier * Time.fixedDeltaTime));
    }
}
