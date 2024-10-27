using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float playerSpeed; 
    public Character player; 

    private Transform characterBody;

    private Rigidbody2D myRigidbody2D;

    private bool isMoving = false;

    private Vector2 moveDirection;
    private Vector2 lookDirection;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        characterBody = transform.Find("Aim");
        Debug.Log(characterBody.name);
        myRigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (isMoving) 
        {
            Moving();
        }   
    }

    #region Functions are calling on Input events
    public void OnMove(InputValue moveValue)
    {
        moveDirection = moveValue.Get<Vector2>().normalized;

        if (Mathf.Abs(moveDirection.magnitude) > 0)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }
    }

    public void OnLook(InputValue lookValue)
    {
        lookDirection = lookValue.Get<Vector2>().normalized;

        if (Mathf.Abs(lookDirection.magnitude) > 0)
        {
            //Should get current mouse pointer position and rotate aim according this value.
        }
    }

    public void onAttack(InputValue attackValue) 
    { 

    }
    #endregion

    public void Moving()
    {
        myRigidbody2D.AddForce(new Vector2(moveDirection.x * playerSpeed, moveDirection.y * playerSpeed));
    }
}
