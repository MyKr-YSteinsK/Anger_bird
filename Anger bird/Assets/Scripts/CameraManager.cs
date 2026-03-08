using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    //默认镜头位置
    [SerializeField] private CinemachineVirtualCamera _idleCam;
    //跟随镜头位置
    [SerializeField] private CinemachineVirtualCamera _followCam;
    private void Awake() 
    {
        SwitchToIdleCam();
    }
    //切换到默认静止镜头
    public void SwitchToIdleCam()
    {
        _idleCam.enabled = true;
        _followCam.enabled = false;
    }
    //切换到跟随镜头
    public void SwitchToFollowCam(Transform followTransform)
    {
        _followCam.Follow = followTransform;
        
        _idleCam.enabled = false;
        _followCam.enabled = true;
    }
}
