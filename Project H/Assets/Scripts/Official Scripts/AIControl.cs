using System.Collections;
using UnityEngine;
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(UnityStandardAssets.Characters.ThirdPerson.ThirdPersonCharacter))]
public class AIControl : MonoBehaviour
{
    public NavMeshAgent agent { get; private set; } // the navmesh agent required for the path finding
    public UnityStandardAssets.Characters.ThirdPerson.ThirdPersonCharacter character { get; private set; } // the character we are controlling

    public Transform[] target; // target to aim for

    private int index;
    [SerializeField]
    private Transform ethanbody;

    [SerializeField]
    private Transform ethanhead;

    public Transform[] stopPoints;
    public Transform[] lookPoints;

    public Transform[] stoppoint1;
    public Transform[] stoppoint2;
    public Transform[] stoppoint3;
    public Transform[] stoppoint4;
    public Transform[] stoppoint5;
    public Transform[] stoppoint6;
    public Transform[] stoppoint7;
    public Transform[] stoppoint8;
    public Transform[] stoppoint9;

    private OfficialStateMachine stateMachine;

    int lookCounter;
    RaycastHit hit;
    int targetCounter;
    private bool shouldLeave;
    bool lookingState;
    bool lookedAtObjects;
    [SerializeField]
    private float lookForSeconds;
    float time;

    public bool inspectionComplete;

    void Start()
    {
        // get the components on the object we need ( should not be null due to require component so no need to check )
        agent = GetComponentInChildren<NavMeshAgent>();
        character = GetComponent<UnityStandardAssets.Characters.ThirdPerson.ThirdPersonCharacter>();
        stateMachine = GetComponent<OfficialStateMachine>();
        index = 0;
        targetCounter = 0;
        lookCounter = 0;
        lookingState = false;
        lookedAtObjects = true;
        shouldLeave = false;
        agent.updateRotation = true;
        agent.updatePosition = true;
        agent.speed = 0.5f;
        stateMachine.ChangeCurrentState(new GoToDoor());
        StartCoroutine("StartGlobalState");
    }

    void FixedUpdate()
    {
        //if (Physics.Raycast(ethanhead.transform.position, ethanhead.transform.up, out hit, 1.5f))
        //{
        //    if (hit.collider.tag == "door")
        //    {
        //        if (hit.collider.GetComponent<DoorScript>().getState() == false)
        //        {
        //            hit.collider.GetComponent<DoorScript>().OpenDoor();
        //        }
        //    }
        //}

    }



    void Update()
    {
        stateMachine.ExecuteSM();
    }

    void LateUpdate()
    {
        if (/*lookedAtObjects == false &&*/ lookingState == true)
        {
            Quaternion firstRot = ethanbody.transform.rotation;
            ethanbody.transform.LookAt(lookPoints[index].position/*getRoom()[lookCounter].GetComponent<Transform>().position*/);
            firstRot.y = ethanbody.transform.localRotation.y;
            ethanbody.transform.localRotation = firstRot;

            //OfficialLookAt(getRoom()[lookCounter].GetComponent<Transform>().position);
            OfficialLookAt(lookPoints[index].position);
            //Debug.Log("looking at: " + getRoom()[lookCounter].GetComponent<Transform>().name);
        }
    }

    IEnumerator StartGlobalState()
    {
        yield return new WaitForSeconds(1.5f); //Wait, so all of the objects can instantiate.
        stateMachine.ChangeGlobalState(new GlobalOfficialState());
    }

    public OfficialStateMachine getFSM()
    {
        return stateMachine;
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

    /// <summary>
    /// Official locks on and looks at the target.
    /// </summary>
    /// <param name="_lookAtTarget">Target to look at.</param>
    public void OfficialLookAt(Vector3 _lookAtTarget)
    {
        ethanhead.transform.LookAt(_lookAtTarget); //Look at those objects.
        ethanhead.transform.Rotate(90, 0, 0);
        ethanhead.transform.Rotate(0, -90, 0);
    }

    public void SetTarget(Transform target)
    {
        this.target[targetCounter] = target;
    }

    public void StopAndLook() //New Inspect Method
    {
        Debug.Log("Moving to: " + stopPoints[index].position);
        if (lookingState == false)
        {
            character.Move(agent.desiredVelocity, false, false);
            if (stopPoints != null)
            {
                if (index != stopPoints.Length)
                {
                    agent.SetDestination(stopPoints[index].position);
                    if (Vector3.Distance(transform.position, stopPoints[index].position) < 0.5f)
                    {
                        agent.Stop();
                        agent.updatePosition = false;
                        time = Time.time;
                        lookingState = true;
                    }
                }
                else
                {
                    shouldLeave = true;
                    return;
                }
            }
        }
        else
        {
            character.Move(Vector3.zero, false, false);
            if (lookPoints != null)
            {
                if (index != lookPoints.Length)
                {
                    if (Time.time - time > lookForSeconds)
                    {
                        index++;
                        agent.Resume();
                        lookingState = false;
                    }
                    
                }
            }
        }
    }

    public bool ShouldILeave()
    {
        return shouldLeave;
    }

    public void OldAIInspectMethod() //Obsolete
    {
        if (lookingState == false)      //If player not looking, walking
        {
            if (target != null)
            {
                if (targetCounter != target.Length)
                {
                    agent.SetDestination(target[targetCounter].position);

                    if (Vector3.Distance(gameObject.transform.position, target[targetCounter].GetComponent<Transform>().position) < 0.5f && targetCounter < target.Length)
                        //If Official is closer than 1.5 to the target and next target is not OutOfBounds
                    {
                        lookingState = true;              // Not walking, looking
                        lookedAtObjects = false;     //Didn't look at every object in the current -new- room
                        time = Time.time;
                        lookCounter = 0;
                        agent.Stop();
                        agent.updateRotation = false;
                    }

                    else // If not close to the target
                    {
                        // use the values to move the character
                        character.Move(agent.desiredVelocity, false, false);
                    }
                }

                else if (targetCounter == target.Length) //Looked at all the rooms.
                    inspectionComplete = true;
            }

        }

        else //If player not walking, looking
        {
            character.Move(Vector3.zero, false, false);

            if (lookedAtObjects == false)
            {

                if (lookCounter < getRoom().Length && Time.time - time > lookForSeconds) // LookCounter is lower than length and current time - last looked time is bigger than lookForSeconds
                {
                    lookCounter++;              //Get the next object to look at, at the next frame.
                    time = Time.time;
                }

                if (lookCounter == getRoom().Length)    //Looked at all the objects. Reset the parameters.
                {
                    lookedAtObjects = true;   //Looked at every object in the room
                    lookingState = false;       //Change the looking state to walking.

                    if (targetCounter < target.Length)//If this is not the last room...
                    {
                        targetCounter++;                    //Change the walking target.
                    }

                    lookCounter = 0;            //Reset the looking counter.
                    agent.Resume();            //Resume the navigation.
                }
            }
        }
    }
}
