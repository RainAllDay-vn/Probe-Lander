using UnityEngine;

// ################
// # DON'T CHANGE #
// ################
public class SpawnLander : MonoBehaviour
{
    public BoxCollider bound;
    public Vector3 padding;
    public float yLevel = 20;
    public void SetRandomPos(Transform lander)
    {
        Vector3 boundMax = bound.bounds.max - padding;
        Vector3 boundMin = bound.bounds.min + padding;
        float randX = Random.Range(boundMax.x, boundMin.x);
        float randZ = Random.Range(boundMin.z, boundMax.z);
        RaycastHit hit;
        float minY = boundMin.y;
        if(Physics.Raycast(new Vector3(randX, boundMax.y, randZ), Vector3.down, out hit))
        {
            minY = boundMax.y - hit.distance;
        }


        float randY = yLevel;

        lander.position = new Vector3(randX, randY, randZ); 
    }
    public Vector3 sampleRandomPos()
    {

        Vector3 boundMax = bound.bounds.max - padding;
        Vector3 boundMin = bound.bounds.min + padding;
        float randX = Random.Range(boundMax.x, boundMin.x);
        float randZ = Random.Range(boundMin.z, boundMax.z);
        float randY = Random.Range(boundMin.y, boundMax.y);
        return new Vector3(randX, randY, randZ) ;
    }
    public Vector3 sampleRandomPos(Vector3 pos, float radius)
    {

        Vector3 boundMax = bound.bounds.max - padding;
        Vector3 boundMin = bound.bounds.min + padding;
        Vector2 dir = Random.insideUnitCircle * radius;
        float randX = Mathf.Clamp(pos.x+dir.x, boundMin.x, boundMax.x);
        float randZ = Mathf.Clamp(pos.z+ dir.y, boundMin.z, boundMax.z);
        float randY = Random.Range(boundMin.y, boundMax.y);
        return new Vector3(randX, randY, randZ);
        
    }
}
