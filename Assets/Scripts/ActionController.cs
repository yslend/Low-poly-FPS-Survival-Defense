using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ActionController : MonoBehaviour
{

    [SerializeField]
    private float range; // 습득 가능한 최대 거리.

    private bool pickupActivated = false; // 습득 가능할 시 true.
    private bool dissolveActivated = false;
    private bool isDissolving = false;

    private bool fireLookActivated = false;
    private bool lookArchemyTable = false;
    private bool lookComputer = false;

    private RaycastHit hitInfo; // 충돌체 정보 저장.


    // 아이템 레이어에만 반응하도록 레이어 마스크를 설정.
    [SerializeField]
    private LayerMask layerMask;

    // 필요한 컴포넌트.
    [SerializeField]
    private Text actionText;
    [SerializeField]
    private Inventory theInventory;
    [SerializeField]
    private WeaponManager theWeaponManager;
    [SerializeField]
    private QuickSlotController theQuickSlot;
    [SerializeField]
    private Transform tf_MEatDissolveTool;
    [SerializeField]
    private ComputerKit theComputer;

    [SerializeField]
    private string sound_meat;

    // Update is called once per frame
    void Update()
    {
        CheckAction();
        TryAction();
    }

    private void TryAction()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            CheckAction();
            CanPickUp();
            CanMeat();
            CanDropFIre();
            CanComputerPowerOn();
            CanArchemyTableOpen();
        }
    }

    private void CanPickUp()
    {
        if (pickupActivated)
        {
            if (hitInfo.transform != null)
            {
                //Debug.Log(hitInfo.transform.GetComponent<ItemPickUp>().item.itemName + " 획득했습니다");
                theInventory.AcquireItem(hitInfo.transform.GetComponent<ItemPickUp>().item);
                Destroy(hitInfo.transform.gameObject);
                InfoDisappear();
            }
        }
    }

    private void CanComputerPowerOn()
    {
        if (lookComputer)
        {
            if (hitInfo.transform != null)
            {
                if (!hitInfo.transform.GetComponent<ComputerKit>().isPowerOn)
                {
                    hitInfo.transform.GetComponent<ComputerKit>().PowerOn();
                    InfoDisappear();
                }               
            }
        }
    }

    private void CanArchemyTableOpen()
    {
        if (lookArchemyTable)
        {
            if (hitInfo.transform != null)
            {
                hitInfo.transform.GetComponent<ArchemyTable>().Window();
                InfoDisappear();
            }
        }
    }

    private void CanMeat()
    {
        if (dissolveActivated)
        {
            if (hitInfo.transform.tag == "WeakAnimal" || hitInfo.transform.tag == "StrongAnimal" && hitInfo.transform.GetComponent<Animal>().isDead && !isDissolving)
            {
                isDissolving = true;
                InfoDisappear();
                StartCoroutine(MeatCoroutine());
            }

        }
    }

    private void CanDropFIre()
    {
        if (fireLookActivated)
        {
            if(hitInfo.transform.tag == "Fire" && hitInfo.transform.GetComponent<Fire>().GetisFire())
            {
                Slot _selectedSlot = theQuickSlot.GetSelectedSlot();
                if(_selectedSlot.item != null)
                {
                    DropAnItem(_selectedSlot);
                }
            }
        }
    }

    private void DropAnItem(Slot _SelectedSlot)
    {
        switch (_SelectedSlot.item.itemType)
        {
            case Item.ItemType.Used:
                if (_SelectedSlot.item.itemName.Contains("고기"))
                {
                    Instantiate(_SelectedSlot.item.itemPrefab, hitInfo.transform.position + Vector3.up, Quaternion.identity);
                    theQuickSlot.DecreaseSelectedItem();
                }
                break;
            case Item.ItemType.Ingredient:
                break;
        }
    }

    IEnumerator MeatCoroutine()
    {
        WeaponManager.isChangeWeapon = true;
        WeaponSway.isActivated = false;
        WeaponManager.currentWeaponAnim.SetTrigger("Weapon_Out");
        PlayerController.isActivated = false;
        yield return new WaitForSeconds(0.2f);

        WeaponManager.currentWeapon.gameObject.SetActive(false);
        tf_MEatDissolveTool.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.2f);
        SoundManager.instance.PlaySE(sound_meat);
        yield return new WaitForSeconds(1.8f);

        theInventory.AcquireItem(hitInfo.transform.GetComponent<Animal>().GetItem(), hitInfo.transform.GetComponent<Animal>().itemNumber);

        WeaponManager.currentWeapon.gameObject.SetActive(true);
        tf_MEatDissolveTool.gameObject.SetActive(false);

        PlayerController.isActivated = true;
        WeaponSway.isActivated = true;
        WeaponManager.isChangeWeapon = false;
        isDissolving = false;
    }

    private void CheckAction()
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hitInfo, range, layerMask))
        {
            if (hitInfo.transform.tag == "Item")
            {
                ItemInfoAppear();
            }
            else if (hitInfo.transform.tag == "WeakAnimal" || hitInfo.transform.tag == "StrongAnimal")
            {
                MeatInfoAppear();
            }
            else if (hitInfo.transform.tag == "Fire")
            {
                FIreInfoAppear();
            }
            else if (hitInfo.transform.tag == "Computer")
            {
                ComputerInfoAppear();
            }
            else if (hitInfo.transform.tag == "ArchemyTable")
            {
                ArchemyInfoAppear();
            }
            else 
            {
                InfoDisappear();
            }
        }
        else
            InfoDisappear();
    }

    private void Reset()
    {
        pickupActivated = false;
        dissolveActivated = false;
        fireLookActivated = false;
    }

    private void ItemInfoAppear()
    {
        Reset();
        pickupActivated = true;
        actionText.gameObject.SetActive(true);
        actionText.text = hitInfo.transform.GetComponent<ItemPickUp>().item.itemName + " 획득 " + "<color=yellow>" + "(E)" + "</color>";
    }

    private void MeatInfoAppear()
    {
        if (hitInfo.transform.GetComponent<Animal>().isDead)
        {
            Reset();
            dissolveActivated = true;
            actionText.gameObject.SetActive(true);
            actionText.text = hitInfo.transform.GetComponent<Animal>().animalName + " 해체하기 " + "<color=yellow>" + "(E)" + "</color>";
        }
        
    }

    private void FIreInfoAppear()
    {
        Reset();
        fireLookActivated = true;

        if (hitInfo.transform.GetComponent<Fire>().GetisFire())
        {
            actionText.gameObject.SetActive(true);
            actionText.text = "선택된 아이템 불에 넣기" + "<color=yellow>" + "(E)" + "</color>";
        }
    }

    private void ComputerInfoAppear()
    {   
        if (!hitInfo.transform.GetComponent<ComputerKit>().isPowerOn)
        {
            Reset();
            lookComputer = true;
            actionText.gameObject.SetActive(true);
            actionText.text = " 컴퓨터 가동 " + "<color=yellow>" + "(E)" + "</color>";
        } 
    }
    private void ArchemyInfoAppear()
    {
        if (!hitInfo.transform.GetComponent<ArchemyTable>().GetIsOpen())
        {
            Reset();
            lookArchemyTable = true;
            actionText.gameObject.SetActive(true);
            actionText.text = " 연금 테이블 조작 " + "<color=yellow>" + "(E)" + "</color>";
        }
    }

    private void InfoDisappear()
    {
        pickupActivated = false;
        dissolveActivated = false;
        fireLookActivated = false;
        lookComputer = false;
        lookArchemyTable = false;
        actionText.gameObject.SetActive(false);
    }
}
