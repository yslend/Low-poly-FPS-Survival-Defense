using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class QuickSlotController : MonoBehaviour
{
    [SerializeField] private Slot[] quickSlots;
    [SerializeField] private Image[] img_CoolTime;
    [SerializeField] private Transform tf_parent;

    [SerializeField] private Transform tf_ItemPos;
    public static GameObject go_HandItem;

    [SerializeField]
    private float coolTIme;
    private float currentCoolTime;
    private bool isCoolTime;

    [SerializeField] private float appearTime;
    private float currentAppearTIme;
    private bool isAppear;

    private int selectedSlot;

    [SerializeField]
    private GameObject go_SelectedImage;
    [SerializeField]
    private WeaponManager theWeaponManager;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        quickSlots = tf_parent.GetComponentsInChildren<Slot>();
        anim = GetComponent<Animator>();
        selectedSlot = 0;
    }

    // Update is called once per frame
    void Update()
    {
        TryInputNumber();
        CoolTimeCacl();
        AppearCalc();
    }

    private void AppearReset()
    {
        currentAppearTIme = appearTime;
        isAppear = true;
        anim.SetBool("Appear", isAppear);
    }

    private void AppearCalc()
    {
        if (Inventory.inventoryActivated)
            AppearReset();
        else
        {
            if (isAppear)
            {
                currentAppearTIme -= Time.deltaTime;
                if (currentAppearTIme <= 0)
                {
                    isAppear = false;
                    anim.SetBool("Appear", isAppear);
                }
            }
        }      
    }

    private void CoolTimeReset()
    {
        currentCoolTime = coolTIme;
        isCoolTime = true;
    }

    private void CoolTimeCacl()
    {
        if (isCoolTime)
        {
            currentCoolTime -= Time.deltaTime;
            for (int i = 0; i < img_CoolTime.Length; i++) 
            {
                img_CoolTime[i].fillAmount = currentCoolTime / coolTIme;
            }

            if (currentCoolTime <= 0) 
            {
                isCoolTime = false;
            }
        }
    }

    private void TryInputNumber()
    {
        if (!isCoolTime)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
                ChangeSlot(0);
            else if (Input.GetKeyDown(KeyCode.Alpha2))
                ChangeSlot(1);
            else if (Input.GetKeyDown(KeyCode.Alpha3))
                ChangeSlot(2);
            else if (Input.GetKeyDown(KeyCode.Alpha4))
                ChangeSlot(3);
            else if (Input.GetKeyDown(KeyCode.Alpha5))
                ChangeSlot(4);
            else if (Input.GetKeyDown(KeyCode.Alpha6))
                ChangeSlot(5);
            else if (Input.GetKeyDown(KeyCode.Alpha7))
                ChangeSlot(6);
            else if (Input.GetKeyDown(KeyCode.Alpha8))
                ChangeSlot(7);
        } 
    }

    public void IsActivatedQuickSlot(int _num)
    {
        if (selectedSlot == _num)
        {
            Execute();
            return;
        }
        if (DragSlot.instance != null)
        {
            if(DragSlot.instance.dragSlot != null)
            {
                if (DragSlot.instance.dragSlot.GetQuickSlotNumber() == selectedSlot)
                {
                    Execute();
                    return;
                }
            }           
        }
    }

    private void ChangeSlot(int _num)
    {
        SelectedSlot(_num);

        Execute();
    }

    private void SelectedSlot(int _num)
    {
        selectedSlot = _num;
        go_SelectedImage.transform.position = quickSlots[selectedSlot].transform.position;
    }

    private void Execute()
    {
        CoolTimeReset();
        AppearReset();

        if (quickSlots[selectedSlot].item != null)
        {
            if (quickSlots[selectedSlot].item.itemType == Item.ItemType.Equipment)
                StartCoroutine(theWeaponManager.ChangeWeaponCoroutine(quickSlots[selectedSlot].item.weaponType, quickSlots[selectedSlot].item.itemName));
            else if (quickSlots[selectedSlot].item.itemType == Item.ItemType.Used || quickSlots[selectedSlot].item.itemType == Item.ItemType.Kit)
                ChanageHand(quickSlots[selectedSlot].item);
            else
                ChanageHand();
        }
        else
        {
            ChanageHand();
        }

    }

    private void ChanageHand(Item _item = null)
    {
        StartCoroutine(theWeaponManager.ChangeWeaponCoroutine("HAND", "¸Ç¼Õ"));

        if (_item != null)
        {           
            StartCoroutine(HandItemCoroutine(_item));
        } 
    }

    IEnumerator HandItemCoroutine(Item _item)
    {
        HandController.isActivate = false;
        yield return new WaitUntil(() => HandController.isActivate);

        if (_item.itemType == Item.ItemType.Kit)
            HandController.currentKit = _item;

        go_HandItem = Instantiate(quickSlots[selectedSlot].item.itemPrefab, tf_ItemPos.position, tf_ItemPos.rotation);
        go_HandItem.GetComponent<Rigidbody>().isKinematic = true;
        go_HandItem.GetComponent<BoxCollider>().enabled = false;
        go_HandItem.tag = "Untagged";
        go_HandItem.layer = 8; 
        go_HandItem.transform.SetParent(tf_ItemPos);
    }

    public void DecreaseSelectedItem()
    {
        CoolTimeReset();
        AppearReset();

        quickSlots[selectedSlot].SetSlotCount(-1);

        if (quickSlots[selectedSlot].itemCount <= 0)
        {
            Destroy(go_HandItem);
        }
    }

    public bool GetIsCoolTIme()
    {
        return isCoolTime;
    }

    public Slot GetSelectedSlot() 
    {
        return quickSlots[selectedSlot];
    }
}
