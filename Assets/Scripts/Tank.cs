using UnityEngine;
using UnityEngine.AI;

public class Tank : Vehicle
{
    private NavMeshAgent _agent;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Move(Vector3 target)
    {
        _agent.speed = EnginePower;
        _agent.SetDestination(target);
    }

    protected override void Update()
    {
        base.Update();
        Move(TargetPosition);
    }
    
    private void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 80, 40), gameObject.name))
        {
            isUserControlled = !isUserControlled;
        }

        var labelText = isUserControlled ? gameObject.name + " Selected" : gameObject.name + " Not Selected";
        GUI.Label(new Rect(10, 40, 140, 40), labelText);
    }
}