using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class GeneticManager : MonoBehaviour
{
    [Tooltip("Seconds per generation")]
    public float generationTime = 10f;

    private float timer = 0f;
    private List<GeneticAgent> agents;
    public float elite_percent = 0.2f;
    public float temperature = 1.0f;

    int elite_count = 1;

    public int survivingAgent;
    public float fitness;
    public List<float> fitnessHistory = new List<float>();
    public List<float> stdHistory = new List<float>();

    void Start()
    {
        agents = FindObjectsOfType<GeneticAgent>().ToList();
        survivingAgent = agents.Count;
        elite_count = Mathf.Max(1, Mathf.CeilToInt(agents.Count * elite_percent));
    }

    void FixedUpdate()
    {
        timer += Time.deltaTime;
        if (timer >= generationTime || survivingAgent <= 0)
        {
            timer = 0f;
            EvolveGeneration();
        }
    }

    void EvolveGeneration()
    {
        float mean = agents.Average(a => a.fitness);
        float std = agents.Sum(a => Mathf.Pow(a.fitness - mean, 2)) / agents.Count;

        fitness = agents.Max(a => a.fitness);
        Debug.Log($"Best Fitness this gen: {fitness}");
        fitnessHistory.Add(mean);
        stdHistory.Add(Mathf.Sqrt(std));

        var sortedAgents = agents.OrderByDescending(a => a.fitness).ToList();
        var eliteAgents = sortedAgents.Take(elite_count).ToList();

        float[] fitnesses = agents.Select(a => a.fitness).ToArray();
        float[] probabilities = new float[fitnesses.Length];
        float sum = 0f;

        for (int i = 0; i < fitnesses.Length; i++)
        {
            probabilities[i] = Mathf.Exp(fitnesses[i] / temperature);
            sum += probabilities[i];
        }

        for (int i = 0; i < probabilities.Length; i++)
            probabilities[i] /= sum;

        foreach (var agent in agents)
        {
            if (eliteAgents.Contains(agent)) continue;

            GeneticAgent parent = agents[SelectIndexByProbability(probabilities)];
            CloneNetwork(parent, agent);
        }

        // Restart all agents
        foreach (var agent in agents)
        {
            agent.fitness = 0f;
            agent.OnEpisodeStart();
            agent.gameObject.SetActive(true);
        }
        survivingAgent = agents.Count;
    }

    int SelectIndexByProbability(float[] probabilities)
    {
        float r = Random.value;
        float cumulative = 0f;

        for (int i = 0; i < probabilities.Length; i++)
        {
            cumulative += probabilities[i];
            if (r <= cumulative)
                return i;
        }
        return probabilities.Length - 1;
    }

    void CloneNetwork(GeneticAgent source, GeneticAgent target)
    {
        for (int layer = 0; layer < source.network.Length; layer++)
        {
            var src = source.network[layer];
            var dst = target.network[layer];

            int rows = src.weight.GetLength(0);
            int cols = src.weight.GetLength(1);

            for (int r = 0; r < rows; r++)
                for (int c = 0; c < cols; c++)
                    dst.weight[r, c] = src.weight[r, c];

            for (int r = 0; r < dst.bias.GetLength(0); r++)
                dst.bias[r, 0] = src.bias[r, 0];

            target.network[layer] = dst;
        }
    }
}
