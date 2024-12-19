// =======================================
//     Auteur: Léon Yu
//     Automne 2023, TIM
// =======================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Utilisé pour la gestion des pièges
/// </summary>
public class Piege : MonoBehaviour
{
    // --- Champs --------------------------------------------------------------

    [SerializeField] private float _delaiDepart; // délai de départ avant que le piège sort
    [SerializeField] private float _delaiPiege = 2f; // délai du piège qui sort et qui rentre
    [SerializeField] private GameObject _joueur; // Afin de calculer la distance pour voir si on joue le clip
    [SerializeField] private AudioClip _sonPiege; // Son joué lorsque le piège sort
    private BoxCollider2D _collision; // Activé si le piège est sortie et vice versa
    private bool _estSorti; // Si le piège est sorti ou non
    private Animator _anim; // gère l'animation du piège

    // --- Initialisation -------------------------------------------------------

    /// <summary>
    /// Appelé dès l'instantiation du script
    /// </summary>
    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _collision = GetComponent<BoxCollider2D>();
        _collision.enabled = false;
    }

    /// <summary>
    /// S'exécute avant la mise à jour du premier frame
    /// </summary>
    private void Start()
    {
        
        Invoke("CommencerCoroutine", _delaiDepart);
    }

    // --- Méthodes Privées -------------------------------------------------------

    /// <summary>
    /// Utiliser pour commencer la coroutine. Je ne sais pas si on peut mettre un délai
    /// départ à une coroutine dont j'ai fait un Invoke.
    /// </summary>
    private void CommencerCoroutine()
    {
        StartCoroutine("ActiverPiege");
    }

    /// <summary>
    /// Coroutine qui sort et ressort le piège selon un délai
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
    /// Si c'est un joueur, il prend des dégats
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

    // --- Méthodes Publiques -------------------------------------------------------

    /// <summary>
    /// Active les collisions lorsque le piège est sorti
    /// Appelé par évènement d'animation pour qu'ils activent à un moment
    /// précise
    /// </summary>
    public void ActiverCollision()
    {
        _collision.enabled = true;
    }

    /// <summary>
    /// Désactive les collisions lorsque le piège est rentré
    /// Appelé par évènement d'animation pour qu'ils activent à un moment
    /// précise
    /// </summary>
    public void DesactiverCollision()
    {
        _collision.enabled = false;
    }
}
