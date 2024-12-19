// =======================================
//     Auteur: Léon Yu
//     Automne 2023, TIM
// =======================================

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Classe principale pour la gestion des ennemis
/// Le script est sur un conteneur
/// </summary>
public class Ennemi : MonoBehaviour
{
    // --- Champs --------------------------------------------------------------

    [SerializeField] private GameObject _barre; //La barre de vie en tant que tel
    [SerializeField] private GameObject _conteneurBarre; //La barre qui contient la barre de vie
    [SerializeField] private ParticleSystem _fxVapeur; //Effet de vapeur instantié à la mort
    [SerializeField] private GameObject _ennemi; // L'ennemi en tant que tel
    [SerializeField] private AudioClip[] _sonsDegats = new AudioClip[5]; // Tableau des différents sons de dégats

    private bool _spawnParBoss = false; //Si l'ennemi à été invoqué
    private Transform _joueur; // Afin de comparer la position avec le joueur
    private float _vitesse = 3f; //Vitesse de l'ennemis
    private Vector2 _move; //Pour le mouvement de l'ennemi
    private Animator _anim; //Animateur de l'ennemi
    private float _distanceJoueur = 5f; //Distance à laquel l'ennemi commence à poursuivre
    private float _distanceJoueurMin = 0.4f; // À quel point il peut être proche du joueur
    private const float _PTS_VIES_MAX = 15f; //Points de vies initial
    private float _ptsVies; //Points de vies actuelles

    // --- Initialisation -------------------------------------------------------

    /// <summary>
    /// Appelé dès l'instantiation du script
    /// </summary>
    private void Awake()
    {
        _ptsVies = _PTS_VIES_MAX;
        _anim = _ennemi.GetComponent<Animator>();
    }

    /// <summary>
    /// S'exécute avant la mise à jour du premier frame
    /// </summary>
    private void Start()
    {
        GameObject gObjet = GameObject.FindGameObjectWithTag("Player");
        _joueur = gObjet.GetComponent<Transform>();
        GameManager.Instance.AjouterCompteur();
    }

    // --- Méthodes Privées -------------------------------------------------------

    /// <summary>
    /// S'exécute selon un temps fixte, pour les physiques.
    /// Décide le mouvement de l'ennemi et son orientation
    /// </summary>
    private void FixedUpdate()
    {
        float distance = Vector2.Distance(transform.position, _joueur.position);
        if ((distance < _distanceJoueur && distance >= _distanceJoueurMin) || _ptsVies < _PTS_VIES_MAX || _spawnParBoss)
        {


            if ((transform.position.x - _joueur.position.x) > 0)
            {
                _ennemi.transform.localScale = new Vector3(-6, 6, 6);
            }
            else
            {
                _ennemi.transform.localScale = new Vector3(6, 6, 6);
            }

            _move = Vector2.MoveTowards(transform.position, _joueur.position, _vitesse * Time.fixedDeltaTime);
            transform.position = _move;
            _anim.SetBool("Bouge", true);
        }
        else
        {
            _anim.SetBool("Bouge", false);
        }
        _conteneurBarre.transform.position = transform.position;
    }

    /// <summary>
    /// Détruit l'objet
    /// </summary>

    private void Mourir()
    {
        Instantiate(_fxVapeur, transform.position, Quaternion.identity);
        GameManager.Instance.EnleverCompteur();
        Destroy(gameObject);
    }

    /// <summary>
    /// Fait perdre de la vie au joueur
    /// </summary>
    /// <param name="other"> Les triggers</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _joueur.GetComponent<Joueur>().PerdreVie();
        }
    }

    // --- Méthodes Publiques -------------------------------------------------------

    /// <summary>
    /// Appelé lorsque les projectiles du joueur le touche.
    /// Fait perdre de la vie.
    /// </summary>
    /// <param name="degat"> Les degat que l'ennemi prend </param>
    public void ModifierVie(float degat)
    {
        _ptsVies = Mathf.Clamp(_ptsVies + degat, 0f, _PTS_VIES_MAX);
        float ratio = _ptsVies / _PTS_VIES_MAX;
        _barre.transform.localScale = new Vector3(ratio, 1, 1);

        int n = Random.Range(0, _sonsDegats.Length);
        SoundManager.instance.JouerSon(_sonsDegats[n], 0.5f);

        if (_ptsVies <= 0)
        { 
            Mourir();
        }

    }

    /// <summary>
    /// Appelé lorsque l'ennemi est invoqué par le boss.
    /// </summary>
    /// <param name="valeur"> Si l'ennemi est spawn par le boss </param>
    public void init(bool valeur)
    {
        _spawnParBoss = valeur;
    }

    

}
