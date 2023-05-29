using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeToBlue : MonoBehaviour
{
    [SerializeField] private Transform _group;
    private List<Transform> _childrens = new List<Transform>();

    private void Start()
    {
        foreach (Transform child in transform)
        {
            if (child.CompareTag(TagType.Character.ToString()))
            {
                _childrens.Add(child);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(TagType.Player.ToString()))
        {
            foreach (Transform child in _childrens)
            {
                if (child.CompareTag(TagType.Character.ToString()))
                {
                    Character character = child.gameObject.GetComponent<Character>();
                    character.ChangeToPlayer();
                }
            }
            
            transform.SetParent(_group);
        }
    }
}
