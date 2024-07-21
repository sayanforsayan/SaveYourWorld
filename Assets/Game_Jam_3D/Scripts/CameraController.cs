using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private CinemachineFreeLook scroll_Cam;
    [SerializeField] private CinemachineVirtualCamera vCam;
    private bool IS_PERMIT;
    private bool IS_PERMIT2;

    public void AssignToCam(Transform earth)
    {
        scroll_Cam.Follow = earth;
        scroll_Cam.LookAt = earth;
    }

    public void IsFocus(bool isFocus)
    {
        if (isFocus)
        {
            scroll_Cam.enabled = !isFocus;
        }
        else
        {
            scroll_Cam.enabled = !isFocus;
        }
    }


    public void ZombieCam(bool isTrue)
    {
        if (isTrue && !IS_PERMIT && vCam != null)
        {
            IS_PERMIT = true;
            vCam.Priority = 10;
            scroll_Cam.Priority = 0;
        }
        else if(!IS_PERMIT2 && vCam != null)
        {
            IS_PERMIT2 = true;
            vCam.Priority = 0;
            scroll_Cam.Priority = 10;
            Destroy(vCam);
        }
    }
}
