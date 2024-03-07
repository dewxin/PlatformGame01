using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MovingBackground : MonoBehaviour
{
    public enum MoveType
    {
        Default,
        BasedOnWidth,
        BasedOnHeight
    }

    public float scale = 1.2f;
    private CameraFollowPlayer cameraMove;
    public MoveType moveType = MoveType.Default;
    // Start is called before the first frame update
    void Start()
    {
        this.transform.localScale = new Vector3(scale * transform.localScale.x, scale * transform.localScale.y, 1);
        cameraMove = Camera.main.GetComponent<CameraFollowPlayer>();
    }

    // Update is called once per frame


    private void LateUpdate()
    {
        MoveBackgroud();
    }

    private void MoveBackgroud()
    {
        if (Boundry.MapBoundry == null)
            return;

        if(moveType == MoveType.Default)
        {
            MoveDefault();
        }
        else if(moveType == MoveType.BasedOnWidth)
        {
            MoveBasedOnWidth();
        }

    }

    private void MoveDefault()
    {
        var pos01 = cameraMove.CameraPosInBoundry01();

        var cameraInBackgroundRect = (scale - 1) * cameraMove.CameraHalfSize() * 2;

        var cameraPos2Middle01 = (pos01 - Vector2.one * 0.5f);


        this.transform.localPosition = -cameraInBackgroundRect * cameraPos2Middle01;
    }

    private void MoveBasedOnWidth()
    {
        var cameraCenterInMapBoundry = cameraMove.CameraPostionBoundry();
        var inMapWidth = cameraCenterInMapBoundry.width;

        var cameraCenterInBackgroundBoundry = (scale - 1) * cameraMove.CameraHalfSize() * 2;
        var inBackgroundWidth = cameraCenterInBackgroundBoundry.x;

        float rate = inBackgroundWidth / inMapWidth;


        var pos = cameraMove.CameraPosRelative2Center();
        var posNew = pos * rate;
        this.transform.localPosition = - posNew;

    }

}
