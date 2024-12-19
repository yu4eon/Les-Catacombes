// =======================================
//     Auteur: Léon Yu
//     Automne 2023, TIM
// =======================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Classe principale pour la gestion du fond, qui peut fade
/// du noir au blanc et vice versa
/// </summary>
public class FadeFromBlack : MonoBehaviour
{
    // --- Champs --------------------------------------------------------------

    [SerializeField] private GameObject _menuFin; // Le menu de game over
    private Image _img; //Le composant image
    private float _alpha =1f; // Le niveau de transparence initial

    [SerializeField]
    [Range(0.1f, 5f)]
    private float _fadeSpeed; //Vitesse du fade

    // --- Initialisation -------------------------------------------------------

    /// <summary>
    /// Appelé dès l'instantiation du script
    /// </summary>
    private void Awake()
    {
        _img = GetComponent<Image>();
    }

    /// <summary>
    /// S'exécute avant la mise à jour du premier frame
    /// </summary>
    private void Start()
    {
        StopAllCoroutines();
        StartCoroutine(FadeBlack());
        ChangeAlpha(_alpha);
    }

    // --- Méthodes Privées -------------------------------------------------------

    /// <summary>
    /// Change la transparence de l'image
    /// </summary>
    /// <param name="valeur"> Le niveau de transparence </param>
    private void ChangeAlpha(float valeur)
    {
        _img.color = new Color(_img.color.r, _img.color.g, _img.color.b, valeur);
    }

    /// <summary>
    /// Met le fond de blanc à noir
    /// </summary>
    private IEnumerator FadeToBlack()
    {
        while (_alpha <= 1f)
        {
            
            _alpha += Time.deltaTime * _fadeSpeed;
            //Debug.Log(_alpha);

            if (_alpha >= 1f)
            {
                ChangeAlpha(1f);

            }
            else
            {
                ChangeAlpha(_alpha);
            }

            yield return new WaitForSeconds(Time.deltaTime);
        }


        _menuFin.SetActive(true);
        Time.timeScale = 0;
        StopAllCoroutines();
    }

    /// <summary>
    /// met le fond de noir à blanc
    /// </summary>
    private IEnumerator FadeBlack()
    {
        while (_alpha >= 0f)
        {
            _alpha -= Time.deltaTime * _fadeSpeed;

            if (_alpha <= 0f)
            {
                ChangeAlpha(0f);
                
            }
            else
            {
                ChangeAlpha(_alpha);
            }

            yield return new WaitForSeconds(Time.deltaTime);
        }
        StopAllCoroutines();
    }

    // --- Méthodes Publiques -------------------------------------------------------

    /// <summary>
    /// Appelle la fonction FadeToBlack. J'aurais voulu appeller directement la Coroutine mais
    /// je ne pense pas que c'est possible de l'appeller à partir d'un autre script
    /// </summary>
    public void CommencerFade()
    {
        StartCoroutine(FadeToBlack());
    }
}
