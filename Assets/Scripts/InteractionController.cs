using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractionController : MonoBehaviour
{
    /// <summary>
    /// 射线的距离
    /// </summary>
    [SerializeField] float _interactionDistance = 10f;
    /// <summary>
    /// 被选择的物体
    /// </summary>
    [SerializeField ] BaseInteraction _baseInteraction;
    /// <summary>
    /// 玩家交互状态（比如：当将物体拿在手上时就是玩家正处于与物体交互状态）
    /// </summary>
    public bool isCanInteraction;

    /// <summary>
    /// 声音效果实例
    /// </summary>
    [SerializeField] GameObject soundEffect;
    /// <summary>
    /// 已播放过音效
    /// </summary>
    bool isPlaySound = false;

    //存储射线检测信息的变量
    public RaycastHit _raycastHit;

    

    PointerEventData _pointerEventData;
    [Header("射线可交互层级")]
    [SerializeField] private LayerMask interactionLayers;
    private void Start()
    {
        isCanInteraction = true;

        EventSystem eventSystem = EventSystem.current;
        _pointerEventData = new PointerEventData(eventSystem);
    }
    void Update()
    {
        HandleRayInteraction();
        Interact();
    }
    /// <summary>
    /// 射线处理
    /// </summary>
    void HandleRayInteraction()
    {
        Debug.DrawRay(transform.position, transform.forward * _interactionDistance, Color.red);
        if (isCanInteraction)
        {
            //发射一条射线并判断射线是否碰到物体
            if (Physics.Raycast(transform.position, transform.forward, out _raycastHit, _interactionDistance, interactionLayers))
            {
                
                //判断射线碰撞的物体是否为可交互物体
                if (_raycastHit.collider.gameObject.layer == 7)
                {

                    if (_baseInteraction != _raycastHit.collider.gameObject.GetComponent<BaseInteraction>())
                    {
                        _baseInteraction?.CancelSelect();
                    }
                    _baseInteraction = _raycastHit.collider.gameObject.GetComponent<BaseInteraction>();
                    _baseInteraction.Select(this);


                    if (!isPlaySound)
                    {
                        //播放音效：SoundEffectManager.Instance.PlaySoundEffect(soundEffect);
                        isPlaySound = true;
                    }
                }
                else
                {
                    CancelSelect();
                }
            }
            else
            {
                CancelSelect();
            }
        }
    }

    void Interact()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            _baseInteraction?.Interact();
        }
        
    }

    void CancelSelect()
    {
        _baseInteraction?.CancelSelect();
        _baseInteraction = null;
    }
}
