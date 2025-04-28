using NUnit.Framework;
using UnityEngine;
using UnityEngine.Events;

// ################
// # DON'T CHANGE #
// ################
public class LanderController : MonoBehaviour
{
    public Rigidbody rb;
    public float thrust = 50;
    public float maxAngle = 5;
    public Transform[] thrusters;
    float[] throttle = new float[3];
    float[] angle = new float[6];
    UnityEvent<Collider> onCollision;
    private int legTouched = 0;

    public int LegTouched
    {
        get { return legTouched; }
    }
    public float HeightFromTerrain
    {
        get { 
            RaycastHit hit; if (Physics.Raycast(transform.position, Vector3.down, out hit)) 
                return hit.distance; 
            else return Mathf.Infinity; 
        }
    }
    public void SetThrusterThrottle(float[] throttle)
    {
        this.throttle = throttle;
    }
    public void SetThrusterAngle(float[] angles)
    {
        this.angle = angles;
    }
    private void Update()
    {
        for (int i = 0; i < thrusters.Length; i++) {
            float thrusterAngleX = angle[i * 2] * maxAngle;
            float thrusterAngleZ = angle[i * 2+1] * maxAngle;
            float thrusterThrottle = Mathf.Clamp01(throttle[i]) * thrust;
            thrusters[i].localRotation = Quaternion.Euler(thrusterAngleX,0,thrusterAngleZ);
            rb.AddForceAtPosition(thrusterThrottle * thrusters[i].up, thrusters[i].position);
        }      
    }
    private void OnCollisionEnter(Collision collision)
    {
        onCollision?.Invoke(collision.collider);
    }
    public void onLegTouched()
    {
        legTouched++;
        if(legTouched > 4)
        {
            Debug.Log("Something wrong with legs");
        }
    }
    public void onLegLeave()
    {
        legTouched--;
        if (legTouched < 0)
        {
            Debug.Log("Something wrong with legs");
        }
    }
}
