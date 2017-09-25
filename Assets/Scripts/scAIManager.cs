using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class scAIManager : MonoBehaviour
{
    public static scAIManager instance;

    public enum enCharacterType
    {
        Player, // player avatar
        PlayerDude, // solider help player
        Enemy // enemy
    }
    public enCharacterType characterType;
    public int Health = 100;
    public float FireSpeed = 1.1f;// Fire Speed By type of gun
    public int SightDegree = 45; // how much degree can see 
    public float EnemySight = 3; // use for Physics.BoxCastAll’s halfExtend
    public float VisibilityDistance = 20;

    public GameObject Target;
    public GameObject Bullet;
    public GameObject Muzzle;
    public int BulletMag = 20;

    public bool Reloading = false;
    public scForts MyFort; // current character use this Fort and each fort have one or more position to Stay (FortsChild.Count)
    //public bool HaveFort = true;
    public float transitionDuration = 2;
    public int StartHealthTemp;

    protected int BulletShooted = 0;
    protected NavMeshAgent agent;
    //protected int StartHealthTemp;

    // Use this for initialization

    void Start()
    {
        instance = this;
    }
    
    public void FortsFinder(List<scForts> FortsList)//Find empty and Nearest Fort
    {
        float tempA = 0, tempB = 0;
        for (int i = 0; i < FortsList.Count; i++)
        {
            tempA = Mathf.Abs(Vector3.Distance(gameObject.transform.position, FortsList[i].transform.position));
            if (tempB == 0)
            {
                tempB = tempA;
            }
            if (tempA <= tempB)
            {
                if (FortsList[i].EnemyInMyForts.Count < FortsList[i].FortsChild.Count)
                {
                    MyFort = FortsList[i];
                    tempB = tempA;
                }
                else
                {
                    tempB = 0;
                }
            }
        }

        if (MyFort != null)
        {
            MyFort.EnemyInMyForts.Add(gameObject);
            Transform FortsTarget = MyFort.FortsChild[MyFort.EnemyInMyForts.Count - 1].gameObject.transform;
            StartCoroutine(Transition(1, FortsTarget));//go to forts child
        }
        else
        {
            //stay and fire or do something else;
        }
    }

    IEnumerator Transition(float wait, Transform target)
    {
           agent.SetDestination(target.position);

        yield return wait;
    }



    public RaycastHit[] hits;
    public void LookForward()
    {
        RotateForDiscover(); // rotate current character to expand it’s field of view
        if (hits.Length > 0)
        {
            float tempA = 0, tempB = 0;
            for (int i = 0; i < hits.Length; i++)
            {
                if (characterType == enCharacterType.Enemy)// Detect if player or PlayerDude is within the field of view
                {
                    if (hits[i].collider.tag == "Player" || hits[i].collider.tag == "PlayerDude")
                    {
                        tempA = Mathf.Abs(Vector3.Distance(gameObject.transform.position, hits[i].collider.transform.position));
                        if (tempB == 0)
                        {
                            tempB = tempA;
                        }
                        if (tempA <= tempB)
                        {
                            tempB = tempA;
                            Target = hits[i].collider.gameObject;
                        }
                    }
                    else
                    {
                        RotateForDiscover();
                    }
                }
                else if (characterType== enCharacterType.Player || characterType == enCharacterType.PlayerDude)
                {
                    if (hits[i].collider.tag == "Enemy")
                    {
                        tempA = Mathf.Abs(Vector3.Distance(gameObject.transform.position, hits[i].collider.transform.position));
                        if (tempB == 0)
                        {
                            tempB = tempA;
                        }
                        if (tempA <= tempB)
                        {
                            tempB = tempA;
                            Target = hits[i].collider.gameObject;
                        }
                    }
                    else
                    {
                        RotateForDiscover();
                    }
                }
            }
        }
    }
    protected float RotateNumber;
    public void RotateForDiscover()
    {

        Quaternion tempR = Quaternion.LookRotation(transform.forward);
        tempR = tempR * Quaternion.Euler(0, RotateNumber, 0);
        Vector3 tempD = tempR * Vector3.forward;
        hits = Physics.BoxCastAll(gameObject.transform.position + new Vector3(0, 1, 0), new Vector3(EnemySight, EnemySight, 0), tempD, Quaternion.identity, VisibilityDistance);
        ExtDebug.DrawBox(gameObject.transform.position + new Vector3(0, 1, 0), new Vector3(EnemySight, EnemySight, 0), tempR, Color.yellow);
        StartCoroutine(ChangeNumberInTime(0.2f));
    }

    protected bool righted = false;
    IEnumerator ChangeNumberInTime(float timer)
    {
        if (righted == false)
        {
            RotateNumber++;
            if (RotateNumber >= SightDegree)
            {
                righted = true;
            }
        }
        else if (righted == true)
        {
            RotateNumber--;
            if (RotateNumber <= -SightDegree)
            {
                righted = false;
            }
        }
        yield return timer;    //Wait one frame
    }

    public void ShootForEnemy()
    {
        transform.LookAt(Target.transform);
        transform.rotation *= Quaternion.Euler(0, 15, 0);
        if (BulletShooted <= BulletMag && Reloading == false)
        {
            gameObject.GetComponent<Animator>().SetBool("Fire", true);
        }
        else if (BulletShooted > BulletMag && Reloading == false)
        {
            gameObject.GetComponent<Animator>().SetTrigger("Reload");
            gameObject.GetComponent<Animator>().SetBool("Fire", false);
            Reloading = true;
        }
    }
    public void _ReloadDone()// used in animation Event
    {
        Reloading = false;
        BulletShooted = 0;
    }

    public virtual void _Shoot() // used in animation Event, Instantiate Bullet and bullet use its own script to move toward on target
    {
        BulletShooted++;
        GameObject BulletGO = Instantiate(Bullet, Muzzle.transform.position, Quaternion.identity);
        BulletGO.GetComponent<scBulletShooting>().Owner = gameObject;
        if (Target!=null)
        {
            BulletGO.GetComponent<scBulletShooting>().target = Target.transform.position;
        }
    }
}
