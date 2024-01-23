using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomParticleStarter : MonoBehaviour
{
    private ParticleSystem embers;
    private float counter;
    public Vector2 randomRange;

    void Start()
    {
        embers = GetComponent<ParticleSystem>();
    }


    void Update()
    {
        DecreaseCounter();
        
    }

    private void DecreaseCounter()
    {
        if(counter <= 0)
        {
            embers.Play();
            counter = Random.Range(randomRange.x, randomRange.y);
        }
        if(counter > 0)
        {
            counter -= 1 * Time.deltaTime;
        }
    }
}
