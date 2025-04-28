using UnityEngine;
using UnityEngine.Events;

// ################
// # DON'T CHANGE #
// ################
public class Leg : MonoBehaviour
{
    public UnityEvent<GameObject> onTouchGround;
    public UnityEvent<GameObject> onLeaveGround;
    public float touchDistance = 0.1f;
    public bool touched = false;
    private void Update()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, Vector3.down, out hit))
        {
            if (hit.distance < touchDistance)
            {
                if (!touched)
                {
                    touched = true;
                    onTouchGround?.Invoke(gameObject);
                }
            }
            else
            {
                if (touched)
                {
                    touched = false; onLeaveGround?.Invoke(gameObject);
                }
            }
        }
    }
}
