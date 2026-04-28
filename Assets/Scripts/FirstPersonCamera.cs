using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    /// <summary>
    /// 鼠标灵敏度
    /// </summary>
    [SerializeField] private float mouseSpeed;
    /// <summary>
    /// 视角绕着x轴的旋转
    /// </summary>
    private float rotationX;
    /// <summary>
    /// 视角绕着y轴的旋转
    /// </summary>
    private float rotationY;
    void Start()
    {
        Cursor.lockState= CursorLockMode.Locked;
    }

    void Update()
    {
        if (PlayerControllerManager.Instance.isCanController)
        {
            HandleMouseInput();
            CameraRotation();
        }
    
     
    }

    public void SetRotationValue(float valeX, float valeY)
    {
        if (valeX >= 270)
        {
            valeX -= 360;
        }
        rotationX = valeX;
        rotationY = valeY;

    }

    /// <summary>
    /// 处理鼠标移动输入的数值
    /// </summary>
    void HandleMouseInput()
    {
        //获取鼠标移动的值
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        rotationX -= mouseY * mouseSpeed;
        rotationY += mouseX * mouseSpeed;
    }

    /// <summary>
    /// 摄像机旋转
    /// </summary>
    void CameraRotation()
    {
        transform.parent.rotation = Quaternion.Euler(0,rotationY,0);
        transform.localRotation= Quaternion.Euler(rotationX, 0, 0);
    }
}
