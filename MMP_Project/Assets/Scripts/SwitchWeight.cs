using UnityEngine;

public class SwitchWeight : MonoBehaviour
{
    public float activationWeight = 1.0f;
    public bool switchActive = false;

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.GetComponent<Rigidbody2D>().mass >= activationWeight)
        {
            switchActive = true;
        }
        else
        {
            switchActive = false;
        }

    }

    public void OnTriggerExit2D()
    {
        switchActive = false;
    }

    public void Update()
    {
        if (switchActive)
        {
            Debug.Log("$Switch Activated");
        }

    }
}

