// =======================================
//     Auteur: L�on Yu
//     Automne 2023, TIM
// =======================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Utilis� pour le changement de sc�ne
/// </summary>
public class ChangerScene : MonoBehaviour
{
    // --- Champs --------------------------------------------------------------

    [SerializeField] private AudioClip _sonConfirmer; //Son de confirmation

    // --- M�thodes Publiques -------------------------------------------------------

    /// <summary>
    /// Permet de changer de sc�ne selon le nom fourni par le bouton
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
