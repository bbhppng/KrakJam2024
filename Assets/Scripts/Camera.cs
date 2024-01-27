using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    [SerializeField] private float _followSpeed;
    [SerializeField] private Transform _target;
    [SerializeField] private float _offset = -10f;

    void Update()
    {
        Vector3 _newPos = new Vector3(_target.position.x, _target.position.y, _offset);
        transform.position = Vector3.Slerp(transform.position, _newPos, _followSpeed * Time.deltaTime);
    }
}
