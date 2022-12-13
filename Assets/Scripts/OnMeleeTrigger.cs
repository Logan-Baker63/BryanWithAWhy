using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnMeleeTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (transform.parent.tag == "Player")
        {
            if (other.tag == "Enemy")
            {
                transform.parent.GetComponent<Attack>().OnEnterMeleeTrigger(other.gameObject);
            }
        }
        else
        {
            if (other.tag == "Player")
            {
                transform.parent.GetComponent<Attack>().OnEnterMeleeTrigger(other.gameObject);
            }
        }

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (transform.parent.tag == "Player")
        {
            if (other.tag == "Enemy")
            {
                transform.parent.GetComponent<Attack>().OnExitMeleeTrigger(other.gameObject);
            }
        }
        else
        {
            if (other.tag == "Player")
            {
                transform.parent.GetComponent<Attack>().OnExitMeleeTrigger(other.gameObject);
            }
        }
    }
}
