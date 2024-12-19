// =======================================
//     Auteur: Léon Yu
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
    [SerializeField] private ParticleSystem _fxSang; //Effet lorsque le joueur prend des dégat
    [SerializeField] private GameObject _armePoint; //Le point de l'arme du joueur
    [SerializeField] private GameObject _arme; // l'arme du joueur
    [SerializeField] private SpriteRenderer _srMainD; // Utilisé lorsqu'on le désactive
    [SerializeField] private SpriteRenderer _srMainG; // Utilisé lorsqu'on le désactive
    [SerializeField] private FadeFromBlack _fond; // Appelle le fond lors de la mort du joueur
    [SerializeField] private GameObject _potion; // Permet au joueur de regagner des points de vies
    [SerializeField] private Animator _animPotion; //Animation joué lorsque le joueur bois


    [SerializeField] private AudioClip _sonTire; // Son joué lorsque le joueur tire
    [SerializeField] private AudioClip _sonBoire; // Son joué lorsque le joueur bois la potion
    [SerializeField] private AudioClip[] _sonsMarche; // Tableau des sons de marche
    [SerializeField] private AudioClip _sonGameOver; // Son lorsque le joueur meurt
    [SerializeField] private AudioClip[] _sonsDegat; // Tableau de sons lorsque le joueur prend des dégats

    private float _vitesse = 5f; // Vitesse du joueur
    private float _delaiTire = 0.5f; //Délai entre les tire
    private float _dureeInvulnerable = 1.5f; //La durée des frames d'invulnérabilité
    private float _delaiFlash = 0.15f; //Délai entre les flash d'invulnérabilité
    private float _delaiGameOver = 1f; //Délai avant que le fade au noir
    private bool _estVivant = true; // Si le joueur est vivant, permet de l'arreter
    private bool _peutTirer = true; // Si le joueur peut tirer
    private bool _estInvulnerable; // Si le joueur est invulnérable
    private bool _possedePotion; // Check si le joueur possède une potion
    private int _nbVies; // Nombre de vies du joueur
    private Coroutine _tirer; // Je l'ai mis en variable pour pouvoir l'arrêter
    private Rigidbody2D _rb; // Utilisé pour le mouvement
    private Collider2D _collision; // Pour le désactiver lorsque le joueur meurt
    private Animator _animator; // Animateur du joueur
    private SpriteRenderer _sr; // Utilisé dans les frames d'invulnérabilité
    private Vector3 _axis; // Pour le mouvement
    private Vector3 _posSouris; // Utilisé pour tourner l'arme vers la souris
    

    // --- Initialisation -------------------------------------------------------

    /// <summary>
    /// Appelé dès l'instantiation du script
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

    // --- Méthodes Privées -------------------------------------------------------

    /// <summary>
    /// S'exécute à chaque frame
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
            //Possibilité d'ajouter un cooldown?
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
    /// S'exécute selon un temps fixte, pour les physiques.
    /// Pour le mouvement
    /// </summary>
    private void FixedUpdate()
    {

        _rb.MovePosition(transform.position + _vitesse * _axis * Time.fixedDeltaTime);
    }

    /// <summary>
    /// Permet au joueur de tirer, même si on maintient la touche.
    /// Tire selon un délai si maintenue
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
    /// Appelé à la mort du joueur. Montre l'écran game over après un délai
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
    /// Rend le joueur invulnérable et ajoute un effet de flash
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
    /// Réstaure les vies du joueur et enlève la potion
    /// </summary>
    private void BoirePotion()
    {
        SoundManager.instance.JouerSon(_sonBoire);
        _nbVies = _coeurs.Length;

        for (int i = 0; i < _coeurs.Length; i++)
        {
            if (i < _nbVies)
            {
                Debug.Log("inchangé");
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
    /// <param name="v1">Objet 1 à comparer</param>
    /// <param name="v2">Objet 2 à comparer</param>
    private float TrouverAngle(Vector2 v1, Vector2 v2)
    {
        Vector2 v = v2 - v1;
        return Mathf.Atan2(v.y, v.x);
    }

    // --- Méthodes Publiques -------------------------------------------------------

    /// <summary>
    /// Fait perdre une vie au joueur et le rend invulnérable.
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
                Debug.Log("inchangé");
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
    /// Utilisé lorsque le joueur touche la potion.
    /// </summary>
    public void ObtenirPotion()
    {
        _possedePotion = true;
        _potion.SetActive(true);
    }

    /// <summary>
    /// Joue un son aléatoire de marche
    /// </summary>
    public void JouerSonMarche()
    {
        int n = Random.Range(0, _sonsMarche.Length);
        SoundManager.instance.JouerSon(_sonsMarche[n], 0.8f);
    }
}
