using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

public enum EnemyState
{
    IDLE,
    RUNNING,
    DEAD
}
public class enemyController : MonoBehaviour
{

    public EnemyState moveEnemyState = EnemyState.IDLE;

    [Header("Movement")]
    public float MoveSpeed = 400.0f;

    public GameObject Player;

    [Header("StateSprites")]
    public RuntimeAnimatorController MoveIdleController;
    public RuntimeAnimatorController MoveRunningController;
    //public RuntimeAnimatorController MoveJumpingController;

    private Animator _MoveAnimatorComponent;

    private bool _IsGoingRight = true;
    private bool _PlayerStateChangd = false;
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
        //if (moveEnemyState == EnemyState.IDLE)
        //{

            float distanceFromPlayer = Vector2.Distance(Player.transform.position, transform.position);

            if (distanceFromPlayer < 3f)
            {
                transform.position = Vector2.MoveTowards(transform.position, Player.transform.position, 5f * Time.deltaTime);
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
            else
            {
                moveEnemyState = EnemyState.IDLE;
            }
        //}
            ChangeAnimator();
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

        gameObject.GetComponent<Animator>().runtimeAnimatorController = newAnimator;
    }
}
