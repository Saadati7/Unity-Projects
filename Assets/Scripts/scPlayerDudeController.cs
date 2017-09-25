using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class scPlayerDudeController : scAIManager
{
    void Start()
    {
        gameObject.GetComponent<Animator>().SetFloat("FireSpeed", FireSpeed);
        agent = GetComponent<NavMeshAgent>();
        StartHealthTemp = Health;
    }

    void Update()
    {
        if (Health <= 0)
        {
            MyFort.EnemyInMyForts.Remove(gameObject);
            scFortsManager.instance.HaveEmptyFortsForEnemy = true;
            Destroy(gameObject);
        }
        if (MyFort == null && scFortsManager.instance.HaveEmptyFortsForPlayer == true)
        {
            FortsFinder(scFortsManager.instance.PlayerFortsList);
        }

        if (Target == null)
        {
            LookForward();
        }
        else if (Target != null)
        {
            ShootForEnemy();
        }
    }
}


