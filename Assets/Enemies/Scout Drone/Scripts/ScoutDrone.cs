using UnityEngine;
using Weapons;

public class ScoutDrone : EnemyController
{
    private Vector2 startPosition;
    private Vector2 endPosition;
    private float horizontalTravel;
    private float bumpHeight;
    private CHANGE_DIRECTION dirSign;
    private float progressPerSecond;
    private float t = 0f;

    private const float FireAngle = 180f; // Drones fly and shoot to the left

    private WeaponController weaponController;
    private bool isAllowedToFire = false;

    private void Awake()
    {
        enabled = false;
    }

    internal override void Initialize(EnemyInitializeSettings initializeSettings)
    {
        base.Initialize(initializeSettings);

        MovementInitialize();

        WeaponInitialize();

        enabled = true;

    }

    void FixedUpdate()
    {
        Move();

        if (isAllowedToFire)
        {
            Fire();
        }
    }

    private void MovementInitialize()
    {
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

        int speed = Mathf.RoundToInt(Random.Range(0.7f, 1.2f) * enemy.Speed(level));
        progressPerSecond = speed / horizontalTravel;
        t = 0f;
    }

    private void WeaponInitialize()
    {
        if (enemy.currentWeapons.Count > 0)
        {
            Transform weaponSlot = FindWeaponSlot();
            if (weaponSlot == null)
            {
                isAllowedToFire = false;
                return;
            }

            GameObject weapon = Instantiate(enemy.currentWeapons[0], weaponSlot.position, Quaternion.identity);
            weapon.transform.SetParent(weaponSlot);

            // Fire the instance attached to this drone, not the prefab asset.
            isAllowedToFire = weapon.TryGetComponent(out weaponController);
        }
    }

    /// <summary>
    /// Find the "Weapon Slot" among this drone's own children (not the whole scene).
    /// </summary>
    private Transform FindWeaponSlot()
    {
        foreach (Transform child in GetComponentsInChildren<Transform>())
        {
            if (child.CompareTag("Weapon Slot"))
            {
                return child;
            }
        }

        return null;
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
            Destroy(gameObject);
        }
    }

    public override void Fire()
    {
        weaponController.Fire(gameObject, enemy.MaxDmg(level), enemy.MinDmg(level), FireAngle);
    }
}
