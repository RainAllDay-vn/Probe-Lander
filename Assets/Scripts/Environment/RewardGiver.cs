using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Events;

public abstract class RewardGiver : MonoBehaviour
{
    public UnityEvent<float> setReward;
    public UnityEvent<float> addReward;
    private float reward = 0;
    
    public float Reward
    {
        get { return reward; }
    }
    public LanderController lander;

    //override the below method
    public abstract void Step();

    //will fire the setReward event which are binded to agent's add reward function or can be directly obtained from the Reward
    protected void AddReward(float re)
    {
        reward += re;
        setReward?.Invoke(re);
    }
    protected void SetReward(float re)
    {
        reward = re;
        setReward?.Invoke(re);
    }
    public void Reset()
    {
        reward = 0;
    }
}
