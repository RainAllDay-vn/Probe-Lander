using Unity.MLAgents;
using UnityEngine;

public class OpenAILLReward : RewardGiver
{
    // references to your rocket and its AgentController
    private PhysicDebugger pd;
    public PPOAgent ac;
    void Awake()
    {
        pd = GetComponent<PhysicDebugger>();

    }

    /// <summary>
    /// Wraps AddReward + EndEpisode exactly like in the first file.
    /// </summary>
    void EndEpisode(float reward)
    {
        AddReward(reward);
        ac.EndEpisode();
    }

    public override void Step()
    {
        // nothing per‐frame here: all rewards come on terminal events
    }

    private void FixedUpdate()
    {
        // mirror the out‐of‐bounds check from RocketController.FixedUpdate
        
        Vector3 localPos = ac.lander.rb.position;
        
        if (ac.lander.rb.position.y > 2 + 50 ||
            ac.lander.rb.position.y < -1 ||
            Mathf.Abs(localPos.x) > 30 ||
            Mathf.Abs(localPos.z) > 100)
        {
            
            EndEpisode(0f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {

        // hard crash
        if (collision.relativeVelocity.y > 2)
        {
            
            EndEpisode(0f);
            return;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        // if flipped upside‐down
        float roll = Mathf.Abs(Vector3.Dot(transform.up, Vector3.right));
        float pitch = Mathf.Abs(Vector3.Dot(transform.up, Vector3.forward));

        if (roll > 0.1f || pitch > 0.1f)
        {
            
            EndEpisode(0f);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // when it settles, check for “sleeping” like in FixedUpdate
        if (ac.lander.rb.IsSleeping())
        {
            bool upright = Vector3.Dot(transform.up, Vector3.up) > 0.9f
                          && Mathf.Abs(Vector3.Dot(transform.up, Vector3.right)) < 0.1f
                          && Mathf.Abs(Vector3.Dot(transform.up, Vector3.forward)) < 0.1f;

            if (upright)
            {
                EndEpisode(1f);
            }
            else
            {
                
                EndEpisode(0f);
            }
        }
    }

    public override void OnEndEpisode() { /* no extra cleanup needed */ }
}
