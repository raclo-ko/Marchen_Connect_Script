using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Drag : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private Transform itemTr;
    private Transform inventoryTr;
    public static Transform parentTr { get; private set; }
    private CanvasGroup canvasGroup;

    public static GameObject draggingItem = null;
    public GameObject weaponPrefabs;
    public GameObject characterPrefabs;

    public Image image;

    private Cainos.CustomizablePixelCharacter.PixelCharacter PixelCharacter;

    public Material HatMaterial;
    public Material HairMaterial;
    public Material EyeMaterial;
    public Material EyeBaseMaterial;
    public Material FacewearMaterial;
    public Material ClothMaterial;
    public Material PantsMaterial;
    public Material SocksMaterial;
    public Material ShoesMaterial;
    public Material BackMaterial;
    public Material BodyMaterial;
    public bool ClipHair;

    // Start is called before the first frame update
    void Start()
    {
        itemTr = GetComponent<Transform>();
        inventoryTr = GameObject.Find("Inventory").GetComponent<Transform>();
        image = gameObject.GetComponent<Image>();

        canvasGroup = GetComponent<CanvasGroup>();

        PixelCharacter = characterPrefabs.GetComponent<Cainos.CustomizablePixelCharacter.PixelCharacter>();

        HatMaterial = PixelCharacter.HatMaterial;
        HairMaterial = PixelCharacter.HairMaterial;
        EyeMaterial = PixelCharacter.EyeMaterial;
        EyeBaseMaterial = PixelCharacter.EyeBaseMaterial;
        FacewearMaterial = PixelCharacter.FacewearMaterial;
        ClothMaterial = PixelCharacter.ClothMaterial;
        PantsMaterial = PixelCharacter.PantsMaterial;
        SocksMaterial = PixelCharacter.SocksMaterial;
        ShoesMaterial = PixelCharacter.ShoesMaterial;
        BackMaterial = PixelCharacter.BackMaterial;
        BodyMaterial = PixelCharacter.BodyMaterial;
        ClipHair = PixelCharacter.ClipHair;
    }

    public void OnDrag(PointerEventData eventData)
    {
        itemTr.position = Input.mousePosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        parentTr = this.transform.parent;
        this.transform.SetParent(inventoryTr);
        draggingItem = this.gameObject;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        draggingItem = null;
        canvasGroup.blocksRaycasts = true;

        //잘못된 조작시 아이템을 원위치 시킨다.
        if (itemTr.parent == inventoryTr) itemTr.SetParent(parentTr.transform);

        parentTr = null;
    }
}
