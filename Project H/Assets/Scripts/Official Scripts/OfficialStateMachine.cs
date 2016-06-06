using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public interface IState
{
    void Enter(AIControl _official);
    void Execute(AIControl _official);
    void Exit(AIControl _official);
}

public class GoToDoor : IState
{
    Transform doorStopPoint = GameObject.Find("AIStopPositions/StopPos#1").GetComponent<Transform>();
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
    Transform doorLookAt = GameObject.Find("AILookPositions/LookPos#1").GetComponent<Transform>();
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
                {
                    _official.OfficialLookAt(Vector3.zero);
                    _official.getFSM().ChangeCurrentState(new Search());
                }
                if (numOfKnocks > 2) //After two knocks, increment the attention.
                    offAttention.IncrementAttention(5f);

                audSouce.Play();
                numOfKnocks++;
                time = Time.time;
            }

            else //If the attention is low, leave.
            {
                _official.OfficialLookAt(Vector3.zero);
                _official.getFSM().ChangeCurrentState(new Leave());
            }
        }
    }

    public void Exit(AIControl _official)
    {
    }
}

public class Search : IState //TODO: implement Search state
{
    List<GameObject> currentStopPoints = new List<GameObject>();

    List<GameObject> currentLookPoints = new List<GameObject>();

    int searchLevel;

    public void Enter(AIControl _official)
    {
        float attention = _official.GetComponent<OfficialAttention>().getAttentionValue();

        //Determine the searchLevel 
        //if (attention <= 50f)                         //0...50
        //    searchLevel = 1;

        if (attention <= 75f && attention > 50f) //50...75
            searchLevel = 2;

        else if (attention < 90f && attention > 75f) //75...90
            searchLevel = 3;

        else if (attention >= 90f)                   //90...100
            searchLevel = 4;

        Debug.Log("Entered Search State");
        _official.agent.updateRotation = true;

        _official.agent.Resume();

        GameObject[] allStopPoints = GameObject.FindGameObjectsWithTag("StopPosition");
        GameObject[] allLookPoints = GameObject.FindGameObjectsWithTag("LookPosition");


        foreach (var item in allStopPoints)                     //Get appropriate stop and look points
            if (item.name.StartsWith("StopPos#" + searchLevel))
                currentStopPoints.Add(item);


        foreach (var item in allLookPoints)
            if (item.name.StartsWith("LookPos#" + searchLevel))
                currentLookPoints.Add(item);

        var sortedBufferPoints = currentStopPoints.OrderBy(go => go.name).ToList(); //Sort look and stop points
        currentStopPoints = sortedBufferPoints;

        sortedBufferPoints = currentLookPoints.OrderBy(go => go.name).ToList();
        currentLookPoints = sortedBufferPoints;

    }

    float lookTime = 0f;
    int index = 0;
    public void Execute(AIControl _official)
    {
        if (currentStopPoints.Count > index)
        {
            if (Vector3.Distance(_official.transform.position, currentStopPoints[index].transform.position) < 0.5f)
            //Looking
            {
                _official.OfficialLookAt(currentLookPoints[index].transform.position);
                _official.character.Move(Vector3.zero, false, false);
                Debug.Log("currently looking at: " + currentLookPoints[index].name);

                if (Time.time - lookTime > _official.lookForSeconds)
                    index++;
            }

            else //Moving
            {
                _official.OfficialLookAt(Vector3.zero);
                _official.agent.SetDestination(currentStopPoints[index].transform.position);
                _official.character.Move(_official.agent.desiredVelocity, false, false);
                Debug.Log("currently going to: " + currentStopPoints[index].name);
                lookTime = Time.time;
            }
        }
        else
        {
            _official.OfficialLookAt(Vector3.zero);
            _official.getFSM().ChangeCurrentState(new Leave());
        }
    }

    public void Exit(AIControl _official)
    {
    }
}

public class Leave : IState
{
    GameObject officialLeavePoint = GameObject.Find("OfficialLeavePos");
    public void Enter(AIControl _official)
    {
        _official.agent.Resume();
        _official.agent.updateRotation = true;
        _official.OfficialLookAt(Vector3.zero);
        _official.agent.SetDestination(officialLeavePoint.transform.position);
        _official.character.Move(_official.agent.desiredVelocity, false, false);
    }

    public void Execute(AIControl _official)
    {
        _official.agent.Resume();
        _official.agent.updateRotation = true;
        _official.agent.SetDestination(officialLeavePoint.transform.position);
        _official.character.Move(_official.agent.desiredVelocity, false, false);

        if (Vector3.Distance(_official.gameObject.transform.position, officialLeavePoint.transform.position) < 0.5)
            _official.inspectionComplete = true;
    }

    public void Exit(AIControl _official)
    { }
}

public class MoneyFound : IState
{
    GameObject cameraObj = new GameObject("Found Money Camera", typeof(Camera));
    Camera moneyCamera;
    NPCDetection detectionScript;
    Camera playerCamera = Camera.main;
    public void Enter(AIControl _official)
    {
        _official.character.Move(Vector3.zero, false, false);
        playerCamera.gameObject.SetActive(false);
        moneyCamera = cameraObj.GetComponent<Camera>();
        moneyCamera.enabled = true;
        detectionScript = _official.GetComponentInChildren<NPCDetection>();
        GameObject.Find("LevelManager").GetComponent<WaveBehaviour>().setComponents(false); //Disable the movements for the player.
    }

    public void Execute(AIControl _official)
    {
        _official.character.Move(Vector3.zero, false, false);
        _official.agent.Stop();
        _official.agent.updateRotation = false;
        _official.agent.SetDestination(_official.transform.position);


        _official.OfficialLookAt(detectionScript.moneyLook.transform.position); //Official looks at the found money.
        moneyCamera.transform.position = detectionScript.moneyLook.transform.position + (Vector3.up * 0.50f) + ((detectionScript.moneyLook.transform.position - _official.GetComponent<Transform>().position).normalized * 1.5f);
        moneyCamera.transform.LookAt(detectionScript.gameObject.transform.position);

        if (Input.GetKeyDown(KeyCode.Space))
            detectionScript.endWave = true;

    }

    public void Exit(AIControl _official)
    {
    }
}
public class GlobalOfficialState : IState
{
    NPCDetection detection;
    bool changedState;
    public void Enter(AIControl _official)
    {
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