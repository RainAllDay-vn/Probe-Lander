using UnityEngine;
using System;
using static MatrixUtils;

public class GeneticAgent : MonoBehaviour
{
    public RewardGiver rewardGiver;
    public LanderController lander;
    public Bound bound;
    public SpawnLander spawner;

    private int inputSize = 10;
    private int hiddenSize = 128;
    private int outputSize = 10;

    public float[,] w1, w2, w3;
    public float[,] b1, b2, b3;

    public float fitness;
    public float[] dna;  // Keep the DNA as a float array

    public void InitFromDNA(float[] dna)
    {
        this.dna = dna; // Store the DNA in the agent

        int index = 0;
        w1 = ReadMatrix(dna, ref index, inputSize, hiddenSize);
        b1 = ReadMatrix(dna, ref index, 1, hiddenSize);
        w2 = ReadMatrix(dna, ref index, hiddenSize, hiddenSize);
        b2 = ReadMatrix(dna, ref index, 1, hiddenSize);
        w3 = ReadMatrix(dna, ref index, hiddenSize, outputSize);
        b3 = ReadMatrix(dna, ref index, 1, outputSize);
        spawner.SetRandomPos(lander.transform);
        lander.transform.rotation = Quaternion.identity;
        lander.rb.angularVelocity = Vector3.zero;
        lander.rb.velocity = Vector3.zero;
    }

    public static int GetDNALength(int input, int hidden, int output)
    {
        return (input * hidden + hidden) + (hidden * hidden + hidden) + (hidden * output + output);
    }

    private void Update()
    {
        float[] inputs = CollectInputs();
        float[] outputs = Forward(inputs);

        float[] throttle = new float[3];
        float[] angles = new float[7];

        for (int i = 0; i < 3; i++) throttle[i] = Mathf.Clamp01(outputs[i]);
        for (int i = 0; i < 6; i++) angles[i] = outputs[3 + i];

        lander.SetThrusterThrottle(throttle);
        lander.SetThrusterAngle(angles);

        fitness = rewardGiver.Reward;

        if (!bound.insideBound(transform.position))
        {
            spawner.SetRandomPos(lander.transform);
        }
    }

    private float[] CollectInputs()
    {
        Vector3 pos = lander.transform.position;
        Vector3 vel = lander.rb.velocity;
        Vector3 rot = lander.transform.eulerAngles/360;

        return new float[]
        {
            pos.x, pos.y, pos.z,
            vel.x, vel.y, vel.z,
            rot.x, rot.y, rot.z,
            1 // bias or padding
        };
    }

    private float[] Forward(float[] input)
    {
        float[,] x = ToMatrix(input);                  
        x = ReLU(AddMatrices(Multiply(x, w1), b1));      
        x = ReLU(AddMatrices(Multiply(x, w2), b2));      
        x = AddMatrices(Multiply(x, w3), b3);            
        return FromMatrix(x);                            
    }

    // Matrix conversion helpers
    float[,] ToMatrix(float[] arr)
    {
        float[,] mat = new float[1, arr.Length];
        for (int i = 0; i < arr.Length; i++) mat[0, i] = arr[i];
        return mat;
    }

    float[] FromMatrix(float[,] mat)
    {
        float[] result = new float[mat.GetLength(1)];
        for (int i = 0; i < result.Length; i++) result[i] = mat[0, i];
        return result;
    }

    float[,] ReadMatrix(float[] dna, ref int index, int rows, int cols)
    {
        float[,] mat = new float[rows, cols];
        for (int r = 0; r < rows; r++)
            for (int c = 0; c < cols; c++)
                mat[r, c] = dna[index++];
        return mat;
    }
}
