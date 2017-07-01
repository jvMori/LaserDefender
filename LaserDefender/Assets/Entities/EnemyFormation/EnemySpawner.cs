using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    public float speed = 1f;
    public GameObject enemyPrefab;
    public float width = 10f;
    public float height = 5f;
    public float xmin;
    public float xmax;
    public float spawnDelay = 0.5f;

    private bool movingRight = true;

	// Use this for initialization
	void Start () {

        SpawnUntilFull();

        float distance = transform.position.z - Camera.main.transform.position.z;
        Vector3 leftMost = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distance));
        Vector3 rightMost = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, distance));
        xmax = rightMost.x;
        xmin = leftMost.x;
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(width, height));
    }

    // Update is called once per frame
    void Update () {

        Move();
        SetMovementBoundaries();

        if (AllMembersAreDead())
        {
            SpawnUntilFull();
        }

    }

   
    Transform NextFreePosition()
    {
        foreach (Transform childPosGameObject in transform)
        {
            if (childPosGameObject.childCount == 0)
                return childPosGameObject;
        }
        return null;
    }


    bool AllMembersAreDead()
    {
        foreach (Transform childPosGameObject in transform)
        {
            if (childPosGameObject.childCount > 0)
                return false;
        }
        return true;
    }


    void SpawnUntilFull()
    {
        Transform freePos = NextFreePosition();
        if (freePos)
        {
            GameObject enemy = Instantiate(enemyPrefab, freePos.position, Quaternion.identity) as GameObject;
            enemy.transform.parent = freePos;
        }

        if (NextFreePosition())
        {
            Invoke("SpawnUntilFull", spawnDelay);
        }

        
        
    }

    void MakeEnemies()
    {
        foreach (Transform child in transform)
        {
            GameObject enemy = Instantiate(enemyPrefab, child.transform.position, Quaternion.identity) as GameObject;
            enemy.transform.parent = child;
        }
    }

    void Move()
    {
        if (movingRight)
        {
            transform.position += Vector3.right * speed * Time.deltaTime;

        }
        else
        {
            transform.position += Vector3.left * speed * Time.deltaTime;
        }
    }

    void SetMovementBoundaries()
    {
        float rightEdgeOfFormation = transform.position.x + (0.5f * width);
        float leftEdgeOfFormation = transform.position.x - (0.5f * width);

        if (rightEdgeOfFormation > xmax)
        {
            movingRight = false;

        } else if (leftEdgeOfFormation < xmin)
        {
            movingRight = true;
        }
    }
}
