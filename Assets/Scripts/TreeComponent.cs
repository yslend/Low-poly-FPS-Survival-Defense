using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeComponent : MonoBehaviour
{
    [SerializeField]
    private GameObject[] go_treePieces;  // ��� ������ ������ ���� 5 ���� ���� �׵θ� ������.
    [SerializeField]
    private GameObject go_treeCenter;  // ��� ����. �� ������ ���������� �ı��Ǹ� ������ �������� �Ѵ�.

    [SerializeField]
    private GameObject go_Log_Prefabs;  // �볪��. ������ ������ ���� ������.

    [SerializeField]
    private float force;  // ������ ���� ���������� �о��� ���� ����(�������� ���� ��) 
    [SerializeField]
    private GameObject go_ChildTree;  // ������ ���� ���κ�. �������� �� ������ ���� �ð� �� �ı� �Ǿ� �ؼ� �ʿ���.

    [SerializeField]
    private CapsuleCollider parentCol;  // ��ü���� ������ �پ��ִ� ĸ�� �ݶ��̴�. ������ �������� �̰� ��Ȱ��ȭ ���־�� ��.
    [SerializeField]
    private CapsuleCollider childCol;  // ������ ������ ���� ���κп� �پ��ִ� ĸ�� �ݶ��̴�. ������ �������� �̰� Ȱ��ȭ ���־�� ��.
    [SerializeField]
    private Rigidbody childRigid; // ������ ������ ���� ���κп� �پ��ִ� Rigidbody�� ���� ������ �������� �߷��� Ȱ��ȭ ���־�� ��.

    [SerializeField]
    private GameObject go_hit_effect_prefab;  // ���� ������ �� ������ ����Ʈ ȿ��(���� ����)
    [SerializeField]
    private float debrisDestroyTime;  // ���� ���� �ð�. ���� ������ ����Ʈ(���� ����) �ı��� �ð�

    [SerializeField]
    private float destroyTime;  // ���� ���� �ð�. ���� �� �κ��� ���� �������� ���� �ı��� �ð�.

    [SerializeField]
    private string chop_sound;  // ���� �������� �����ų ���� �̸� 
    [SerializeField]
    private string falldown_sound;  // ���� ������ �� �����ų ���� �̸� 
    [SerializeField]
    private string logChange_sound;  // ���� �������� �볪���� �ٲ� �� �����ų ���� �̸�


    // ������ �� ��ġ�� �˾ƾ� �� ������ ����Ʈ ȿ�� �����
    // �÷��̾ ������ �� Y ���� ȸ������ �˾ƾ� ��� ������ �¾Ҵ����� �� �� ����.
    public void Chop(Vector3 _pos, float angleY)
    {
        Hit(_pos);

        AngleCalc(angleY);

        if (CheckTreePieces())
            return;

        FallDownTree();
    }

    private void Hit(Vector3 _pos)
    {
        SoundManager.instance.PlaySE(chop_sound);

        GameObject clone = Instantiate(go_hit_effect_prefab, _pos, Quaternion.Euler(Vector3.zero));
        Destroy(clone, debrisDestroyTime);
    }

    public Vector3 GetTreeCenterPosition()
    {
        return go_treeCenter.transform.position;
    }

    private void AngleCalc(float _angleY)
    {
        if (_angleY >= 0 && _angleY <= 70)
            DestroyPiece(2);
        else if (_angleY >= 70 && _angleY <= 140)
            DestroyPiece(3);
        else if (_angleY >= 140 && _angleY <= 210)
            DestroyPiece(4);
        else if (_angleY >= 210 && _angleY <= 280)
            DestroyPiece(0);
        else if (_angleY >= 280 && _angleY <= 360)
            DestroyPiece(1);
    }

    private void DestroyPiece(int _num)
    {
        if (go_treePieces[_num].gameObject != null)
        {
            GameObject clone = Instantiate(go_hit_effect_prefab, go_treePieces[_num].transform.position, Quaternion.Euler(Vector3.zero));
            Destroy(clone, debrisDestroyTime);
            Destroy(go_treePieces[_num].gameObject);
        }
    }

    private bool CheckTreePieces()
    {
        for (int i = 0; i < go_treePieces.Length; i++)
        {
            if (go_treePieces[i].gameObject != null)
            {
                return true;
            }
        }
        return false;
    }

    private void FallDownTree()
    {
        SoundManager.instance.PlaySE(falldown_sound);

        Destroy(go_treeCenter);

        parentCol.enabled = false;
        childCol.enabled = true;
        childRigid.useGravity = true;

        childRigid.AddForce(Random.Range(-force, force), 0f, Random.Range(-force, force));

        StartCoroutine(LogCoroutine());
    }

    IEnumerator LogCoroutine()
    {
        yield return new WaitForSeconds(destroyTime);

        SoundManager.instance.PlaySE(logChange_sound);

        Instantiate(go_Log_Prefabs, go_ChildTree.transform.position + (go_ChildTree.transform.up * 3f), Quaternion.LookRotation(go_ChildTree.transform.up));
        Instantiate(go_Log_Prefabs, go_ChildTree.transform.position + (go_ChildTree.transform.up * 6f), Quaternion.LookRotation(go_ChildTree.transform.up));
        Instantiate(go_Log_Prefabs, go_ChildTree.transform.position + (go_ChildTree.transform.up * 9f), Quaternion.LookRotation(go_ChildTree.transform.up));

        Destroy(go_ChildTree.gameObject);
    }
}