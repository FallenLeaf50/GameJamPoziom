using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MassCheck : MonoBehaviour
{
    public MassCounter m_Counter1;
    public MassCounter m_Counter2;

    public Transform SpiningWalls;

    private int _TargetXAngle = 90;
    private float _RotationSpeed = 90;

    Vector3 currentEueler;
    private void FixedUpdate()
    {
        _TargetXAngle = 0;
        _TargetXAngle = (m_Counter1.TotalMass - m_Counter2.TotalMass)* 10;
        Debug.Log(_TargetXAngle);

        currentEueler = SpiningWalls.transform.root.eulerAngles;
        UnityEngine.Quaternion targetRotate = UnityEngine.Quaternion.Euler(_TargetXAngle, currentEueler.y, currentEueler.z );
        SpiningWalls.transform.rotation = Quaternion.RotateTowards(SpiningWalls.transform.rotation, targetRotate, _RotationSpeed * Time.deltaTime);


    }
}
