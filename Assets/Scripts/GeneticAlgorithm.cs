using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GeneticAlgorithm : MonoBehaviour
{
    public Vector2 m_Limit = new Vector2(16, 10);
    public GameObject m_ChameleonPrefab;
    public int m_PopulationSize = 20;
    public List<Chameleon> m_Population = new List<Chameleon>();
    public static float m_SimulationTime = 0.0f;
    public int m_TournamentSize = 2;
    public float m_MutationRate = 0.02f;
    public int m_Generation = 0;
    private bool m_Running = false;

    private void Start() {
        InitiliazeRandomPopulation();
    }

    public void InitiliazeRandomPopulation()
    {
        for (int i = 0; i < m_PopulationSize; i++)
        {
            var go = Instantiate(m_ChameleonPrefab);
            go.transform.position = GetRandomPosition();

            var chameleon = go.GetComponent<Chameleon>();
            chameleon.Initialize();

            m_Population.Add(chameleon);
        }
    }

    private Vector2 GetRandomPosition()
    {
        var position = Vector3.zero;
        position.x = Random.Range(-m_Limit.x / 2.0f, m_Limit.x / 2.0f);
        position.y = Random.Range(-m_Limit.y / 2.0f, m_Limit.y / 2.0f);
        position.z = 0.0f;
        return position;
    }

    public Chameleon Selection() 
    { 
        List<Chameleon> list = new List<Chameleon>();
        for (int i = 0; i < m_TournamentSize; i++)
        {
            var index = Random.Range(0, m_PopulationSize);
            list.Add(m_Population[index]);
        }
        
        return list.OrderByDescending(x => x.m_LifeTime).ToList()[0];
    }

    public bool HasSurvivors => m_Population.Count(x => x.gameObject.activeInHierarchy) > 0;

    public float m_MaxTime = 10.0f;

    private void Update() {
        m_SimulationTime += Time.deltaTime;
        if (!HasSurvivors || m_SimulationTime >= m_MaxTime){
            for (int i = 0; i < m_PopulationSize; i++){
                m_Population[i].Kill();
            }

            NextGeneration();
        }
    }

    public void Crossover(Chameleon parent1, Chameleon parent2, Chameleon chameleon) 
    { 
        float red = (parent1.m_Red + parent2.m_Red) / 2.0f;
        float green = (parent1.m_Green + parent2.m_Green) / 2.0f;
        float blue = (parent1.m_Blue + parent2.m_Blue) / 2.0f;
        chameleon.Initialize(red, green, blue);
    }

    public void Mutation(Chameleon chameleon) 
    {
        if (Random.Range(0.0f, 1.0f) < m_MutationRate) chameleon.m_Red = Random.Range(0.0f, 1.0f);
        if (Random.Range(0.0f, 1.0f) < m_MutationRate) chameleon.m_Green = Random.Range(0.0f, 1.0f);
        if (Random.Range(0.0f, 1.0f) < m_MutationRate) chameleon.m_Blue = Random.Range(0.0f, 1.0f);
    }

    public void NextGeneration() { 
        

        m_Population = m_Population.OrderByDescending(x => x.m_LifeTime).ToList();
        var copy = m_Population[0];

        
        List<Chameleon> newPopulation = new List<Chameleon>();

        var child = Instantiate(m_ChameleonPrefab);
        child.transform.position = GetRandomPosition();
        var chameleon = child.GetComponent<Chameleon>();
        chameleon.Initialize(copy.m_Red, copy.m_Green, copy.m_Blue);
        newPopulation.Add(chameleon);

        while (newPopulation.Count < m_PopulationSize) {
            var parent1 = Selection();
            var parent2 = Selection();
            child = Instantiate(m_ChameleonPrefab);
            child.transform.position = GetRandomPosition();
            chameleon = child.GetComponent<Chameleon>();
            Crossover(parent1, parent2, chameleon);
            Mutation(chameleon);
            newPopulation.Add(chameleon);
        }

        for (int i = 0; i < m_PopulationSize; i++)
            Destroy(m_Population[i].gameObject);

        m_Population = newPopulation;
        m_Generation++;
        m_SimulationTime = 0.0f;
    }
}