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
    { }

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

public class Search : IState //TODO: implement Search state
{
    public void Enter(AIControl _official)
    {
        Debug.Log("Entered Search State");
        _official.agent.updateRotation = true;
        
        _official.agent.Resume();
    }

    public void Execute(AIControl _official)
    {
        _official.StopAndLook();
        if (_official.ShouldILeave())
        {
            Exit(_official);
        }
    }

    public void Exit(AIControl _official)
    {
        _official.getFSM().ChangeCurrentState(new Leave());
    }
}

public class MoneyFound : IState
{
    GameObject cameraObj = new GameObject("Found Money Camera", typeof(Camera));
    Camera playerCamera = Camera.main;
    public void Enter(AIControl _official)
    {
        _official.character.Move(Vector3.zero, false, false);
        playerCamera.gameObject.SetActive(false);
        Debug.Log("flag1");
        cameraObj.GetComponent<Camera>().enabled = true;
        GameObject.Find("LevelManager").GetComponent<WaveBehaviour>().setComponents(false); //Disable the movements for the player.
    }

    public void Execute(AIControl _official)
    {
        _official.character.Move(Vector3.zero, false, false);
        _official.agent.Stop();
        _official.agent.updateRotation = false;
        _official.character.Move(Vector3.zero, false, false);
        _official.agent.SetDestination(_official.transform.position);


        _official.OfficialLookAt(_official.GetComponentInChildren<NPCDetection>().moneyLook.transform.position); //Official looks at the found money.
        cameraObj.GetComponent<Camera>().transform.position = _official.GetComponentInChildren<NPCDetection>().moneyLook.transform.position + (Vector3.up * 0.50f) + ((_official.GetComponentInChildren<NPCDetection>().moneyLook.transform.position - _official.GetComponent<Transform>().position).normalized * 1.5f);
        cameraObj.GetComponent<Camera>().transform.LookAt(_official.GetComponentInChildren<NPCDetection>().gameObject.transform.position);

        if (Input.GetKeyDown(KeyCode.Space))
            _official.GetComponentInChildren<NPCDetection>().endWave = true;

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

        _official.agent.Resume();
        _official.agent.updateRotation = true;
    }

    public void Execute(AIControl _official)
    {
        _official.agent.SetDestination(officialLeavePoint.transform.position);
        _official.character.Move(_official.agent.desiredVelocity, false, false);

        if (Vector3.Distance(_official.gameObject.transform.position, officialLeavePoint.position) < 0.5)
            _official.inspectionComplete = true;
    }

    public void Exit(AIControl _official)
    { }
}

public class GlobalOfficialState : IState
{
    NPCDetection detection;
    bool changedState;
    public void Enter(AIControl _official)
    {
        //detection = _official.GetComponentInChildren<NPCDetection>();
        detection = _official.GetComponentInChildren<NPCDetection>();
    }

    public void Execute(AIControl _official)
    {

        if (detection.moneyFound && !changedState) //Change state if money has been found
        {
            _official.getFSM().ChangeCurrentState(new MoneyFound());
            changedState = true;
        }
    }

    public void Exit(AIControl _official)
    {
    }
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

    public void ChangeCurrentState(IState _state)
    {
        currentState = _state;
        currentState.Enter(official);
    }

    public void ChangeGlobalState(IState _state)
    {
        globalState = _state;
        globalState.Enter(official);
    }


}