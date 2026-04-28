using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtInteraction : BaseInteraction
{
    public Vector3 startPosition;
    public Vector3 startRotation;

    public bool isLooking=false;

    public float rotateSpeed;

    public Transform player;
    public Transform artParent;

    private float xRotation;
    private float yRotation;

    public float dis;
    protected override void BaseAwake()
    {
        base.BaseAwake();
        
    }

    protected override void BaseStart()
    {
        base.BaseStart();
        startPosition = transform.position;
        startRotation = transform.eulerAngles;
        
    }

    protected override void BaseUpdate()
    {
        base.BaseUpdate();
        if (isLooking)
        {
            RatatePaint();
        }
        
    }

    public override void Interact()
    {
        base.Interact();
        if (isLooking)
        {
            PutDownPaint();
            
        }
        else
        {
            PickUpPaint();
        }
    }

    protected virtual void PickUpPaint()
    {
        isLooking = true;
        PlayerControllerManager.Instance.isCanController = false;
        
        transform.parent = player;
        transform.localPosition = new Vector3(0, 0, dis);
        
    }


    protected virtual void PutDownPaint()
    {
        isLooking = false;
        PlayerControllerManager.Instance.isCanController = true;
        
        transform.parent = artParent;
        transform.localPosition = startPosition;
        transform.eulerAngles = startRotation;
        xRotation = 0;
        yRotation = 0;
    }

    private void RatatePaint()
    {
        float horizontal = Input.GetAxis("Horizontal"); // A/D 或 左右箭头
        float vertical = Input.GetAxis("Vertical");     // W/S 或 上下箭头

        xRotation += horizontal * rotateSpeed * Time.deltaTime; // 左右旋转（Y轴）
        yRotation += -vertical * rotateSpeed * Time.deltaTime; // 上下旋转（X轴）

        transform.eulerAngles = new Vector3(yRotation, xRotation, 0);
        
    }

}
