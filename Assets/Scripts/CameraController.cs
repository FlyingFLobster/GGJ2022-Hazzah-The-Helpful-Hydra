// @author Zachary Kidd-Smith
// This script controls the movement of the camera, should just follow around the main character,
// could be good to add interactions for boundaries later.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject focus; // Object that the camera will focus on, could be changed to focus on NPCs during dialogue.

    private Vector3 offset; // Offset of the camera's position from its focus.
    private int horizontalModifer; // Modifies the offset horizontally.

    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - focus.transform.position;
    }

    // Late Update is called once per frame but after the regular Update call (for scripts that rely on other scripts already being updated).
    void LateUpdate()
    {
        Vector3 modifiedOffset = new Vector3(offset.x + horizontalModifer, offset.y, offset.z);
        transform.position = focus.transform.position + modifiedOffset;
    }

    public void ChangeFocus(GameObject inFocus, int inHorizontalModifier = 0)
    {
        focus = inFocus;
        horizontalModifer = inHorizontalModifier;
    }
}
