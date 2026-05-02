using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum CharacterState
{
    IDLE,
    RUNNING,
    JUMPING,
    DEAD
}

public class CharacterController : MonoBehaviour
{
    
    public CharacterState movePlayerState = CharacterState.IDLE;

    [Header("Movement")]
    public float MoveSpeed = 500.0f;
    public float MoveJumpStrength = 10.0f;

    [Header("StateSprites")]
    public RuntimeAnimatorController MoveIdleController;
    public RuntimeAnimatorController MoveRunningController;
    public RuntimeAnimatorController MoveJumpingController;

    private Animator _MoveAnimatorComponent;

    private bool _IsGoingRight = true;
    private bool _PlayerStateChangd = false;

    // Start is called before the first frame update
    void Start()
    {
        _MoveAnimatorComponent = gameObject.GetComponent<Animator>();
        _MoveAnimatorComponent.runtimeAnimatorController = MoveIdleController;
    }

    // Update is called once per frame
    void Update()
    {
        _PlayerStateChangd = false; 
        if(movePlayerState == CharacterState.IDLE)
        {
            if (Input.GetKey(KeyCode.D) || (Input.GetKey(KeyCode.A)))
            {
                _PlayerStateChangd = true;
                movePlayerState = CharacterState.RUNNING;
                if (Input.GetKey(KeyCode.D))
                {
                    _IsGoingRight = true;
                }
                else
                {
                    _IsGoingRight = false;
                }
                
            } 
            else if (Input.GetKey(KeyCode.W))
            {
                gameObject.GetComponent<Rigidbody2D>().velocity =transform.up * MoveJumpStrength;
                _PlayerStateChangd = true;
                movePlayerState = CharacterState.JUMPING;
                StartCoroutine("CheckGrounded");
            }
            
        }
        else if (movePlayerState == CharacterState.RUNNING)
        {
            if ((Input.GetKey(KeyCode.W) || (Input.GetKey(KeyCode.Space))))
            {
                gameObject.GetComponent<Rigidbody2D>().velocity = transform.up * MoveJumpStrength;
                _PlayerStateChangd = true;
                movePlayerState = CharacterState.JUMPING;
                StartCoroutine("CheckGrounded");
            }
            else if (!Input.GetKey(KeyCode.D) && (!Input.GetKey(KeyCode.A)))
            {
                _PlayerStateChangd = true;
                movePlayerState = CharacterState.IDLE;
            }
            ChangeAnimator();

        }

        if (movePlayerState == CharacterState.JUMPING || movePlayerState == CharacterState.RUNNING)
        {
            if (Input.GetKey(KeyCode.D))
            {
                _IsGoingRight = true;
                transform.Translate(transform.right * Time.deltaTime * MoveSpeed);
            }
            else if (Input.GetKey(KeyCode.A))
            {
                _IsGoingRight= false;
                transform.Translate (-transform.right * Time.deltaTime * MoveSpeed);
            }

            ChangeAnimator();
        }

    }

    IEnumerator CheckGrounded() 
    {
        yield return new WaitForSeconds(0.5f);

        while (true)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position - Vector3.up * 1.3f, -Vector2.up, 0.5f);
            if (hit.collider != null)
            {
                if (hit.transform.tag == "Terrain")
                {
                    if ((Input.GetKey(KeyCode.D) || (Input.GetKey(KeyCode.A))))
                    {
                        movePlayerState = CharacterState.RUNNING;
                    }
                    else
                    {
                        movePlayerState = CharacterState.IDLE;
                    }
                    break;
                }
                
            }
            yield return new WaitForSeconds(0.05f);
        
        }

        ChangeAnimator();
        yield return null;
    }

    public void ChangeAnimator()
    {
        RuntimeAnimatorController newAnimator = MoveIdleController;

        if (movePlayerState == CharacterState.RUNNING /*|| movePlayerState == CharacterState.JUMPING*/)
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

        if (movePlayerState == CharacterState.JUMPING)
        {
            newAnimator = MoveJumpingController;
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

