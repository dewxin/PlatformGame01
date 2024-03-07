
public enum StateType
{
    None, Idle, Patrol, HitRecovery, Dead
}

public abstract class StateBase
{
    public abstract StateType Type { get;}
    public StateManagerBase Manager { get; set; }
    public virtual void OnStart() { }
    public virtual void OnUpdate() { }

    public virtual void OnEnter() { }
    public virtual void OnExit() { }
}

public abstract class IdleStateBase : StateBase
{
    public override StateType Type { get { return StateType.Idle;} }
}

public abstract class PatrolStateBase :StateBase
{
    public override StateType Type { get { return StateType.Patrol; } }
}

public abstract class HitRecoveryBase : StateBase
{
    public override StateType Type { get { return StateType.HitRecovery; } }
}

public abstract class DeathStateBase : StateBase
{
    public override StateType Type { get { return StateType.Dead; } }
}
