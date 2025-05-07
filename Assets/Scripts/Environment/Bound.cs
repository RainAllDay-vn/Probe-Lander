using UnityEngine;
using UnityEngine.Events;

// ################
// # DON'T CHANGE #
// ################
public class Bound : MonoBehaviour
{
    public UnityEvent<Collider> boundEnter;
    public UnityEvent<Collider> boundLeave;
    public BoxCollider boxCollider;
    private void OnTriggerEnter(Collider other)
    {
        boundEnter?.Invoke(other);
    }
    private void OnTriggerExit(Collider other)
    {
        boundLeave?.Invoke(other);
    }
    public bool insideBound(Vector3 pos)
    {
        Debug.Log(pos);
        Vector3 max = boxCollider.bounds.max;
        Vector3 min = boxCollider.bounds.min;
        for(int i = 0; i< 3; i++)
        {
            if (pos[i]> max[i] ||  pos[i]< min[i]) 
                return false;
        }
        return true;

    }
}
