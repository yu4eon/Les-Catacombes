// =======================================
//     Auteur: L�on Yu
//     Automne 2023, TIM
// =======================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


/// <summary>
/// Classe principale du gestion du boss, dont
/// la gestion de ses attaques, et de ses �tats
/// </summary>
public class Boss : MonoBehaviour
{
    // --- Champs --------------------------------------------------------------

    //Les positions possibles que le boss peut prendre
    [SerializeField] private GameObject[] _positions = new GameObject[5]; 
    
    //J'ai du mettre le sprite du boss dans un enfant pour faire que le mouvement marche
    [SerializeField] private GameObject _boss; 

    [SerializeField] private GameObject _pointArme; //Point de l'arme pour les projectiles
    [SerializeField] private GameObject _prefabEnnemi; //Les ennemis invoqu�s
    [SerializeField] private GameObject _prefabProjectile; //Projectile du boss
    [SerializeField] private GameObject _joueur; //Joueur pour comparer leur position
    [SerializeField] private GameObject _effetInvocation; //Effet lorsque les ennemis sont invoqu�s
    [SerializeField] private GameObject _barre; //Barre de vie du boss, activ� lorsque le joueur rentre dans la salle
    [SerializeField] private GameObject _barreVie; //La section de vie en tant que telle, qui mesure la vie
    [SerializeField] private GameObject _bouclier; //Le bouclier qui s'active � 50% de vie
    [SerializeField] private AudioClip[] _sonsDegat; //tableau de sons jou�s lorsque le boss prend des d�gats
    [SerializeField] private AudioClip _sonAttaque1; //Son de l'attaque directe
    [SerializeField] private AudioClip _sonAttaque2; //Son de l'attaque AOE
    [SerializeField] private AudioClip _sonInvocation; //Son lorsque les ennemis sont invoqu�s
    [SerializeField] private AudioClip _sonBouclier; //Son jou� lorsque le bouclier s'active

    private Vector3 _startPos; //Position actuel du boss
    private Vector3 _endPos; //Position d�sir�
    private float _vitesse = 6f; //Vitesse de mouvement
    private float _vitesseInitial; //Vitesse inital du boss
    private float _delaiAttaque = 3f; //D�lai entre les attaques du boss
    private const float _PTS_VIES_MAX = 150; //Points de vies initial du boss
    private float _ptsVies; //Points de vies actuel
    private float _delaiProjectile = 1f; //D�lai entre les projectiles
    private float _delaiProjectileInitial; //D�lai de d�part des projectiles
    private float _angleAttaque = 30; //Angle des projectiles dans l'attaque AOE
    private int _nbAttaques = 3; //Nombre d'attaques diff�rents du boss
    private int _nbProjectilesDirectes = 3; //Nombre de projectiles directes
    private int _nbinvocation = 1; //Nombre de monstres invoqu�s
    private int _nbProjectileAutour = 2; //Nombre de fois le boss fais des attaques AOE
    private bool _estActif; //Le boss ne bougera pas si c'est false.
    private bool _estEnVie = true; //Si le boss est en vie ou non
    private bool _aBouclier; //Si le boss � son bouclier d'activ�

    //Les colliders, inactif au d�but,afin que le joueur ne puisse pas l'attaquer pendant
    //qu'il est en dehors de la salle de boss
    private Collider2D _collision; 
    private Animator _anim; //gestionnaire des animations

    // --- Initialisation -------------------------------------------------------

    /// <summary>
    /// Appel� d�s l'instantiation du script
    /// </summary>
    private void Awake()
    {
        _anim = _boss.GetComponent<Animator>();
        _collision = _boss.GetComponent<Collider2D>();
        _ptsVies = _PTS_VIES_MAX;
    }

