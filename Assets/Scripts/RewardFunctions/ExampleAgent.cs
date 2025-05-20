using UnityEngine;

public class ExampleAgent : MonoBehaviour
{
    public RewardGiver rewardGiver;//your own class extended from rewardgiver
    public LanderController lander; //lander controller
    public Bound bound; // bound, can be found in the bound gameobject in environment
    public SpawnLander spawner;//spawner, can also be found in the bound gameobject in environment
    private void Start()
    {
        
    }
    private void Update()
    {
        //set throttle and angles
        float[] throttle = { 0, 0, 0, 0, 0, 0 };
        float[] angles = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        lander.SetThrusterAngle(angles);
        lander.SetThrusterThrottle(throttle);

        //lander info
        Vector3 velocity = lander.rb.velocity;
        Vector3 worldRotation = lander.transform.eulerAngles;
        Vector3 angularVel = lander.rb.angularVelocity;
        float heightFromTerrain = lander.HeightFromTerrain;
        int legTouched = lander.LegTouched; //leg are considered touched when the sphere cast overlap with ground


        //update reward giver by stepping it
        if(rewardGiver != null)
            rewardGiver.Step();
        float reward = rewardGiver.Reward;
        //Reset must be called when ending an episode if use rewardGiver.Reward to get reward
        rewardGiver.Reset();
        if (!bound.insideBound(transform.position))
        {
            Debug.Log("Outside bound");
            spawner.SetRandomPos(transform);
        }
    }
}
