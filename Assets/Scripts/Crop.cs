using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Crop : MonoBehaviour
{
    //MAHSUL SCRİPTİ

    private CropData curCrop;
    private int plantDay;
    private int daysSinceLastWatered;

    public SpriteRenderer sr;

    public static event UnityAction<CropData> onPlantCrop;
    public static event UnityAction<CropData> onHarvestCrop;

    // ilk ekim
    public void Plant (CropData crop)
    {
        curCrop = crop;
        plantDay = GameManager.instance.curDay;
        daysSinceLastWatered = 1;
        UpdateCropSprite();

        onPlantCrop?.Invoke(crop);
    }

    // yeni gün
    public void NewDayCheck ()
    {
        daysSinceLastWatered++;

        if(daysSinceLastWatered > 3)
        {
            Destroy(gameObject);
        }

        UpdateCropSprite();
    }

    // mahsul büyüdüğünde
    void UpdateCropSprite ()
    {
        int cropProg = CropProgress();

        if(cropProg < curCrop.daysToGrow)
        {
            sr.sprite = curCrop.growProgressSprites[cropProg];
        }
        else
        {
            sr.sprite = curCrop.readyToHarvestSprite;
        }
    }

    // sulama işlemi
    public void Water ()
    {
        daysSinceLastWatered = 0;
    }

    // hasat ekme
    public void Harvest ()
    {
        if(CanHarvest())
        {
            onHarvestCrop?.Invoke(curCrop);
            Destroy(gameObject);
        }
    }

    // gün sayısını döndürür
    int CropProgress ()
    {
        return GameManager.instance.curDay - plantDay;
    }

    // hassat edebilir miyiz?
    public bool CanHarvest ()
    {
        return CropProgress() >= curCrop.daysToGrow;
    }
}