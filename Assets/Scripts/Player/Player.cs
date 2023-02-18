using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Ship;

[RequireComponent(typeof(ShipBase))]
public class Player : MonoBehaviour, IKiller
{
    public ShipBase ship;
    public UIFillUpdater healthUI;

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
        ship.health.OnDamage += UpdateHealthUI;

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

    public void Pause()
    {
        _inputs.Disable();
    }

    public void Unpause()
    {
        _inputs.Enable();
    }

    private void UpdateHealthUI(HealthBase hp)
    {
        healthUI.UpdateValue(hp.GetCurHealth()/hp.baseHealth);
    }

    public void OnKill(HealthBase hp)
    {
        GameManager.Instance.score.Value += hp.scorePoints;
    }

    #region MOVE
    private void Move()
    {
        ship.SetMoving(true);
    }

    private void StopMove()
    {
        ship.SetMoving(false);
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
        ship.ShootCannon(Cannon.CannonSide.FRONT, this);
    }

    private void LeftSideCannons()
    {
        ship.ShootCannon(Cannon.CannonSide.LEFT, this);
    }

    private void RightSideCannons()
    {
        ship.ShootCannon(Cannon.CannonSide.RIGHT, this);
    }

    private void SideCannons()
    {
        LeftSideCannons();
        RightSideCannons();
    }
    #endregion

}