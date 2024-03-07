using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class PlayerSingleton:Singleton<PlayerSingleton>
{
    public PlayerMain PlayerMain { get; set; }

    public BagManager Bag { get; private set; } 
    public CharacterManager Character { get; private set; } 
    public PlayerAvatarManager Avatar { get; private set; }
    public PlayerState State { get; private set; }


    public PlayerSingleton()
    {
        Bag = new BagManager(this);
        Character = new CharacterManager(this);
        Avatar= new PlayerAvatarManager(this);
        State= new PlayerState(this);
    }

    protected override void Init()
    {
        base.Init();

        Bag.Init();
        Character.Init();
        Avatar.Init();
        State.Init();
    }


    public void Update()
    {
        State.Update();

    }



}
