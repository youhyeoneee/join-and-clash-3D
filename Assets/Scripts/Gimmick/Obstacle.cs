using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : Gimmick
{
    protected override void Rotate()
    {
        Quaternion targetRotation = Quaternion.Euler(_startRotation.eulerAngles.x, _startRotation.eulerAngles.y, transform.rotation.eulerAngles.z + (_rotateSpeed * Time.deltaTime));
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotateSpeed * Time.deltaTime);
    }
    
    protected override void ActivateGimmick(GameObject hitObject)
    {
        Character character = hitObject.GetComponent<Character>();
        StartCoroutine(character.Die());
    }
}