using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BossState
{
    IDLE,
    RUNNING,
    Punching,
    DEAD
}
public class bossController : MonoBehaviour
{
    public int BossHealth = 100;
    public bool InAttackRange = false;
    private CharacterController PlayerScript;

    private Vector3 Pos;
    public GameObject LeftWaypoint;
    public GameObject RightWaypoint;

    public GameObject doordoor;

    private bool hitRightWaypoint = false;
    private bool hitLeftWaypoint = false;
    public bool roaming = false;

    public BossState moveBossState = BossState.IDLE;

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

    private bool isDead = false;
    void Start()
    {
        _MoveAnimatorComponent = gameObject.GetComponent<Animator>();
        _MoveAnimatorComponent.runtimeAnimatorController = MoveIdleController;
        Player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (BossHealth <= 0)
        {
            Destroy(doordoor);
            Destroy(gameObject);

        }

        if (!takinghit)
        {
            PlayerScript = Player.GetComponent<CharacterController>();
            if (InAttackRange)
            {
                takinghit = true;
                StartCoroutine(playerTakeDmg());

                if (moveBossState != BossState.Punching)
                {
                    moveBossState = BossState.Punching;
                    ChangeAnimator();
                    //_PlayerStateChangd

                }
            }

        }

        if (moveBossState == BossState.IDLE)
        {
            StartCoroutine(WaitThenRoam());

        }
        if (roaming)
        {
            moveBossState = BossState.RUNNING;
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
            moveBossState = BossState.RUNNING;

            if (Player.transform.position.x > transform.position.x)
            {
                _IsGoingRight = false;
            }
            else
            {
                _IsGoingRight = true;
            }


        }
        else if (!roaming && distanceFromPlayer > 6)
        {
            moveBossState = BossState.IDLE;
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

        if (moveBossState == BossState.RUNNING)
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

        if (moveBossState == BossState.Punching)
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
        BossHealth -= 25;
    }
    IEnumerator WaitThenRoam()
    {
        yield return new WaitForSeconds(5);

        roaming = true;


    }
}
