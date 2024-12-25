using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseTower : MonoBehaviour
{
    [SerializeField] private string towerName;
    [SerializeField] private float range;
    [SerializeField] private int damage;
    [SerializeField] private float rateOfAccurasy;
    [SerializeField] private float rateOfFire;
    private float currentRateOfFIre;
    [SerializeField] private float viewAngle;
    [SerializeField] private float spinSpeed;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Transform tf_TopGun;
    [SerializeField] private ParticleSystem particle_MuzzleFlash;
    [SerializeField] private GameObject go_HitEffect_Prefab;

    private RaycastHit hitInfo;
    private Animator anim;
    private AudioSource theAudio;

    private bool isFindTarget = false;
    private bool isAttack = false;

    private Transform tf_Target;

    [SerializeField] private AudioClip sound_Fire;

    // Start is called before the first frame update
    void Start()
    {
        theAudio = GetComponent<AudioSource>();
        theAudio.clip = sound_Fire;
        anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        Spin();
        SearchEnemy();
        LookTarget();
        Attack();
    }

    private void Spin()
    {
        if (!isFindTarget && !isAttack)
        {
            Quaternion _spin = Quaternion.Euler(0f, tf_TopGun.eulerAngles.y + (1f * spinSpeed * Time.deltaTime), 0f);
            tf_TopGun.rotation = _spin;
        }
    }

    private void SearchEnemy()
    {
        Collider[] _targets = Physics.OverlapSphere(tf_TopGun.position, range, layerMask);

        for (int i = 0; i < _targets.Length; i++)
        {
            Transform _targetTf = _targets[i].transform;

            if(_targetTf.name == "Player")
            {
                Vector3 _direction = (_targetTf.position - tf_TopGun.position).normalized;
                float _angle = Vector3.Angle(_direction, tf_TopGun.forward);

                if(_angle < viewAngle * 0.5f)
                {
                    tf_Target = _targetTf;
                    isFindTarget = true;

                    if(_angle < 5f)
                    {
                        isAttack = true;
                    }
                    else
                    {
                        isAttack = false;
                    }
                    return;
                }
            }
        }

        tf_Target = null;
        isAttack = false;
        isFindTarget = false;
    }

    private void LookTarget()
    {
        if (isFindTarget)
        {
            Vector3 _dircetion = (tf_Target.position - tf_TopGun.position).normalized;
            Quaternion _lookRotation = Quaternion.LookRotation(_dircetion);
            Quaternion _rotation = Quaternion.Lerp(tf_TopGun.rotation, _lookRotation, 0.2f);
            tf_TopGun.rotation = _rotation;
        }
    }

    private void Attack()
    {
        if (isAttack)
        {
            currentRateOfFIre += Time.deltaTime;
            if(currentRateOfFIre >= rateOfFire)
            {
                currentRateOfFIre = 0;
                anim.SetTrigger("Fire");
                theAudio.Play();
                particle_MuzzleFlash.Play();

                if(Physics.Raycast(tf_TopGun.position,
                                   tf_TopGun.forward + new Vector3(Random.Range(-1, 1f) * rateOfAccurasy, Random.Range(-1, 1f) * rateOfAccurasy, 0f), 
                                   out hitInfo, range, layerMask)){
                    GameObject _temp = Instantiate(go_HitEffect_Prefab, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                    Destroy(_temp, 1f);

                    if(hitInfo.transform.name == "Player")
                    {
                        hitInfo.transform.GetComponent<StatusController>().DecreaseHP(damage);
                    }
                }
            }
        }
    }
}
