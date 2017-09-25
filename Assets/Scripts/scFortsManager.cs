using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scFortsManager : MonoBehaviour {
    public static scFortsManager instance;
    public GameObject FortsPrefab;
    public bool HaveEmptyFortsForEnemy = true;
    public bool HaveEmptyFortsForPlayer = true;
    public List<scForts> PlayerFortsList;
    public List<scForts> EnemyFortsList;
	// Use this for initialization
	void Start () {
        instance = this;
	}

}
