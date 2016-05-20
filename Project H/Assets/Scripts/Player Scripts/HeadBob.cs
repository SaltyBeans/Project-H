using UnityEngine;
using UnityStandardAssets.Utility;

public class HeadBob : MonoBehaviour
{
    public Camera Camera;
    public CurveControlledBob motionBob = new CurveControlledBob();
    public LerpControlledBob jumpAndLandingBob = new LerpControlledBob();
    private PlayerController Controller;
    public float StrideInterval;
    [Range(0f, 1f)]
    public float RunningStrideLengthen;

    // private CameraRefocus m_CameraRefocus;
    private bool m_PreviouslyGrounded;
    private Vector3 m_OriginalCameraPosition;

    private Rigidbody rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        Controller = GetComponent<PlayerController>();
        motionBob.Setup(Camera, StrideInterval);
        m_OriginalCameraPosition = Camera.transform.localPosition;
    }


    private void Update()
    {
        Vector3 newCameraPosition;

        if (Controller.GetVelocity().magnitude > 0 && Controller.IsGrounded())
        {
            Camera.GetComponent<Transform>().localPosition = motionBob.DoHeadBob(Controller.GetVelocity().magnitude * (Controller.isRunning ? RunningStrideLengthen : 1f));
            newCameraPosition = Camera.transform.localPosition;
            newCameraPosition.y = Camera.transform.localPosition.y - jumpAndLandingBob.Offset();
        }
        else
        {
            newCameraPosition = Camera.transform.localPosition;
            newCameraPosition.y = m_OriginalCameraPosition.y - jumpAndLandingBob.Offset();
        }
        Camera.transform.localPosition = newCameraPosition;

        if (!m_PreviouslyGrounded && Controller.IsGrounded())
        {
            StartCoroutine(jumpAndLandingBob.DoBobCycle());
        }

        m_PreviouslyGrounded = Controller.IsGrounded();
    }
}
