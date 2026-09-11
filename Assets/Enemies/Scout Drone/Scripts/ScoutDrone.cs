using UnityEngine;

public class ScoutDrone : EnemyController
{
    private Vector2 startPosition;
    private Vector2 endPosition;
    private float horizontalTravel;
    private float bumpHeight;
    private CHANGE_DIRECTION dirSign;
    private float progressPerSecond;
    private float t = 0f;

    private void Awake()
    {
        enabled = false;
    }

    internal override void Initialize(EnemyInitializeSettings initializeSettings)
    {
        base.Initialize(initializeSettings);

        Camera cam = Camera.main;
        float camHalfHeight = cam.orthographicSize;
        float camHalfWidth = camHalfHeight * cam.aspect;
        float camTop = cam.transform.position.y + camHalfHeight;
        float camBot = cam.transform.position.y - camHalfHeight;
        float camLeft = cam.transform.position.x - camHalfWidth;

        startPosition = transform.position;
        endPosition = new Vector2(camLeft - 2, startPosition.y); // Move off-screen to the left
        horizontalTravel = Mathf.Abs(endPosition.x - startPosition.x);
        bumpHeight = Random.Range(1f, camHalfHeight * 0.8f);

        if (startPosition.y + bumpHeight > camTop)
            dirSign = CHANGE_DIRECTION.DECREASE;
        else if (startPosition.y - bumpHeight < camBot)
            dirSign = CHANGE_DIRECTION.INCREASE;
        else
            dirSign = Random.value < 0.5f ? CHANGE_DIRECTION.DECREASE : CHANGE_DIRECTION.INCREASE;

        float speed = Random.Range(0.7f, 1.2f) * enemy.Speed(level);
        progressPerSecond = speed / horizontalTravel;
        t = 0f;

        enabled = true;

        Debug.Log($"ScoutDrone initialized with level {level}, health {currentHealth}, speed {speed}, bumpHeight {bumpHeight}, dirSign {dirSign}, progressPerSecond {progressPerSecond}, horizontalTravel {horizontalTravel}, starting position ({startPosition.x}, {startPosition.y}), ending position ({endPosition.x}, {endPosition.y})");
    }

    void Update()
    {
        Move();
    }

    public override void Move()
    {
        t = Mathf.Min(t + progressPerSecond * Time.deltaTime, 1f);

        // Always calculate from the original spawn position. Applying the total
        // offset to transform.position every frame compounds the movement.
        Vector2 position = Vector2.Lerp(startPosition, endPosition, t);
        position.y += (int)dirSign * bumpHeight * Mathf.Sin(Mathf.PI * t);
        transform.position = position;

        if (t >= 1f)
        {
            Debug.Log("ScoutDrone has exited the screen and will be destroyed.");
            Die();
        }
    }

    public override void Fire() { }

    protected override void Die()
    {
        Destroy(gameObject);
    }
}
