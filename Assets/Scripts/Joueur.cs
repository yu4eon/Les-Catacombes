// =======================================
//     Auteur: L�on Yu
//     Automne 2023, TIM
// =======================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe principale pour la gestion du joueur
/// </summary>
public class Joueur : MonoBehaviour
{
    // --- Champs --------------------------------------------------------------

    [SerializeField] private GameObject[] _coeurs; //Tableau des vies du joueurs dans le UI
    [SerializeField] private GameObject _prefabProjectile; // Projectile lorsque le joueur tire
    [SerializeField] private ParticleSystem _fxSang; //Effet lorsque le joueur prend des d�gat
    [SerializeField] private GameObject _armePoint; //Le point de l'arme du joueur
    [SerializeField] private GameObject _arme; // l'arme du joueur
    [SerializeField] private SpriteRenderer _srMainD; // Utilis� lorsqu'on le d�sactive
    [SerializeField] private SpriteRenderer _srMainG; // Utilis� lorsqu'on le d�sactive
    [SerializeField] private FadeFromBlack _fond; // Appelle le fond lors de la mort du joueur
    [SerializeField] private GameObject _potion; // Permet au joueur de regagner des points de vies
    [SerializeField] private Animator _animPotion; //Animation jou� lorsque le joueur bois


    [SerializeField] private AudioClip _sonTire; // Son jou� lorsque le joueur tire
    [SerializeField] private AudioClip _sonBoire; // Son jou� lorsque le joueur bois la potion
    [SerializeField] private AudioClip[] _sonsMarche; // Tableau des sons de marche
    [SerializeField] private AudioClip _sonGameOver; // Son lorsque le joueur meurt
    [SerializeField] private AudioClip[] _sonsDegat; // Tableau de sons lorsque le joueur prend des d�gats

    private float _vitesse = 5f; // Vitesse du joueur
    private float _delaiTire = 0.5f; //D�lai entre les tire
    private float _dureeInvulnerable = 1.5f; //La dur�e des frames d'invuln�rabilit�
    private float _delaiFlash = 0.15f; //D�lai entre les flash d'invuln�rabilit�
    private float _delaiGameOver = 1f; //D�lai avant que le fade au noir
    private bool _estVivant = true; // Si le joueur est vivant, permet de l'arreter
    private bool _peutTirer = true; // Si le joueur peut tirer
    private bool _estInvulnerable; // Si le joueur est invuln�rable
    private bool _possedePotion; // Check si le joueur poss�de une potion
    private int _nbVies; // Nombre de vies du joueur
    private Coroutine _tirer; // Je l'ai mis en variable pour pouvoir l'arr�ter
    private Rigidbody2D _rb; // Utilis� pour le mouvement
    private Collider2D _collision; // Pour le d�sactiver lorsque le joueur meurt
    private Animator _animator; // Animateur du joueur
    private SpriteRenderer _sr; // Utilis� dans les frames d'invuln�rabilit�
    private Vector3 _axis; // Pour le mouvement
    private Vector3 _posSouris; // Utilis� pour tourner l'arme vers la souris
    

    // --- Initialisation -------------------------------------------------------

    /// <summary>
    /// Appel� d�s l'instantiation du script
    /// </summary>
    private void Awake()
    {
        _nbVies = _coeurs.Length;
        _rb = GetComponent<Rigidbody2D>();
        _collision = GetComponent<Collider2D>();
        _animator = GetComponent<Animator>();
        _sr = GetComponent<SpriteRenderer>();
        _potion.SetActive(false);
    }

    // --- M�thodes Priv�es -------------------------------------------------------

    /// <summary>
    /// S'ex�cute � chaque frame
    /// Check le mouvement et ses inputs pour le tire et les potions
    /// </summary>
    private void Update()
    {
        if (!_estVivant)
        {
            _axis = Vector3.zero;
            return;
        }

        _axis.x = Input.GetAxisRaw("Horizontal");
        _axis.y = Input.GetAxisRaw("Vertical");

        _axis.Normalize();

        _animator.SetFloat("Magnitude", _axis.magnitude);
        _animator.SetFloat("Horizontal", _axis.x);

        if (_axis.magnitude != 0) 
        {
            _animator.SetFloat("LastHorizontal", _axis.x);
        }

        _posSouris = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        float angle = TrouverAngle(transform.position, _posSouris) * Mathf.Rad2Deg;
        _armePoint.transform.rotation = Quaternion.Euler(0,0,angle);


        if(Input.GetMouseButtonDown(0) && _peutTirer) 
        {
            //Possibilit� d'ajouter un cooldown?
            _tirer = StartCoroutine(Tirer());
        }

        if(Input.GetMouseButtonUp(0))
        {
            StopCoroutine(_tirer);
        }

        if (Input.GetKeyDown(KeyCode.Q) && _possedePotion)
        {
            BoirePotion();
        }
    }

    /// <summary>
    /// S'ex�cute selon un temps fixte, pour les physiques.
    /// Pour le mouvement
    /// </summary>
    private void FixedUpdate()
    {

        _rb.MovePosition(transform.position + _vitesse * _axis * Time.fixedDeltaTime);
    }

