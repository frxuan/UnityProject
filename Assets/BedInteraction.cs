using DG.Tweening;
using DG.Tweening.Core.Easing;
using Palmmedia.ReportGenerator.Core.Reporting.Builders;
using System.Collections;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;
public class BedInteraction : BaseInteraction
{
    public GameObject player;
    public bool isDayTime;
    public bool isOnBed;

    public Light dLinght; 
   
    [SerializeField] GameObject _playerSleepTarget;
    [SerializeField] GameObject _playerWakeUpTarget;
    public override void Interact()
    {
        base.Interact();
        if (isOnBed == false)
        {
            CancelSelect();
            _interactionController.isCanInteraction = false;
            PlayerControllerManager.Instance.isCanController = false;
            GetComponent<BoxCollider>().isTrigger = true;
            player.transform.DOMove(_playerSleepTarget.transform.position, 1.0f);
            player.transform.DORotate(_playerSleepTarget.transform.eulerAngles, 1.0f).OnComplete(() =>
            {
                isOnBed = true;
                FadeToBlack.Instance.StartFadeToBlack();
                // Invoke("StartFadeToClear", 4f);
                StartCoroutine(WaitAndFade());
            });
        }
        else
        {
            player.transform.DOMove(_playerWakeUpTarget.transform.position, 1.0f);
            player.transform.DORotate(_playerWakeUpTarget.transform.eulerAngles, 1.0f).OnComplete(() =>
            {
                PlayerControllerManager.Instance.isCanController = true;
                GetComponent<BoxCollider>().isTrigger = false;
                isOnBed = false;
                _interactionController.isCanInteraction = true;
                //调用第一人称时间设置旋转值的方法，让第一人称旋转值与目标角度保持一致，确保视角不会偏移回做动画前的位置
                player.GetComponentInChildren<FirstPersonCamera>().SetRotationValue(_playerWakeUpTarget.transform.eulerAngles.x, _playerWakeUpTarget.transform.eulerAngles.y);
            });
        }
    }

    protected override void ChangeInformationText()
    {
        InteractionButtonManager.Instance.ShowButton(isDayTime?"睡到晚上":"睡到白天");
    }
    IEnumerator WaitAndFade()
    {
        yield return new WaitForSeconds(4f);
        FadeToBlack.Instance.StartFadeToClear();
        if (isDayTime)
        {
            dLinght.color = new Color(0.127f, 0.149f, 0.19f);
            isDayTime = false;
        }
        else
        {
            dLinght.color = new Color(1, 1, 1);
            isDayTime = true;
        }

    }


}
