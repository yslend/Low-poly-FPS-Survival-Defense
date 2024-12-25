using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Animal : MonoBehaviour
{
    protected StatusController thePlayerStatus;

    [SerializeField] public string animalName; // 동물의 이름
    [SerializeField] protected int hp; // 동물의 체력.

    [SerializeField]
    private Item item_Prefab;
    [SerializeField]
    public  int itemNumber;

    [SerializeField] protected float walkSpeed; // 걷기 스피드.
    [SerializeField] protected float runSpeed; // 뛰기 스피드.

    protected Vector3 destination; // 방향.


    // 상태변수
    protected bool isAction; // 행동중인지 아닌지 판별.
    protected bool isWalking; // 걷는지 안 걷는지 판별.
    protected bool isRunning;
    protected bool isChasing;
    protected bool isAttacking;
    public bool isDead;

    [SerializeField] protected float walkTime; // 걷기 시간
    [SerializeField] protected float waitTime; // 대기 시간.
    [SerializeField] protected float runTime;
    protected float currentTime;


    // 필요한 컴포넌트
    [SerializeField] protected Animator anim;
    [SerializeField] protected Rigidbody rigid;
    [SerializeField] protected BoxCollider boxCol;

    protected AudioSource theAudio;
    protected NavMeshAgent nav;
    protected FieldOfViewAngle theViewAngle;

    [SerializeField] protected AudioClip[] sound_Normal;
    [SerializeField] protected AudioClip sound_Hurt;
    [SerializeField] protected AudioClip sound_Dead;

    void Start()
    {
        thePlayerStatus = FindObjectOfType<StatusController>();
        theViewAngle = GetComponent<FieldOfViewAngle>();
        nav = GetComponent<NavMeshAgent>();
        theAudio = GetComponent<AudioSource>();
        currentTime = waitTime;
        isAction = true;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (!isDead)
        {
            Move();
            ElapseTime();
        }
    }
        
    protected void Move()
    {
        if (isWalking || isRunning)
            nav.SetDestination(transform.position + destination * 5f);
            //rigid.MovePosition(transform.position + (transform.forward * applySpeed * Time.deltaTime));
    }
        
    protected void ElapseTime()
    {
        if (isAction)
        {
            currentTime -= Time.deltaTime;
            if (currentTime <= 0 && !isChasing && !isAttacking)
                SetReSet();
        }
    }  

    protected virtual void SetReSet()
    {
        isWalking = false; isRunning = false; isAction = true;
        nav.speed = walkSpeed;
        nav.ResetPath();
        anim.SetBool("Walking", isWalking); anim.SetBool("Running", isRunning);
        destination.Set(Random.Range(-0.2f, 0.2f), 0f, Random.Range(0.5f, 1f));
    }

    protected void TryWalk()
    {
        isWalking = true;
        anim.SetBool("Walking", isWalking);
        currentTime = walkTime;
        nav.speed = walkSpeed;
        Debug.Log("걷기");
    }

    public virtual void Damage(int _dmg, Vector3 _targetPos)
    {
        if (!isDead)
        {
            hp -= _dmg;

            if (hp <= 0)
            {
                Dead();
                return;
            }

            PlaySE(sound_Hurt);
            anim.SetTrigger("Hurt");
        }
    }

    protected void Dead()
    {
        PlaySE(sound_Dead);
        isWalking = false;
        isRunning = false;
        isChasing = false;
        isAttacking = false;
        isDead = true;
        nav.ResetPath();
        StopAllCoroutines();
        anim.SetTrigger("Dead");
    }

    protected void RandomSound()
    {
        int _random = Random.Range(0, 3); // 일상 사운드 3개
        PlaySE(sound_Normal[_random]);
    }

    protected void PlaySE(AudioClip _Clip)
    {
        theAudio.clip = _Clip;
        theAudio.Play();
    }

    public Item GetItem()
    {
        this.gameObject.tag = "Untagged";
        Destroy(this.gameObject, 3f);
        return item_Prefab;
    }
}
