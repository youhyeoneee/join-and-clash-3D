using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : Gimmick
{
    
    [Header("Key")]
    [SerializeField] private GameObject _particleObj;
    [SerializeField] private GameObject _effectObj;
    [SerializeField] private MeshRenderer _mr;

    protected override void Rotate()
    {
        Quaternion targetRotation = Quaternion.Euler(_startRotation.eulerAngles.x, transform.rotation.eulerAngles.y + (_rotateSpeed * Time.deltaTime), _startRotation.eulerAngles.z);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotateSpeed * Time.deltaTime);
    }
    
    protected override void ActivateGimmick(GameObject hitObject)
    {
        _particleObj.SetActive(false);
        _effectObj.SetActive(true);
        _mr.enabled = false;
    }
}
