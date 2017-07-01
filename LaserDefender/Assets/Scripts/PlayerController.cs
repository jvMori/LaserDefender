using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float health = 200f;
    public float speed = 10f;
    public float padding = 0.3f;
    public GameObject projectile;
    public float projectileSpeed;
    public float firingRate = 0.2f;

    public AudioClip fireSound;
   

    float xmin;
    float xmax;

    private void Start()
    {
       
        float distance = transform.position.z - Camera.main.transform.position.z;
        Vector3 leftMost = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distance));
        Vector3 rightMost = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, distance));
        xmin = leftMost.x + padding;
        xmax = rightMost.x - padding;
    }


    void FixedUpdate()
    {


        //float horizontalMove = Input.GetAxisRaw("Horizontal") * speed * Time.deltaTime;
        //Vector2 horizontalPos = GetComponent<Rigidbody2D>().velocity;
        //horizontalPos = new Vector2(horizontalMove, 0);

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position += Vector3.left * speed * Time.deltaTime;

        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.position += Vector3.right * speed * Time.deltaTime;
        }

        float newPosX = Mathf.Clamp(transform.position.x, xmin, xmax);

        transform.position = new Vector3(newPosX, transform.position.y, transform.position.z);


        //laser instantiate
        if (Input.GetKeyDown(KeyCode.Space))
        {
            InvokeRepeating("Fire", 0.000001f, firingRate);
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            CancelInvoke("Fire");
        }


    }
    void OnTriggerEnter2D(Collider2D collider)
    {

        Projectile missile = collider.gameObject.GetComponent<Projectile>();

        if (missile)
        {
            health -= missile.GetDamage();
            missile.Hit();
            if (health <= 0)
            {
                Die();
            }
        }

    }

    void Die()
    {
        LevelManager man = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        man.LoadLevel("Win Screen");
        Destroy(gameObject);
    }

    void Fire()
    {
        Vector3 offset = transform.position + new Vector3(0, 1, 0);
        GameObject beam = Instantiate(projectile, offset, Quaternion.identity) as GameObject;
        beam.GetComponent<Rigidbody2D>().velocity = new Vector3(0, projectileSpeed);
        AudioSource.PlayClipAtPoint(fireSound, transform.position);
    }
}
