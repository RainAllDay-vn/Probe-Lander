using UnityEngine;

// ################
// # DON'T CHANGE #
// ################
public class SpawnLander : MonoBehaviour
{
    public float xDistance = 20;
    public float zDistance = 20;
    public float yLevel = 20;
    public float[] initVelecity = new float[2];
    public float[] initOrientation = new float[6];
    public void SetRandomPos(Transform lander)
    {
        float randX = Random.Range(-xDistance, xDistance);
        float randZ = Random.Range(-zDistance, zDistance);
        RaycastHit hit;
        float groundHeight = 0;
        if (Physics.Raycast(new Vector3(randX, 1000, randZ), Vector3.down, out hit))
        {
            groundHeight = 1000 - hit.distance;
        }
        float randY = groundHeight + yLevel ;
        lander.position = new Vector3(randX, randY, randZ); 
        lander.rotation = Quaternion.identity;
    }
}
