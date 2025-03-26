using RPG.Attributes;
using RPG.Core;
using RPG.Saving;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement 
{
    public class Mover : MonoBehaviour, IAction, ISaveable
    {

        [SerializeField] float maxSpeed = 10f;
        [SerializeField] float maxNavPathLength = 40f;
        NavMeshAgent navMeshAgent;
        Health health;

        void Awake()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            health = GetComponent<Health>();
        }

        void Start(){

        }

        void Update()
        {
            //disable collision of dead player
            navMeshAgent.enabled = !health.IsDead();
            UpdateAnimatior();
        }

        public void StartMoveAction(Vector3 destination, float speedFraction)
        {
            //stop fight with movement - if player is in action, terminate with movement
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination, speedFraction);
            
        }

        public bool CanMoveTo(Vector3 destination)
        {
            NavMeshPath path = new NavMeshPath();
            bool hasPath = NavMesh.CalculatePath(transform.position, destination, NavMesh.AllAreas, path);
            if (!hasPath) return false;
            if (path.status != NavMeshPathStatus.PathComplete) return false;
            if (GetPathLength(path) > maxNavPathLength) return false;

            return true;
        }

        public void MoveTo(Vector3 destination, float speedFraction)
        {
            //moving machine 
            navMeshAgent.destination = destination;
            navMeshAgent.speed = maxSpeed * Mathf.Clamp01(speedFraction);
            navMeshAgent.isStopped = false;
        }

        public void Cancel()
        {
            //stoping machine -> stop moving
            navMeshAgent.isStopped = true;  
        }

    

        private void UpdateAnimatior(){
            //update animation 
            Vector3 velocity = navMeshAgent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;

            GetComponent<Animator>().SetFloat("forwardSpeed", speed);
        }

        private float GetPathLength(NavMeshPath path)
        {
            float total = 0;
            if (path.corners.Length < 2) return total;
            for (int i = 0; i < path.corners.Length - 1; i++)
            {
                total += Vector3.Distance(path.corners[i], path.corners[i + 1]);
            }

            return total;
        }

        public object CaptureState()
        {
            return new SerializableVector3(transform.position);
        }

        public void RestoreState(object state)
        {
            SerializableVector3 postition = (SerializableVector3) state;
            navMeshAgent.enabled = false;
            transform.position = postition.ToVector();
            navMeshAgent.enabled = true ;
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

    }
}
