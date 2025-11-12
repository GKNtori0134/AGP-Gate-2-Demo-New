using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonTrigger : MonoBehaviour
{

    public TargetObject targetObject;
    public GameObject anotherButtonComp;
    public Material[] materials;
    public int bounceTimes = 2;
    bool triggered = false;

    public void triggerTarget()
    {
        if (triggered) return;
        targetObject.activate();
        if(materials.Length != 0) GetComponent<MeshRenderer>().material = materials[bounceTimes];
        if(materials.Length != 0) anotherButtonComp.GetComponent<MeshRenderer>().material = materials[bounceTimes];
        triggered = true;
    }
}
