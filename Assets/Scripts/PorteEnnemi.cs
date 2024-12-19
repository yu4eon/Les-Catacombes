// =======================================
//     Auteur: Léon Yu
//     Automne 2023, TIM
// =======================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe principale du gestion de la porte de la salle
/// ennemis et celle du boss
/// </summary>
public class PorteEnnemi : MonoBehaviour
{
    // --- Champs --------------------------------------------------------------

    private SpriteRenderer _sr; // Pour changer le sprite de la porte
    [SerializeField] private Sprite[] _spritesPortes = new Sprite[2]; // Les états de portes
    [SerializeField] private GameObject _joueur; // Afin de comparer le y avec le joueur
    [SerializeField] private AudioClip _sonFerme; // Son lorsque la porte se ferme
    //Le son ouvrir se trouve dans le GameManager, pour éviter d'entendre le bruit immédiatement

    private int _ordreLayerInitial = 0; // Order in layer par défaut
    private int _ordreLayerDessus = 150; // Order in layer lorsque le joueur est plus haut
    private Collider2D _collision; // Déactivé lorsque la porte s'ouvre

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
        OuvrirPorte();
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
    /// Appelé dans la salle ennemi par le GameManager
    /// dès qu'il n'y a plus d'ennemis
    /// </summary>
    public void OuvrirPorte()
    {
        _sr.sprite = _spritesPortes[1];
        _collision.enabled = false;
    }

    /// <summary>
    /// Appelé par un trigger
    /// </summary>
    public void FermerPorte()
    {
        SoundManager.instance.JouerSon(_sonFerme);
        _sr.sprite = _spritesPortes[0];
        _collision.enabled = true;
    }
}
