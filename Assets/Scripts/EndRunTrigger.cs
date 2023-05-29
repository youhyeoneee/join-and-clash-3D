using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndRunTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag(TagType.Player.ToString()))
        {
            GameManager.Instance.isEndRun = true;
        }
    }
}
