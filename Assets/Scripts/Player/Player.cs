using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Ship;

[RequireComponent(typeof(ShipBase))]
public class Player : MonoBehaviour
{
    public ShipBase ship;
    public CannonBase frontCannon;
    public CannonBase[] leftCannon = new CannonBase[3];
    public CannonBase[] rightCannon = new CannonBase[3];

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
        ship.health.ResetLife();

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
        _inputs.Gameplay_PC.FrontShoot.performed += ctx => FrontCannons();
        _inputs.Gameplay_PC.LeftSideShoot.performed += ctx => LeftSideCannons();
        _inputs.Gameplay_PC.RightSideShoot.performed += ctx => RightSideCannons();
        _inputs.Gameplay_PC.SideShoot.performed += ctx => SideCannons();
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
            yield return new WaitForEndOfFrame();
        }
    }

    private void StopTurning()
    {
        _turning = false;
    }
    #endregion

    #region CANNONS
    private void FrontCannons()
    {
        frontCannon.Shoot();
    }

    private void LeftSideCannons()
    {
        foreach (var item in leftCannon)
        {
            item.Shoot();
        }
    }

    private void RightSideCannons()
    {
        foreach (var item in rightCannon)
        {
            item.Shoot();
        }
    }

    private void SideCannons()
    {
        LeftSideCannons();
        RightSideCannons();
    }
    #endregion

}