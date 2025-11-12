using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _SpinFan : TargetObject
{
    private bool isOff = false;
    public float rotateSpeed = 720f;
    private Collider fanCollider;

    private void Start()
    {
        fanCollider = GetComponent<Collider>();
    }

    private void Update()
    {
        if (!isOff) transform.Rotate(0f, 0f, rotateSpeed * Time.deltaTime);
    }

    public override void activate()
    {
        isOff = true;
        fanCollider.enabled = false;
    }
}
