using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot_s : MonoBehaviour
{
    [Header("- ���� ���� -")]
    [Tooltip("���� �ȿ� ��� �ִ� ��� �̸�")]
    public string Block_Name;
    [Tooltip("���� �ȿ� ��� �ִ� ��� ����")]
    public int Block_Count;

    [Header("- �ʼ� ���� -")]
    [Tooltip("���� �̹���")]
    public Image image;
    [Tooltip("���� �ȿ� ��� �ִ� �� ���� �ؽ�Ʈ")]
    public Text text;
    [Tooltip("���� ���� �̹���")]
    public Sprite[] spr = new Sprite[1];

    void Update()
    {
        if (Block_Count > 0)
        {
            image.gameObject.SetActive(true);
            switch (Block_Name)
            {
                case "test_block":
                    image.sprite = spr[0];
                    break;
                case "test_turret":
                    image.sprite = spr[1];
                    break;
                default:
                    image.gameObject.SetActive(false);
                    break;
            }
        }
        else
        {
            Block_Name = null;
            image.gameObject.SetActive(false);
        }

        text.text = Block_Count.ToString();
    }

    public void Change_Count(int num)
    {
        Block_Count += num;
    }
}
