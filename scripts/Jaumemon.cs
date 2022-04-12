using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jaumemon : MonoBehaviour
{
    ParticleSystem ps;  // ref al sistema de partícules
    // Start is called before the first frame update
    void Start()
    {
        // Inicialment desactivem el SP
        ps=GetComponent<ParticleSystem>();
        ps.Stop();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Detectem col·lissió amb la pilota
        Debug.Log("Collide");
        Debug.Log(other.gameObject.tag);

        if (other.gameObject.tag=="Ball"){
            // Si col·lissiona, activem el sistema de partícules i el parem als 5 segons
            Debug.Log("Xoc pilota");
            ps.Play(true);
            StartCoroutine(stopParticles());
            
        }
        
    }

    private IEnumerator stopParticles(){
        yield return new WaitForSeconds(5f);
        ps.Stop();
    }
}
