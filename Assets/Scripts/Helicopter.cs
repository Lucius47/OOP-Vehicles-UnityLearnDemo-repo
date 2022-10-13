using System.Collections;
using UnityEngine;

public class Helicopter : Vehicle // INHERITANCE
{
    [SerializeField] private Transform _mainRotor;
    [SerializeField] private float turningSpeed = 10;

    private float _currentAltitude;
    private float targetAltitude;
    [SerializeField] private float maxAltitude = 1;
    
    private Vector3 currentPosition;

    private State _currentState;

    protected override void Update() // POLYMORPHISM
    {
        base.Update();

        if (IsAtTargetPosition)
        {
            targetAltitude = 0;
        }
        else
        {
            targetAltitude = maxAltitude;
        }
        
        _mainRotor.Rotate(0, 10, 0);
        var position = transform.position;
        _currentAltitude = position.y;
        currentPosition = position;

        StartCoroutine(SetAltitude());
        StartCoroutine(SetDirection());
        StartCoroutine(SetPosition());
    }

    private IEnumerator SetAltitude()
    {
        if (IsAtTargetAltitude) yield return null;
        
        _currentState = State.Climbing;
        transform.position += Time.deltaTime * DistanceToTargetAltitude * EnginePower * transform.up;
        yield return null;
    }
    
    private IEnumerator SetDirection()
    {
        if (IsFacingTargetPosition) yield return null;
        
        _currentState = State.Turning;
        transform.Rotate(0, AngleToTargetPosition * turningSpeed * Time.deltaTime, 0);
        yield return null;
    }
    
    private IEnumerator SetPosition()
    {
        if (IsAtTargetPosition || !IsFacingTargetPosition) yield break;
        
        _currentState = State.Moving;
        transform.position += Time.deltaTime * DistanceToTargetPosition * EnginePower * transform.forward;
        yield return null;
    }

    private float DistanceToTargetAltitude => targetAltitude - _currentAltitude;
    private bool IsAtTargetAltitude => (Mathf.Abs(DistanceToTargetAltitude) < 0.2f);
    
    private float DistanceToTargetPosition
    {
        get
        {
            var curPosIn2D = new Vector2(transform.position.x, transform.position.z);
            var tarPosIn2D = new Vector2(TargetPosition.x, TargetPosition.z);
            
            return Vector2.Distance(curPosIn2D, tarPosIn2D);
        }
    }

    private bool IsAtTargetPosition => (Mathf.Abs(DistanceToTargetPosition) < 0.2f);
    
    private float AngleToTargetPosition
    {
        get
        {
            var vectorToTarget = (TargetPosition - currentPosition).normalized;
            vectorToTarget.y = 0;
            var angle = Vector3.SignedAngle(transform.forward, vectorToTarget, transform.up);
            
            Debug.DrawRay(transform.position, transform.forward, Color.cyan);
            Debug.DrawRay(transform.position, vectorToTarget, Color.magenta);
            
            return angle;
        }
    }

    private bool IsFacingTargetPosition => Mathf.Abs(AngleToTargetPosition) < 10;

    private enum State
    {
        Climbing,
        Turning,
        Moving,
    }
    
    private void OnGUI()
    {
        if (GUI.Button(new Rect(10, 70, 80, 40), gameObject.name))
        {
            isUserControlled = !isUserControlled;
        }

        var labelText = isUserControlled ? gameObject.name + " Selected" : gameObject.name + " Not Selected";
        GUI.Label(new Rect(10, 100, 140, 40), labelText);
    }
}