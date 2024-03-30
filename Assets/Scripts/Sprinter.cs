using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Sprinter : MonoBehaviour
{
    #region Variables

    public MovementStateManager playerMov;
    public Animator anim;
    
    private float maxStamina = 1f;
    private float stamina = 1f;    
    private float runSpeed;
    private float walkSpeed;
    private bool isRunning;

    private Slider visualStaminaBar;
    private PhotonView view;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        visualStaminaBar = GameObject.FindGameObjectWithTag("Sprinter").GetComponent<Slider>();
    }

    private void Start()
    {
        playerMov = playerMov.GetComponent<MovementStateManager>();
        view = GetComponent<PhotonView>();
        walkSpeed = playerMov.moveSpeed;
        runSpeed = walkSpeed * 4;
    }

    private void Update()
    {
        if (view.IsMine) Sprint();
    }

    #endregion

    #region Private Methods
    //Method to check if the player is running
    private void SetRunning(bool isRunning)
    {
        this.isRunning = isRunning;
        playerMov.moveSpeed = isRunning ? runSpeed : walkSpeed;
    }

    //Method to sprint
    private void Sprint()
    {
        if (GameController.instance.gamePlaying)
        {          
            visualStaminaBar.value = stamina;

            if (Input.GetKeyDown(KeyCode.LeftShift) && playerMov.dir.magnitude >= 0.1f)
            {
                SetRunning(true);
                anim.SetFloat("Speed", 3f);
            }

            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                SetRunning(false);
                anim.SetFloat("Speed", 1f);
            }

            if (isRunning)
            {
                stamina -= Time.deltaTime;
                if (stamina < 0)
                {
                    stamina = 0;
                    SetRunning(false);
                    anim.SetFloat("Speed", 1f);
                }
            }

            //Refill the stamina bar if the player is not running
            else if (stamina < maxStamina && isRunning == false)
            {
                stamina += Time.deltaTime / 4;
            }
        }

        else
        {
            visualStaminaBar.gameObject.SetActive(false);
        }
    }
    #endregion
}
