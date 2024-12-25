using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    [SerializeField] private string fireName;

    [SerializeField] private int damage;

    [SerializeField] private float damageTime;
    private float currentDamageTime;

    [SerializeField] private float durationTIme;
    private float currentDurationTIme;

    [SerializeField]
    private ParticleSystem ps_Flame;

    private StatusController thePlayerStatus;

    private bool isFIre = true;

    private void Start()
    {   
        thePlayerStatus = FindObjectOfType<StatusController>();
        currentDurationTIme = durationTIme;
    }

    // Update is called once per frame
    void Update()
    {
        if  (isFIre)
        {
            ElapseTime();
        }
    }

    private void ElapseTime()
    {
        currentDurationTIme -= Time.deltaTime;

        if(currentDamageTime > 0)
        {
            currentDamageTime -= Time.deltaTime;
        }

        if(currentDurationTIme <= 0)
        {
            Off();
        }
    }

    private void Off()
    {
        ps_Flame.Stop();
        isFIre = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (isFIre && other.transform.tag == "Player")
        {
            if (currentDamageTime <= 0)
            {
                other.GetComponent<Burn>().StartBurning();
                thePlayerStatus.DecreaseHP(damage);
                currentDamageTime = damageTime;
            }
        }
    }

    public bool GetisFire()
    {
        return isFIre;
    }
}
