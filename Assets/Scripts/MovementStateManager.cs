using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementStateManager : MonoBehaviour
{
    #region Variables

    [Header("References")]
    public float moveSpeed = 3;
    public Animator anim;
    public CharacterController controller;    
    public Rigidbody rb;    
    public GameObject TPCam;

    //Audio
    private float minPitch = 0.5f;
    private float maxPitch = 1.5f;
    SoundManager soundManager;

    [HideInInspector] public Vector3 dir;

    private float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;
    private Transform cam;
    private PhotonView view;

    private float hzInput, vInput;
    private float targetAngle, angle;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        cam = GameObject.FindGameObjectWithTag("Main Camera").GetComponent<Transform>();
    }

    private void Start()
    {
        view = GetComponent<PhotonView>();

        if (soundManager == null)
        {
            soundManager = SoundManager.instance;
            if (soundManager == null)
            {
                Debug.LogError("Sound Manager instance not found!");
                return;
            }
        }

        if (view.IsMine)
        {
            TPCam.SetActive(true);
        }
    }

    private void Update()
    {
        //Allow movement only for the local player
        if (view.IsMine)
        {
            if (GameController.instance.gamePlaying)
            {
                GetDirectionAndMove();
            }

            else
            {
                rb.constraints = RigidbodyConstraints.FreezePosition;
                anim.SetBool("IsMoving", false);

                soundManager.stepsSource.Stop();
            }
        }
    }

    #endregion

    #region Private Methods
    //Method to calculate movement direction and move the player
    private void GetDirectionAndMove()
    {
        //Get hor and ver input from the player
        hzInput = Input.GetAxisRaw("Horizontal");
        vInput = Input.GetAxisRaw("Vertical");

        //Calculate move dir based on player input
        dir = transform.forward * vInput + transform.right * hzInput;        
        
        //Calculate the target rotation angle
        targetAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Deg2Rad + cam.eulerAngles.y;
        //Smooth the rotation
        angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);        

        //Normalize direction so the player doesn't go faster when moving diagonally
        controller.Move(dir.normalized * moveSpeed * Time.deltaTime);

        //Update move animation and audio based on player input
        if (dir.magnitude >= 0.1f)
        {
            //Audio
            if(!soundManager.stepsSource.isPlaying)
                soundManager.PlaySteps("Footsteps");

            soundManager.stepsSource.pitch = UnityEngine.Random.Range(minPitch, maxPitch);

            //Anim
            anim.SetBool("IsMoving", true);
        }
        else
        {
            //Audio
            soundManager.stepsSource.Stop();

            //Anim
            anim.SetBool("IsMoving", false);
        }            
    }
    #endregion

}
