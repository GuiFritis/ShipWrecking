using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class DestroyHelper : MonoBehaviour
{
    [Tooltip("If destroy is checked, the game object will be destroyed, else it will be inactivated")]
    public bool destroy = false;
    public float timeToDestroy = 2f;

    void Start()
    {
        StartCoroutine(DeleteMe());
    }

    private IEnumerator DeleteMe()
    {
        yield return new WaitForSeconds(timeToDestroy);
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
