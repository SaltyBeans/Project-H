using System;
using UnityEngine;

namespace UnityStandardAssets.Characters.ThirdPerson
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(ThirdPersonCharacter))]
    public class AICharacterControl : MonoBehaviour
    {
        public NavMeshAgent agent { get; private set; } // the navmesh agent required for the path finding
        public ThirdPersonCharacter character { get; private set; } // the character we are controlling
        public Transform[] target; // target to aim for

        Animator animator;

        [SerializeField]
        private Transform ethanbody;

        [SerializeField]
        private Transform ethanhead;

        public Transform[] stoppoint1;
        public Transform[] stoppoint2;
        public Transform[] stoppoint3;
        public Transform[] stoppoint4;
        public Transform[] stoppoint5;
        public Transform[] stoppoint6;
        public Transform[] stoppoint7;
        public Transform[] stoppoint8;
        public Transform[] stoppoint9;


        int lookCounter;
        RaycastHit hit;
        int targetCounter;
        bool lookingState;
        bool lookedAtObjects;
        [SerializeField]
        private float lookForSeconds;
        float time;

        // Use this for initialization
        private void Start()
        {
            // get the components on the object we need ( should not be null due to require component so no need to check )
            agent = GetComponentInChildren<NavMeshAgent>();
            character = GetComponent<ThirdPersonCharacter>();
            animator = GetComponent<Animator>();

            targetCounter = 0;
            lookCounter = 0;
            lookingState = false;
            lookedAtObjects = true;
            agent.updateRotation = true;
            agent.updatePosition = true;
            agent.speed = 0.5f;

        }
        void FixedUpdate()
        {
            Physics.Raycast(ethanhead.transform.position, ethanhead.transform.up, out hit, 1.5f);
        }

        // Update is called once per frame
        private void Update()
        {
            if (lookingState == false)      //If player not looking, walking
            {

                if (target != null)
                {
                    agent.SetDestination(target[targetCounter].position);

                    int boundCheck = targetCounter + 1;

                    if (Vector3.Distance(gameObject.transform.position, target[targetCounter].GetComponent<Transform>().position) < 1.5 && boundCheck < target.Length)
                    {
                        lookingState = true;              // Not walking, looking
                        lookedAtObjects = false;     //Didn't look at every object in the current -new- room
                        time = Time.time;
                        lookCounter = 0;
                        Debug.Log("stopped");
                        agent.Stop();
                        agent.updateRotation = false;
                    }

                    else // If not close to the target
                    {
                        // use the values to move the character
                        character.Move(agent.desiredVelocity, false, false);
                    }

                }

            }

            else //If player not walking, looking
            {
                character.Move(agent.desiredVelocity, false, false);

                if (lookedAtObjects == false)   /*  I might not need this. */
                {

                    if (lookCounter < getRoom().Length && Time.time - time > lookForSeconds) // LookCounter is lower than length and current time - last looked time is bigger than lookForSeconds
                    {
                        lookCounter++;              //Get the next object to look at, at the next frame.
                        time = Time.time;
                    }

                    if (lookCounter == getRoom().Length)    //Looked at all the objects. Reset the parameters.
                    {
                        lookedAtObjects = true; //Looked at every object in the room
                        lookingState = false;       //Change the looking state to walking.
                        targetCounter++;            //Change the walking target.
                        lookCounter = 0;            //Reset the looking counter.
                        agent.Resume();            //Resume the navigation.
                    }

                }

            }
            if (hit.collider.tag == "door")
            {
                if (hit.collider.GetComponent<DoorScript>().getState() == false)
                {
                    hit.collider.GetComponent<DoorScript>().OpenDoor();
                }
            }
        }

        void LateUpdate()
        {
            if (lookedAtObjects == false && lookingState == true)
            {
                Quaternion firstRot = ethanbody.transform.rotation;
                ethanbody.transform.LookAt(getRoom()[lookCounter].GetComponent<Transform>().position);
                firstRot.y = ethanbody.transform.localRotation.y;
                ethanbody.transform.localRotation = firstRot;

                ethanhead.transform.LookAt(getRoom()[lookCounter].GetComponent<Transform>().position); //Look at those objects.
                ethanhead.transform.Rotate(90, 0, 0);
                ethanhead.transform.Rotate(0, -90, 0);

                Debug.Log("looking at: " + getRoom()[lookCounter].GetComponent<Transform>().name);
            }
        }
        Transform[] getRoom()
        {
            switch (targetCounter)
            {
                case 0:
                    return stoppoint1;
                case 1:
                    return stoppoint2;
                case 2:
                    return stoppoint3;
                case 3:
                    return stoppoint4;
                case 4:
                    return stoppoint5;
                case 5:
                    return stoppoint6;
                case 6:
                    return stoppoint7;
                case 7:
                    return stoppoint8;
                case 8:
                    return stoppoint9;
                default:
                    return null;
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.tag == "SpeedCollider")
            {
                if (other.name == "RampCollider")
                {
                    agent.speed = 5;
                }
                if (other.name == "HouseCollider")
                {
                    agent.speed = 0.5f;
                }
            }
        }
        public void SetTarget(Transform target)
        {
            this.target[targetCounter] = target;
        }
    }
}
