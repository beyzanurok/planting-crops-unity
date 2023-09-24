using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldTile : MonoBehaviour
{

    //ALAN SCRİPTİ (EKİLEN KISIM)

    private Crop curCrop;
    public GameObject cropPrefab;

    public SpriteRenderer sr;
    [SerializeField] 
    private bool tilled;

    [Header("Sprites")]
    public Sprite grassSprite;
    public Sprite tilledSprite;
    public Sprite wateredTilledSprite;

    void Start ()
    {
        // çimleri ayarlama
        sr.sprite = grassSprite;
    }



    void OnEnable()
    {
        GameManager.instance.onNewDay += OnNewDay;
    }
    void OnDisable()
    {
        GameManager.instance.onNewDay -= OnNewDay;
    }


    // player etkileşimi
    public void Interact ()
    {
        if(!tilled)
        {
            Till();
        }
        else if(!HasCrop() && GameManager.instance.CanPlantCrop())
        {
            PlantNewCrop(GameManager.instance.selectedCropToPlant);
        }
        else if(HasCrop() && curCrop.CanHarvest())
        {
            curCrop.Harvest();
        }
        else
        {
            Water();
        }
    }

    // mahsulleri ekme
    void PlantNewCrop (CropData crop)
    {
        if(!tilled)
            return;

        curCrop = Instantiate(cropPrefab, transform).GetComponent<Crop>();
        curCrop.Plant(crop);

       
    }

    // çim
    void Till ()
    {
        tilled = true;
        sr.sprite = tilledSprite;
    }

    // çim kesme işlemi
    void Water ()
    {
        sr.sprite = wateredTilledSprite;

        if(HasCrop())
        {
            curCrop.Water();
        }
    }

    
    void OnNewDay ()
    {
        if(curCrop == null)
        {
            tilled = false;
            sr.sprite = grassSprite;

           
        }
        else if(curCrop != null)
        {
            sr.sprite = tilledSprite;
            curCrop.NewDayCheck();
        }
    }

    // ürün eklendi mi?
    bool HasCrop ()
    {
        return curCrop != null;
    }
}