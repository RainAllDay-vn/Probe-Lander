using System;
using System.Collections.Generic;
using UnityEngine;

public class GeneticAlgorithmManager : MonoBehaviour
{
    public int populationSize = 50;
    public int dnaSize = 10;
    public float mutationRate = 0.01f;
    public int elitism = 5;

    private List<DNAFloat> population;
    private List<DNAFloat> newPopulation;
    private float fitnessSum;
    private float bestFitness;
    private float[] bestGenes;
    private int generation;

    void Start()
    {
        generation = 1;
        bestGenes = new float[dnaSize];
        population = new List<DNAFloat>(populationSize);
        newPopulation = new List<DNAFloat>(populationSize);

        for (int i = 0; i < populationSize; i++)
        {
            DNAFloat dna = new GameObject("DNA_" + i).AddComponent<DNAFloat>();
            dna.Init(dnaSize, GetRandomGene, EvaluateFitness);
            population.Add(dna);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            NewGeneration();
            Debug.Log($"Generation {generation} | Best Fitness: {bestFitness}");
        }
    }

    float GetRandomGene()
    {
        return UnityEngine.Random.Range(0f, 1f);
    }

    float EvaluateFitness(int index)
    {
        float total = 0f;
        foreach (var gene in population[index].Genes)
        {
            total += gene;
        }
        return total; // Tổng gene càng cao, fitness càng tốt
    }

    void NewGeneration()
    {
        CalculateFitness();
        population.Sort((a, b) => b.Fitness.CompareTo(a.Fitness));
        newPopulation.Clear();

        for (int i = 0; i < population.Count; i++)
        {
            if (i < elitism)
            {
                newPopulation.Add(population[i]);
            }
            else
            {
                DNAFloat parent1 = ChooseParent();
                DNAFloat parent2 = ChooseParent();
                DNAFloat child = parent1.Crossover(parent2);
                child.Mutate(mutationRate);
                newPopulation.Add(child);
            }
        }

        foreach (var old in population)
        {
            Destroy(old.gameObject);
        }

        population = new List<DNAFloat>(newPopulation);
        generation++;
    }

    void CalculateFitness()
    {
        fitnessSum = 0f;
        DNAFloat best = population[0];

        for (int i = 0; i < population.Count; i++)
        {
            fitnessSum += population[i].CalculateFitness(i);
            if (population[i].Fitness > best.Fitness)
            {
                best = population[i];
            }
        }

        bestFitness = best.Fitness;
        best.Genes.CopyTo(bestGenes, 0);
    }

    DNAFloat ChooseParent()
    {
        float rand = UnityEngine.Random.Range(0f, fitnessSum);
        foreach (var dna in population)
        {
            if (rand < dna.Fitness)
                return dna;
            rand -= dna.Fitness;
        }
        return population[0];
    }
}
