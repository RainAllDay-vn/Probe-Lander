using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;



public class FormerSparseReward : RewardGiver
{
    bool _done = false;
    bool _hasTouchedDown = false;
    Vector3 lastWin = Vector3.zero;
    //bool hasWon = false;
    //public List<Vector3> points = new List<Vector3>();
    //public Trajector trajectory;
    public override void Step()
    {

    }
    private void Update()
    {
        //if (hasWon)
        //{
        //    Debug.DrawRay(lastWin, Vector3.up * 8, Color.red);
        //}
    }
    private void FixedUpdate()
    {

        if (lander.rb.velocity.y > 1)
        {
            SetReward(-1);
            EndEpisode();
            return;
        }
        if (lander.rb.angularVelocity.magnitude > 3)
        {
            SetReward(-1);
            EndEpisode();
            return;
        }
        if (Vector3.Angle(Vector3.up, transform.up) > 30)
        {
            SetReward(-1);
            EndEpisode();
            return;
        }
        else
        {
            AddReward(0.1f);
        }

        for (int i = 0; i < lander.throttle.Length; i++)
        {
            AddReward(lander.throttle[i] * -0.003f);
        }
    }

    public override void OnEndEpisode()
    {
        _done = false;
        _hasTouchedDown = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_done) return;
        if (collision.gameObject.name == "AgentRocket") return;
        if (collision.relativeVelocity.magnitude > 3)
        {
            SetReward(-1);
            EndEpisode();
            return;
        }
        Vector2 pos2d = new Vector2(transform.position.x, transform.position.z);

        if (pos2d.magnitude > 10)
        {
            Debug.Log("Land outsite");
            SetReward(0.5f);
            EndEpisode();
            return;
        }
        // mark that we’ve touched ground at least once
        _hasTouchedDown = true;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (_done || !_hasTouchedDown) return;
        // ignore self‐collision
        if (collision.gameObject.name == "AgentRocket") return;

        // check for roll-over
        float angle = Vector3.Angle(Vector3.up, lander.transform.up);
        if (angle > 90f)
        {
            SetReward(-1f);
            _done = true;
            EndEpisode();
            return;
        }

        // check leg count
        if (lander.LegTouched >= 4)
        {
            HeightScheduler.instance.count--;
            if (HeightScheduler.instance.count == 0)
            {
                HeightScheduler.instance.count = HeightScheduler.instance.max_count;
                if(HeightScheduler.instance.spawner.yLevel < 22)
                    HeightScheduler.instance.spawner.yLevel += HeightScheduler.instance.heighIncrease;
            }
            // full success
            //hasWon = true;
            lastWin = transform.position;
            
            Debug.Log("Win");
            NumberDisplay.Instance.Count++;
            SetReward(+1f);
            _done = true;
            EndEpisode();

        }
        // else: fewer than 4 legs, keep waiting
    }
}
