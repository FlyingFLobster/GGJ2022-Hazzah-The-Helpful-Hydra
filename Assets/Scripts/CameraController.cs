// @author Zachary Kidd-Smith
// This script controls the movement of the camera, should just follow around the main character,
// could be good to add interactions for boundaries later.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject focus; // Object that the camera will focus on, could be changed to focus on NPCs during dialogue.

    private Vector3 offset; // Offset of the camera's position from its focus

    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - focus.transform.position;
    }

    // Late Update is called once per frame but after the regular Update call (for scripts that rely on other scripts already being updated).
    void LateUpdate()
    {
        transform.position = focus.transform.position + offset;
    }

    public void ChangeFocus(GameObject inFocus)
    {
        focus = inFocus;
    }
}
