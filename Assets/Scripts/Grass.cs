using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grass : MonoBehaviour
{
    [SerializeField]
    private int hp;  // Ǯ�� ü��

    [SerializeField]
    private float destroyTime;  // Ǯ ����Ʈ ����� �ð�
    [SerializeField]
    private float explosionForce;  // ���� ����


    [SerializeField]
    private GameObject go_hit_effect_prefab;  // Ǯ ������ ���� �߻���ų ����Ʈ ȿ�� ������

    [SerializeField]
    private Item item_Leaf;
    [SerializeField]
    private int leafCount;
    private Inventory thelnven;

    private Rigidbody[] rigidbodys;  // Ǯ �ı��� �ڽ��� Ǯ �����鿡�� �پ��ִ� Rigidbody���� �߷� Ȱ��ȭ�ϱ� ���Ͽ�
    private BoxCollider[] boxColiders;  // Ǯ �ı��� �ڽ��� Ǯ �����鿡�� �پ��ִ� Box Collider ���� Ȱ��ȭ�ϱ� ���Ͽ�

    [SerializeField]
    private string hit_sound;  // Ǯ Ÿ�ݽ� �Ҹ�

    void Start()
    {
        thelnven = FindObjectOfType<Inventory>();
        rigidbodys = this.transform.GetComponentsInChildren<Rigidbody>();
        boxColiders = this.transform.GetComponentsInChildren<BoxCollider>();
    }

    public void Damage()
    {
        hp--;

        Hit();

        if (hp <= 0)
        {
            Destruction();
        }
    }

    private void Hit()
    {
        SoundManager.instance.PlaySE(hit_sound);

        var clone = Instantiate(go_hit_effect_prefab, transform.position + Vector3.up, Quaternion.identity);
        Destroy(clone, destroyTime);
    }

    private void Destruction()
    {
        thelnven.AcquireItem(item_Leaf, leafCount);

        for (int i = 0; i < rigidbodys.Length; i++)
        {
            rigidbodys[i].useGravity = true;
            rigidbodys[i].AddExplosionForce(explosionForce, transform.position, 1f); // ���ڸ����� �߷� �޾� �׳� �������°� �ƴ϶� ���� ȿ���� �� (���� ����, ���� ��ġ, ���� �ݰ�)
            boxColiders[i].enabled = true;
        }

        Destroy(this.gameObject, destroyTime);
    }
}