using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseWeapon : MonoBehaviour
{

    public string closeWeaponName; // ���� ���� �̸�.

    // ���� ����.
    public bool isHand;
    public bool isAxe;
    public bool isPickaxe;

    public float range; // ���� ���� 
    public int damage; // ���ݷ�.
    public float workSpeed; // �۾� �ӵ�.
    public float attackDelay;  // ���� ������. ���콺 Ŭ���ϴ� ���� ���� ������ �� �����Ƿ�.
    public float attackDelayA;  // ���� Ȱ��ȭ ����. ���� �ִϸ��̼� �߿��� �ָ��� �� �������� �� ���� ���� �������� ���� �Ѵ�.
    public float attackDelayB;  // ���� ��Ȱ��ȭ ����. ���� �� ������ �ָ��� ���� �ִϸ��̼��� ���۵Ǹ� ���� �������� ���� �ȵȴ�.

    public float workDelay;  // �۾� ������
    public float workDelayA;  // �۾� Ȱ��ȭ ����.
    public float workDelayB;  // �۾� ��Ȱ��ȭ ����.

    public Animator anim; // �ִϸ��̼�.

}
