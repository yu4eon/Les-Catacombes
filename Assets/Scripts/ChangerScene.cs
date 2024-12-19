// =======================================
//     Auteur: Léon Yu
//     Automne 2023, TIM
// =======================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Utilisé pour le changement de scène
/// </summary>
public class ChangerScene : MonoBehaviour
{
    // --- Champs --------------------------------------------------------------

    [SerializeField] private AudioClip _sonConfirmer; //Son de confirmation

    // --- Méthodes Publiques -------------------------------------------------------

    /// <summary>
    /// Permet de changer de scène selon le nom fourni par le bouton
    /// </summary>
    public void Aller(string nomScene)
    {
        SoundManager.instance.JouerSon(_sonConfirmer);
        
        SceneManager.LoadScene(nomScene);
    }

    public void Quitter()
    {
        Debug.Log("Quitte");
        Application.Quit();
    }
}
