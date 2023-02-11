using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Ship;

[RequireComponent(typeof(ShipBase))]
public class Player : MonoBehaviour
{
    public ShipBase ship;
    private Inputs _inputs;
    private bool _turning;

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
        _inputs = new Inputs();
        _inputs.Enable();
        SetInputs();
    }

    private void SetInputs()
    {
        _inputs.Gameplay_PC.Move.started += ctx => Move();
        _inputs.Gameplay_PC.Move.canceled += ctx => StopMove();
        _inputs.Gameplay_PC.Turn.started += ctx => Turn((int)ctx.ReadValue<float>());
        _inputs.Gameplay_PC.Turn.canceled += ctx => StopTurning();
    }

    #region MOVE
    private void Move()
    {
        ship.moving = true;
    }

    private void StopMove()
    {
        ship.moving = false;
    }
    #endregion

    #region TURN
    private void Turn(int turnDirection)
    {
        _turning = true;
        StartCoroutine(TurnCoroutine(turnDirection));
    }

    private IEnumerator TurnCoroutine(int turnDirection)
    {
        while(_turning)
        {
            ship.Turn(turnDirection);
            yield return new WaitForFixedUpdate();
        }
    }

    private void StopTurning()
    {
        _turning = false;
    }
    #endregion

}