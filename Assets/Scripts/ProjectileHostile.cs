// =======================================
//     Auteur: L�on Yu
//     Automne 2023, TIM
// =======================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe principale du gestion des projectiles du boss
/// </summary>
public class ProjectileHostile : MonoBehaviour
{
    // --- Champs --------------------------------------------------------------

    [SerializeField] private ParticleSystem _fxBoule; // Effet lorsque le projectile touche un obstacles
    [SerializeField] private ParticleSystem _fxSang; // Effet lorsque ca touche une entit�
    [SerializeField] private GameObject _pointProjectile; // La pointe de la boule lorsque les effets sont instanti�s
    private float _vitesse = 5f; // Vitesse du projectile
    private int _compteur = 0;  // Compteur pour la dur�e que le projectile existe
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
        if (other.CompareTag("Mur"))
        {
            Instantiate(_fxBoule, _pointProjectile.transform.position, transform.rotation);
            Destroy(gameObject);
        }
        if (other.CompareTag("Player"))
        {
            Instantiate(_fxSang, _pointProjectile.transform.position, transform.rotation);
            other.GetComponent<Joueur>().PerdreVie();
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Ajoute un au compteur et d�truit s'il et au-dessus de la limite
    /// Plus pour pr�caution
    /// </summary>
    private void Compter()
    {

        _compteur++;
        if (_compteur > _limiteCompteur)
        {
            Destroy(gameObject);
        }
    }
}
