using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinPos : MonoBehaviour
{
    public ParticleSystem lihuaParticle;
    public ParticleSystem lihua1Particle;
    //public ParticleSystem chestOpenParticle;
    public GameObject chestOpen;
    //// Start is called before the first frame update
    //void Start()
    //{
        
    //}

    //// Update is called once per frame
    //void Update()
    //{
        
    //}
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag =="Player")
        {
            lihuaParticle.Play();
            lihua1Particle.Play();
            //chestOpenParticle.Play();
            chestOpen.SetActive(true);

        }
    }
}
