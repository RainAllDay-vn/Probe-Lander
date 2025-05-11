using System;
using UnityEngine;

public class DNAFloat : MonoBehaviour
{
    public float[] Genes { get; private set; }
    public float Fitness { get; private set; }

    private System.Func<float> getRandomGene;
    private System.Func<int, float> fitnessFunction;

    public void Init(int size, System.Func<float> getRandomGene, System.Func<int, float> fitnessFunction, bool shouldInitGenes = true)
    {
        this.getRandomGene = getRandomGene;
        this.fitnessFunction = fitnessFunction;

        Genes = new float[size];

        if (shouldInitGenes)
        {
            for (int i = 0; i < Genes.Length; i++)
            {
                Genes[i] = getRandomGene();
            }
        }
    }

    public float CalculateFitness(int index)
    {
        Fitness = fitnessFunction(index);
        return Fitness;
    }

    public DNAFloat Crossover(DNAFloat otherParent)
    {
        GameObject childObj = new GameObject("DNAChild");
        DNAFloat child = childObj.AddComponent<DNAFloat>();
        child.Init(Genes.Length, getRandomGene, fitnessFunction, shouldInitGenes: false);

        for (int i = 0; i < Genes.Length; i++)
        {
            child.Genes[i] = UnityEngine.Random.value < 0.5f ? Genes[i] : otherParent.Genes[i];
        }

        return child;
    }

    public void Mutate(float mutationRate)
    {
        for (int i = 0; i < Genes.Length; i++)
        {
            if (UnityEngine.Random.value < mutationRate)
            {
                Genes[i] = getRandomGene();
            }
        }
    }
}
