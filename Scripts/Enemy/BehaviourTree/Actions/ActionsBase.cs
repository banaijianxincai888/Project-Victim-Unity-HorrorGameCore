using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace ns
{   
	///summary
	///行为黑板变量类
	///summary 
	public class ActionsBase :Action
	{

        public SharedTransform sharedTransform;

        protected EnemyBlackBoard enemyBlackBoard;


        public override void OnAwake()
        {
            enemyBlackBoard = GetComponent<EnemyBlackBoard>();



        }

        public void RotateToTarget(Vector3 target)
        {
            Vector3 direc = (target - transform.position).normalized;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(direc),enemyBlackBoard.rotateSpeed * Time.deltaTime);

        }

        public void RotateToDirection(Quaternion target)
        {

            transform.rotation = Quaternion.RotateTowards(transform.rotation,target,enemyBlackBoard. rotateSpeed* Time.deltaTime);
        }

        public void AgentRotate(Vector3 target)
        {
            enemyBlackBoard. agent.enabled = true;

            //获取Agent的方向！
            enemyBlackBoard. agent.destination = target;
            //获取路径上的最新点与自身的方向，并且给旋转铺路
            var direc = (enemyBlackBoard.agent.steeringTarget - transform.position).normalized;
            //自己旋转，不用依靠Agent的旋转
            if (direc != Vector3.zero)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(direc), enemyBlackBoard.rotateSpeed * Time.deltaTime);

            }
        }

        public float DistanceXZ(Vector3 a, Vector3 b)
        {
            float dx = a.x - b.x;
            float dz = a.z - b.z;
            return Mathf.Sqrt(dx * dx + dz * dz);
        }



    }
}

