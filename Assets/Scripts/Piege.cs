// =======================================
//     Auteur: L�on Yu
//     Automne 2023, TIM
// =======================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Utilis� pour la gestion des pi�ges
/// </summary>
public class Piege : MonoBehaviour
{
    // --- Champs --------------------------------------------------------------

    [SerializeField] private float _delaiDepart; // d�lai de d�part avant que le pi�ge sort
    [SerializeField] private float _delaiPiege = 2f; // d�lai du pi�ge qui sort et qui rentre
    [SerializeField] private GameObject _joueur; // Afin de calculer la distance pour voir si on joue le clip
    [SerializeField] private AudioClip _sonPiege; // Son jou� lorsque le pi�ge sort
    private BoxCollider2D _collision; // Activ� si le pi�ge est sortie et vice versa
    private bool _estSorti; // Si le pi�ge est sorti ou non
    private Animator _anim; // g�re l'animation du pi�ge

    // --- Initialisation -------------------------------------------------------

    /// <summary>
    /// Appel� d�s l'instantiation du script
    /// </summary>
    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _collision = GetComponent<BoxCollider2D>();
        _collision.enabled = false;
    }

    /// <summary>
    /// S'ex�cute avant la mise � jour du premier frame
    /// </summary>
    private void Start()
    {
        
        Invoke("CommencerCoroutine", _delaiDepart);
    }

    // --- M�thodes Priv�es -------------------------------------------------------

    /// <summary>
    /// Utiliser pour commencer la coroutine. Je ne sais pas si on peut mettre un d�lai
    /// d�part � une coroutine dont j'ai fait un Invoke.
    /// </summary>
    private void CommencerCoroutine()
    {
        StartCoroutine("ActiverPiege");
    }

    /// <summary>
    /// Coroutine qui sort et ressort le pi�ge selon un d�lai
    /// </summary>
    private IEnumerator ActiverPiege()
    {
        
        while (true)
        {
            float distance = Vector2.Distance(transform.position, _joueur.transform.position);
            if (_estSorti)
            {

                _anim.SetTrigger("Rentre");
                _estSorti = false;
            }
            else
            {
                if (distance < 15f && _sonPiege != null) SoundManager.instance.JouerSon(_sonPiege, 0.4f);

                _anim.SetTrigger("Sort");
                _estSorti=true;
                
            }

            //Debug.Log(_estSorti);
            yield return new WaitForSeconds(_delaiPiege);
        }
    }

    /// <summary>
    /// Si c'est un joueur, il prend des d�gats
    /// </summary>
    /// <param name="coll"> Les triggers </param>
    private void OnTriggerEnter2D(Collider2D coll)
    {
        
        if (coll.CompareTag("Player"))
        {
            //Debug.Log("Touche");
            coll.GetComponent<Joueur>().PerdreVie();
        }
    }

    // --- M�thodes Publiques -------------------------------------------------------

    /// <summary>
    /// Active les collisions lorsque le pi�ge est sorti
    /// Appel� par �v�nement d'animation pour qu'ils activent � un moment
    /// pr�cise
    /// </summary>
    public void ActiverCollision()
    {
        _collision.enabled = true;
    }

    /// <summary>
    /// D�sactive les collisions lorsque le pi�ge est rentr�
    /// Appel� par �v�nement d'animation pour qu'ils activent � un moment
    /// pr�cise
    /// </summary>
    public void DesactiverCollision()
    {
        _collision.enabled = false;
    }
}