    /// <summary>
    /// Permet au joueur de tirer, m�me si on maintient la touche.
    /// Tire selon un d�lai si maintenue
    /// </summary>
    private IEnumerator Tirer()
    {
        while (true)
        {
            Debug.Log("Le joueur tire");
            _peutTirer = false;
            Invoke("PermetTire", _delaiTire);
            SoundManager.instance.JouerSon(_sonTire, 1.5f);
            _arme.GetComponent<Animator>().SetTrigger("Tire");
            Instantiate(_prefabProjectile, _arme.transform.position, _armePoint.transform.rotation);
            yield return new WaitForSeconds(_delaiTire);
        }
    }

    /// <summary>
    /// Permet au joueur de tirer, pour qu'il ne puisse pas spammer
    /// </summary>
    private void PermetTire()
    {
        _peutTirer = true;
    }


    /// <summary>
    /// Appel� � la mort du joueur. Montre l'�cran game over apr�s un d�lai
    /// </summary>
    private void Mourir()
    {
        StopAllCoroutines();
        _collision.enabled = false;
        _estVivant = false;
        _animator.SetBool("Vivant", false);
        SoundManager.instance.JouerSon(_sonGameOver, 0.7f);

        Invoke("AppellerEcranFin", _delaiGameOver);
    }

    /// <summary>
    /// Appelle le fade au fond
    /// </summary>

    private void AppellerEcranFin()
    {
        _fond.CommencerFade();
    }

    /// <summary>
    /// Rend le joueur invuln�rable et ajoute un effet de flash
    /// </summary>
    private IEnumerator RendreInvulnerable() 
    {
        _estInvulnerable = true;

        for(float i = 0; i < _dureeInvulnerable; i+= _delaiFlash)
        {
            if(_sr.color.a == 1)
            {
                _arme.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0f);
                _srMainD.color = new Color(1,1,1, 0f);
                _srMainG.color = new Color(1, 1, 1, 0f);
                _sr.color = new Color(1, 1, 1, 0f);
            }
            else
            {
                _arme.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1f);
                _srMainD.color = new Color(1, 1, 1, 1f);
                _srMainG.color = new Color(1, 1, 1, 1f);
                _sr.color = new Color(1, 1, 1, 1);
            }
            yield return new WaitForSeconds(_delaiFlash);
        }

        _arme.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1f);
        _srMainD.color = new Color(1, 1, 1, 1f);
        _srMainG.color = new Color(1, 1, 1, 1f);
        _sr.color = new Color(1, 1, 1, 1);
        _estInvulnerable = false;
        yield return null;
    }

    /// <summary>
    /// R�staure les vies du joueur et enl�ve la potion
    /// </summary>
    private void BoirePotion()
    {
        SoundManager.instance.JouerSon(_sonBoire);
        _nbVies = _coeurs.Length;

        for (int i = 0; i < _coeurs.Length; i++)
        {
            if (i < _nbVies)
            {
                Debug.Log("inchang�");
                _coeurs[i].SetActive(true);
            }
            else
            {
                Debug.Log("perdre un coeur");
                _coeurs[i].SetActive(false);
            }
        }

        _possedePotion = false;
        _potion.SetActive(false);
        _animPotion.SetTrigger("Boire");
    }

    /// <summary>
    /// Trouve l'angle entre deux objets
    /// </summary>
    /// <param name="v1">Objet 1 � comparer</param>
    /// <param name="v2">Objet 2 � comparer</param>
    private float TrouverAngle(Vector2 v1, Vector2 v2)
    {
        Vector2 v = v2 - v1;
        return Mathf.Atan2(v.y, v.x);
    }

    // --- M�thodes Publiques -------------------------------------------------------

    /// <summary>
    /// Fait perdre une vie au joueur et le rend invuln�rable.
    /// Check s'il lui reste des vies.
    /// </summary>
    public void PerdreVie()
    {
        if (_estInvulnerable) return;
        _nbVies--;
        Instantiate(_fxSang, transform.position, Quaternion.identity);

        int n = Random.Range(0, _sonsDegat.Length);
        SoundManager.instance.JouerSon(_sonsDegat[n]);



        for (int i = 0; i < _coeurs.Length; i++)
        {
            if (i < _nbVies)
            {
                Debug.Log("inchang�");
                _coeurs[i].SetActive(true);
            }
            else
            {
                Debug.Log("perdre un coeur");
                _coeurs[i].SetActive(false);
            }
        }

        if (_nbVies == 0)
        {
            Mourir();
        }
        else if( _nbVies > 0)
        {
            StartCoroutine(RendreInvulnerable());
        }
        
    }

    /// <summary>
    /// Utilis� lorsque le joueur touche la potion.
    /// </summary>
    public void ObtenirPotion()
    {
        _possedePotion = true;
        _potion.SetActive(true);
    }

    /// <summary>
    /// Joue un son al�atoire de marche
    /// </summary>
    public void JouerSonMarche()
    {
        int n = Random.Range(0, _sonsMarche.Length);
        SoundManager.instance.JouerSon(_sonsMarche[n], 0.8f);
    }
}
