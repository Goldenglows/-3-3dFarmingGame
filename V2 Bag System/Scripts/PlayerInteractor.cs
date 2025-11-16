using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    PlayerController playerController;

    Land selectedLand;

    // Start is called before the first frame update
    void Start()
    {
        playerController = transform.parent.GetComponent<PlayerController>(); 
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, Vector3.down, out hit, 1))
        {
            OnInteractableHit(hit);
        }
    }

    void OnInteractableHit(RaycastHit hit)
    {
        //Debug.Log(hit);
        Collider other = hit.collider;

        if(other.tag == "Land")
        {
            Land land = other.GetComponent<Land>();
            SelectLand(land);
            return;
        }

        if (selectedLand != null)
        {
            selectedLand.Select(false);
            selectedLand = null;
        }


    }

    void SelectLand(Land land)
    {
        if(selectedLand != null)
        {
            selectedLand.Select(false);
        }
        selectedLand = land;
        land.Select(true);
    }

    public void Interact()
    {
        if(selectedLand != null) {
            selectedLand.Interact();
            return;
        }
    }

}
