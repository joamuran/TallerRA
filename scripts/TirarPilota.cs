using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Tot componente que afegim a un GameObject ha de descendir
// de la classe MonoBehaviour
public class TirarPilota : MonoBehaviour
{
    // Definició dels camps de la classe
    Vector2 startPos, endPos, direction, mpos;      // Posicions en arrossegar el dit per la pantalla
    Vector3 initialPosition;                        // Posició inicial de l'objecte

    float touchTimeStart, touchTimeFinish, timeInterval, timeM; // Temps

    // Camps serialitzables: Són privats, però els podem donar valor
    // des de l'Inspector
    [SerializeField] AudioClip pong;                   // Clip d'audio
    [SerializeField] float thowForceInXandY=10f;       // Força del llançament en X i Y
    [SerializeField] float thowForceInZ=30f;          // Força del llançament en Z

    GameObject debugText;                           // Text de depuració
    Rigidbody rb;                          // Referència al component RigidBody


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start");                     // Aquests missatges els voríem al Log

        // Inicialitzem la referència al RigidBody
        rb=GetComponent<Rigidbody>();           
        
        // Establim la posició inicial de la pilota
        //initialPosition=new Vector3(0, 3, -1);    // Si la posem en el mateix target
        initialPosition=new Vector3(0, -0.3f, 0.5f);    // Si va lligada a la càmera
        transform.localPosition=initialPosition;

        // inicialitzant l'audio
        GetComponent<AudioSource> ().playOnAwake = false;
        GetComponent<AudioSource> ().clip = pong;

        // Per preparar la depuració depurar
        debugText=GameObject.Find("debug");
        //if (debugText) debugText.GetComponent<TMPro.TextMeshPro>().text="Inici";

    }



    void Update(){
        // Depuració de la posició de la pilota
        //if (debugText) debugText.GetComponent<TMPro.TextMeshPro>().text=transform.position.x.ToString("0.000")+";"+transform.position.y.ToString("0.000")+";"+transform.position.z.ToString("0.000");
    }

    void OnMouseDown(){
        // Detecta quan iniciem el "tap"

        Debug.Log("MouseDown");
        touchTimeStart = Time.time;
        startPos=Input.mousePosition;
        //startPos=Camera.main.ScreenToWorldPoint(startPos);

    }

    void OnMouseUp(){
        Debug.Log("MouseUp");
        // Per calcular com de ràpid ha fet el llançament
        touchTimeFinish=Time.time;
        timeInterval=touchTimeFinish-touchTimeStart;

        // Distancia entre la posició inicial i final
        endPos=Input.mousePosition;
        
        // Calculem la direcció del llançament
        direction=endPos-startPos;
        
        // Indiquem que no és cinemàtic sino físic (per poder aplicar-li forces)
        rb.isKinematic=false;
        
        
         // Càlcul de les forces

         float forceX=direction.x*thowForceInXandY/2;   // ref 0
         float forceY=thowForceInZ/(timeInterval*20)*-1f;    // ref -300
         float forceZ=direction.y * thowForceInXandY/4; // ref 100

         // Mostrem les forces en el "debug"
         // GameObject.Find("debug").GetComponent<TMPro.TextMeshPro>().text=(forceX).ToString("0.000")+";"+(forceY).ToString("0.000")+";"+(forceZ).ToString("0.000");
        
        // Apliquem les forces
        rb.AddRelativeForce(forceX, forceY , forceZ);
        //rb.AddRelativeForce(new Vector3(0,-300,100)); 
        
        // Iniciem una corrutina per tornar a posar la bola
        StartCoroutine(restartBall());

    }

    private IEnumerator restartBall(){
        // Esperem 3 segons...
        yield return new WaitForSeconds(3f);

        // Fem el so de la bola, i la reinicialitzem
        GetComponent<AudioSource>().Play();
        rb.isKinematic=true;        // Ara és cinemàtica (no física)
        transform.localPosition=initialPosition;    // Posició inicial
        rb.velocity=Vector3.zero;                   // I sense velocitat
        
    }
    private void OnTriggerEnter(Collider other)
    {
        // Detecta col·lissió amb el bitxo
        if (other.gameObject.tag=="Enemy"){
            // Amaguem el bitxo
            other.gameObject.transform.GetChild(0).gameObject.SetActive(false);

            // I el tornem a mostrar en 10 segons
            StartCoroutine(reloadJaumemon(other.gameObject.transform.GetChild(0).gameObject));
        }
        
    }


    private IEnumerator reloadJaumemon(GameObject go){
        // Mostra el gameObject al cap de 10 segons
        yield return new WaitForSeconds(10f);
        go.SetActive(true);        
        
    }

}
