using System;
using UnityEngine;

public class MatrixUtils 
{
    public static float[,] Multiply(float[,] A, float[,] B)
    {
        int aRows = A.GetLength(0), aCols = A.GetLength(1);
        int bRows = B.GetLength(0), bCols = B.GetLength(1);

        if (aCols != bRows)
            throw new InvalidOperationException("Incompatible matrix dimensions.");

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

        if (rows != B.GetLength(0) || cols != B.GetLength(1))
            throw new InvalidOperationException("Matrices must have the same dimensions.");

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
                result[i, j] = Math.Max(0, matrix[i, j]);
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
                matrix[i, j] += modifier;  // Example: element-wise addition with the scalar
                                           // Or you could multiply: matrix[i, j] *= modifier;  // For multiplication
            }
        }
    }

}
