// =======================================
//     Auteur: L�on Yu
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
    [SerializeField] private Sprite[] _spritesPortes = new Sprite[2]; // Les �tats de portes
    [SerializeField] private GameObject _joueur; // Afin de comparer le y avec le joueur
    [SerializeField] private AudioClip _sonFerme; // Son lorsque la porte se ferme
    //Le son ouvrir se trouve dans le GameManager, pour �viter d'entendre le bruit imm�diatement

    private int _ordreLayerInitial = 0; // Order in layer par d�faut
    private int _ordreLayerDessus = 150; // Order in layer lorsque le joueur est plus haut
    private Collider2D _collision; // D�activ� lorsque la porte s'ouvre

    // --- Initialisation -------------------------------------------------------

    /// <summary>
    /// Appel� d�s l'instantiation du script
    /// </summary>
    private void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
        _collision = GetComponent<Collider2D>();
    }

    /// <summary>
    /// S'ex�cute avant la mise � jour du premier frame
    /// </summary>
    private void Start()
    {
        OuvrirPorte();
    }

    // --- M�thodes Priv�es -------------------------------------------------------

    /// <summary>
    /// S'ex�cute � chaque frame
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
    /// Appel� dans la salle ennemi par le GameManager
    /// d�s qu'il n'y a plus d'ennemis
    /// </summary>
    public void OuvrirPorte()
    {
        _sr.sprite = _spritesPortes[1];
        _collision.enabled = false;
    }

    /// <summary>
    /// Appel� par un trigger
    /// </summary>
    public void FermerPorte()
    {
        SoundManager.instance.JouerSon(_sonFerme);
        _sr.sprite = _spritesPortes[0];
        _collision.enabled = true;
    }
}
