// =======================================
//     Auteur: Léon Yu
//     Automne 2023, TIM
// =======================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Gestionnaire du jeu en général. Gère le compteur.
/// téléportaion, et emmener le joueur à la fin
/// </summary>
public class GameManager : MonoBehaviour
{
    // --- Champs --------------------------------------------------------------
    [SerializeField] private GameObject _joueur; //Permet au joueur de se teleporter aux salles
    [SerializeField] private PorteEnnemi[] _portes; //Les portes de la salle d'ennemis
    [SerializeField] private GameObject _trigger; //Le trigger dans la salle d'ennemis

    // vvv Les téléporteurs des salles différentes vvv 
    //Honnêtement, j'aurais du les mettrent dans un tableau.
    [SerializeField] private GameObject _sallePiege; 
    [SerializeField] private GameObject _sallePuzzle; 
    [SerializeField] private GameObject _salleMonstres;
    [SerializeField] private GameObject _salleBoss;

    //Je l'ai mis ici plutot que dans les portes,
    //puisqu'ils appellent la fonction Ouvrir dans
    //le Start, ce qui jouerait le son au départ de la scène
    [SerializeField] private AudioClip _sonOuvrir;

    private int _compteurEnnemi; //Compteur des ennemis dans la scène

    private static GameManager _instance; //Getter

    public static GameManager Instance
    {
        get { return _instance; } 
    }

    


    // --- Initialisation -------------------------------------------------------

    /// <summary>
    /// Appelé dès l'instantiation du script
    /// </summary>
    private void Awake()
    {
        _instance = this;
        _compteurEnnemi = 0;
    }

    /// <summary>
    /// S'exécute avant la mise à jour du premier frame
    /// </summary>
    private void Start()
    {
        SoundManager.instance.ChangerMusique(0);
        Time.timeScale = 1;
    }

    // --- Méthodes Privées -------------------------------------------------------

    /// <summary>
    /// S'exécute à chaque frame
    /// Vérifie si le joueur pèse sur les numéros et le téléporte
    /// si c'est le cas.
    /// </summary>
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            _joueur.transform.position = _sallePiege.transform.position;
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            _joueur.transform.position = _sallePuzzle.transform.position;
        }
        else if( Input.GetKeyDown(KeyCode.Alpha3))
        {
            _joueur.transform.position = _salleMonstres.transform.position;
        }

        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            _joueur.transform.position = _salleBoss.transform.position;
        }
    }

    // --- Méthodes Publiques -------------------------------------------------------

    /// <summary>
    /// Ajoute un au compteur d'ennemis
    /// </summary>
    public void AjouterCompteur()
    {
        _compteurEnnemi++;
    }

    /// <summary>
    /// Enlève un au compteur d'ennemis et check
    /// s'il reste au compteur. Ouvre la porte s'il
    /// ne reste plus d'ennemis
    /// </summary>
    public void EnleverCompteur()
    {
        _compteurEnnemi--;
        if (_compteurEnnemi ==0)
        {
            SoundManager.instance.JouerSon(_sonOuvrir);
            Destroy(_trigger); //Au cas ou le joueur tue les ennemis sans rentrer dans la salle
            for (int i = 0; i < _portes.Length; i++)
            {
                _portes[i].OuvrirPorte();
            }
        }
    }

    /// <summary>
    /// Emmène le joueur à la scène de fin
    /// </summary>
    public void AllerFin()
    {
        SoundManager.instance.ChangerMusique(2);
        SceneManager.LoadScene("Fin");
    }
}