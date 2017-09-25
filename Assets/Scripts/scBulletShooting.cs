using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class scBulletShooting : MonoBehaviour
{
    public GameObject Owner;
    [Header("Bullet Properties")]
    public int BulletPower = 2;
    public int BulletSpeed = 100;
    public Vector2 TargetRandomRange = new Vector2(-3, 3);
    public Vector3 target;
    
    // Use this for initialization
    void Start()
    {
        transform.SetParent(scGamePlayManager.instance.RunTimeGameObjects.transform);
        float tempValue = Random.Range(TargetRandomRange.x, TargetRandomRange.y);
        target.x = Random.Range(target.x, target.x + tempValue);
        target.y = Random.Range(target.y, target.y + tempValue);
        target.z = Random.Range(target.z, target.z + tempValue);
        target.y = target.y + 2;

        transform.LookAt(target);
    }

    void Update()
    {
        OnEnemyShoot();
    }

    public void OnEnemyShoot()
    {
        // Move our position a step closer to the target.
            transform.position = transform.position + transform.forward * BulletSpeed * Time.deltaTime;

    }

    void OnCollisionEnter(Collision collision)
    {
        if (Owner != null)
        {
            if (Owner.tag == "Enemy")
            {
                if (collision.gameObject.tag == "Player")
                {
                    collision.gameObject.GetComponent<scPlayerController>().Health -= BulletPower;
                    Destroy(gameObject);
                }
                else if (collision.gameObject.tag == "PlayerDude")
                {
                    collision.gameObject.GetComponent<scPlayerDudeController>().Health -= BulletPower;
                    Destroy(gameObject);
                }
                else if (collision.gameObject.tag == "Forts")
                {
                    collision.gameObject.GetComponent<scForts>().Health -= BulletPower;
                }
            }
            else if (Owner.tag == "Player" || Owner.tag == "PlayerDude")
            {
                if (collision.gameObject.tag == "Enemy")
                {
                    collision.gameObject.GetComponent<scEnemyController>().Health -= BulletPower;
                    Destroy(gameObject);
                }
                else if (collision.gameObject.tag == "Forts")
                {
                    collision.gameObject.GetComponent<scForts>().Health -= BulletPower;
                }
            }
        }

        Destroy(gameObject);
    }
}