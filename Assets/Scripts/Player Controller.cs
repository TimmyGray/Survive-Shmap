using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float playerSpeed; 
    public Character player; 

    private Transform aimBody;

    private Rigidbody2D myRigidbody2D;

    private bool isMoving = false;

    private Vector2 moveDirection;
    private Vector2 lookDirection;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        aimBody = transform.Find("Aim");
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
        Vector2 mousePosition = lookValue.Get<Vector2>();
        Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        Vector3 direction = worldMousePosition - aimBody.position;
        direction.Normalize();

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        aimBody.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
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
