using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    private KeyCode ShopActiveKey = KeyCode.R;
    private KeyCode InventoryKey = KeyCode.E;
    private KeyCode SettingKey = KeyCode.Escape;
    public GameObject Shop;
    public CanvasGroup inventoryCG;
    //public CanvasGroup SettingCG;
    private bool isOpen = false;
    //private bool isOpenSetting = false;
    private int isCheck = 0;

    public List<CanvasGroup> inventoryList = new List<CanvasGroup>();
    public Text inventoryText;

    public Text SeceneName;

    public GameObject RedWeaponUI;
    public GameObject BlueWeaponUI;
    public GameObject YellowWeaponUI;

    private RectTransform RedWeaponTr;
    private RectTransform BlueWeaponTr;
    private RectTransform YellowWeaponTr;

    private RectTransform CurrectTransform;

    private CanvasGroup RedWeaponCG;
    private CanvasGroup BlueWeaponCG;
    private CanvasGroup YellowWeaponCG;

    private Image RedWeaponImage;
    private Image BlueWeaponImage;
    private Image YellowWeaponImage;

    private CanvasGroup CurCG;

    public float UItransform = 15f;

    public float Redtime = 0f;
    public float Bluetime = 0f;
    public float Yellowtime = 0f;

    public Dictionary<EColor, float> UITime = new Dictionary<EColor, float>();
    public float keeptime;

    public Weapon weapon;
    public Weapon RedWeapon;
    public Weapon BlueWeapon;
    public Weapon YellowWeapon;
    //private Weapon Curweapon;

    public Dictionary<EColor, Weapon> UIWeapon = new Dictionary<EColor, Weapon>();

    public static GameUI instance = null;

    void OnSceneLoaded(Scene scene, LoadSceneMode level)
    {
        SeceneName.text = "- " + SceneManager.GetActiveScene().name + " -";
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            GameObject.Destroy(this.gameObject);
        }
        DontDestroyOnLoad(instance);
    }

    // Start is called before the first frame update
    void Start()
    {
        Shop.SetActive(false);
        OnInventoryOpen(isOpen);

        if (RedWeaponUI.transform.childCount > 0) RedWeaponImage = RedWeaponUI.transform.GetChild(0).GetComponent<Image>();
        if (BlueWeaponUI.transform.childCount > 0) BlueWeaponImage = BlueWeaponUI.transform.GetChild(0).GetComponent<Image>();
        if (YellowWeaponUI.transform.childCount > 0) YellowWeaponImage = YellowWeaponUI.transform.GetChild(0).GetComponent<Image>();

        RedWeaponTr = RedWeaponUI.GetComponent<RectTransform>();
        BlueWeaponTr = BlueWeaponUI.GetComponent<RectTransform>();
        YellowWeaponTr = YellowWeaponUI.GetComponent<RectTransform>();

        CurrectTransform = RedWeaponTr;

        RedWeaponCG = RedWeaponUI.GetComponent<CanvasGroup>();
        BlueWeaponCG = BlueWeaponUI.GetComponent<CanvasGroup>();
        YellowWeaponCG = YellowWeaponUI.GetComponent<CanvasGroup>();

        CurCG = RedWeaponCG;

        UITime.Add(EColor.Red, 0f);
        UITime.Add(EColor.Blue, 0f);
        UITime.Add(EColor.Yellow, 0f);
        UIWeapon.Add(EColor.Red, null);
        UIWeapon.Add(EColor.Blue, null);
        UIWeapon.Add(EColor.Yellow, null);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(ShopActiveKey))
        {
            Shop.SetActive(!Shop.activeSelf);
        }

        if (Input.GetKeyDown(InventoryKey))
        {
            isOpen = !isOpen;
            OnInventoryOpen(isOpen);
        }

        if (Input.GetKeyDown(SettingKey))
        {
            Application.Quit();
        }
    }

    public void OnInventoryOpen(bool isOpened)
    {
        inventoryCG.alpha = (isOpened) ? 1.0f : 0.0f;
        inventoryCG.interactable = isOpened;
        inventoryCG.blocksRaycasts = isOpened;
    }

    public void ChangeInventory()
    {
        inventoryList[isCheck].alpha = 0.0f;
        inventoryList[isCheck].interactable = !isOpen;
        inventoryList[isCheck].blocksRaycasts = !isOpen;

        isCheck = (isCheck == 1) ? 0 : 1;

        inventoryList[isCheck].alpha = 1.0f;
        inventoryList[isCheck].interactable = isOpen;
        inventoryList[isCheck].blocksRaycasts = isOpen;

        inventoryText.text = 1 + isCheck + "/2";
    }

    public void CoolTimeUI()
    {
        weapon = Managers.Player.PCtrl.curWeapon;

        switch (weapon.itsColor)
        {
            case EColor.Red:
                StartCoroutine(RedCoolTimeUI());
                break;
            case EColor.Blue:
                StartCoroutine(BlueCoolTimeUI());
                break;
            case EColor.Yellow:
                StartCoroutine(YellowCoolTimeUI());
                break;
        }
    }

    public void RedUI()
    {
        CurrectTransform.anchoredPosition = new Vector3(CurrectTransform.anchoredPosition.x + UItransform, CurrectTransform.anchoredPosition.y);
        CurCG.alpha = 0.6f;

        CurrectTransform = RedWeaponTr;
        CurCG = RedWeaponCG;

        CurrectTransform.anchoredPosition = new Vector3(CurrectTransform.anchoredPosition.x - UItransform, CurrectTransform.anchoredPosition.y);
        CurCG.alpha = 1f;
    }

    public void BlueUI()
    {
        CurrectTransform.anchoredPosition = new Vector3(CurrectTransform.anchoredPosition.x + UItransform, CurrectTransform.anchoredPosition.y);
        CurCG.alpha = 0.6f;

        CurrectTransform = BlueWeaponTr;
        CurCG = BlueWeaponCG;

        CurrectTransform.anchoredPosition = new Vector3(CurrectTransform.anchoredPosition.x - UItransform, CurrectTransform.anchoredPosition.y);
        CurCG.alpha = 1f;
    }

    public void YellowUI()
    {
        CurrectTransform.anchoredPosition = new Vector3(CurrectTransform.anchoredPosition.x + UItransform, CurrectTransform.anchoredPosition.y);
        CurCG.alpha = 0.6f;

        CurrectTransform = YellowWeaponTr;
        CurCG = YellowWeaponCG;

        CurrectTransform.anchoredPosition = new Vector3(CurrectTransform.anchoredPosition.x - UItransform, CurrectTransform.anchoredPosition.y);
        CurCG.alpha = 1f;
    }

    public void ChangeWeapon(EColor color, Sprite sprite)
    {
        switch (color)
        {
            case EColor.Red :
                RedWeaponImage.sprite = sprite;
                RedUI();
                break;
            case EColor.Blue:
                BlueWeaponImage.sprite = sprite;
                BlueUI();
                break;
            case EColor.Yellow:
                YellowWeaponImage.sprite = sprite;
                YellowUI();
                break;
            default:
                break;
        }
    }

    public IEnumerator RedCoolTimeUI()
    {
        //float time = 0;
        //Weapon Curweapon = null;
        //if (weapon != null) RedWeapon = weapon;
        if (weapon != null) UIWeapon[EColor.Red] = weapon;

        yield return new WaitForSeconds(UIWeapon[EColor.Red].skilldata.Skill_Duration);
        
        if (UIWeapon[EColor.Red] != null)
        {
            RedWeaponImage.fillAmount = 0f;
            while (!UIWeapon[EColor.Red].isSkillOn())
            {
                UITime[EColor.Red] += Time.deltaTime;
                RedWeaponImage.fillAmount = Mathf.Lerp(0f, 1f, UITime[EColor.Red] / UIWeapon[EColor.Red].skilldata.Cool_Down);

                if (UITime[EColor.Red] >= UIWeapon[EColor.Red].skilldata.Cool_Down)
                {
                    RedWeaponImage.fillAmount = 1f;
                }

                yield return null;
            }

            RedWeaponImage.fillAmount = 1f;
            UITime[EColor.Red] = 0f;
        }
    }

    public IEnumerator BlueCoolTimeUI()
    {
        //float time = 0;
        //Weapon Curweapon = null;
        if (weapon != null) UIWeapon[EColor.Blue] = weapon;

        yield return new WaitForSeconds(UIWeapon[EColor.Blue].skilldata.Skill_Duration);

        if (UIWeapon[EColor.Blue] != null)
        {
            BlueWeaponImage.fillAmount = 0f;
            while (!UIWeapon[EColor.Blue].isSkillOn())
            {
                UITime[EColor.Blue] += Time.deltaTime;
                BlueWeaponImage.fillAmount = Mathf.Lerp(0f, 1f, UITime[EColor.Blue] / UIWeapon[EColor.Blue].skilldata.Cool_Down);

                if (UITime[EColor.Blue] >= UIWeapon[EColor.Blue].skilldata.Cool_Down)
                {
                    BlueWeaponImage.fillAmount = 1f;
                }

                yield return null;
            }

            BlueWeaponImage.fillAmount = 1f;
            UITime[EColor.Blue] = 0f;
        }
    }

    public IEnumerator YellowCoolTimeUI()
    {
        //float time = 0;
        //Weapon Curweapon = null;
        if (weapon != null) UIWeapon[EColor.Yellow] = weapon;

        yield return new WaitForSeconds(UIWeapon[EColor.Yellow].skilldata.Skill_Duration);

        if (UIWeapon[EColor.Yellow] != null)
        {
            YellowWeaponImage.fillAmount = 0f;
            while (!UIWeapon[EColor.Yellow].isSkillOn())
            {
                UITime[EColor.Yellow] += Time.deltaTime;
                YellowWeaponImage.fillAmount = Mathf.Lerp(0f, 1f, UITime[EColor.Yellow] / UIWeapon[EColor.Yellow].skilldata.Cool_Down);

                if (UITime[EColor.Yellow] >= UIWeapon[EColor.Yellow].skilldata.Cool_Down)
                {
                    YellowWeaponImage.fillAmount = 1f;
                }

                yield return null;
            }

            YellowWeaponImage.fillAmount = 1f;
            UITime[EColor.Yellow] = 0f;
        }
    }
}
