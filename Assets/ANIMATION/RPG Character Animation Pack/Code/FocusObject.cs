using UnityEngine;
using System.Collections;

public class FocusObject : MonoBehaviour
{
    public Transform focus_object;


	void LateUpdate ()
    {
        transform.LookAt(focus_object);
    }
}
