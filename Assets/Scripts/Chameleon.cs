using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chameleon : MonoBehaviour
{
    public float m_Red;
    public float m_Blue;
    public float m_Green;
    public float m_LifeTime;
    private bool m_Alive = true;

    private void SetColor(){
        var renderer = GetComponent<Renderer>();
        var color = new Color(m_Red, m_Green, m_Blue, 1.0f);
        renderer.material.SetColor("_BaseColor", color);
    }

    public void Initialize()
    {
        m_Red = Random.Range(0.0f, 1.0f);
        m_Green = Random.Range(0.0f, 1.0f);
        m_Blue = Random.Range(0.0f, 1.0f);
        Initialize(m_Red, m_Green, m_Blue);
    }

    public void Initialize(float red, float green, float blue)
    {
        m_Red = red;
        m_Green = green;
        m_Blue = blue;
        m_LifeTime = 0.0f;
        gameObject.SetActive(true);
        SetColor();
        m_Alive = true;
    }

    public void Kill()
    {
        if (m_Alive){
            m_Alive = false;
            m_LifeTime = GeneticAlgorithm.m_SimulationTime;
            gameObject.SetActive(false);
        }
    }

    private void OnMouseDown() 
    {
        Kill();    
    }
}
