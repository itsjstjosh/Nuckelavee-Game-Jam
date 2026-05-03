using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public enum EnemyState
{
    IDLE,
    RUNNING,
    Punching,
    DEAD
}
public class enemyController : MonoBehaviour
{
    public int enemyHealth = 100;
    public bool InAttackRange = false;
    private CharacterController PlayerScript;

    private Vector3 Pos;
    public GameObject LeftWaypoint;
    public GameObject RightWaypoint;

    private bool hitRightWaypoint = false;
    private bool hitLeftWaypoint = false;
    public bool roaming = false;

    public EnemyState moveEnemyState = EnemyState.IDLE;

    [Header("Movement")]
    public float MoveSpeed = 400.0f;

    public GameObject Player;

    [Header("StateSprites")]
    public RuntimeAnimatorController MoveIdleController;
    public RuntimeAnimatorController MoveRunningController;
    public RuntimeAnimatorController MovePunchController;
    //public RuntimeAnimatorController MoveJumpingController;

    private Animator _MoveAnimatorComponent;

    private bool _IsGoingRight = true;
    private bool _PlayerStateChangd = false;
    public bool takinghit = false;
    // Start is called before the first frame update
    void Start()
    {
        _MoveAnimatorComponent = gameObject.GetComponent<Animator>();
        _MoveAnimatorComponent.runtimeAnimatorController = MoveIdleController;
        Player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyHealth <= 0)
        {
            Destroy(gameObject);
        }

        if (!takinghit)
        {
            PlayerScript = Player.GetComponent<CharacterController>();
            if (InAttackRange)
            {
                takinghit = true;
                StartCoroutine(playerTakeDmg());

                if (moveEnemyState != EnemyState.Punching)
                {
                    moveEnemyState = EnemyState.Punching;
                    ChangeAnimator();
                    //_PlayerStateChangd

                }
            }
            
        }

        if (moveEnemyState == EnemyState.IDLE)
        {
            StartCoroutine(WaitThenRoam());
            
        }
        if (roaming)
        {
            moveEnemyState = EnemyState.RUNNING;
            if (!hitRightWaypoint)
            {
                transform.position = Vector2.MoveTowards(transform.position, RightWaypoint.transform.position, 2f * Time.deltaTime);
                _IsGoingRight = false;
            }
            else if (!hitLeftWaypoint)
            {
                transform.position = Vector2.MoveTowards(transform.position, LeftWaypoint.transform.position, 2f * Time.deltaTime);
                _IsGoingRight = true;
                 
            }
            ChangeAnimator();

        }

        

        float distanceFromPlayer = Vector2.Distance(Player.transform.position, transform.position);

        if (distanceFromPlayer < 6f && !InAttackRange)
        {
            roaming = false;
            transform.position = Vector2.MoveTowards(transform.position, Player.transform.position, 4f * Time.deltaTime);
            moveEnemyState = EnemyState.RUNNING;

            if (Player.transform.position.x > transform.position.x)
            {
                _IsGoingRight = false;
            }
            else
            {
                _IsGoingRight = true;
            }


        }
        else if (!roaming && distanceFromPlayer >6)
        {
            moveEnemyState = EnemyState.IDLE;
        }
        ChangeAnimator();
    }

    IEnumerator playerTakeDmg()
    {
        yield return new WaitForSeconds(2f);
        PlayerScript.PlayerTakeDamage();
        takinghit = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("LeftWaypoint"))
        {
            hitLeftWaypoint = true;
            hitRightWaypoint = false;
        }
        if (collision.gameObject.CompareTag("RightWaypoint"))
        {
            hitRightWaypoint = true;
            hitLeftWaypoint = false;
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            InAttackRange = true;
            
        }


    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                InAttackRange = false;
                //PlayerScript = null;
            }


        }
    }

    public void ChangeAnimator()
    {
        RuntimeAnimatorController newAnimator = MoveIdleController;

        if (moveEnemyState == EnemyState.RUNNING)
        {
            newAnimator = MoveRunningController;
            if (_IsGoingRight)
            {
                gameObject.GetComponent<SpriteRenderer>().flipX = false;

            }
            else
            {
                gameObject.GetComponent<SpriteRenderer>().flipX = true;

            }
        }

        if (moveEnemyState == EnemyState.Punching)
        {
            newAnimator = MovePunchController;
            if (_IsGoingRight)
            {
                gameObject.GetComponent<SpriteRenderer>().flipX = false;

            }
            else
            {
                gameObject.GetComponent<SpriteRenderer>().flipX = true;

            }
        }

        gameObject.GetComponent<Animator>().runtimeAnimatorController = newAnimator;
    }

    public void takeDamage()
    {
        enemyHealth -= 25;
    }
    IEnumerator WaitThenRoam()
    {
        yield return new WaitForSeconds(5);

        roaming = true;

        
    }
}
