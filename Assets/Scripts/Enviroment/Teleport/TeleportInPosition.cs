using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class TeleportInPosition:MonoBehaviour
{

    public TeleportPosEnum TeleportEnum;


    private void Start()
    {
        if (TeleportEnum == MapManager.Instance.DestPos)
        {

            PlayerSingleton.Instance.PlayerMain.SetPosWhenChangeScene(transform.position);
        }
    }


}
