using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public string playerTag = "Player";

    public float attackDistance = 1.5f;
    public float attackTimer = 0.2f;
    public float goToDistance = 13;
    public float speed = 5;

    private float curTime;

    private Player playerScript;

    public Transform target;
 
    public State curState;

    public enum State
    {
        LOOKFOR,
        GOTO,
        ATTACK
    }

    // Start is called before the first frame update
    IEnumerator Start()
    {
        target = GameObject.FindGameObjectWithTag(playerTag).transform;
        curTime = attackTimer;

        if(target != null)
        {
            playerScript = target.GetComponent<Player>();
        }
        while (true)
        {
            switch(curState)
            {
                case State.LOOKFOR:
                    LookFor();
                    break;
                case State.GOTO:
                    GoTo();
                    break; 
                case State.ATTACK:
                    Attack();
                    break;
            }
            yield return 0;
        }
    }
    
    void LookFor()
    {
        print("Lookfor state");

        if (Vector3.Distance(target.position, transform.position) < goToDistance)
        {
            curState = State.GOTO;
        }
    }

    void GoTo()
    {
        print("Goto state");

        transform.LookAt(target);
        Vector3 fwd = transform.TransformDirection(Vector3.forward);

        if (Physics.Raycast(transform.position, fwd, out RaycastHit buddy))
        {
            if (!buddy.transform.CompareTag ("Player"))
            {
                curState = State.LOOKFOR;
                return;
            }
        }

        if (Vector3.Distance(target.position, transform.position) > attackDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        }
        else
        {
            curState = State.ATTACK;
        }
    }

    void Attack()
    {
        print("Attack state");
        Debug.Log(curTime);

        transform.LookAt(target);
        curTime -= Time.deltaTime;

        if(curTime < 0)
        {
            playerScript.health--;
            curTime = attackTimer;
        }

        if (Vector3.Distance(target.position, transform.position) > attackDistance)
        {
            curState = State.GOTO;
            curTime = attackTimer;
        }
    }
}
