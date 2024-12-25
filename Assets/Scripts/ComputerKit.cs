using System.Collections;
using UnityEngine;

[System.Serializable]
public class Kit
{
    public string KitName;
    public string KitDescription;
    public string[] needItemName;
    public int[] needItemNubmer;

    public GameObject go_Kit_Prefab;
}
public class ComputerKit : MonoBehaviour
{
    [SerializeField]
    private Kit[] kits;

    [SerializeField] private Transform tf_ItemAppear;
    [SerializeField] private GameObject go_BaseUi;

    private bool isCraft = false;
    public bool isPowerOn = false;

    private Inventory theInven;
    [SerializeField]
    private ComputerToolTip thetoolTip;

    private AudioSource theAudio;
    [SerializeField] private AudioClip sound_ButtonClick;
    [SerializeField] private AudioClip sound_Beep;
    [SerializeField] private AudioClip sound_Activated;
    [SerializeField] private AudioClip sound_Output;

    private void Start()
    {         
        theInven = FindObjectOfType<Inventory>();
        theAudio = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (isPowerOn)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                PowerOff();
            }
        }
    }

    public void PowerOn()
    {
        GameManager.IsOpenComputerKit = true;
        isPowerOn = true;
        go_BaseUi.SetActive(true);
    }

    public void PowerOff()
    {   
        GameManager.IsOpenComputerKit = false;
        isPowerOn = false;
        thetoolTip.HideToolTip();
        go_BaseUi.SetActive(false);
    }

    public void ShowToolTip(int _buttonNum)
    {
        thetoolTip.ShowToolTip(kits[_buttonNum].KitName, kits[_buttonNum].KitDescription, kits[_buttonNum].needItemName, kits[_buttonNum].needItemNubmer);
    }

    public void HideToolTip()
    {
        thetoolTip.HideToolTip();
    }

    private void PlaySE(AudioClip _clip)
    {
        theAudio.clip = _clip;
        theAudio.Play();
    }

    public void ClickButton(int _slotNumber)
    {
        PlaySE(sound_ButtonClick);
        if (!isCraft)
        {   
            if (!CheckIngredient(_slotNumber))
            {
                return;
            }

            isCraft = true;
            UseIngredient(_slotNumber);

            StartCoroutine(CraftCoroutine(_slotNumber));
        }
    }

    private bool CheckIngredient(int _slotNumber)
    {
        for (int i = 0; i < kits[_slotNumber].needItemName.Length; i++)
        {
            if (theInven.GetItemCount(kits[_slotNumber].needItemName[i])< kits[_slotNumber].needItemNubmer[i])
            {
                PlaySE(sound_Beep);
                return false;
            }
                
        }

        return true;
    }

    private void UseIngredient(int _slotNumber)
    {
        for (int i = 0; i < kits[_slotNumber].needItemName.Length; i++)
        {
           theInven.SetItemCount(kits[_slotNumber].needItemName[i], kits[_slotNumber].needItemNubmer[i]);
        }
    }

    IEnumerator CraftCoroutine(int _slotNumber)
    {
        PlaySE(sound_Activated);
        yield return new WaitForSeconds(3f);
        PlaySE(sound_Output);

        Instantiate(kits[_slotNumber].go_Kit_Prefab, tf_ItemAppear.position, Quaternion.identity);
        isCraft = false;
    }
}
