//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using UnityEngine;

//public class ChaseState : StateBase
//{
//    private FSM manager;
//    private Parameter parameter;

//    public ChaseState(FSM manager)
//    {
//        this.manager = manager;
//        this.parameter = manager.parameter;
//    }
//    public override void OnEnter()
//    {
//        parameter.animator.Play("Walk");
//    }

//    public override void OnUpdate()
//    {
//        manager.FlipTo(parameter.target);
//        if (parameter.target)
//            manager.transform.position = Vector2.MoveTowards(manager.transform.position,
//            parameter.target.position, parameter.chaseSpeed * Time.deltaTime);

//    }


//    public override bool CanEnterNewState(ref StateType state)
//    {

//        if (parameter.target == null ||
//            manager.transform.position.x < parameter.chasePoints[0].position.x ||
//            manager.transform.position.x > parameter.chasePoints[1].position.x)
//        {
//            state = StateType.Idle;
//            return true;
//        }

//        if (Physics2D.OverlapCircle(parameter.attackPoint.position, parameter.attackArea, parameter.targetLayer))
//        {
//            state = StateType.Attack;
//            return true;
//        }

//        return false;
//    }
//}