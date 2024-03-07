using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Windows;

//�ṩһ��λ�ø����׷�٣�Ŀ����ͻ����Ծ������������;�ͷ��ҡ�θ�
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



    //TODO ������޸�Ŀ�꣬��Ҫ������ �����ٶȵ�һ�׵��Ͷ��׵���ֵ ��ʵ�ֲ�����Ϸ
    public void Update()
    {

        var pos = transform.position;

        var targetPos = transform.position;
        if(IsJumping && transform.position.y >= yPosWhenEnterAir) 
            targetPos = new Vector3(pos.x, yPosWhenEnterAir, pos.z);

        var targetPosBeforeChange = targetPos;

        //TODO �ܲ���lerp����������ĳɹ̶����ƶ��ٶ�
        targetPos.y = PosLerpY(targetPos);

       CameraTrackPosition = targetPos;


        lastTargetPos = targetPosBeforeChange;
    }

    private float PosLerpY(Vector3 targetPos)
    {
        var yDiff = targetPos.y - lastTargetPos.y;

		//TODO ��Ҫ���� �Ƿ���ƽ̨��
		//һ��� ���� ���� ��xһ����lerp�� ���ǵ��䵽ƽ̨�ϵ�ʱ��
		//��Ҫ��� ����Զ��������ʱ���� lerp
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

            //��ʱlinearY ֵΪ1��0���˴�����ת��Ϊ(x-1)^2+ (y-1)^2 = 1Բ�����²���

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

    //�����ƽ̨�ϣ���ֱ���ṩ��ɫ��λ��
    //������������¼��ǰ�ĸ߶ȣ����ֻ׷������߶ȣ������˲�������
    //ֱ����ɫ�ĸ߶ȵ�������߶ȣ������ƶ�����һ��ƽ̨�ϣ����׷���Ǹ�λ��


    //����Ƿ���Ҫdamp����������ǰ׷�ٵ�λ�ú���һ��׷�ٵ�λ��̫������Ҫdamp
}
