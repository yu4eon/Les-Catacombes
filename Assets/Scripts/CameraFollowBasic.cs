// =======================================
//     Auteur: Léon Yu
//     Automne 2023, TIM
// =======================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe pour gèrer le mouvement de la camera qui suit le joueur
/// </summary>
public class CameraFollowBasic : MonoBehaviour
{
    // --- Champs --------------------------------------------------------------

    [SerializeField] private Transform _cible; //La position du joueur
    [SerializeField] private float _vitesse; //La vitesse à laquelle la camera bouge

    private Vector3 _startPos; //Position actuelle de la camera
    private Vector3 _endPos; //Position du joueur

    // --- Méthodes Privées -------------------------------------------------------

    /// <summary>
    /// S'exécute selon un temps fixte, pour les physiques.
    /// J'ai mis ce mouvement dans Fixed puisque sinon il y
    /// avait un peu de jitter dans les mouvement.
    /// </summary>
    private void FixedUpdate()
    {
        _startPos = transform.position;
        _endPos = _cible.position;
        _endPos.z = transform.position.z; 
        transform.position = Vector3.Lerp(_startPos, _endPos, _vitesse * Time.fixedDeltaTime);  
    }
}