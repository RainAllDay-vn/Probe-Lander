using UnityEngine;

public class OpenAILLReward : RewardGiver
{
    public override void Step()
    {
        float speed = lander.rb.linearVelocity.magnitude;
        float angularSpeed = lander.rb.angularVelocity.magnitude;
        float angle = Vector3.Dot(Vector3.up,lander.transform.up);
        float height = transform.position.y;
        float speedScore = -speed;
        float angularSpeedScore = -angularSpeed;
        float angleScore = angle;
        float heightScore = -height;
        float reward = angularSpeedScore;
        /*
        if (height < 10 && speed > 2)
            reward -= 100*speed;
        */
        AddReward(reward);
        SetReward(reward);
    }
}
