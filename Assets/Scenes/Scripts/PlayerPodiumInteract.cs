using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPodiumInteract : MonoBehaviour
{
    Podium currentPodium = null;

    //Only need these to allow for persistent bone showing
    public Podium carnotaurusPodium;
    public Podium triceratopsPodium;
    public Podium spinosaurusPodium;

    Transform[] carnotaurusParts;
    Transform[] triceratopsParts;
    Transform[] spinosaurusParts;

    Inventory inventory;

    static int numBonesOnCar = 0;
    static int numBonesOnTri = 0;
    static int numBonesOnSpi = 0;

    // Start is called before the first frame update
    void Start()
    {
        inventory = GetComponent<Inventory>();

        carnotaurusParts = new Transform[]
        {
            carnotaurusPodium.transform.GetChild(0).GetChild(0),
            carnotaurusPodium.transform.GetChild(0).GetChild(1),
            carnotaurusPodium.transform.GetChild(0).GetChild(2),
            carnotaurusPodium.transform.GetChild(0).GetChild(3),
            carnotaurusPodium.transform.GetChild(0).GetChild(4),
            carnotaurusPodium.transform.GetChild(0).GetChild(5),
            carnotaurusPodium.transform.GetChild(0).GetChild(6)
        };

        triceratopsParts = new Transform[]
        {
            triceratopsPodium.transform.GetChild(0).GetChild(0),
            triceratopsPodium.transform.GetChild(0).GetChild(1),
            triceratopsPodium.transform.GetChild(0).GetChild(2),
            triceratopsPodium.transform.GetChild(0).GetChild(3),
            triceratopsPodium.transform.GetChild(0).GetChild(4),
            triceratopsPodium.transform.GetChild(0).GetChild(5),
            triceratopsPodium.transform.GetChild(0).GetChild(6)
        };

        spinosaurusParts = new Transform[]
        {
            spinosaurusPodium.transform.GetChild(0).GetChild(0),
            spinosaurusPodium.transform.GetChild(0).GetChild(1),
            spinosaurusPodium.transform.GetChild(0).GetChild(2),
            spinosaurusPodium.transform.GetChild(0).GetChild(3),
            spinosaurusPodium.transform.GetChild(0).GetChild(4),
            spinosaurusPodium.transform.GetChild(0).GetChild(5),
            spinosaurusPodium.transform.GetChild(0).GetChild(6)
        };
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.M)) {
            numBonesOnCar++;
            numBonesOnSpi++;
            numBonesOnTri++;
        }
        for(int i = 0; i < carnotaurusParts.Length; i++)
        {
            carnotaurusParts[i].gameObject.SetActive(i < numBonesOnCar);
        }
        for (int i = 0; i < triceratopsParts.Length; i++)
        {
            triceratopsParts[i].gameObject.SetActive(i < numBonesOnTri);
        }
        for (int i = 0; i < spinosaurusParts.Length; i++)
        {
            if(numBonesOnSpi == 3)
            {
                spinosaurusPodium.transform.GetChild(0).GetChild(2).GetChild(0).gameObject.SetActive(true);
            }
            spinosaurusParts[i].gameObject.SetActive(i < numBonesOnSpi);
        }


        if (currentPodium != null && currentPodium.tag == "CarnotaurusPodium")
        {
            if(Input.GetKeyDown(KeyCode.F) && inventory.inventory_pos >= 0 && inventory.GetInventory(inventory.inventory_pos) == "Carnotaurus")
            {
                numBonesOnCar++;
                inventory.SetInventory(inventory.inventory_pos, null);

                Debug.Log("You placed a Carnotaurus bone");
            }
        }else if(currentPodium != null && currentPodium.tag == "TriceratopsPodium")
        {
            if (Input.GetKeyDown(KeyCode.F) && inventory.inventory_pos >= 0 && inventory.GetInventory(inventory.inventory_pos) == "Triceratops")
            {
                numBonesOnTri++;
                inventory.SetInventory(inventory.inventory_pos, null);
                Debug.Log("You placed a Triceratops bone");
            }
        }else if(currentPodium != null && currentPodium.tag == "SpinosaurusPodium")
        {
            Debug.Log("in spino pod");
            if (Input.GetKeyDown(KeyCode.F) && inventory.inventory_pos >= 0 && inventory.GetInventory(inventory.inventory_pos) == "Spinosaurus")
            {
                numBonesOnSpi++;
                inventory.SetInventory(inventory.inventory_pos, null);
                Debug.Log("You placed a Spinosaurus bone");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Podium obj;
        if (other.TryGetComponent(out obj))
        {
            Debug.Log("called");
            currentPodium = obj;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        currentPodium = null;
    }
}
