// =======================================
//     Auteur: L�on Yu
//     Automne 2023, TIM
// =======================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Utilis� pour la gestion du menu de pause
/// </summary>
public class Pause : MonoBehaviour
{
    // --- Champs --------------------------------------------------------------

    [SerializeField] private GameObject _pause; //Le panel de pause, qui est d�sactiv� par d�faut
    [SerializeField] private AudioClip _sonPause; //Son pause
    [SerializeField] private AudioClip _sonUnpause; //Son unpause
    private bool _paused; //Permet de d�terminer si on a paus� le jeu ou non

    // --- Initialisation -------------------------------------------------------

    /// <summary>
    /// S'ex�cute avant la mise � jour du premier frame
    /// </summary>
    private void Start()
    {
        _pause.SetActive(false);
        _paused = false;
    }

    // --- M�thodes Priv�es -------------------------------------------------------

    /// <summary>
    /// S'ex�cute � chaque frame
    /// </summary>
    private void Update()
    {
        //Permet d'arr�ter le jeu
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

    // --- M�thodes Publiques -------------------------------------------------------

    /// <summary>
    /// Appel� lorsqu'on unpause, soit par un bouton d�di�
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
