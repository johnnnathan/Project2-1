using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;
using Unity.MLAgents.Sensors;
using System.Collections.Generic;

public enum Team
{
    Blue = 0,
    Purple = 1
}

public class AgentSoccer : Agent
{
    // Note that that the detectable tags are different for the blue and purple teams. The order is
    // * ball
    // * own goal
    // * opposing goal
    // * wall
    // * own teammate
    // * opposing player
    private Vision raySensor;
    private HearingSensor hearingSensor;
    private MemorySensor memorySensor;
    public enum Position
    {
        Striker,
        Goalie,
        Generic
    }

    [HideInInspector]
    public Team team;
    float m_KickPower;
    // The coefficient for the reward for colliding with a ball. Set using curriculum.
    float m_BallTouch;
    public Position position;

    const float k_Power = 2000f;
    float m_Existential;
    float m_LateralSpeed;
    float m_ForwardSpeed;


    [HideInInspector]
    public Rigidbody agentRb;
    SoccerSettings m_SoccerSettings;
    BehaviorParameters m_BehaviorParameters;
    public Vector3 initialPos;
    public float rotSign;

    EnvironmentParameters m_ResetParams;

    public override void Initialize()
    {
        attachSensors();

        SoccerEnvController envController = GetComponentInParent<SoccerEnvController>();
        if (envController != null)
        {
            m_Existential = 1f / envController.MaxEnvironmentSteps;
        }
        else
        {
            m_Existential = 1f / MaxStep;
        }

        m_BehaviorParameters = gameObject.GetComponent<BehaviorParameters>();
        if (m_BehaviorParameters.TeamId == (int)Team.Blue)
        {
            team = Team.Blue;
            initialPos = new Vector3(transform.position.x - 5f, .5f, transform.position.z);
            rotSign = 1f;
        }
        else
        {
            team = Team.Purple;
            initialPos = new Vector3(transform.position.x + 5f, .5f, transform.position.z);
            rotSign = -1f;
        }
        if (position == Position.Goalie)
        {
            m_LateralSpeed = 1.0f;
            m_ForwardSpeed = 1.0f;
        }
        else if (position == Position.Striker)
        {
            m_LateralSpeed = 0.3f;
            m_ForwardSpeed = 1.3f;
        }
        else
        {
            m_LateralSpeed = 0.3f;
            m_ForwardSpeed = 1.0f;
        }
        m_SoccerSettings = FindObjectOfType<SoccerSettings>();
        agentRb = GetComponent<Rigidbody>();
        agentRb.maxAngularVelocity = 500;

        m_ResetParams = Academy.Instance.EnvironmentParameters;


        Debug.Log($"Listing all components on {gameObject.name}:");

        // Get all components attached to this GameObject
        Component[] components = GetComponents<Component>();

        // Loop through each component and log its type
        foreach (Component component in components)
        {
            Debug.Log(component.GetType().Name);
        }
        
    }

    private void attachSensors()
    {
        hearingSensor = GetComponent<HearingSensor>();
        if (hearingSensor == null)
        {
            Debug.LogError("HearingSensor component not found on the agent.");
        }

        memorySensor = GetComponent<MemorySensor>();
        if (memorySensor == null)
        {
            Debug.LogError("MemorySensor component not found on the agent.");
        }
        raySensor = transform.Find("Core")?.GetComponentInChildren<Vision>();
        if (raySensor == null){
            Debug.LogError("Vision component not found on the agent.");
        }
    }

    public void MoveAgent(ActionSegment<int> act)
    {

        var dirToGo = Vector3.zero;
        var rotateDir = Vector3.zero;

        m_KickPower = 0f;

        var forwardAxis = act[0];
        var rightAxis = act[1];
        var rotateAxis = act[2];

        switch (forwardAxis)
        {
            case 1:
                dirToGo = transform.forward * m_ForwardSpeed;
                m_KickPower = 1f;
                break;
            case 2:
                dirToGo = transform.forward * -m_ForwardSpeed;
                break;
        }

        switch (rightAxis)
        {
            case 1:
                dirToGo = transform.right * m_LateralSpeed;
                break;
            case 2:
                dirToGo = transform.right * -m_LateralSpeed;
                break;
        }

        switch (rotateAxis)
        {
            case 1:
                rotateDir = transform.up * -1f;
                break;
            case 2:
                rotateDir = transform.up * 1f;
                break;
        }

        transform.Rotate(rotateDir, Time.deltaTime * 100f);
        agentRb.AddForce(dirToGo * m_SoccerSettings.agentRunSpeed,
            ForceMode.VelocityChange);
        UpdateSensors();
    }

