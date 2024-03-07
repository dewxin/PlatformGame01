using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Windows;

//提供一个位置给相机追踪，目的是突出跳跃这个动作，降低镜头的摇晃感
public class PlayerTrack : MonoBehaviour
{
    public enum TrackCurve
    {
        Linear,
        Circle,
        Quadratic,
    }
    private PlatformRigidBody platformRigidBody;
    private bool IsJumping;
    private float yPosWhenEnterAir;

    private Vector3 _trackPos;
    private Vector3 lastTargetPos;


    public TrackCurve curve;
    [SerializeField]
    private float lerpYIfTargetYChangeSoMuch = 0.5f;
    [SerializeField]
    private float time2LerpComplete = 0.5f;
    [SerializeField]
    private float time2LerpYLeft = 0f;
    [SerializeField]
    private bool isLerpingY => time2LerpYLeft > 0;
    [SerializeField]
    private Vector3 originTrackPos;

    [SerializeField]
    private float gizmosRaius = 0.1f;

    [HideInInspector]
    public Vector3 CameraTrackPosition 
    { 
        get=>_trackPos;
        private set => _trackPos=value;
    }

    void Awake()
    {
        platformRigidBody = GetComponent<PlatformRigidBody>();
        ResetTrackPos();
    }

    public void ResetTrackPos()
    {
        _trackPos = transform.position;
        lastTargetPos= transform.position;
        originTrackPos = transform.position;
    }

    private void OnEnable()
    {
        platformRigidBody.OnNextPlatformNull += OnNextPlatformNull;
        platformRigidBody.OnLandPlatformFromJump += OnLandPlatformFromAir;
        platformRigidBody.OnEnterPlatform += OnEnterPlatform;
        platformRigidBody.OnLeavePlatform += OnLeavePlatform;
    }

    private void OnDisable()
    {
        platformRigidBody.OnNextPlatformNull -= OnNextPlatformNull;
        platformRigidBody.OnLandPlatformFromJump -= OnLandPlatformFromAir;
        platformRigidBody.OnEnterPlatform -= OnEnterPlatform;
        platformRigidBody.OnLeavePlatform -= OnLeavePlatform;
    }

    private void OnEnterPlatform(Platform platform)
    {
        if (platform == null)
            return;
    }

    private void OnLeavePlatform(Platform platform)
    {
    }

    private void OnNextPlatformNull(Vector3 pos, Vector2 vec)
    {
        yPosWhenEnterAir = transform.position.y;
        IsJumping = true;
    }

    private void OnLandPlatformFromAir(Platform platform)
    {
        IsJumping = false;
    }



    //TODO 这里的修改目标，主要是依靠 降低速度的一阶导和二阶导的值 来实现不晕游戏
    public void Update()
    {

        var pos = transform.position;

        var targetPos = transform.position;
        if(IsJumping && transform.position.y >= yPosWhenEnterAir) 
            targetPos = new Vector3(pos.x, yPosWhenEnterAir, pos.z);

        var targetPosBeforeChange = targetPos;

        //TODO 能不能lerp，而是相机改成固定的移动速度
        targetPos.y = PosLerpY(targetPos);

       CameraTrackPosition = targetPos;


        lastTargetPos = targetPosBeforeChange;
    }

    private float PosLerpY(Vector3 targetPos)
    {
        var yDiff = targetPos.y - lastTargetPos.y;

		//TODO 需要考虑 是否在平台上
		//一般的 下跳 就是 跟x一样的lerp， 但是当落到平台上的时候，
		//需要如果 还很远，就这种时间内 lerp
        if(Math.Abs(yDiff) > lerpYIfTargetYChangeSoMuch)
        {
            originTrackPos = _trackPos;

            time2LerpYLeft = time2LerpComplete;
        }


        if (isLerpingY)
        {

            var timeFrame = Time.deltaTime;

            time2LerpYLeft -= timeFrame;

            var linearY = time2LerpYLeft / time2LerpComplete;
            linearY = Mathf.Clamp01(linearY);

            //此时linearY 值为1～0，此处将其转化为(x-1)^2+ (y-1)^2 = 1圆的左下部分

            float lerp = 0;
            if (curve == TrackCurve.Linear)
            {
                lerp = linearY;
            }
            else if (curve == TrackCurve.Circle)
            {
                lerp = (float)(1 - Math.Sqrt(1 - linearY * linearY));
            }
            else if (curve == TrackCurve.Quadratic)
            {
                lerp = linearY * linearY;
            }
            targetPos.y = Mathf.Lerp(targetPos.y, originTrackPos.y, lerp);

            return targetPos.y;

        }

        return targetPos.y;

    }


#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(CameraTrackPosition, gizmosRaius);
    }
#endif

    //如果在平台上，则直接提供角色的位置
    //如果起跳，则记录当前的高度，相机只追踪这个高度，起到了滤波的作用
    //直到角色的高度低于这个高度，或者移动到另一个平台上，相机追踪那个位置


    //相机是否需要damp，如果相机当前追踪的位置和下一个追踪的位置太大，则需要damp
}
