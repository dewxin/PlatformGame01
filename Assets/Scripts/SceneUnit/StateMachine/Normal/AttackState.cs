//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using UnityEngine;

//public class AttackState : StateBase
//{
//    private FSM manager;
//    private Parameter parameter;

//    public AttackState(FSM manager)
//    {
//        this.manager = manager;
//        this.parameter = manager.parameter;
//    }

//    public override void OnEnter()
//    {
//        parameter.animator.Play("Attack");
//    }


//    public override bool CanEnterNewState(ref StateType state)
//    {

//        var info = parameter.animator.GetCurrentAnimatorStateInfo(0);
//        if (info.normalizedTime >= .95f)
//        {
//            state = StateType.Chase;
//            return true;
//        }

//        return false;
//    }
//}