    /// <summary>
    /// S'ex�cute avant la mise � jour du premier frame
    /// </summary>
    private void Start()
    {
        _bouclier.SetActive(false);
        _barre.SetActive(false);
        _startPos = transform.position;
        _endPos = transform.position;
        _collision.enabled = false;
        _vitesseInitial = _vitesse;
        _delaiProjectileInitial = _delaiProjectile;
    }

    // --- M�thodes Priv�es -------------------------------------------------------

    /// <summary>
    /// S'ex�cute � chaque frame
    /// Tourne le boss selon la position du joueur
    /// </summary>
    private void Update()
    {

        if (!_estActif || !_estEnVie) return;
        if ((transform.position.x - _joueur.transform.position.x) > 0)
        {
            _boss.transform.localScale = new Vector3(-1.5f, 1.5f, 1.5f);
        }
        else
        {
            _boss.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        }

        //D�commenter si vous voulez un petit cheat code
        //if (Input.GetKeyDown(KeyCode.K))
        //{
        //    Mourir();
        //}
    }
    /// <summary>
    /// S'ex�cute selon un temps fixte, pour les physiques.
    /// G�re les mouvements
    /// </summary>
    private void FixedUpdate()
    {
        if (!_estEnVie) return;
        //Debug.Log((_endPos - _startPos).magnitude);
        _startPos = transform.position;
        transform.position = Vector3.MoveTowards(_startPos, _endPos, _vitesse * Time.fixedDeltaTime);
        if ((_endPos - _startPos).magnitude < 0.2f)
        {
            transform.position = _endPos;
            _anim.SetBool("Bouge", false);
        }
    }

    /// <summary>
    /// D�cide quel attaque le boss utilise selon un nombre random. L'intensit�
    /// change selon les points de vie du boss. Il bouge aussi � tout les deux attaques.
    /// </summary>
    private IEnumerator ChoisirAttaque()
    {
        int compteur = 0;
        while (_ptsVies > 0)
        {
            int n = Random.Range(0, _nbAttaques);

            compteur++;
            if (compteur == 2)
            {
                ChangerPosition();
                compteur = 0;
            }

            if (_ptsVies >= _PTS_VIES_MAX / 2)
            {
                
                if (n == 0)
                {
                    StartCoroutine(AttaqueDirecte(_nbProjectilesDirectes));
                }
                else if (n == 1)
                {
                    SoundManager.instance.JouerSon(_sonInvocation);
                    StartCoroutine(InvoquerEnnemi(_nbinvocation));
                }

                else
                {
                    StartCoroutine(AttaqueAutour(_nbProjectileAutour));
                }
            }

            else
            {
                if (n == 0)
                {
                    StartCoroutine(AttaqueDirecte(_nbProjectilesDirectes * 2));
                }
                else if (n == 1)
                {
                    SoundManager.instance.JouerSon(_sonInvocation);
                    StartCoroutine(InvoquerEnnemi(_nbinvocation * 2));
                }

                else
                {
                    StartCoroutine(AttaqueAutour(_nbProjectileAutour * 2));
                }
            }

            yield return new WaitForSeconds(_delaiAttaque);
        }
    }

    /// <summary>
    /// D�cide � quelle position le boss se dirige selon un nombre random
    /// Si la position est la m�me qu'ou le boss est pr�sentement, le random
    /// se refait.
    /// </summary>
    private void ChangerPosition()
    {
        int n = Random.Range(0, _positions.Length);
        
        _endPos = _positions[n].transform.position;
        _anim.SetBool("Bouge", true);
        if (_endPos == _startPos)
        {
            ChangerPosition();
        }
    }

    /// <summary>
    /// L'attaque vise la position du joueur et lance des projectile selon un d�lai
    /// </summary>
    /// <param name="quantite">La quantit� de projectile � lancer</param>
    private IEnumerator AttaqueDirecte(int quantite)
    {
        int i = 0;
        while (i < quantite)
        {
            i++;
            SoundManager.instance.JouerSon(_sonAttaque1);
            float angle = TrouverAngle(_pointArme.transform.position, _joueur.transform.position) * Mathf.Rad2Deg;
            Instantiate(_prefabProjectile,_pointArme.transform.position, Quaternion.Euler(0,0, angle));
            yield return new WaitForSeconds(_delaiProjectile);
        }
    }

