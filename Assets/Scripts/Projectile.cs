using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Sprite projectileView;
    private float projectileSpeed;
    public Projectile(Sprite projectileView, float projectileSpeed)
    {
        this.projectileView = projectileView;
        this.projectileSpeed = projectileSpeed;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
