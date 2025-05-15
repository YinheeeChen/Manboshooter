using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{

    [Header(" Player Controller Variables ")]
    [SerializeField] private MobileJoystick playerJoystick;
    private Rigidbody2D rig;

    [Header(" Player Speed ")]
    [SerializeField] private float speed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        rig.velocity = Vector2.right;
    }

    private  void FixedUpdate() {
        rig.velocity = playerJoystick.GetMoveVector() * speed * Time.deltaTime;
    }
}
