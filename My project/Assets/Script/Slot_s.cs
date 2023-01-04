using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot_s : MonoBehaviour
{
    [Header("- 변수 설정 -")]
    [Tooltip("슬롯 안에 들어 있는 블록 이름")]
    public string Block_Name;
    [Tooltip("슬롯 안에 들어 있는 블록 개수")]
    public int Block_Count;

    [Header("- 필수 설정 -")]
    [Tooltip("슬롯 이미지")]
    public Image image;
    [Tooltip("슬롯 안에 들어 있는 블럭 개수 텍스트")]
    public Text text;
    [Tooltip("슬롯 변경 이미지")]
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
