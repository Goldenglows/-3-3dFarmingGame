using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropBehaviour : MonoBehaviour
{

    SeedData seedToGrow;

    [Header("Stages of life")]
    public GameObject seed;
    public GameObject seeding;
    public GameObject harvestable;

    int growth;
    int maxGrowth;

    public enum CropState
    {
        Seed,Seeding,Harvestable
    }

    public CropState cropState;

    public void Plant(SeedData seedToGrow)
    {
        this.seedToGrow = seedToGrow;

        seeding = Instantiate(seedToGrow.seedling, transform);

        ItemData cropToYield = seedToGrow.cropToYield;

        harvestable = Instantiate(cropToYield.gameModel, transform);

        int hoursToGrow = GameTimestamp.DaysToHours(seedToGrow.daysToGrow);

        maxGrowth = GameTimestamp.HoursToMinutes(hoursToGrow);

        SwitchState(CropState.Seed);

    }

    public void Grow()
    {
        growth++;

        if(growth >= maxGrowth / 2 && cropState == CropState.Seed)
        {
            SwitchState(CropState.Seeding);
        }

        if(growth >= maxGrowth && cropState == CropState.Seeding)
        {
            SwitchState(CropState.Harvestable);
        }

    }

    public void SwitchState(CropState stateToSwitch)
    {
        seed.SetActive(false);
        seeding.SetActive(false);
        harvestable.SetActive(false);

        switch (stateToSwitch)
        {
            case CropState.Seed:
                seed.SetActive (true);
                break;

            case CropState.Seeding:
                seeding.SetActive (true); 
                break;

            case CropState.Harvestable:
                harvestable.SetActive(true);
                harvestable.transform.parent = null;

                Destroy(gameObject);

                break;

        }

    }


}
