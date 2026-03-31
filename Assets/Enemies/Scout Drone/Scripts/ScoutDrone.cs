using UnityEngine;

public class ScoutDrone : EnemyController
{
    // Ellipse parameters
    private float ellipseA; // Horizontal radius
    private float ellipseB; // Vertical radius
    private float ellipseAngle; // Current angle (0 to pi)
    private float ellipseSpeed; // How fast to move along the ellipse
    private Vector2 ellipseCenter; // Center of the ellipse
    private float verticalOffset; // Random vertical offset
    private bool initialized = false;

    protected override void Initialize(int? level = null)
    {
        base.Initialize(level);
        InitializeEllipse();
    }

    /// <summary>
    /// Initialize the ellipse parameters for the drone movement
    /// </summary>
    private void InitializeEllipse()
    {
        // Set ellipse radii randomly within a range (for individual drones)
        ellipseA = Random.Range(3f, 7f); // Horizontal radius
        ellipseB = Random.Range(1f, 3f); // Vertical radius
        ellipseAngle = 0f;
        ellipseSpeed = Random.Range(0.7f, 1.2f) * enemy.Speed(level);
        // Start at right edge of the screen
        float camHeight = 2f * Camera.main.orthographicSize;
        float camWidth = camHeight * Camera.main.aspect;
        float startY = Random.Range(-camHeight / 2f + 1f, camHeight / 2f - 1f);
        ellipseCenter = new Vector2(
            Camera.main.transform.position.x + camWidth / 2f - ellipseA,
            startY
        );
        verticalOffset = startY;

        initialized = true;
        // Set initial position
        SetPositionOnEllipse();
    }

    private void Update()
    {
        if (!initialized)
            return;
        ellipseAngle += ellipseSpeed * Time.deltaTime / ellipseA; // Normalize speed by horizontal radius
        if (ellipseAngle > Mathf.PI) // Only half ellipse
        {
            Destroy(gameObject);
            return;
        }
        SetPositionOnEllipse();
    }

    private void SetPositionOnEllipse()
    {
        float x = ellipseCenter.x - ellipseA * Mathf.Cos(ellipseAngle);
        float y = ellipseCenter.y + ellipseB * Mathf.Sin(ellipseAngle);
        transform.position = new Vector2(x, y);
    }

    public override void Move(Vector2 direction)
    {
        // Movement is handled in Update() along the ellipse
    }

    public override void Fire()
    {
        // TODO: Implement firing logic if needed
    }

    protected override void Die()
    {
        Destroy(gameObject);
    }
}
