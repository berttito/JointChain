using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Clase que gestiona la asignacion de los joints en funcion de si la cadena esta subiendo o bajando
/// </summary>
public class JointController : MonoBehaviour {

    /// <summary>
    /// Referencia al siguiente joint de la cadena
    /// </summary>
    public HingeJoint next;

    /// <summary>
    /// Referencia al gameobject auxiliar situado al principio de la cadena
    /// </summary>
    private Rigidbody auxRb;    

    /// <summary>
    /// Referencia al script que gestiona el comportamiento de la cadena
    /// </summary>
    ChainController cc;

    /// <summary>
    /// Referencia al joint anterior de la cadena
    /// </summary>
    public HingeJoint previos;

    /// <summary>
    /// Referencia al joint que tiene cada eslabon de la cadena
    /// </summary>
    public HingeJoint thisJoint;

    private void Start()
    {
        auxRb = GameObject.Find("GameObject").GetComponent<Rigidbody>();       
        cc = GameObject.Find("Controller").GetComponent<ChainController>();
    }

    public void assignNext()
    {
        //En el caso de que suba la cadena asigno al primer eslabon el rigidbody del gameobject auxiliar situado al principio de la cadena
        next.connectedBody = auxRb;        
        cc.i++;        
    }

   public void assignPrevious()
   {
        //En el caso de que baje la cadena asigno al joint del eslabon actual el rigidbody del previo 
        //y al previo el rigidbody del gameobject auxiliar situado al principio de la cadena

        thisJoint.connectedBody = previos.GetComponent<Rigidbody>();
        previos.connectedBody = auxRb;
        cc.i--;
   }
}
