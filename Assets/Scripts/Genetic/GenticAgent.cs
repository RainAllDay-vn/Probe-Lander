using UnityEngine;

public class GeneticAgent : MonoBehaviour
{
    public LanderController lander;
    public Bound bound;
    public SpawnLander spawner;
    public RewardGiver rewardGiver;
    public float mutationPower = 0.05f;
    public float vel_weight = 0.2f, pos_weight = 0.1f;
    public Vector3 landingPad = Vector3.zero;
    public float fitness = 0;
    public float  landPadSpawnRadius = 1;
    public float max_rand_angle = 15;
    public GeneticManager manager;
    public class Layer
    {
        public float[,] weight;
        public float[,] bias;
        public Layer(int nbInput, int nbOutput)
        {
            weight = new float[nbOutput, nbInput];
            bias = new float[nbOutput, 1];
        }
    }

    public Layer[] network;
    float[] lastObservation;

    void Start()
    {
        rewardGiver.addReward.AddListener(this.OnAddReward);
        rewardGiver.setReward.AddListener(this.OnSetReward);
        rewardGiver.endEpisode.AddListener(this.OnEndEpisode);

        network = new Layer[]
        {
            new Layer(21, 128),
            new Layer(128, 16),
            new Layer(16, 9), // 6 angles + 3 throttle
        };
    }
    public void OnEpisodeStart()
    {
        float[] thrusterAngle = { 0, 0, 0, 0, 0, 0 };
        float[] thruster = { 0, 0, 0 };
        lander.SetThrusterAngle(thrusterAngle);
        lander.SetThrusterThrottle(thruster);
        spawner.SetRandomPos(transform);
        lander.transform.rotation = Quaternion.Euler(Random.Range(-max_rand_angle, max_rand_angle), Random.Range(-max_rand_angle, max_rand_angle), Random.Range(-max_rand_angle, max_rand_angle));
        lander.rb.velocity = Vector3.zero;
        lander.rb.angularVelocity = Vector3.zero;
        MutateNetwork();
    }

    void FixedUpdate()
    {
        if (!bound.insideBound(transform.position))
        {
            EndEpisode();
            return;
        }

        if (lander.rb.angularVelocity.magnitude > 30)
        {
            EndEpisode();
        }

        float[] input = CollectObservations();
        float[] output = Forward(input);
        ApplyAction(output);
        rewardGiver.Step();
    }
    float[] CollectObservations()
    {
        Vector3 velocity = normalize(lander.rb.velocity * vel_weight);
        Vector3 angularVel = normalize(lander.rb.angularVelocity * vel_weight);
        Vector3 position = normalize(lander.transform.position * pos_weight);
        Vector3 rotation = lander.transform.eulerAngles / 360f;
        Vector3 landPad = normalize(landingPad * pos_weight);

        var obs = new float[]
        {
            velocity.x, velocity.y, velocity.z,
            angularVel.x, angularVel.y, angularVel.z,
            position.x, position.y, position.z,
            rotation.x, rotation.y, rotation.z,
            landPad.x, landPad.y, landPad.z,
            0,0,0,
            0,0,0
        };
        return obs;
    }

    float[] Forward(float[] input)
    {
        float[] x = input;
        for (int i = 0; i < network.Length; i++)
        {
            x = MatrixUtils.Forward(network[i].weight, network[i].bias, x, i < network.Length - 1);
        }
        return x;
    }

    void ApplyAction(float[] output)
    {
        float[] angles = new float[6];
        float[] throttle = new float[3];

        for (int i = 0; i < 6; i++)
            angles[i] = Mathf.Clamp(output[i], -1f, 1f);
        for (int i = 0; i < 3; i++)
            throttle[i] = Mathf.Clamp(output[6 + i] / 2f + 0.5f, 0f, 1f);

        lander.SetThrusterAngle(angles);
        lander.SetThrusterThrottle(throttle);
    }

    void MutateNetwork()
    {
        for(int i = 0; i< network.Length; i++)
        {
            MatrixUtils.MutateMatrixWithScalar(ref network[i].weight, mutationPower);
            MatrixUtils.MutateMatrixWithScalar(ref network[i].bias, mutationPower);
        }
    }

    Vector3 normalize(Vector3 v)
    {
        Vector3 abs = Vector3.Max(v, -v);
        v.Scale(new Vector3(1 / (abs.x + 1), 1 / (abs.y + 1), 1 / (abs.z + 1)));
        return v;
    }

    float[] PadObservation(float[] input, int size)
    {
        float[] padded = new float[size];
        for (int i = 0; i < Mathf.Min(size, input.Length); i++)
            padded[i] = input[i];
        return padded;
    }

    void EndEpisode()
    {
        gameObject.SetActive(false);
        manager.survivingAgent--;
    }

    void OnAddReward(float val) { fitness += val; }
    void OnSetReward(float val) { fitness = val; }
    void OnEndEpisode() { EndEpisode(); }
}
