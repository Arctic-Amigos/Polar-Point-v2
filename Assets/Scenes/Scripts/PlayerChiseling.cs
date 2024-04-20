using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

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

    public Animator chiselAnimator;

    public int chiselValue = 0;

    Inventory inventory;

    WorkbenchInteract workbenchInteract;

    GameObject workbenchChisel;
    GameObject workbenchBrush;

    public bool doneCleaning;

    public Dictionary<int, int> boneChiselCount = new Dictionary<int, int>();

    public bool requestOffBench = false;

    //Progress bar
    public UnityEngine.UI.Slider progressBar;

    void Start()
    {
        inventory = GetComponent<Inventory>();
        workbenchInteract = GetComponent<WorkbenchInteract>();

        workbenchChisel = workbenchInteract.wbChisel;
        workbenchBrush = workbenchInteract.wbBrush;

        boneChiselCount.Add(0, 0);
        boneChiselCount.Add(1, 0);
        boneChiselCount.Add(2, 0);
        boneChiselCount.Add(3, 0);
        boneChiselCount.Add(4, 0);
    }

    // Update is called once per frame
    void Update()
    {
        //If player stops interacting with workbench, disable chisel and brush
        if(!workbenchInteract.IsWorkbenchInteracting())
        {
            workbenchChisel.SetActive(false);
            workbenchBrush.SetActive(false);
        }
        StartCoroutine(ChiselAnim());
        //if player interacts with workbench with a bone in their hand allow them to start chiseling
        if (Input.GetKeyDown(KeyCode.F) && inventory.inventory_pos >= 0 && workBenchFull == false) 
        {
            if (inventory.GetInventory(inventory.inventory_pos) == "ChiselableBone" ||
                inventory.GetInventory(inventory.inventory_pos) == "ChiselableBone1" ||
                inventory.GetInventory(inventory.inventory_pos) == "ChiselableBone2" ||
                inventory.GetInventory(inventory.inventory_pos) == "ChiselableBone3")
            {
                requestOffBench = false;
                //Get the inventory slot of which bone was placed onto the workbench
                currentBoneOnWorkbench = inventory.inventory_pos;
                workBenchFull = true;
                inventory.inventory_pos = -2;

                //remove bone from inventory and disable players ability to scroll
                inventory.SetInventory(currentBoneOnWorkbench, null);
                inventory.SetScrollingNotAllowed();

                bone.gameObject.SetActive(true);

                chiselValue = boneChiselCount[currentBoneOnWorkbench]; //get the chisel state of the bone currently placed on workbench
                Transform topLeft = bone.transform.GetChild(0);
                Transform topRight = bone.transform.GetChild(1);
                Transform bottomLeft = bone.transform.GetChild(2);
                Transform bottomRight = bone.transform.GetChild(3);
                if (chiselValue == 1)
                {
                    topLeft.gameObject.SetActive(false);
                    topRight.gameObject.SetActive(true);
                    bottomLeft.gameObject.SetActive(true);
                    bottomRight.gameObject.SetActive(true);
                }
                else if (chiselValue == 2)
                {
                    topLeft.gameObject.SetActive(false);
                    topRight.gameObject.SetActive(false);
                    bottomLeft.gameObject.SetActive(true);
                    bottomRight.gameObject.SetActive(true);
                }
                else if (chiselValue == 3)
                {
                    topLeft.gameObject.SetActive(false);
                    topRight.gameObject.SetActive(false);
                    bottomLeft.gameObject.SetActive(false);
                    bottomRight.gameObject.SetActive(true);
                }
                else if (chiselValue == 4)
                {
                    //Could set tag to brushable to start the brushing feature
                    Debug.Log("Bone is fully chiseled");
                    bone.gameObject.tag = "Brushable";
                    inventory.SetScrollingAllowed();
                }
                else
                {
                    topLeft.gameObject.SetActive(true);
                    topRight.gameObject.SetActive(true);
                    bottomLeft.gameObject.SetActive(true);
                    bottomRight.gameObject.SetActive(true);
                }
            }
        }
        //if player interacts with workbench with a bone on the table remove the bone and set the players inventory position to where the bone was previously
        else if(Input.GetKeyDown(KeyCode.F) && workBenchFull == true && doneCleaning) 
        {
            chiselValue = boneChiselCount[currentBoneOnWorkbench]; //get the chisel state of the bone currently placed on workbench
            inventory.inventory_pos = currentBoneOnWorkbench;
            currentBoneOnWorkbench = -5; //random number not equal to a slot in the inventory
            workBenchFull = false;

            //set the bone back into the inventory and reenable players ability to scroll
            if (chiselValue == 1)
            {
                doneCleaning = false;
                //chiseled once
                inventory.SetInventory(inventory.inventory_pos, "ChiselableBone1");
            }
            else if (chiselValue == 2)
            {
                //chiseled twice
                inventory.SetInventory(inventory.inventory_pos, "ChiselableBone2");
            }
            else if (chiselValue == 3)
            {
                //chiseled three times
                inventory.SetInventory(inventory.inventory_pos, "ChiselableBone3");
            }
            else if (chiselValue == 4) 
            {
                //completely chiseled
                //inventory.SetInventory(inventory.inventory_pos, "CleanBone");
                //Move on to brushing
            }
            else
            {
                //completely unchiseled
                inventory.SetInventory(inventory.inventory_pos, "ChiselableBone");
            }
            inventory.SetScrollingAllowed();

            Cleaning boneCleaningComponent = bone.GetComponent <Cleaning>();
            string dinosaurName = boneCleaningComponent.getDinosaurName();

            boneCleaningComponent.SetCleaningStage(0);

            inventory.SetInventory(inventory.inventory_pos, dinosaurName);

            boneChiselCount[inventory.inventory_pos] = 0;

            bone.gameObject.SetActive(false);
            requestOffBench = true;
        }

        if(Input.GetMouseButtonDown(0) && workBenchFull && inventory.inventory_pos == -2 && WithinBounds(workbenchChisel.transform.position.x, workbenchChisel.transform.position.z)) 
        {
            if(!isChiseling)
            {
                // FOR BLAKE 4/19
                //Vector3 rayOrigin = workbenchInteract.GetToolCurrentPosition();
                //rayOrigin += new Vector3(0f, 0f, -0.4f);
                //Vector3 rayDirection = Vector3.down;
                //Ray ray = new Ray(rayOrigin, rayDirection);
                //Camera wbiToolCamera = workbenchInteract.GetToolCamera();
                /*Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                RaycastHit hitinfo;

                if (Physics.Raycast(ray, out hitinfo, Mathf.Infinity, ~layerMaskToIgnore))
                {
                    ObjectChiselable chiselableObject = hitinfo.collider.gameObject.GetComponent<ObjectChiselable>();
                    
                    if (chiselableObject != null)
                    {
                        //do stuff when player clicks chiselable object
                        

                    }
                }*/
                StartCoroutine(Chiseling(bone));
            }
        }
    }

    //HARDCODED TO BONES ON WORKBENCH IN 1.5 HOMEBASE
    bool WithinBounds(float x, float z)
    {
        if(x < 2.35f && x > 1.00f && z < 3.39f && z > 2.54f)
        {
            return true;
        }
        return false;
    }


    IEnumerator ChiselAnim()
    {
        if (Input.GetMouseButton(0))
        {
            chiselAnimator.SetBool("chiselingActive", true);
            yield return new WaitForSeconds(.3f);
        }
        else
        {
            chiselAnimator.SetBool("chiselingActive", false);
        }
    }

    IEnumerator Chiseling(ObjectChiselable _chiselableObject)
    {
        
        AudioManager.instance.Play("Chisel");
        isChiseling = true;

        float startTime = Time.time;
        float elapsedTime = 0.0f;

        progressBar.gameObject.SetActive(true);

        while (elapsedTime < 3.0f && Input.GetMouseButton(0))
        {
            progressBar.value = elapsedTime / 3.0f;
            Debug.Log(elapsedTime);
            elapsedTime = Time.time - startTime;
            yield return null;
        }

        if (elapsedTime >= 3.0f)
        {

            if (boneChiselCount.ContainsKey(currentBoneOnWorkbench))
            {
                boneChiselCount[currentBoneOnWorkbench]++;
            }
            else
            {
                boneChiselCount[currentBoneOnWorkbench] = 1;
            }
            _chiselableObject.IncrementChiselCount(currentBoneOnWorkbench);
        }
        isChiseling = false;
        AudioManager.instance.Stop("Chisel");
        progressBar.gameObject.SetActive(false);
    }

    public GameObject GetWorkbenchBrush()
    {
        return workbenchBrush;
    }
}
