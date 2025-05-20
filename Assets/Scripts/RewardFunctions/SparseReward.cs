using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SparseReward : RewardGiver
{
    bool _done = false;
    bool _hasTouchedDown = false;
    //Vector3 lastWin = Vector3.zero;
    //bool hasWon = false;
    public List<Trajector.State> states = new List<Trajector.State>();
    public override void Step()
    {
        
    }
    private void Update()
    {
        //if (hasWon) {
        //    Debug.DrawRay(lastWin, Vector3.up* 8, Color.blue);
        //}
    }
    private void FixedUpdate()
    {
        //Trajector.State state =new Trajector.State();
        //state.throttle = new float[3];
        //lander.throttle.CopyTo(state.throttle,0);
        //state.thrusterAngle = new float[6];
        //lander.angle.CopyTo(state.thrusterAngle,0);
        //state.rotation = transform.rotation;
        //state.pos = transform.position;
        //states.Add(state); 
       
        if (lander.rb.velocity.y > 1)
        {
            AddReward(-100);
            EndEpisode();
            return;
        }
        if (lander.rb.angularVelocity.magnitude > 3)
        {
            AddReward(-100);
            EndEpisode();
            return;
        }
        if (Vector3.Angle(Vector3.up, transform.up) > 30)
        {
            AddReward(-100);
            EndEpisode();
            return;
        }
        else
        {
            AddReward(0.1f);
        }
        //for (int i = 0; i < lander.throttle.Length; i++)
        //{
        //    AddReward(lander.throttle[i] * -0.03f);
        //}
    }

    public override void OnEndEpisode()
    {
        states.Clear();
        _done = false;
        _hasTouchedDown = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_done) return;
        if (collision.gameObject.name == "AgentRocket") return;
        if (collision.relativeVelocity.magnitude > 3)
        {
            AddReward(-100);
            EndEpisode();
            return;
        }
        // mark that we’ve touched ground at least once
        _hasTouchedDown = true;
    }

    private void OnCollisionStay(Collision collision)
    {
        return;
        if (_done || !_hasTouchedDown) return;

        Vector2 pos2d = new Vector2(transform.position.x, transform.position.z);
        // check leg count
        if (lander.LegTouched >= 4)
        {
            HeightScheduler.instance.count--;
            if(HeightScheduler.instance.count == 0)
            {
                HeightScheduler.instance.count = HeightScheduler.instance.max_count;
                HeightScheduler.instance.spawner.yLevel+= HeightScheduler.instance.heighIncrease;
            }
            // full success
            if (pos2d.magnitude < 10)
            {
                //hasWon = true;
                //lastWin = transform.position;
                Debug.Log("Win");
                
                AddReward(20);
            }
                
            AddReward((40 - pos2d.magnitude) * 200);
            
            _done = true;
            EndEpisode();

        }
        // else: fewer than 4 legs, keep waiting
    }
}
