using UnityEngine;
using UnityEngine.Events;

public abstract class RewardGiver : MonoBehaviour
{
    public UnityEvent<float> setReward;
    public UnityEvent<float> addReward;
    public UnityEvent endEpisode;
    private float reward = 0;
    public bool inPlayBack = false;
    public float Reward
    {
        get { return reward; }
    }
    public LanderController lander;

    //override the below method
    public abstract void Step();
    public abstract void OnEndEpisode();

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
    
    protected void EndEpisode()
    {
        if (inPlayBack)
            return;
        endEpisode?.Invoke();
        reward = 0;
        OnEndEpisode();
        
    }
    public void Reset()
    {
        reward = 0;
    }
}
