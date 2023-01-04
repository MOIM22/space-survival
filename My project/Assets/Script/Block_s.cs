using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block_s : MonoBehaviour
{
    public bool On;

    [Header("- ���ġ ��� �ʼ� ���� -")]
    [Tooltip("���ġ ��� ĵ���� ������Ʈ")]
    public GameObject Relocation_Mode;
    [Tooltip("��ġ ������ ���� �� ������Ʈ")]
    public GameObject[] Install_Blocks = new GameObject[4];

    void Start()
    {
        On = true;
    }
    void Update()
    {
        if (Relocation_Mode == null)
            Relocation_Mode = GameObject.Find("Relocation_Mode");
        else if (Relocation_Mode.activeSelf && On)
            Slot_Surch();
        else
            for (int i = 0; i < 4; i++)
            {
                if (Install_Blocks[i] == null)
                    continue;
                Install_Blocks[i].SetActive(false);
            }
    }
    void Slot_Surch()
    {
        for (int i = 0; i < 4; i++)
            if (Install_Blocks[i] == null)
                return;

        if (Physics.Raycast(transform.position, transform.forward, 1, 1 << 0) == false)
            Install_Blocks[0].SetActive(true);
        else
            Install_Blocks[0].SetActive(false);

        if (Physics.Raycast(transform.position, -transform.forward, 1, 1 << 0) == false)
            Install_Blocks[1].SetActive(true);
        else
            Install_Blocks[1].SetActive(false);
        if (Physics.Raycast(transform.position, transform.right, 1, 1 << 0) == false)
            Install_Blocks[2].SetActive(true);
        else
            Install_Blocks[2].SetActive(false);

        if (Physics.Raycast(transform.position, -transform.right, 1, 1 << 0) == false)
            Install_Blocks[3].SetActive(true);
        else
            Install_Blocks[3].SetActive(false);
    }
}
