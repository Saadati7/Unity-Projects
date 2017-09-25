using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.CrossPlatformInput;

public class scPlayerController : scAIManager
{
    public static scPlayerController instancePlayer;

    public GameObject Gun;
    public GameObject Crosshair;
    public float Smoothness = 4;
    public Vector2 Sensitivity = new Vector2(4, 4);
    public Vector2 LimitX = new Vector2(-70, 80);
    public Vector2 LimitY = new Vector2(-70, 80);
    
    private Vector2 NewCoord;
    [HideInInspector]
    public Vector2 CurrentCoord;
    private Vector2 vel;
    // Use this for initialization
    void Start()
    {
        instancePlayer = this;
        gameObject.GetComponent<Animator>().SetFloat("FireSpeed", FireSpeed);
        agent = GetComponent<NavMeshAgent>();
        StartHealthTemp = Health;
    }

    // Update is called once per frame
    void Update()
    {
        NewCoord.x = Mathf.Clamp(NewCoord.x, LimitX.x, LimitX.y);
        NewCoord.y = Mathf.Clamp(NewCoord.y, LimitY.x, LimitY.y);
        NewCoord.x -= CrossPlatformInputManager.GetAxis("Mouse Y") * Sensitivity.x;
        NewCoord.y += CrossPlatformInputManager.GetAxis("Mouse X") * Sensitivity.y;
        CurrentCoord.x = Mathf.SmoothDamp(CurrentCoord.x, NewCoord.x, ref vel.x, Smoothness / 100);
        CurrentCoord.y = Mathf.SmoothDamp(CurrentCoord.y, NewCoord.y, ref vel.y, Smoothness / 100);
        Camera.main.transform.rotation = Quaternion.Euler(CurrentCoord.x, CurrentCoord.y, 0);
        transform.LookAt(Crosshair.transform);
        transform.rotation *= Quaternion.Euler(0, 15, 0);

        if (CrossPlatformInputManager.GetButtonDown("Fire1"))
        {
            gameObject.GetComponent<Animator>().SetBool("Fire", true);
        }
        else if (CrossPlatformInputManager.GetButtonUp("Fire1"))
        {
            gameObject.GetComponent<Animator>().SetBool("Fire", false);
        }

        if (MyFort == null && scFortsManager.instance.HaveEmptyFortsForPlayer == true)
        {
            FortsFinder(scFortsManager.instance.PlayerFortsList);
        }
    }

    public override void _Shoot()
    {
        GameObject BulletGO = Instantiate(Bullet, Muzzle.transform.position, Quaternion.identity);
        BulletGO.GetComponent<scBulletShooting>().Owner = gameObject;
        BulletGO.GetComponent<scBulletShooting>().target = Crosshair.transform.position;
    }
}
