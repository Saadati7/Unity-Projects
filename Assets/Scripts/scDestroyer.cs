using UnityEngine;
using System.Collections;

public class scDestroyer : MonoBehaviour {

    public float DestroyAfter = 4;
	
	void Update () {
		if(!IsInvoking("AutoDestruct"))
		{
            Invoke("AutoDestruct", DestroyAfter);
		}
	}
	void AutoDestruct()
	{
		Destroy(gameObject);

	}
}

