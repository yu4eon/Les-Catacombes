// =======================================
//     Auteur: L�on Yu
//     Automne 2023, TIM
// =======================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Utilis� pour la gestion des sons.
/// Appel� pour jouer des sons.
/// J'ai mis une sur la sc�ne jeu pour rendre le testage plus rapide,
/// mais normalement il y en aurait pas.
/// </summary>
public class SoundManager : MonoBehaviour
{
    // --- Champs --------------------------------------------------------------

    [SerializeField] private AudioClip[] _musiques;
    private AudioSource _audioSource; //l'audioSource du Soundmanager
    private int _musiquePrecedante;
    private static SoundManager _instance; //singleton
    public static SoundManager instance
    {
        get { return _instance; }
    }

    // --- Initialisation -------------------------------------------------------

    /// <summary>
    /// S'ex�cute avant le Start
    /// </summary>
    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    /// <summary>
    /// S'ex�cute avant la mise � jour du premier frame
    /// </summary>
    private void Start()
    {
        _musiquePrecedante = 0;
        DontDestroyOnLoad(gameObject);
    }

    // --- M�thodes Publiques -------------------------------------------------------

    /// <summary>
    /// Fonction appel� pour jouer un son
    /// </summary>
    public void JouerSon(AudioClip son, float volume = 1)

    {
        _audioSource.PlayOneShot(son, volume);
    }

    public void ChangerMusique(int valeur)
    {
        if(_musiquePrecedante == valeur) return;

        _musiquePrecedante = valeur;
        //_audioSource.volume = 1f; 
        _audioSource.clip = _musiques[valeur];
        _audioSource.Play();
    }

    public void ArretterMusique()
    {
        _audioSource.clip = null;
        //StartCoroutine(DescendreVolume());
    }

    //private IEnumerator DescendreVolume()
    //{
    //    while (_audioSource.volume > 0)
    //    {
    //        _audioSource.volume-= _delaiVolume;
    //        yield return new WaitForSeconds(_delaiVolume);
    //    }

    //    _audioSource.Stop();
    //}
}
