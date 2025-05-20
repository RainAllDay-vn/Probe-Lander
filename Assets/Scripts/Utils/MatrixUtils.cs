using UnityEngine;

public class MatrixUtils 
{
    public static float[,] Multiply(float[,] A, float[,] B)
    {
        int aRows = A.GetLength(0), aCols = A.GetLength(1);
        int bRows = B.GetLength(0), bCols = B.GetLength(1);


        float[,] result = new float[aRows, bCols];

        for (int i = 0; i < aRows; i++)
            for (int j = 0; j < bCols; j++)
                for (int k = 0; k < aCols; k++)
                    result[i, j] += A[i, k] * B[k, j];

        return result;
    }
    public static float[,] AddMatrices(float[,] A, float[,] B)
    {
        int rows = A.GetLength(0);
        int cols = A.GetLength(1);

        float[,] result = new float[rows, cols];
        for (int i = 0; i < rows; i++)
            for (int j = 0; j < cols; j++)
                result[i, j] = A[i, j] + B[i, j];

        return result;
    }
    public static float[,] Transpose(float[,] matrix)
    {
        int rows = matrix.GetLength(0);
        int cols = matrix.GetLength(1);
        float[,] transposed = new float[cols, rows];

        for (int i = 0; i < rows; i++)
            for (int j = 0; j < cols; j++)
                transposed[j, i] = matrix[i, j];

        return transposed;
    }
    public static float[,] ReLU(float[,] matrix)
    {
        int rows = matrix.GetLength(0);
        int cols = matrix.GetLength(1);
        float[,] result = new float[rows, cols];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                result[i, j] = Mathf.Max(0, matrix[i, j]);
            }
        }

        return result;
    }
    public static float[,] ReLUDerivative(float[,] matrix)
    {
        int rows = matrix.GetLength(0);
        int cols = matrix.GetLength(1);
        float[,] result = new float[rows, cols];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                result[i, j] = matrix[i, j] > 0 ? 1 : 0;
            }
        }

        return result;
    }

    public static void MutateMatrixWithScalar(ref float[,] matrix, float modifier)
    {
        int rows = matrix.GetLength(0);
        int cols = matrix.GetLength(1);

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                // pick a random value in [-modifier, +modifier]
                float noise = Random.Range(-modifier, modifier);
                matrix[i, j] += noise;
            }
        }
    }

    public static float[] Forward(float[,] weights, float[,] bias, float[] input, bool useReLU = true)
    {
        int inputSize = weights.GetLength(1);
        int outputSize = weights.GetLength(0);


        // Convert input to column vector
        float[,] inputCol = new float[inputSize, 1];
        for (int i = 0; i < inputSize; i++)
            inputCol[i, 0] = input[i];

        float[,] result = AddMatrices(Multiply(weights, inputCol), bias);
        if (useReLU)
            result = ReLU(result);

        // Convert result back to 1D array
        float[] output = new float[outputSize];
        for (int i = 0; i < outputSize; i++)
            output[i] = result[i, 0];

        return output;
    }

}
