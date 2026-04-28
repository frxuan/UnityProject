using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]private float speed;
    
    private Rigidbody _rigidbody;
    
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerControllerManager.Instance.isCanController)
        {
            Move();
        }
       
        
    }

    private void Move()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 moveDir = transform.right * horizontal + transform.forward * vertical;

        moveDir.Normalize();

        _rigidbody.velocity = new Vector3(
            moveDir.x * speed,
            _rigidbody.velocity.y,  // 保留原本的Y轴速度
            moveDir.z * speed);
       
    }

   
   

    
}
