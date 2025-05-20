using UnityEngine;
using UnityEngine.Events;

public class Leggg : MonoBehaviour
{
    public UnityEvent<GameObject> onTouchGround;
    public UnityEvent<GameObject> onLeaveGround;
    public float touchDistance = 0.1f;
    public bool touched = false;
    private void FixedUpdate()
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
