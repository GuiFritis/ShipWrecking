using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyHelper : MonoBehaviour
{
    [Tooltip("If destroy is checked, the game object will be destroyed, else it will be inactivated")]
    public bool destroy = false;

    public void DeleteMe()
    {
        if(destroy)
        {
            Destroy(gameObject);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
