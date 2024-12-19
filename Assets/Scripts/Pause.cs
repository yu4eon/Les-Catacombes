// =======================================
//     Auteur: Léon Yu
//     Automne 2023, TIM
// =======================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Utilisé pour la gestion du menu de pause
/// </summary>
public class Pause : MonoBehaviour
{
    // --- Champs --------------------------------------------------------------

    [SerializeField] private GameObject _pause; //Le panel de pause, qui est désactivé par défaut
    [SerializeField] private AudioClip _sonPause; //Son pause
    [SerializeField] private AudioClip _sonUnpause; //Son unpause
    private bool _paused; //Permet de déterminer si on a pausé le jeu ou non

    // --- Initialisation -------------------------------------------------------

    /// <summary>
    /// S'exécute avant la mise à jour du premier frame
    /// </summary>
    private void Start()
    {
        _pause.SetActive(false);
        _paused = false;
    }

    // --- Méthodes Privées -------------------------------------------------------

    /// <summary>
    /// S'exécute à chaque frame
    /// </summary>
    private void Update()
    {
        //Permet d'arrêter le jeu
        if (_paused)
        {
            Time.timeScale = 0;
        }

        //Pause le jeu
        if (Input.GetKeyDown(KeyCode.Escape) && _paused == false)
        {
            _paused = true;
            _pause.SetActive(true);
            //SoundManager.instance.JouerSon(_sonPause);
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && _paused)
        {
            Resumer();
        }
    }

    // --- Méthodes Publiques -------------------------------------------------------

    /// <summary>
    /// Appelé lorsqu'on unpause, soit par un bouton dédié
    /// ou touche de clavier
    /// </summary>
    public void Resumer() 
    {
        //SoundManager.instance.JouerSon(_sonUnpause);
        _paused = false;
        _pause.SetActive(false);
        Time.timeScale = 1;
    }

}
