using UnityEngine;

public class BaseAgent : MonoBehaviour
{
    public RewardGiver rewardGiver; //rewardgiver
    public LanderController lander; //lander controller
    public Bound bound;             //bound, can be found in the bound gameobject in environment
    public SpawnLander spawner;     //spawner, can also be found in the bound gameobject in environment
    private void Update()
    {


        Debug.Log(rewardGiver.Reward);
        if (!bound.insideBound(transform.position))
        {
            Debug.Log("Outside bound");
            spawner.SetRandomPos(lander);
        }
    } 
}
