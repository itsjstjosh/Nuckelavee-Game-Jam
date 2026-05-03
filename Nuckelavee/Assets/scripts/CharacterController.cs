using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Burst.CompilerServices;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public enum CharacterState
{
    IDLE,
    RUNNING,
    JUMPING,
    DEAD,
    punching
}

public class CharacterController : MonoBehaviour
{

    public int Health = 100;
    public bool InAttackRange = false;
    [SerializeField] private enemyController currentEnemy;
    [SerializeField] private bossController bossScript;

    public CharacterState movePlayerState = CharacterState.IDLE;

    [Header("Movement")]
    public float MoveSpeed = 500.0f;
    public float MoveJumpStrength = 10.0f;

    [Header("StateSprites")]
    public RuntimeAnimatorController MoveIdleController;
    public RuntimeAnimatorController MoveRunningController;
    public RuntimeAnimatorController MoveJumpingController;
    public RuntimeAnimatorController MovePunchController;

    private Animator _MoveAnimatorComponent;
    [field: Header("Footsteps")]
    [field: SerializeField] public EventReference footStepsGrass { get; private set; }
    [field: SerializeField] public EventReference footStepsStone { get; private set; }
    private EventInstance _FootStepInstance;
    private EventInstance _FootStepStone;
    [field: SerializeField] public EventReference footStepswater { get; private set; }
    private EventInstance _FootStepwater;

    private bool _IsGoingRight = true;
    private bool _PlayerStateChangd = false;


    // Start is called before the first frame update
    void Start()
    {
        _FootStepwater = RuntimeManager.CreateInstance(footStepswater);
        _FootStepStone = RuntimeManager.CreateInstance(footStepsStone);
        _FootStepInstance = RuntimeManager.CreateInstance(footStepsGrass);
        _MoveAnimatorComponent = gameObject.GetComponent<Animator>();
        _MoveAnimatorComponent.runtimeAnimatorController = MoveIdleController;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateFootSteps();
        _PlayerStateChangd = false;

        if(Health <= 0)
        {
            string currentScene = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(currentScene);
        }

        

        if (movePlayerState == CharacterState.IDLE)
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
                gameObject.GetComponent<Rigidbody2D>().velocity = transform.up * MoveJumpStrength;
                _PlayerStateChangd = true;
                movePlayerState = CharacterState.JUMPING;
                StartCoroutine("CheckGrounded");
            }

        }
        else if (movePlayerState == CharacterState.RUNNING)
        {
            
            if ((Input.GetKey(KeyCode.W)))
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
        
        
        if (movePlayerState != CharacterState.punching)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _PlayerStateChangd = true;
                movePlayerState = CharacterState.punching;
                StartCoroutine(backtoIdle());
                if (InAttackRange)
                {
                    Debug.Log("HitEnemy");
                    if (currentEnemy != null)
                    {
                        currentEnemy.takeDamage();
                    }
                    
                    if (bossScript != null)
                    {
                        bossScript.takeDamage();
                    }

                }
                else { Debug.Log("no enemy"); }
            ChangeAnimator();
            }
            /*
            else if (Input.GetKeyUp(KeyCode.Space))
            {
                _PlayerStateChangd = true;
                movePlayerState = CharacterState.IDLE;
            }
            */
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
                _IsGoingRight = false;
                transform.Translate(-transform.right * Time.deltaTime * MoveSpeed);
            }

            ChangeAnimator();
        }

    }

    IEnumerator backtoIdle()
    {
        yield return new WaitForSeconds(0.6f);
        _PlayerStateChangd = true;
        movePlayerState = CharacterState.IDLE;

        ChangeAnimator();

    }
    IEnumerator CheckGrounded()
    {
        yield return new WaitForSeconds(0.5f);

        while (true)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position - Vector3.up * 1.5f, -Vector2.up, 0.8f);
            if (hit.collider != null)
            {
                if (hit.transform.tag == "Terrain" || hit.transform.tag == "stone" || hit.transform.tag == "water")
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

        if (movePlayerState == CharacterState.punching)
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

    public void Attack()
    {
        //RaycastHit2D AttackHit = Physics2D.Raycast(Vector3.up * 50f, -Vector2.up, 100f);
        
        //if (AttackHit.collider != null)
        {
            

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        {
            if (collision.gameObject.CompareTag("enemy"))
            {
                InAttackRange = true;
                currentEnemy = collision.GetComponent<enemyController>();
            }
            
            if (collision.gameObject.CompareTag("Boss"))
            {
                InAttackRange = true;
                bossScript = collision.GetComponent<bossController>();
            }
            

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        {
            if (collision.gameObject.CompareTag("enemy"))
            {
                InAttackRange = false;
                currentEnemy = null;
            }
            
            if (collision.gameObject.CompareTag("Boss"))
            {
                InAttackRange = false;
                bossScript = null;
            }


        }
    }

    
        public void PlayerTakeDamage()
        {
            Health = Health - 15;
            
        }

    

    public void UpdateFootSteps()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position - Vector3.up * 1.5f, -Vector2.up, 0.8f);
        if (movePlayerState == CharacterState.RUNNING && hit.transform.tag == "Terrain")
        {
            //Debug.Log(hit.transform.tag);
            PLAYBACK_STATE playbackstate;
            _FootStepInstance.getPlaybackState(out playbackstate);
            if (playbackstate.Equals(PLAYBACK_STATE.STOPPED))
            {
                _FootStepInstance.start();
            }
        }
        else
        {
            _FootStepInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        }
        if (movePlayerState == CharacterState.RUNNING && hit.transform.tag == "stone")
        {
            Debug.Log(hit.transform.tag);
            PLAYBACK_STATE playbackstate;
            _FootStepStone.getPlaybackState(out playbackstate);
            if (playbackstate.Equals(PLAYBACK_STATE.STOPPED))
            {
                _FootStepStone.start();
            }
        }
        else
        {
            _FootStepStone.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        }
        if (movePlayerState == CharacterState.RUNNING && hit.transform.tag == "water")
        {
            Debug.Log(hit.transform.tag);
            PLAYBACK_STATE playbackstate;
            _FootStepwater.getPlaybackState(out playbackstate);
            if (playbackstate.Equals(PLAYBACK_STATE.STOPPED))
            {
                _FootStepwater.start();
            }
        }
        else
        {
            _FootStepwater.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        }
    

    }
    void OnDestroy()
    {
        // Release memory
        _FootStepInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        _FootStepStone.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        _FootStepwater.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }

}

