using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Clase que controla el comportamiento de la cadena
/// </summary>
public class ChainController : MonoBehaviour
{
    /// <summary>
    /// Lista de los eslabones que conforman la cadena
    /// </summary>
    public List<GameObject> chain;
    
    private Rigidbody chainRb;
  
    /// <summary>
    /// Puntos de ascenso y descenso que se toman como referencia para mover la cadena
    /// </summary>
    private Transform pointAscend;
    private Transform pointDescend;     

    /// <summary>
    /// Referencia al objeto auxiliar situado al principio de la cadena
    /// </summary>
    public GameObject auxObject;

    /// <summary>
    /// Vector donde guardamos la posicion del objeto auxiliar
    /// </summary>
    Vector3 startPosition;

    /// <summary>
    /// Referencia al controlador de joints
    /// </summary>
    JointController jc;    

   /// <summary>
   /// Velocidad de movimiento de la cadena
   /// </summary>
    float speed = 2;

    /// <summary>
    /// Variable auxiliar para recorrer la cadena
    /// </summary>
    public int i;
    
    /// <summary>
    /// Booleanos auxiliares para controlar el movimiento de la cadena mediante los botones
    /// </summary>
    bool click = false;
    bool click2 = false;

    float step;

    // Use this for initialization
    void Start()
    {       
        pointAscend = GameObject.Find("PointToAscend").GetComponent<Transform>();
        pointDescend = GameObject.Find("PointToDescend").GetComponent<Transform>();

        startPosition = auxObject.transform.position;   

        i = 0;      
    }

    // Update is called once per frame
    void Update()
    {
        //Recorro todos los eslabones desactivados y los posiciono en el mismo punto de ascenso 
        //para que al spawnearlos se encuentren en el lugar adecuado
        
        for (int i = 0; i < chain.Count; i++)
        {
            if (!chain[i].activeSelf)
            {
                chain[i].transform.position = pointAscend.position;
            }
        }

        step = speed * Time.deltaTime;

        //Booleanos para llamar a los diferentes metodos
        if (click2)
            releaseChain();

        if (click)
            collectChain();


        auxObject.transform.position = new Vector3(pointAscend.position.x, auxObject.transform.position.y, pointAscend.position.z);
    }

    public void clickToTrue()
    {
        click = true;

        //Le seteo la gravedad de nuevo al ultimo elemento de la cadena
        chainRb = chain[i].GetComponent<Rigidbody>();

        chainRb.useGravity = true;
        chainRb.isKinematic = false;
    }

    public void clickToFalse()
    {
        click = false;

        //Le seteo la gravedad de nuevo al ultimo elemento de la cadena
        chainRb = chain[i].GetComponent<Rigidbody>();

        chainRb.useGravity = true;
        chainRb.isKinematic = false;
    }

    //Metodos para el control de los botones
    public void click2ToTrue()
    {
        click2 = true;
    }

    public void click2ToFalse()
    {
        click2 = false;
    }

    //Subida de la cadena
    public void collectChain()
    {
        if (i != 23)
        {
            //Por cada eslabon se habilita el movimiento poniendolos kinematic
            chainRb = chain[i].GetComponent<Rigidbody>();
            jc = chain[i].GetComponent<JointController>();

            chainRb.useGravity = false;
            chainRb.isKinematic = true;

            //Se mueven hasta el punto de ascenso
            chain[i].transform.position = Vector3.MoveTowards(chain[i].transform.position, pointAscend.position, step);

            //Cuando llega al punto de ascenso se desactiva y se asignan los valores correspondientes al siguiente eslabon
            if (chain[i].transform.position == pointAscend.position)
            {
                chain[i].SetActive(false);
                jc.assignNext();
            }
        }
    }

    //Bajada de la cadena
    public void releaseChain()
    {
        //Por cada eslabon se habilita su gravedad, activandola
        chainRb = chain[i].GetComponent<Rigidbody>();
        jc = chain[i].GetComponent<JointController>();

        chainRb.useGravity = true;
        chainRb.isKinematic = false;

        if (i > 0)
        {
            //Se mueve el objeto auxiliar hasta el punto de descenso
            auxObject.transform.position = Vector3.MoveTowards(auxObject.transform.position, pointDescend.position, step);

            //Durante el movimiento
            if (auxObject.transform.position.y <= pointDescend.position.y)
            {
                //Activamos el eslabon previo y se asignan los valores correspondientes
                auxObject.transform.position = startPosition;
                chain[i - 1].SetActive(true);
                jc.assignPrevious();
            }
        }

    }

}


