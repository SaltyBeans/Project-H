using UnityEngine;

public interface IState
{
    void Enter(AIControl _official);
    void Execute(AIControl _official);
    void Exit(AIControl _official);
}

public class GoToDoor : IState
{
    Transform doorStopPoint = GameObject.Find("Targets/DoorStopPoint").GetComponent<Transform>();
    HideWaveScript waveTimer = GameObject.Find("LevelManager").GetComponent<HideWaveScript>();
    bool targetSet = false;
    public void Enter(AIControl _official)
    {


    }

    public void Execute(AIControl _official)
    {
        _official.agent.SetDestination(doorStopPoint.transform.position);
        _official.character.Move(_official.agent.desiredVelocity, false, false);

        if (Vector3.Distance(_official.transform.position, doorStopPoint.transform.position) < 0.5f)
        {
            _official.agent.Stop();
            _official.agent.updateRotation = false;
            _official.character.Move(Vector3.zero, false, false);
            _official.agent.SetDestination(_official.transform.position);
            _official.getFSM().ChangeCurrentState(new LookAtDoor());
        }

    }

    public void Exit(AIControl _official)
    {

    }
}

public class LookAtDoor : IState
{

    float time = Time.time;
    Transform doorLookAt = GameObject.Find("DoorLookAt").GetComponent<Transform>();
    AudioSource audSouce;
    OfficialAttention offAttention;
    uint numOfKnocks = 0;
    GameObject[] doors = GameObject.FindGameObjectsWithTag("door");
    DoorScript doorScript;
    public void Enter(AIControl _official)
    {
        _official.character.Move(Vector3.zero, false, false);

        audSouce = _official.GetComponentInChildren<AudioSource>();
        offAttention = _official.GetComponent<OfficialAttention>();
        audSouce.clip = Resources.Load("knocking_sound") as AudioClip;

        foreach (var item in doors) //Get the door script of the outside door.
            if (item.name == "outsidedoor")
                doorScript = item.GetComponent<DoorScript>();

    }

    public void Execute(AIControl _official)
    {
        _official.OfficialLookAt(doorLookAt.position);
        _official.character.Move(Vector3.zero, false, false);

        if (Time.time - time > 4.5f) //Wait 4.5 seconds
        {
            if (offAttention.getAttentionValue() > 50f) //If the attention is high enough, knock.
            {
                if (doorScript.getState()) //If the door is open
                    _official.getFSM().ChangeCurrentState(new Search());

                if (numOfKnocks > 2) //After two knocks, increment the attention.
                    offAttention.IncrementAttention(5f);

                audSouce.Play();
                numOfKnocks++;
                time = Time.time;
            }

            else //If the attention is low, leave.
            {
                _official.GetComponent<OfficialStateMachine>().ChangeCurrentState(new Leave());
            }
        }
    }

    public void Exit(AIControl _official)
    {

    }
}

public class Leave : IState
{
    Transform officialLeavePoint = GameObject.Find("Targets/OfficialLeavePoint").GetComponent<Transform>();
    public void Enter(AIControl _official)
    {
        _official.agent.SetDestination(officialLeavePoint.position);
    }

    public void Execute(AIControl _official)
    { }

    public void Exit(AIControl _official)
    { }
}

public class OfficialStateMachine : MonoBehaviour
{
    private IState currentState;
    private IState globalState;
    private AIControl official;

    void Start()
    {
        official = gameObject.GetComponent<AIControl>();
    }

    public void ExecuteSM()
    {
        if (currentState != null)
        {
            Debug.Log(currentState);
            currentState.Execute(official);
        }

        if (globalState != null)
        {
            globalState.Execute(official);
        }
    }

    public void ChangeCurrentState(IState newState)
    {
        currentState = newState;
        currentState.Enter(official);
    }

    public void ChangeGlobalState(IState _state)
    {
        globalState = _state;
        globalState.Enter(official);
    }
}