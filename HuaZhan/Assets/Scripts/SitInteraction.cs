using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SitInteraction : BaseInteraction
{
    /// <summary>
    /// 玩家坐下的目标位置
    /// </summary>
    [SerializeField] GameObject _playerSitDownTarget;
    /// <summary>
    /// 玩家起立的目标位置
    /// </summary>
    [SerializeField] GameObject _playerStandUpTarget;
    /// <summary>
    /// 玩家物体
    /// </summary>
    [SerializeField] GameObject _player;
    /// <summary>
    /// 当前状态是否为坐下状态
    /// </summary>
    public bool isSitDown;
    /// <summary>
    /// 第一人称视角脚本实例的调用（我们需要调用这个脚本的方法）
    /// </summary>
    FirstPersonCamera _firstPersonCamera;
    /// <summary>
    /// 玩家的刚体
    /// </summary>
    Rigidbody _rigidbody;
    /// <summary>
    /// 玩家的碰撞体
    /// </summary>
    Collider _collider;
    //物体用dotween做动画时的状态
    public bool isDoTween;


    protected override void BaseStart()
    {
        base.BaseStart();
        _rigidbody = _player.GetComponent<Rigidbody>();
        _collider = _player.GetComponent<Collider>();
        _firstPersonCamera = _player.GetComponentInChildren<FirstPersonCamera>();
    }
    public override void Interact()
    {
        base.Interact();
        if (!isSitDown)
        {
            SitDown();
        }
        else
        {
            StandUp();
        }

    }


    private void SitDown()
    {
        //如果当前正在做坐下的动画，那么再次按交互键将就不会再次执行交互方法
        if (isDoTween == true)
        {
            return;
        }
        //隐藏交互按钮
        CancelSelect();
        //进入玩家交互状态（在交互状态时玩家不可以再与其他物体交互）
        _interactionController.isCanInteraction = false;
        //冻结玩家刚体
        _rigidbody.isKinematic = true;
        //关闭碰撞
        //_collider.enabled = false;
        //进入做移动和旋转动画状态（在这个状态下，玩家不可以进行第一人称的视角移动）
        isDoTween = true;
        //得到挂载到玩家下的摄像机，让摄像机做与玩家旋转保持一致的旋转动画
        _player.transform.GetChild(0).DOLocalRotate(new Vector3(0, 0, 0), 1.0f);
        //玩家移动动画，移动到目标位置
        _player.transform.DOMove(_playerSitDownTarget.transform.position, 1.0f);
        //玩家旋转动画，旋转到目标角度
        _player.transform.DORotate(_playerSitDownTarget.transform.eulerAngles, 1.0f).OnComplete(() =>
        {
            //进入坐下状态
            isSitDown = true;
            //显示交互按钮
            Select();
            //动画完成后，退出动画状态
            isDoTween = false;
            //调用第一人称时间设置旋转值的方法，让第一人称旋转值与目标角度保持一致，确保视角不会偏移回做动画前的位置
            _firstPersonCamera.SetRotationValue(_playerSitDownTarget.transform.eulerAngles.x, _playerSitDownTarget.transform.eulerAngles.y);
        }
            );
    }
    private void StandUp()
    {
        //如果当前正在做站立的动画，那么再次按交互键将就不会再次执行交互方法
        if (isDoTween == true)
        {
            return;
        }
        //隐藏交互按钮
        CancelSelect();
        //退出坐下状态
        isSitDown = false;
        //进入动画状态
        isDoTween = true;
        //得到挂载到玩家下的摄像机，让摄像机做与玩家旋转保持一致的旋转动画
        _player.transform.GetChild(0).DOLocalRotate(new Vector3(0, 0, 0), 1.0f);
        //玩家移动动画，移动到目标位置
        _player.transform.DOMove(_playerStandUpTarget.transform.position, 1.0f);
        //玩家旋转动画，旋转到目标角度
        _player.transform.DORotate(_playerStandUpTarget.transform.eulerAngles, 1.0f).OnComplete(() =>
        {

            //玩家退出交互状态（变成可交互）
            _interactionController.isCanInteraction = true;
            //解冻玩家的刚体
            _rigidbody.isKinematic = false;
            //打开玩家的碰撞
            //_collider.enabled = true;
            //退出动画状态
            isDoTween = false;
            //调用第一人称时间设置旋转值的方法，让第一人称旋转值与目标角度保持一致，确保视角不会偏移回做动画前的位置
            _firstPersonCamera.SetRotationValue(_playerStandUpTarget.transform.eulerAngles.x, _playerStandUpTarget.transform.eulerAngles.y);
        }
        );
    }


    protected override void ChangeInformationText()
    {
        InteractionButtonManager.Instance.ShowButton(isSitDown ? "起身" : "坐下");
    }
}
