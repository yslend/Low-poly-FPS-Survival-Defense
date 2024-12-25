using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class HandController : CloseWeaponController
{
    // 활성화 여부.
    public static bool isActivate = true;
    public static Item currentKit;

    private bool isPreview = false;

    private GameObject go_preview;
    private Vector3 previewPos;
    [SerializeField] private float rangeAdd;

    [SerializeField]
    private QuickSlotController theQuickSlot;

    // Update is called once per frame
    void Update()
    {

        if (isActivate && !Inventory.inventoryActivated)
        {
            if (currentKit == null)
            {
                if (QuickSlotController.go_HandItem == null)
                    TryAttack();
                else
                    TryEating();
            }
            else
            {
                if (!isPreview)
                    InstallPreviewKit();
                PreviewPositionUpdate();
                Build();
            }
        }
    }

    private void InstallPreviewKit()
    {
        isPreview = true;
        go_preview = Instantiate(currentKit.kitPreviewPrefab, transform.position, Quaternion.identity);
    }

    private void PreviewPositionUpdate()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, currentCloseWeapon.range + rangeAdd, layerMask))
        {
            previewPos = hitInfo.point;
            go_preview.transform.position = previewPos;
        }
    }

    private void Build()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (go_preview.GetComponent<PreviewObject>().IsBuildable())
            {
                theQuickSlot.DecreaseSelectedItem();
                GameObject temp = Instantiate(currentKit.kitPrefab, previewPos, Quaternion.identity);
                temp.name = currentKit.itemName;
                Destroy(go_preview);
                currentKit = null;
                isPreview = false;
            }
        }
    }

    public void Cancel()
    {
        Destroy(go_preview);
        currentKit = null;
        isPreview = false;
    }

    private void TryEating()
    {
        if (Input.GetButton("Fire1") && !theQuickSlot.GetIsCoolTIme())
        {
            currentCloseWeapon.anim.SetTrigger("Eat");
            theQuickSlot.DecreaseSelectedItem();
        }
    }

    protected override IEnumerator HitCoroutine()
    {
        while (isSwing)
        {
            if (CheckObject())
            {
                isSwing = false;
                Debug.Log(hitInfo.transform.name);
            }
            yield return null;
        }
    }

    public override void CloseWeaponChange(CloseWeapon _closeWeapon)
    {
        base.CloseWeaponChange(_closeWeapon);
        isActivate = true;
    }
}
