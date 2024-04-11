using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPodiumInteract : MonoBehaviour
{
    public GameObject Carnotaurus;
    GameObject Pedestal;
    Transform LeftLeg;
    Transform RightLeg;
    Transform Body;
    Transform LeftArm;
    Transform RightArm;
    Transform Tail;
    Transform Head;
    // Start is called before the first frame update
    void Start()
    {
        LeftLeg = Carnotaurus.transform.GetChild(0);
        RightLeg = Carnotaurus.transform.GetChild(1);
        Body = Carnotaurus.transform.GetChild(2);
        LeftArm = Carnotaurus.transform.GetChild(3);
        RightArm = Carnotaurus.transform.GetChild(4);
        Tail = Carnotaurus.transform.GetChild(5);
        Head = Carnotaurus.transform.GetChild(6);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("CarnotaurusPedestal"))
        {

        }
    }

    private void OnTriggerExit(Collider other)
    {

    }
}
