using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapManager:Singleton<MapManager>
{
    public TeleportPosEnum DestPos { get; private set; } = TeleportPosEnum.Default;

    public Action BeforeLoadScene = delegate { };

    public void Teleport(string sceneName, TeleportPosEnum teleportPosEnum)
    {
        DestPos = teleportPosEnum;
        LoadScene(sceneName);
        Debug.Log($"teleport to {sceneName} {teleportPosEnum}");
    }

    public void LoadScene(string sceneName)
    {
        BeforeLoadScene();
        SceneManager.LoadScene(sceneName);
    }

}
