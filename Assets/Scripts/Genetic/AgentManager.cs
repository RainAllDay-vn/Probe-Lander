using System.Collections.Generic;
using UnityEngine;

public class AgentManager : MonoBehaviour
{
    [Header("Genetic Parameters")]
    public int populationSize = 50;
    public float mutationRate = 0.02f;
    public int elitism = 5;

    [Header("Network Structure")]
    public int inputSize = 10;
    public int hiddenSize = 128;
    public int outputSize = 10;

    private List<GeneticAgent> agents = new List<GeneticAgent>();
    private int dnaLength;
    private float bestFitness = 0f;
    private float[] bestDNA;
    private int generation = 1;

    void Start()
    {
        dnaLength = GeneticAgent.GetDNALength(inputSize, hiddenSize, outputSize);
        FindAllAgentsInScene(); // Find existing agents in the scene
        InvokeRepeating("EvolvePopulation",0,10);
    }

    void Update()
    {
    }

    // Find all agents already present in the scene
    void FindAllAgentsInScene()
    {
        agents.Clear(); // Clear the list of agents to ensure it's fresh

        // Find all GeneticAgent objects in the scene
        GeneticAgent[] foundAgents = FindObjectsOfType<GeneticAgent>();
        foreach (var agent in foundAgents)
        {
            agents.Add(agent);
        }

        // Initialize the agents with random DNA
        foreach (var agent in agents)
        {
            float[] dna = GenerateRandomDNA();
            agent.InitFromDNA(dna); // Initialize each agent with a random DNA
        }
    }

    float[] GenerateRandomDNA()
    {
        float[] dna = new float[dnaLength];
        for (int i = 0; i < dnaLength; i++)
        {
            dna[i] = Random.Range(-1f, 1f); // Neural weights can be negative or positive
        }
        return dna;
    }

    void EvolvePopulation()
    {
        if (agents.Count == 0)
        {
            Debug.LogWarning("No agents available to evolve.");
            return;
        }

        // Sort agents by fitness descending
        agents.Sort((a, b) => b.fitness.CompareTo(a.fitness));

        // Safety check for elitism count
        int eliteCount = Mathf.Min(elitism, agents.Count);

        bestFitness = agents[0].fitness;
        bestDNA = new float[dnaLength];
        agents[0].dna.CopyTo(bestDNA, 0);

        Debug.Log($"Generation {generation} | Best fitness: {bestFitness}");

        List<GeneticAgent> newAgents = new List<GeneticAgent>();

        // --- ELITISM ---
        for (int i = 0; i < eliteCount; i++)
        {
            float[] eliteDNA = new float[dnaLength];
            agents[i].dna.CopyTo(eliteDNA, 0);
            agents[i].InitFromDNA(eliteDNA);
            newAgents.Add(agents[i]);
        }

        // --- CROSSOVER & MUTATION ---
        int reusedAgentIndex = eliteCount;

        while (newAgents.Count < agents.Count)
        {
            GeneticAgent parent1 = SelectParent();
            GeneticAgent parent2 = SelectParent();

            float[] childDNA = Crossover(parent1.dna, parent2.dna);
            Mutate(childDNA);

            if (reusedAgentIndex < agents.Count)
            {
                agents[reusedAgentIndex].InitFromDNA(childDNA);
                newAgents.Add(agents[reusedAgentIndex]);
                reusedAgentIndex++;
            }
            else
            {
                Debug.LogWarning("Not enough agents in scene to fill population after elitism.");
                break;
            }
        }

        agents = newAgents;
        generation++;
    }


    GeneticAgent SelectParent()
    {
        float totalFitness = 0f;
        foreach (var agent in agents)
            totalFitness += agent.fitness;

        float randomPoint = Random.Range(0f, totalFitness);
        float currentSum = 0f;

        foreach (var agent in agents)
        {
            currentSum += agent.fitness;
            if (currentSum >= randomPoint)
                return agent;
        }

        return agents[0];
    }

    float[] Crossover(float[] dna1, float[] dna2)
    {
        float[] child = new float[dnaLength];
        for (int i = 0; i < dnaLength; i++)
        {
            child[i] = Random.value < 0.5f ? dna1[i] : dna2[i];
        }
        return child;
    }

    void Mutate(float[] dna)
    {
        for (int i = 0; i < dna.Length; i++)
        {
            if (Random.value < mutationRate)
            {
                dna[i] += Random.Range(-0.5f, 0.5f); // Mutate slightly
            }
        }
    }
}
