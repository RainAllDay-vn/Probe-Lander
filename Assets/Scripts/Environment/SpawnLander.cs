using UnityEngine;

// ################
// # DON'T CHANGE #
// ################
public class SpawnLander : MonoBehaviour
{
    public BoxCollider bound;
    public Vector3 padding;
    public float yLevel = 20;
    public float spawnRadius = 4;
    public void SetRandomPos(Transform lander)
    {
        lander.position = sampleRandomPos();
    }
    public Vector3 sampleRandomPos()
    {
        float angle = Random.Range(-3.14f,3.14f);
        Vector2 randPos =new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * spawnRadius;
        return new Vector3(randPos.x, yLevel, randPos.y) ;
    }
    public Vector3 sampleRandomPos(Vector3 pos, float radius)
    {

        Vector3 boundMax = bound.bounds.max - padding;
        Vector3 boundMin = bound.bounds.min + padding;
        Vector2 dir = Random.insideUnitCircle * radius;

        float randX = Mathf.Clamp(pos.x+dir.x, boundMin.x, boundMax.x);
        float randZ = Mathf.Clamp(pos.z+ dir.y, boundMin.z, boundMax.z);
        return new Vector3(randX, 0, randZ);
        
    }
}
