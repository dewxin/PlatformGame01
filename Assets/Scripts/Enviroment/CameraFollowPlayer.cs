using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    private Camera camera;
    public PlayerTrack PlayerTrack { get; private set; }

    [Range(0f,1f)]
    public float yOffsetRatio = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        var player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMain>();
        player.CameraMove = this;
        PlayerTrack = player.PlayerTrack;

        camera = GetComponent<Camera>();

        SetCameraPosAsPlayer();

        DontDestroyOnLoad(this.gameObject);
    }

    void LateUpdate()
    {

        FollowPlayer();
    }


    public void SetCameraPosAsPlayer()
    {
        var position = GetCameraPos(PlayerTrack.CameraTrackPosition);
        if(Boundry.MapBoundry != null)
            position = ClampPosInBoundry(position);
        this.transform.position = position;
    }

    private Vector3 GetCameraPos(Vector3 playerPos)
    {
        return new Vector3(playerPos.x, playerPos.y, transform.position.z) + Vector3.up * camera.orthographicSize *2 * yOffsetRatio;
    }

    private void FollowPlayer()
    {
        var dest = GetCameraPos(PlayerTrack.CameraTrackPosition);

        if(Boundry.MapBoundry != null)
            transform.position = ClampPosInBoundry(dest);
    }

    public Vector2 CameraHalfSize()
    {
        var size = new Vector2(camera.orthographicSize * camera.aspect, camera.orthographicSize);
        return size;
    }

    public Rect CameraPostionBoundry()
    {
        var padding = CameraHalfSize();
        var cameraBoundry = Boundry.MapBoundry.GetBoundryPadding(padding);
        return cameraBoundry;
    }

    public Vector2 CameraPosInBoundry01()
    {
        var cameraPosBoundry = CameraPostionBoundry();

        var pos = new Vector2(transform.position.x, transform.position.y);
        var posFloat = (pos - cameraPosBoundry.min) / cameraPosBoundry.size;
        return posFloat;
    }

    public Vector2 CameraPosRelative2Center()
    {
        var cameraPosBoundry = CameraPostionBoundry();

        var pos = new Vector2(transform.position.x, transform.position.y);
        return pos - cameraPosBoundry.center;
    }

    private Vector3 ClampPosInBoundry(Vector3 position)
    {

        var cameraBoundry = CameraPostionBoundry();

        if (position.x> cameraBoundry.xMax)
            position.x = cameraBoundry.xMax;

        if (position.x < cameraBoundry.xMin)
            position.x = cameraBoundry.xMin;

        if (position.y > cameraBoundry.yMax)
            position.y = cameraBoundry.yMax;

        if (position.y < cameraBoundry.yMin)
            position.y = cameraBoundry.yMin;

        return position;
    }



}
