using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;


[RequireComponent(typeof(BoxCollider))]
public class TeleportOutPosition : MonoBehaviour
{

    public string scenePath;
    public string sceneName;
    public TeleportPosEnum TeleportPos;

    private BoxCollider boxCollider;

    private void Start()
    {
        boxCollider= GetComponent<BoxCollider>();
        
    }

    private void OnEnable()
    {
    }

    private void OnDisable()
    {
    }

    public void Update()
    {
        var player = PlayerSingleton.Instance.PlayerMain;
        if (player == null)
            return;
        if (player.PlayerMoveController.inputY > 0 && player.PlatformRigidBody.onPlatform)
        {

            if (boxCollider.bounds.Contains(player.transform.position))
                MapManager.Instance.Teleport(sceneName, TeleportPos);
        }

    }


}
