using UnityEngine;
using System.Collections;

public class HidingSpotMovement : MonoBehaviour
{

    public Vector3 positionChange;
    public Quaternion rotationChange;
    public float changeAmount;
    public GameObject movementObject;

    SpotMove movement;
    void Start()
    {
        movement = new SpotMove(positionChange, rotationChange, changeAmount, movementObject);
    }

    void Update()
    {
        if (movement.getNextPosition())
        {
            //do that shit.

        //umm
        }


    }
}

class SpotMove
{
    private Vector3 positionChange;
    private Quaternion rotationChange;
    public float changeAmount;
    private GameObject currentObject;

    public Vector3 finalPosition;
    public Quaternion finalRotation;

    public SpotMove(Vector3 _positionChange, Quaternion _rotationChange, float _changeAmount, GameObject _currentObject)
    {
        finalPosition = _positionChange;
        positionChange = _currentObject.transform.position - _positionChange;
        finalRotation = _rotationChange;
        rotationChange.eulerAngles = (_currentObject.transform.rotation.eulerAngles - _rotationChange.eulerAngles) / _changeAmount;
        changeAmount = _changeAmount;
        currentObject = _currentObject;

    }

    public bool getNextRotation()
    {
        if (currentObject.transform.rotation.eulerAngles - finalRotation.eulerAngles != Quaternion.identity.eulerAngles)
        {
            currentObject.transform.rotation *= rotationChange;
            return true;
        }
        else
        {
            return false;
        }

    }

    public bool getNextPosition()
    {
        if (currentObject.transform.position.x - finalPosition.x > Vector3.zero.x &&
            currentObject.transform.position.y - finalPosition.y > Vector3.zero.y &&
            currentObject.transform.position.z - finalPosition.z > Vector3.zero.z)
        {
            currentObject.transform.position += positionChange;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void getNextMove()
    {

    }


}
