using UnityEngine;

public abstract class Vehicle : MonoBehaviour
{
    [SerializeField] private float enginePower;
    [SerializeField] protected bool isUserControlled;
    protected Vector3 TargetPosition;

    protected float EnginePower
    {
        get => enginePower;
        set
        {
            if (value < 0)
            {
                Debug.LogError("Engine power cannot be negative.");
                return;
            }
            enginePower = value;
        }
    }

    protected virtual void Update()
    {
        if (isUserControlled)
        {
            if (Input.GetMouseButton(0))
            {
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out var hit, 100, LayerMask.NameToLayer("Ground")))
                {
                    TargetPosition = hit.point;
                }
            }
        }
        else
        {
            TargetPosition = transform.position;
        }
        
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(TargetPosition, 0.1f);
    }
}