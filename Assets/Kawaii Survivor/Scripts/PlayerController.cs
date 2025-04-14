using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    /// Variables
    private Rigidbody2D rig;
    [SerializeField] private MobileJoystick playerJoystick;
    [SerializeField] private float speed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        rig.velocity = Vector2.right;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private  void FixedUpdate() {
        rig.velocity = playerJoystick.GetMoveVector() * speed * Time.deltaTime;
    }
}
