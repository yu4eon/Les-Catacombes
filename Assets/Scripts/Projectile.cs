// =======================================
//     Auteur: L�on Yu
//     Automne 2023, TIM
// =======================================

using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

/// <summary>
/// Classe principale du gestion des projectiles du joueur
/// </summary>
public class Projectile : MonoBehaviour
{
    // --- Champs --------------------------------------------------------------

    [SerializeField] private ParticleSystem _fxFleche; // Effet lorsque le projectile touche un obstacles
    [SerializeField] private ParticleSystem _fxSang; // Effet lorsque ca touche une entit�
    [SerializeField] private GameObject _pointProjectile; // La pointe de la fl�che lorsque les effets sont instanti�s
    private float _vitesse = 10f; // vitesse du projectile
    private float _degat = -5f; // D�gats que le projectile fait
    private int _compteur = 0; // Compteur pour la dur�e que le projectile existe
    private int _limiteCompteur = 2; // Limite que le projectile peut exister


    // --- Initialisation -------------------------------------------------------

    /// <summary>
    /// S'ex�cute avant la mise � jour du premier frame
    /// </summary>
    private void Start()
    {
        InvokeRepeating("Compter", 1f, 1f);
    }

    // --- M�thodes Priv�es -------------------------------------------------------

    /// <summary>
    /// S'ex�cute � chaque frame
    /// Pour le mouvement
    /// </summary>
    private void Update()
    {
        //Debug.Log(transform.rotation.z);
        float angle = (transform.rotation.z) * Mathf.Deg2Rad;
        Vector2 dV = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        transform.Translate(Vector3.right * _vitesse * Time.deltaTime, Space.Self);
    }

    /// <summary>
    /// Check si la fl�che touche quelque chose
    /// </summary>
    /// <param name="other"> Les triggers </param>
    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Mur") || other.CompareTag("Bouclier"))
        {
            Instantiate(_fxFleche, _pointProjectile.transform.position, transform.rotation);
            Destroy(gameObject);
        }

        if (other.CompareTag("Ennemi"))
        {
            Instantiate(_fxSang, _pointProjectile.transform.position, transform.rotation);
            other.GetComponentInParent<Ennemi>().ModifierVie(_degat);
            Destroy(gameObject);
        }

        if (other.CompareTag("Boss"))
        {
            Instantiate(_fxSang, _pointProjectile.transform.position, transform.rotation);
            other.GetComponentInParent<Boss>().ModifierVie(_degat);
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Ajoute un au compteur et d�truit s'il et au-dessus de la limite
    /// </summary>
    private void Compter()
    {
        
        _compteur++;
        if(_compteur > _limiteCompteur)
        {
            Destroy(gameObject);
        }
    }
}
