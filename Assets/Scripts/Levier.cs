// =======================================
//     Auteur: L�on Yu
//     Automne 2023, TIM
// =======================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe principale pour la gestion du levier, qui
/// appelle � la suite � la porte puzzle de comparer.
/// </summary>
public class Levier : MonoBehaviour
{
    // --- Champs --------------------------------------------------------------

    [SerializeField] private Porte _porte; // La porte puzzle
    [SerializeField] private GameObject _prompt; // Le prompt qui apparait lorsque le joueur est assez proche
    [SerializeField] private AudioClip _sonPull; // Son jou� lorsque le joueur int�ragit avec le levier
    private bool _estCliquable; // V�rifie si le joueur peut int�ragir avec le levier
    private Animator _anim; // G�re l'animation du levier


    // --- Initialisation -------------------------------------------------------

    /// <summary>
    /// Appel� d�s l'instantiation du script
    /// </summary>
    private void Awake()
    {
        _prompt.SetActive(false);
        _anim = GetComponent<Animator>();
    }

    // --- M�thodes Priv�es -------------------------------------------------------

    /// <summary>
    /// S'ex�cute � chaque frame
    /// V�rifie les int�ractions avec le joueur
    /// </summary>
    private void Update()
    {
        if (_estCliquable && Input.GetKeyDown(KeyCode.F))
        {
            // Pas s�r pourquoi, mais ca crash si cette ligne est dans la m�thode Activer.
            _porte.ComparerCombinaison();

            Activer();
        }

        if (_estCliquable)
        {
            _prompt.SetActive(true);
        }

        else
        {
            _prompt.SetActive(false);
        }
    }

    /// <summary>
    /// V�rifie si le joueur vient proche du levier
    /// et active le prompt.
    /// </summary>
    /// <param name="other"> Les triggers </param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Joueur entre");
            _estCliquable = true;
            
        }
        
    }

    /// <summary>
    /// V�rifie si le joueur quitte la zone
    /// </summary>
    /// <param name="other"> Les triggers </param>
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _estCliquable = false;
        }
    }

    // --- M�thodes Publiques -------------------------------------------------------

    /// <summary>
    /// Lorsque le joueur int�ragit avec le levier
    /// </summary>
    public void Activer()
    {
        SoundManager.instance.JouerSon(_sonPull);
        _anim.SetTrigger("Activer");
        _estCliquable = false;
    }
}
