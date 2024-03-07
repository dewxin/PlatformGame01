using Assets.Scripts.ScriptableObj;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// todo 需要用 <see cref="SceneObjStateManager"/> 代替吗？
public class PlayerState 
{

    private PlayerSingleton playerManager;

    public SceneUnitEffectManager SceneObjStateManager => playerManager.PlayerMain.SceneEffectManager;

    public SceneUnitAvatarHolder AvatarHolder=> playerManager.PlayerMain.AvatarHolder;
    public PlatformRigidBody PlatformRigidBody => playerManager.PlayerMain.PlatformRigidBody;

    public bool IsFighting { get; set; }

    public PlayerState(PlayerSingleton playerManager)
    {
        this.playerManager = playerManager;
    }

    public bool IsOnPlatform => PlatformRigidBody.onPlatform;


    public void Init()
    {

    }

    public void Update()
    {
    }


    public bool CanInput()
    {
        if (playerManager == null || playerManager.PlayerMain == null)
            return false;

        var forbidMove = SceneObjStateManager.HasEffect<ForbidInputEffect>();
        return !forbidMove;
    }

    public bool CanUseSkill()
    {
        if (playerManager == null || playerManager.PlayerMain == null)
            return false;

        bool can = IsOnPlatform && PlatformRigidBody.PlatformDirect == Platform.Direction.Horizontal;

        return can && CanInput();
    }
}