    /// <summary>
    /// Le boss invoque des ennemis � sa position, qui vont directement vers le joueur
    /// </summary>
    /// <param name="quantite">La quantit� d'ennemi � invoquer</param>
    private IEnumerator InvoquerEnnemi(int quantite)
    {
        int i = 0;
        while (i < quantite)
        {
            i++;
            Instantiate(_effetInvocation, _pointArme.transform.position, Quaternion.identity);

            GameObject monstre = Instantiate(_prefabEnnemi, _pointArme.transform.position, Quaternion.identity);
            monstre.GetComponent<Ennemi>().init(true);

            yield return new WaitForSeconds(_delaiProjectile);
        }
    }

    /// <summary>
    /// Fait appelle � l'attaque AOE
    /// </summary>
    /// <param name="quantite">Nombre de vague d'attaque</param>
    private IEnumerator AttaqueAutour(int quantite)
    {
        int i = 0;
        while (i < quantite)
        {
            i++;
            SoundManager.instance.JouerSon(_sonAttaque2, 2f);
            InstantiterAttaqueAutour();
            
            yield return new WaitForSeconds(_delaiProjectile);
        }
    }

    /// <summary>
    /// Instantie les vagues d'attaques selon un angle donn�
    /// </summary>
    private void InstantiterAttaqueAutour()
    {
        for (int i = 0; i < 360 / _angleAttaque; i++)
        {
            Instantiate(_prefabProjectile, _pointArme.transform.position, Quaternion.Euler(0, 0, i * _angleAttaque));
        }
    }

    /// <summary>
    /// Appel� lorsque le boss n'as plus de vies. D�sactive toutes
    /// et am�ne le joueur � la sc�ne de fin apr�s un d�lai
    /// </summary>
    private void Mourir()
    {
        _bouclier.SetActive(false);
        _collision.enabled = false;
        _estEnVie = false;
        StopAllCoroutines();
        _barre.SetActive(false);
        _anim.SetTrigger("Mort");
        Invoke("AppellerFin", 2f);
    }

    /// <summary>
    /// Demande � GameManager de loader la sc�ne fin.
    /// </summary>
    private void AppellerFin()
    {
        GameManager.Instance.AllerFin();
    }

    /// <summary>
    /// Trouve et retourne l'angle entre deux objets.
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
    /// Modifie la vie du boss. Modifie le la vitesse et d�lai
    /// des projectiles s'il tombe en bas de 50%
    /// </summary>
    public void ModifierVie(float degat)
    {
        _ptsVies = Mathf.Clamp(_ptsVies + degat, 0f, _PTS_VIES_MAX);
        float ratio = _ptsVies / _PTS_VIES_MAX;
        _barreVie.transform.localScale = new Vector3(ratio, 1, 1);

        int n = Random.Range(0, _sonsDegat.Length);
        SoundManager.instance.JouerSon(_sonsDegat[n], 0.4f);

        if (_ptsVies <= _PTS_VIES_MAX / 2)
        {
            if(!_aBouclier) SoundManager.instance.JouerSon(_sonBouclier, 1.5f);
            _aBouclier = true;
            _bouclier.SetActive(true);
            _vitesse = _vitesseInitial * 1.5f;
            _delaiProjectile = _delaiProjectileInitial * 0.5f;
        }
        if (_ptsVies <= 0)
        {
            Mourir();
        }

    }

    /// <summary>
    /// Commence le boss
    /// </summary>
    public void MettreActif()
    {
        _barre.SetActive(true);
        _estActif = true;
        _collision.enabled = true;
        StartCoroutine(ChoisirAttaque());
    }

    
}
