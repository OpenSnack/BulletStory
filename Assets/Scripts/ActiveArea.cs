using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveArea : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D collision)
    {
        // Destroy(collision.gameObject.transform.parent.gameObject);
        Destroy(collision.gameObject);
    }
}
