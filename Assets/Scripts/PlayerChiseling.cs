using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChiseling : MonoBehaviour

{
    //check to see if player is currently chiseling
    private bool isChiseling = false;

    //bone currently placed on workbench
    public int currentBoneOnWorkbench = -5;

    //see if workbench has a bone on it
    private bool workBenchFull = false;

    //Set this to Player so that the rays ignore the players model also set this to DirtCoveringBone
    public LayerMask layerMaskToIgnore;

    //Get bone from workbench
    public ObjectChiselable bone;

    public int chiselValue = 0;

    Inventory inventory;

    public Dictionary<int, int> boneChiselCount = new Dictionary<int, int>();

    void Start()
    {
        inventory = GetComponent<Inventory>();
        boneChiselCount.Add(0, 0);
        boneChiselCount.Add(1, 0);
        boneChiselCount.Add(2, 0);
        boneChiselCount.Add(3, 0);
        boneChiselCount.Add(4, 0);
    }

    // Update is called once per frame
    void Update()
    {
        //if player interacts with workbench with a bone in their hand allow them to start chiseling
        if (Input.GetKeyDown(KeyCode.F) && inventory.inventory_pos >= 0 && workBenchFull == false)
        {
            //Get the inventory slot of which bone was placed onto the workbench
            currentBoneOnWorkbench = inventory.inventory_pos;
            workBenchFull = true;
            inventory.inventory_pos = -1;

            //remove bone from inventory and disable players ability to scroll
            inventory.SetInventory(currentBoneOnWorkbench, null);
            inventory.SetScrollingNotAllowed();

            bone.gameObject.SetActive(true);

            chiselValue = boneChiselCount[currentBoneOnWorkbench]; //get the chisel state of the bone currently placed on workbench
            Transform rockLayer = bone.transform.GetChild(0);
            Transform heavyDirtLayer = bone.transform.GetChild(1);
            Transform lightDirtLayer = bone.transform.GetChild(2);
            if (chiselValue == 1)
            {
                rockLayer.gameObject.SetActive(false);
                heavyDirtLayer.gameObject.SetActive(true);
                lightDirtLayer.gameObject.SetActive(true);
            }else if (chiselValue == 2)
            {
                rockLayer.gameObject.SetActive(false);
                heavyDirtLayer.gameObject.SetActive(false);
                lightDirtLayer.gameObject.SetActive(true);
            }else if (chiselValue == 3) {
                //Could set tag to brushable to start the brushing feature
                Debug.Log("Bone is fully chiseled");
                bone.gameObject.SetActive(false);
            }else
            {
                rockLayer.gameObject.SetActive(true);
                heavyDirtLayer.gameObject.SetActive(true);
                lightDirtLayer.gameObject.SetActive(true);
            }
        }
        //if player interacts with workbench with a bone on the table remove the bone and set the players inventory position to where the bone was previously
        else if(Input.GetKeyDown(KeyCode.F) && workBenchFull == true)
        {
            chiselValue = boneChiselCount[currentBoneOnWorkbench]; //get the chisel state of the bone currently placed on workbench
            inventory.inventory_pos = currentBoneOnWorkbench;
            currentBoneOnWorkbench = -5; //random number not equal to a slot in the inventory
            workBenchFull = false;

            //set the bone back into the inventory and reenable players ability to scroll
            if(chiselValue == 1)
            {
                inventory.SetInventory(inventory.inventory_pos, "ChiselableBone1");
            }else if(chiselValue == 2)
            {
                inventory.SetInventory(inventory.inventory_pos, "ChiselableBone2");
            }else if(chiselValue == 3) {
                inventory.SetInventory(inventory.inventory_pos, "CleanBone");
            }else
            {
                inventory.SetInventory(inventory.inventory_pos, "ChiselableBone");
            }
            inventory.SetScrollingAllowed();

            bone.gameObject.SetActive(false);
        }

        if(Input.GetMouseButtonDown(0) && workBenchFull && inventory.inventory_pos == -1)
        {
            if (!isChiseling)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hitinfo;

                if (Physics.Raycast(ray, out hitinfo, Mathf.Infinity, ~layerMaskToIgnore))
                {
                    ObjectChiselable chiselableObject = hitinfo.collider.gameObject.GetComponent<ObjectChiselable>();
                    
                    if (chiselableObject != null)
                    {
                        //do stuff when player clicks chiselable object
                        StartCoroutine(Chiseling(chiselableObject));

                    }
                }
            }
        }
    }
    IEnumerator Chiseling(ObjectChiselable _chiselableObject)
    {
        isChiseling = true;
        yield return new WaitForSeconds(3f);
        if (boneChiselCount.ContainsKey(currentBoneOnWorkbench))
        {
            boneChiselCount[currentBoneOnWorkbench]++;
        }
        else
        {
            boneChiselCount[currentBoneOnWorkbench] = 1;
        }
        _chiselableObject.IncrementChiselCount(currentBoneOnWorkbench);
        isChiseling = false;
    }
}
