using UnityEngine;

public class PlasmaProjectile : MonoBehaviour
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

    public void Launch(float speed, float? angle = null)
    {
        _rb.AddForce(new Vector2(speed,0), ForceMode2D.Impulse);
    }

    void OnBecameInvisible()
    {
        // Destroy the projectile when it leaves the camera view
        Destroy(gameObject);
    }
}
