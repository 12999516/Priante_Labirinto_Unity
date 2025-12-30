using sistema_input;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.InputSystem;

public class player : MonoBehaviour
{
    player_movement player_movement;
    CharacterController characterController;
    Animator animator;

    int iswalkinghash;
    int isrunninghash;

    Vector2 current_movement_input;
    Vector3 currentmovement;
    Vector3 currentrunmovement;
    bool ismovementpressed;
    bool isrunpressed;
    float runmultiplier = 3.0f;
    float rotationfactorperframe = 15.0f;

    private void Awake()
    {
        player_movement = new player_movement();
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        isrunninghash = Animator.StringToHash("isrunning");
        iswalkinghash = Animator.StringToHash("iswalking");

        player_movement.player.move.started += onmovementinput;
        player_movement.player.move.canceled += onmovementinput;
        player_movement.player.move.performed += onmovementinput;
        player_movement.player.sprint.started += onrun;
        player_movement.player.sprint.canceled += onrun;
    }

    void onmovementinput(InputAction.CallbackContext context)
    {
        //Debug.Log(currentmovement);
        current_movement_input = context.ReadValue<Vector2>();
        currentmovement.x = current_movement_input.x;
        currentmovement.z = current_movement_input.y;
        currentrunmovement.x = current_movement_input.x * runmultiplier;
        currentrunmovement.z = current_movement_input.y * runmultiplier;
        ismovementpressed = current_movement_input.x != 0 || current_movement_input.y != 0;
    }

    void onrun(InputAction.CallbackContext context)
    {
        isrunpressed = context.ReadValueAsButton();
    }

    void handlerotation()
    {
        Vector3 positiontolookat;

        positiontolookat.x = currentmovement.x;
        positiontolookat.y = 0.0f;
        positiontolookat.z = currentmovement.z;

        Quaternion currentrotation = transform.rotation;

        if(ismovementpressed)
        {
            Quaternion targetrotation = Quaternion.LookRotation(positiontolookat);
            transform.rotation = Quaternion.Slerp(currentrotation, targetrotation, rotationfactorperframe * Time.deltaTime);
        }


    }

    void handleanimation()
    {
        bool iswalking = animator.GetBool("iswalking");
        bool isrunning = animator.GetBool("isrunning");

        if (ismovementpressed && !iswalking)
        {
            animator.SetBool("iswalking", true);
        }
        else if (!ismovementpressed && iswalking)
        {
            animator.SetBool("iswalking", false);
        }

        if((!ismovementpressed && isrunpressed) && !isrunning)
        {
            animator.SetBool("isrunning", true);
        }else if((!ismovementpressed || !isrunpressed) && !isrunning)
        {
            animator.SetBool("isrunning", false);
        }
    }

    float verticalVelocity;

    void handlegravity()
    {
        if (characterController.isGrounded)
        {
            if (verticalVelocity < 0)
                verticalVelocity = -2f; // piccolo valore
        }
        else
        {
            verticalVelocity += -9.81f * Time.deltaTime;
        }
    }


    /*void handlegravity()
    {
        if(characterController.isGrounded)
        {
            float groundedgravity = -.05f;
            currentmovement.y = groundedgravity;
            currentrunmovement.y = groundedgravity;
        }
        else
        {
            float gravityacceleration = -9.81f;
            currentmovement.y += gravityacceleration;
            currentrunmovement.y += gravityacceleration;
        }
    }*/

    private void Update()
    {

        handlegravity();
        handlerotation();
        handleanimation();

        Vector3 move = isrunpressed ? currentrunmovement : currentmovement;
        move.y = verticalVelocity;

        characterController.Move(move * Time.deltaTime);


        if (isrunpressed)
        {
            characterController.Move(currentrunmovement * Time.deltaTime);
        } else
        {
            characterController.Move(currentmovement * Time.deltaTime);
        }


    }

    private void OnEnable()
    {
        player_movement.player.Enable();
    }

    private void OnDisable()
    {
        player_movement.player.Disable();
    }
}
