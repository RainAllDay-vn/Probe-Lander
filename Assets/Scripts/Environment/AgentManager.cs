using Unity.MLAgents;
using UnityEngine;

public class AgentManager : MonoBehaviour
{
    ExampleAgent[] agents;
    private void Start()
    {
        agents = GameObject.FindObjectsByType<ExampleAgent>(FindObjectsSortMode.None);
        //get all agents in scene
    }
    private void Update()
    {
        foreach (ExampleAgent agent in agents) {
            //Do some stuffs 
        }
    }
}
