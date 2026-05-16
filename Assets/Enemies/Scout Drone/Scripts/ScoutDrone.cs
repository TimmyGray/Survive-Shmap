using UnityEngine;

public class ScoutDrone : EnemyController
{
    private float startX;
    private float startY;
    private float horizontalTravel;
    private float bumpHeight;
    private int dirSign;
    private float tPerSecond;
    private float t;

    private void Awake()
    {
        enabled = false;
    }

    internal override void Initialize(int? level = null)
    {
        base.Initialize(level);

        Camera cam = Camera.main;
        float camHalfHeight = cam.orthographicSize;
        float camHalfWidth = camHalfHeight * cam.aspect;
        float camTop = cam.transform.position.y + camHalfHeight;
        float camBot = cam.transform.position.y - camHalfHeight;
        float camLeft = cam.transform.position.x - camHalfWidth;

        startX = transform.position.x;
        startY = transform.position.y;

        horizontalTravel = startX - camLeft + 1f;
        bumpHeight = Random.Range(1f, camHalfHeight * 0.8f);

        if (startY + bumpHeight > camTop)
            dirSign = -1;
        else if (startY - bumpHeight < camBot)
            dirSign = 1;
        else
            dirSign = Random.value < 0.5f ? -1 : 1;

        float speed = Random.Range(0.7f, 1.2f) * enemy.Speed(this.level);
        tPerSecond = speed / horizontalTravel;
        t = 0f;

        enabled = true;
    }

    private void Update()
    {
        t += tPerSecond * Time.deltaTime;
        if (t >= 1f)
        {
            Destroy(gameObject);
            return;
        }

        float x = startX - horizontalTravel * t;
        float y = startY + dirSign * bumpHeight * Mathf.Sin(Mathf.PI * t);
        transform.position = new Vector2(x, y);
    }

    public override void Move(Vector2 direction) { }

    public override void Fire() { }

    protected override void Die()
    {
        Destroy(gameObject);
    }
}
