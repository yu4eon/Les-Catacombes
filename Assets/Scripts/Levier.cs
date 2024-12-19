// =======================================
//     Auteur: Léon Yu
//     Automne 2023, TIM
// =======================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe principale pour la gestion du levier, qui
/// appelle à la suite à la porte puzzle de comparer.
/// </summary>
public class Levier : MonoBehaviour
{
    // --- Champs --------------------------------------------------------------

    [SerializeField] private Porte _porte; // La porte puzzle
    [SerializeField] private GameObject _prompt; // Le prompt qui apparait lorsque le joueur est assez proche
    [SerializeField] private AudioClip _sonPull; // Son joué lorsque le joueur intéragit avec le levier
    private bool _estCliquable; // Vérifie si le joueur peut intéragir avec le levier
    private Animator _anim; // Gère l'animation du levier


    // --- Initialisation -------------------------------------------------------

    /// <summary>
    /// Appelé dès l'instantiation du script
    /// </summary>
    private void Awake()
    {
        _prompt.SetActive(false);
        _anim = GetComponent<Animator>();
    }

    // --- Méthodes Privées -------------------------------------------------------

    /// <summary>
    /// S'exécute à chaque frame
    /// Vérifie les intéractions avec le joueur
    /// </summary>
    private void Update()
    {
        if (_estCliquable && Input.GetKeyDown(KeyCode.F))
        {
            // Pas sûr pourquoi, mais ca crash si cette ligne est dans la méthode Activer.
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
    /// Vérifie si le joueur vient proche du levier
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
    /// Vérifie si le joueur quitte la zone
    /// </summary>
    /// <param name="other"> Les triggers </param>
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _estCliquable = false;
        }
    }

    // --- Méthodes Publiques -------------------------------------------------------

    /// <summary>
    /// Lorsque le joueur intéragit avec le levier
    /// </summary>
    public void Activer()
    {
        SoundManager.instance.JouerSon(_sonPull);
        _anim.SetTrigger("Activer");
        _estCliquable = false;
    }
}
