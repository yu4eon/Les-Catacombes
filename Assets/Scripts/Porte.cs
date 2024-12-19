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
/// Classe principale du gestion de la porte puzzle
/// </summary>
public class Porte : MonoBehaviour
{
    // --- Champs --------------------------------------------------------------

    private SpriteRenderer _sr; // Pour changer le sprite de la porte
    [SerializeField] private Sprite[] _spritesPortes = new Sprite[2]; //Les états de la porte
    [SerializeField] private Sprite[] _spritesCombinaison = new Sprite[6]; // Les différentes runes
    [SerializeField] private SpriteRenderer[] _srRunesPortes; // Pour changer les sprites des runes sur la porte
    [SerializeField] private GameObject _joueur; // Pour comparer le y avec le joueur
    [SerializeField] private GameObject[] _boutons; // Les boutons dans la salle pour la combinaison
    [SerializeField] private Levier _levier; // Le levier qui permet de soummetre la combinaison
    [SerializeField] private AudioClip _sonOuvrir; // Son de la porte qui s'ouvre

    private int _ordreLayerInitial = 0; // Order in layer par défaut
    private int _ordreLayerDessus = 150; // Order in layer lorsque le joueur est plus haut
    private Collider2D _collision; // Déactivé lorsque la porte s'ouvre
    private List<int> _numeroSpritesinitial = new List<int>() { 0, 1, 2, 3, 4, 5 }; // Liste initial des runes
    private List<int> _numeroSprites = new List<int>() { 0, 1, 2, 3, 4, 5 }; // Liste des runes
    private string _combinaisonCorrecte; // La bonne combinaison
    private string _combinaisonJoueur; // La combinaison du joueur

    // --- Initialisation -------------------------------------------------------

    /// <summary>
    /// Appelé dès l'instantiation du script
    /// </summary>
    private void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
        _collision = GetComponent<Collider2D>();
    }

    /// <summary>
    /// S'exécute avant la mise à jour du premier frame
    /// </summary>
    private void Start()
    {
        CreerCombinaison();
    }

    // --- Méthodes Privées -------------------------------------------------------

    /// <summary>
    /// S'exécute à chaque frame
    /// Check si le joueur est plus haut que la porte
    /// </summary>
    private void Update()
    {
        if (_joueur.transform.position.y > transform.position.y)
        {
            _sr.sortingOrder = _ordreLayerDessus;
        }
        else
        {
            _sr.sortingOrder = _ordreLayerInitial;
        }

    }

    /// <summary>
    /// Créer une combinaison et l'envoie dans une ordre
    /// aléatoire au boutons.
    /// </summary>
    private void CreerCombinaison()
    {
        List<int> numeroRunes = new List<int>() { 0, 1, 2, 3 };

        for (int i = 0; i < numeroRunes.Count; i++)
        {
            int r = Random.Range(0, numeroRunes.Count);
            numeroRunes.Add(numeroRunes[r]);
            numeroRunes.RemoveAt(r);
            Debug.Log(numeroRunes[r]);
        }
        for (int i = 0; i < _boutons.Length ; i++)
        {
            int n = Random.Range(0, _numeroSprites.Count);
            _boutons[numeroRunes[i]].GetComponent<Bouton>().Melanger(_numeroSprites[n]);
            _combinaisonCorrecte += _numeroSprites[n].ToString();

            _srRunesPortes[i].sprite = _spritesCombinaison[_numeroSprites[n]];


            
            _numeroSprites.RemoveAt(n);
            Debug.Log(_combinaisonCorrecte);
        }
    }


    /// <summary>
    /// Appelé si la combinaison est correcte
    /// </summary>
    private void OuvrirPorte()
    {
        SoundManager.instance.JouerSon(_sonOuvrir);
        _sr.sprite = _spritesPortes[1];
        for (int i = 0; i < _srRunesPortes.Length; i++)
        {
            _srRunesPortes[i].enabled = false;
        }
        _collision.enabled = false;
    }

    // --- Méthodes Publiques -------------------------------------------------------

    /// <summary>
    /// Compare la combinaison du joueur avec celle correcte
    /// et ouvre la porte ou créer une nouvelle combinaison
    /// </summary>
    public void ComparerCombinaison()
    {
        if (_combinaisonJoueur == _combinaisonCorrecte)
        {
            OuvrirPorte();
        }
        else
        {
            _numeroSprites.Clear();

            for (int i = 0; i < _numeroSpritesinitial.Count; i++)
            {
                _numeroSprites.Add(_numeroSpritesinitial[i]);
            }
            
            _combinaisonCorrecte = "";
            _combinaisonJoueur = "";
            CreerCombinaison();
            _levier.Activer();
        }
    }

    /// <summary>
    /// Appelé par les boutons, ajoute le numero de la rune à la combinaison
    /// du joueur
    /// </summary>
    /// <param name="combinaison"> numero en string donné par le bouton</param>
    public void AjouterCombinaison(string combinaison)
    {
        _combinaisonJoueur += combinaison;
        Debug.Log(_combinaisonJoueur);
    }
}
