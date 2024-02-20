using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    // First 3 spots in inventory will be static for hand, pickaxe, brush. Last 5 spots will be open for fossils/bones
    // Hand, Pickaxe, Brush | Fossil, Fossil, Fossil, Fossil, Fossil
    // -3  , -2     , -1    | 0     , 1     , 2     , 3     , 4
    string[] fossil_inventory = new string[5]; // Inventory spots that will be filled up with fossils/bones. Type can be changed to fossil/bone type as needed
    public int inventory_pos; // Position in inventory that will be scrolled through
    GameObject Bone1;
    GameObject Bone2;
    GameObject Bone3;
    GameObject Bone4;
    GameObject Bone5;
    // Start is called before the first frame update
    void Start()
    {
        inventory_pos = -3;
        StartDisplayBones();
    }

    // Update is called once per frame
    void Update()
    {
        // Number button functionality
        if (Input.GetKeyDown(KeyCode.Alpha1))
            inventory_pos = -3;
        if (Input.GetKeyDown(KeyCode.Alpha2))
            inventory_pos = -2;
        if (Input.GetKeyDown(KeyCode.Alpha3))
            inventory_pos = -1;
        if (Input.GetKeyDown(KeyCode.Alpha4))
            inventory_pos = 0;
        if (Input.GetKeyDown(KeyCode.Alpha5))
            inventory_pos = 1;
        if (Input.GetKeyDown(KeyCode.Alpha6))
            inventory_pos = 2;
        if (Input.GetKeyDown(KeyCode.Alpha7))
            inventory_pos = 3;
        if (Input.GetKeyDown(KeyCode.Alpha8))
            inventory_pos = 4;
        // Scrolling functionality
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll > 0)
            inventory_pos++;
        else if (scroll < 0)
            inventory_pos--;
        if (inventory_pos > 4)
            inventory_pos = -3;
        if (inventory_pos < -3)
            inventory_pos = 4;
        // Other functions
        UpdateDisplayBones();
    }
    public string GetInventory(int index)
    {
        return fossil_inventory[index];
    }

    public void SetInventory(int index, string item)
    {
        fossil_inventory[index] = item;
    }
    public bool IndexIsEmpty(int index) // Returns whether an index is empty
    {
        if (fossil_inventory[index] == null) 
            return true;
        return false;
    }
    public int FirstEmpty() // Returns the index of the first empty index or 9 if all inventory slots are full
    {
        if (IndexIsEmpty(0))
            return 0;
        else if (IndexIsEmpty(1))
            return 1;
        else if (IndexIsEmpty(2))
            return 2;
        else if (IndexIsEmpty(3))
            return 3;
        else if (IndexIsEmpty(4))
            return 4;
        return 9;
    }
    public void StartDisplayBones()
    {
        Bone1 = GameObject.FindWithTag("Bone1Tag");
        Bone2 = GameObject.FindWithTag("Bone2Tag");
        Bone3 = GameObject.FindWithTag("Bone3Tag");
        Bone4 = GameObject.FindWithTag("Bone4Tag");
        Bone5 = GameObject.FindWithTag("Bone5Tag");

        Bone1.SetActive(false);
        Bone2.SetActive(false);
        Bone3.SetActive(false);
        Bone4.SetActive(false);
        Bone5.SetActive(false);
    }
    public void UpdateDisplayBones()
    {
        if (GetInventory(0) == "bone")
            Bone1.SetActive(true);
        if (GetInventory(1) == "bone")
            Bone2.SetActive(true);
        if (GetInventory(2) == "bone")
            Bone3.SetActive(true);
        if (GetInventory(3) == "bone")
            Bone4.SetActive(true);
        if (GetInventory(4) == "bone")
            Bone5.SetActive(true);

        if (GetInventory(0) == null)
            Bone1.SetActive(false);
        if (GetInventory(1) == null)
            Bone2.SetActive(false);
        if (GetInventory(2) == null)
            Bone3.SetActive(false);
        if (GetInventory(3) == null)
            Bone4.SetActive(false);
        if (GetInventory(4) == null)
            Bone5.SetActive(false);
    }

}
