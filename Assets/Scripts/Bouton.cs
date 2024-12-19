// =======================================
//     Auteur: L�on Yu
//     Automne 2023, TIM
// =======================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe principale du gestion du bouton
/// Poss�de une rune selon la valeur donn� par la porte
/// </summary>
public class Bouton : MonoBehaviour
{
    // --- Champs --------------------------------------------------------------

    [SerializeField] private Sprite[] _spritesBoutons = new Sprite[2]; // Sprites des �tats de boutons
    [SerializeField] private Sprite[] _spritesCombinaison = new Sprite[6]; // Sprites de runes
    [SerializeField] private AudioClip _sonPese; // Son lorsque le joueur touche le bouton
    [SerializeField] private GameObject _rune; // L'objet rune sur la rune
    [SerializeField] private Porte _porte; // La porte qui montre la combinaison

    private SpriteRenderer _sr; //Sprite renderer du bouton
    private SpriteRenderer _srRune; //Sprite renderer de la rune
    private Collider2D _collision; //Collision du bouton, d�sactiv� lorsque le joueur p�se dessus
    private Vector3 _positionRuneinitial; // Position initial du rune
    private float _positionRuneActiveY = 0.006f; //La position de la rune en Y lorsque le bouton est activ�
    private float _CouleurNoirci = 0.7f; //Alpha du bouton lorsu'elle est pes�
    private string _numeroActuel; //En string pour pouvoir envoyer a porte plus tard

    // --- Initialisation -------------------------------------------------------

    /// <summary>
    /// Appel� d�s l'instantiation du script
    /// </summary>
    private void Awake()
    {
        _positionRuneinitial = _rune.transform.localPosition;
        _sr = GetComponent<SpriteRenderer>();
        _srRune = _rune.GetComponent<SpriteRenderer>();
        _collision = GetComponent<Collider2D>();
    }

    // --- M�thodes Priv�es -------------------------------------------------------

    /// <summary>
    /// L'interaction lorsque le joueur touche le bouton
    /// Envoie � la porte la rune du bouton
    /// </summary>
    /// <param name="other"> les triggers </param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _sr.sprite = _spritesBoutons[1];
            SoundManager.instance.JouerSon(_sonPese);
            _rune.transform.localPosition = new Vector3(0, _positionRuneActiveY);
            _srRune.color = new Color(_CouleurNoirci, _CouleurNoirci, _CouleurNoirci);
            
            _porte.AjouterCombinaison(_numeroActuel);
            
            _collision.enabled = false;
        }
    }

    // --- M�thodes Publiques -------------------------------------------------------

    /// <summary>
    /// Appel� par la porte, d�cide quelle rune sur le bouton
    /// </summary>
    /// <param name="numeroSprite"> D�cide quelle rune montr� sur le bouton </param>
    public void Melanger(int numeroSprite)
    {
        _sr.sprite = _spritesBoutons[0];
        _rune.transform.localPosition = _positionRuneinitial;
        _srRune.color = new Color(1,1,1);
        _srRune.sprite = _spritesCombinaison[numeroSprite];
        _numeroActuel = numeroSprite.ToString();
        _collision.enabled = true;
    }
}
