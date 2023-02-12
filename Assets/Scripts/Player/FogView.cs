using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class FogView : MonoBehaviour
{
    public CinemachineVirtualCamera cinemachine;
    public string fogTag = "Fog";
    public float fogView = 10f;
    public float transitionTime = 1f;

    private float _normalView;
    private bool _insideFog = false;

    void Awake()
    {
        _normalView = cinemachine.m_Lens.OrthographicSize;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag(fogTag))
        {
            _insideFog = true;
            StartCoroutine(EnterFog());
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag(fogTag))
        {
            _insideFog = false;
            StartCoroutine(ExitFog());
        }
    }

    private IEnumerator EnterFog()
    {
        while(cinemachine.m_Lens.OrthographicSize > fogView && _insideFog)
        {
            cinemachine.m_Lens.OrthographicSize -= Time.deltaTime / transitionTime;
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator ExitFog()
    {
        while(cinemachine.m_Lens.OrthographicSize < _normalView && !_insideFog)
        {
            cinemachine.m_Lens.OrthographicSize += Time.deltaTime * transitionTime;
            yield return new WaitForEndOfFrame();
        }
    }
}