    private void UpdateSensors()
    {
        UpdateRaySensor();
        UpdateHearingSensor();
    }

    private void UpdateRaySensor()
    {
        if (raySensor == null)
        {
            raySensor = GetComponent<Vision>();
        }

        if (raySensor != null)
        {

            
            // Rotate the sensor's local transform slightly
            raySensor.transform.localRotation *= Quaternion.Euler(0f, Random.Range(-25f, 25f), 0f);
        }
    }

    private void UpdateHearingSensor()
    {
        if (hearingSensor == null)
        {
            hearingSensor = GetComponent<HearingSensor>(); // Attach the HearingSensor component
        }

        if (hearingSensor != null)
        {
            hearingSensor.CollectNearbyObjects(); // Update detected object positions
        }

        if (raySensor != null){
            raySensor.GetDetectedObjects();
        }
    }

    //TO DO somehow add visual observations to memory
    public override void CollectObservations(VectorSensor sensor)
    {
        
        if (sensor == null)
        {
            Debug.LogError("VectorSensor is null in CollectObservations!");
            return;
        }

        base.CollectObservations(sensor);

        // Add hearing observations
        if (hearingSensor != null)
        {
            Debug.Log("hear collection called");
            List<Vector3> positions = hearingSensor.GetObjectPositions();
            memorySensor.AddMemory(positions);
            foreach (Vector3 position in positions)
            {
                Vector3 relativePosition = position - transform.position;
                // Add x, y, z separately
                sensor.AddObservation(relativePosition.x);
                sensor.AddObservation(relativePosition.y);
                sensor.AddObservation(relativePosition.z);
            }

            // Pad with zeros if fewer objects are detected
            for (int i = positions.Count; i < hearingSensor.maxObjects; ++i)
            {
                sensor.AddObservation(0f); // x
                sensor.AddObservation(0f); // y
                sensor.AddObservation(0f); // z
            }
        }
        

        if (raySensor != null)
        {
            List<Vector3> visibleObjects = raySensor.GetDetectedObjects();
            memorySensor.AddMemory(visibleObjects);

            foreach (Vector3 position in visibleObjects)
            {
                Vector3 relativePosition = position - transform.position;

                sensor.AddObservation(relativePosition.x);
                sensor.AddObservation(relativePosition.y);
                sensor.AddObservation(relativePosition.z);
            }

            // Pad with zeros if fewer objects are detected
            for (int i = visibleObjects.Count; i < raySensor.maxObjects; i++)
            {
                sensor.AddObservation(0f); // x
                sensor.AddObservation(0f); // y
                sensor.AddObservation(0f); // z
            }
        }

        

        if (memorySensor != null)
        {
            
            List<List<Vector3>> memories = memorySensor.GetMemories();
            foreach (List<Vector3> obs in memories){
                foreach (Vector3 item  in obs){
                sensor.AddObservation(item.x); // x
                sensor.AddObservation(item.y); // y
                sensor.AddObservation(item.z); // z
                }
            }
        }
    }



    public override void OnActionReceived(ActionBuffers actionBuffers)

    {

        if (position == Position.Goalie)
        {
            // Existential bonus for Goalies.
            AddReward(m_Existential);
        }
        else if (position == Position.Striker)
        {
            // Existential penalty for Strikers
            AddReward(-m_Existential);
        }
        MoveAgent(actionBuffers.DiscreteActions);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActionsOut = actionsOut.DiscreteActions;
        //forward
        if (Input.GetKey(KeyCode.W))
        {
            discreteActionsOut[0] = 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            discreteActionsOut[0] = 2;
        }
        //rotate
        if (Input.GetKey(KeyCode.A))
        {
            discreteActionsOut[2] = 1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            discreteActionsOut[2] = 2;
        }
        //right
        if (Input.GetKey(KeyCode.E))
        {
            discreteActionsOut[1] = 1;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            discreteActionsOut[1] = 2;
        }
    }
    /// <summary>
    /// Used to provide a "kick" to the ball.
    /// </summary>
    void OnCollisionEnter(Collision c)
    {
        var force = k_Power * m_KickPower;
        if (position == Position.Goalie)
        {
            force = k_Power;
        }
        if (c.gameObject.CompareTag("ball"))
        {
            AddReward(.2f * m_BallTouch);
            var dir = c.contacts[0].point - transform.position;
            dir = dir.normalized;
            c.gameObject.GetComponent<Rigidbody>().AddForce(dir * force);
        }
    }

    public override void OnEpisodeBegin()
    {
        m_BallTouch = m_ResetParams.GetWithDefault("ball_touch", 0);
    }

}