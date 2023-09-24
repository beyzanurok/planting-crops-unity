using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class GameManager : MonoBehaviour
{
    public int curDay;
    public int money;
    public int cropInventory;

   
    public CropData selectedCropToPlant;
    public TextMeshProUGUI statsText;

    public event UnityAction onNewDay;

    
    public static GameManager instance;

    void OnEnable ()
    {
        Crop.onPlantCrop += OnPlantCrop;
        Crop.onHarvestCrop += OnHarvestCrop;
    }

    void OnDisable ()
    {
        Crop.onPlantCrop -= OnPlantCrop;
        Crop.onHarvestCrop -= OnHarvestCrop;
    }

    void Awake ()
    {
        // Initialize the singleton.
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    
    protected void Start()
    {
       
        UpdateStatsText();
    }


    // DİĞER GÜN BUTONU
    public void SetNextDay ()
    {
        curDay++;
        onNewDay?.Invoke();
        UpdateStatsText();
    }

    // ÜRÜN EKİLDİĞİNDE ÇALIŞIR
    public void OnPlantCrop (CropData cop)
    {
        cropInventory--;
        UpdateStatsText();
    }

    // HASAT EDİLDİĞİNDE ÇAĞIRILIR
    public void OnHarvestCrop (CropData crop)
    {
        money += crop.sellPrice;
        UpdateStatsText();
    }

    // ÜRÜN SATIN ALMAK İSTENDİĞİNDE ÇALIŞIR
    public void PurchaseCrop (CropData crop)
    {
        money -= crop.purchasePrice;
        cropInventory++;
        UpdateStatsText();
    }

    // MAHSUL VERİLERİNİ KONTROL EDER
    public bool CanPlantCrop ()
    {
        return cropInventory > 0;
    }

    // SATIN AL BUTONU
    public void OnBuyCropButton (CropData crop)
    {
        if(money >= crop.purchasePrice)
        {
            PurchaseCrop(crop);
        }
    }

    // GÖSTERGE
    void UpdateStatsText ()
    {
        statsText.text = $"Gün: {curDay}\nPara: ${money}\nEnvanter: {cropInventory}";
    }
}