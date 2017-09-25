using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scForts : MonoBehaviour {
    public int Health = 1000;
    public List<Transform> FortsChild;
    public List<GameObject> EnemyInMyForts;
	void Update () {

        if (Health <=0)
        {
            scFortsManager.instance.PlayerFortsList.Remove(this);
            scFortsManager.instance.EnemyFortsList.Remove(this);
            for (int i = 0; i < EnemyInMyForts.Count-1; i++)
            {
                EnemyInMyForts[i].GetComponent<scAIManager>().MyFort = null;
            }
            Destroy(gameObject);
        }
	}
}
