using UnityEngine;

public class LazerProjectile : MonoBehaviour
{
    private Rigidbody2D _rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Launch(float projectileSpeed, float? angle = null)
    {
        if (angle != null)
        {
            transform.eulerAngles = new Vector3(0, 0, angle.Value);
        }

        // Convert angle to direction vector
        Vector2 direction = transform.right; // This gets the local right direction based on rotation
        _rb.AddForce(direction * projectileSpeed, ForceMode2D.Impulse);     
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
