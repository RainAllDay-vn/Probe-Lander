using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;

public class GeneticExample : MonoBehaviour
{
    struct Layer
    {
        public float[,] weight;
        public float[,] bias;
        public Layer(int nbInput, int nbOutput)
        {
            //dimension might be wrong
            weight = new float[nbInput, nbOutput];
            bias = new float[nbOutput,1];
        }
    }

    Layer[] stack = {
        new Layer(16, 16),
        new Layer(16, 16),
        new Layer(16, 16),
        new Layer(16, 16),
    };
    private void Update()
    {
        float mutationPower = 0.1f;
        float[,] input = new float[16,1];
        float[,] output = input;
        for(int i = 0; i< stack.Length; i++) {
            output = MatrixUtils.ReLU(MatrixUtils.AddMatrices(MatrixUtils.Multiply(stack[i].weight, output), stack[i].bias));
            MatrixUtils.MutateMatrixWithScalar(ref stack[i].weight, mutationPower);
            MatrixUtils.MutateMatrixWithScalar(ref stack[i].bias, mutationPower);
        }
        //=> output

    }
    
}
