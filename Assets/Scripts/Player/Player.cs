using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Ship;

[RequireComponent(typeof(ShipBase))]
public class Player : MonoBehaviour
{
    public ShipBase ship;

    private InputAction _inputs;

    void OnValidate()
    {
        ship = GetComponent<ShipBase>();
    }

    void Awake()
    {
        Init();
    }

    private void Init()
    {
        _inputs.Enable();
        SetInputs();
    }

    private void SetInputs()
    {

    }

    

}
