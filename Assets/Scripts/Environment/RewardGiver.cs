using UnityEngine;

// ################
// # DON'T CHANGE #
// ################
// Give if the agent be able to matain upright orientation and low velocity
public class RewardGiver : MonoBehaviour
{
    public LanderController lander;
    float Normalize(float val)
    {
        return val / (Mathf.Abs(val) + 1f);
    }
    public float Reward
    {
        get {
            Vector3 landPadPos = Vector3.zero;
            Vector3 pos2d = transform.position;
            pos2d.y = 0;
            Rigidbody rb = lander.rb;

            float distanceFromLandPad = Vector3.Distance(pos2d, landPadPos);
            float speed = rb.velocity.magnitude;
            float y_speed = Mathf.Abs(rb.velocity.y);
            float angularSpeed = rb.angularVelocity.magnitude;
            float angleAlignment = Vector3.Dot(Vector3.up, lander.transform.up); // 1 when upright
            float height = lander.transform.position.y;

            float speedPenalty = Normalize(-speed) * 0.7f;
            float angularPenalty = Normalize(-angularSpeed) * 0.3f;
            float landPadDistancePenalty = Normalize(-distanceFromLandPad * 0.1f) * 1.5f;
            float y_speed_penalty = Normalize(-y_speed * .1f) * 0.2f;

            float reward = angleAlignment + angularPenalty + y_speed_penalty + landPadDistancePenalty;
            return reward;
        }
    }
}