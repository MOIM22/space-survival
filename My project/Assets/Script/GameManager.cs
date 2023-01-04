using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("- �⺻ �ʼ� ���� -")]
    public Transform Player;

    [Header("- ���ġ ��� ���� ���� -")]
    [Range(0, 20)]
    [Tooltip("���� ����")]
    public int Slot_Count = 5;
    //���� ���� ����
    int OriginalSlot_Count;
    [Range(0, 5)]
    [Tooltip("���� ����")]
    public float Slot_Interval = 5;
    //ī�޶� �����̴� ��ġ ����
    [System.Serializable]
    public struct Change_Main_Camera
    {
        [Tooltip("���ġ ����, ����Ǵ� ��ġ")]
        public Vector3 position;
        [Tooltip("���ġ ��� ��, ����Ǵ� ȸ����")]
        public Quaternion rotation;
        [Space(20)]
        [Tooltip("���ٰ� ���� ����")]
        public bool orthographic;
    }
    public Change_Main_Camera Change_Cam;

    [Header("- ���ġ ��� �ʼ� ���� -")]
    [Tooltip("���� ĵ���� ������Ʈ")]
    public GameObject MainGame_Canvas;
    [Tooltip("���ġ ��� ĵ���� ������Ʈ")]
    public GameObject Relocation_Mode;
    [Tooltip("�κ��丮 ������Ʈ")]
    public GameObject Inventory;
    [Tooltip("�κ��丮 ���� ������Ʈ")]
    public GameObject Slot_Obj;
    [Tooltip("�κ��丮�� �ְų� ���� �� �ִ� ������Ʈ��")]
    public GameObject[] Objs = new GameObject[1];


    //��ȯ�� ���� ������Ʈ ����Ʈ
    List<GameObject> Slot_List = new List<GameObject>();
    //���� ��� �ִ� ������Ʈ
    Transform Hand;
    //���� ��� �ִ� ������Ʈ�� ���� ��ġ
    struct HandOriginal
    {
        public Vector3 position;
        public Quaternion rotation;
        public string slot_name;
    }
    HandOriginal handOriginal;
    //���� ī�޶� ��ġ, ȸ����
    struct Original_Main_Camera
    {
        public Vector3 position;
        public Quaternion rotation;
    }
    Original_Main_Camera Original_Cam;
    //ī�޶�
    Transform MainCam_Tr;

    void Start()
    {
        MainCam_Tr = Camera.main.transform;
        Original_Cam.position = MainCam_Tr.position;
        Original_Cam.rotation = MainCam_Tr.rotation;
        Slot_Setting(Slot_Count);
        Relocation_Mode.SetActive(false);
        MainGame_Canvas.SetActive(true);
    }
    void Update()
    {
        if (Relocation_Mode.activeSelf)
            Mode();
    }
    void Mode()
    {
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(mouseRay, out hit, 1000, ~(1 << LayerMask.NameToLayer("Back_Area"))))
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (hit.transform.tag == "Turret" || hit.transform.tag == "Block")
                {
                    Hand = hit.transform;
                    Hand.GetComponent<Collider>().enabled = false;
                    Hand.transform.SetParent(Player);
                    handOriginal.position = Hand.position;
                    handOriginal.rotation = Hand.rotation;
                }
                else if (hit.transform.tag == "Slot")
                {
                    Slot_s slot = hit.transform.GetComponent<Slot_s>();
                    int i = -1;
                    switch (slot.Block_Name)
                    {
                        case "test_block":
                            i = 0;
                            break;
                        case "test_turret":
                            i = 1;
                            break;
                    }

                    if (i != -1)
                    {
                        Hand = Instantiate(Objs[i], hit.transform.position, Quaternion.Euler(0, 0, 0)).transform;
                        Hand.name = Hand.name.Replace("(Clone)", string.Empty);
                        Hand.GetComponent<Collider>().enabled = false;
                        handOriginal.slot_name = hit.transform.name;
                        slot.Change_Count(-1);
                    }
                }

                if (Hand != null && Hand.tag == "Block")
                    Hand.GetComponent<Block_s>().On = true;
            }
        }
        if (Input.GetMouseButton(0) && Hand != null && Physics.Raycast(mouseRay, out hit, 1000))
        {
            Hand.position = new Vector3(hit.point.x, 0, hit.point.z);
            if (Hand.tag == "Turret" && hit.transform.tag == "area")
            {
                switch (hit.transform.name)
                {
                    case "forword":
                        Hand.transform.rotation = Quaternion.Euler(0, 0, 0);
                        break;
                    case "back":
                        Hand.transform.rotation = Quaternion.Euler(0, 180, 0);
                        break;
                    case "right":
                        Hand.transform.rotation = Quaternion.Euler(0, 90, 0);
                        break;
                    case "left":
                        Hand.transform.rotation = Quaternion.Euler(0, 270, 0);
                        break;
                }
            }
            else
            {
                Hand.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
        if (Physics.Raycast(mouseRay, out hit, 1000) && Input.GetMouseButtonUp(0) && Hand != null)//������ ����
        {
            if (hit.transform.tag == "Slot")//���Կ� �ֱ�
            {
                if (Hand.tag == "Turret" || Hand.tag == "Block")
                {
                    Slot_s slot = hit.transform.GetComponent<Slot_s>();
                    if (slot.Block_Name == Hand.name)
                    {
                        slot.Change_Count(1);
                        Destroy(Hand.gameObject);
                    }
                    else//��ġ ����
                    {
                        if (slot.Block_Name == null)
                        {
                            slot.Block_Count = 1;
                            slot.Block_Name = Hand.name;
                            Destroy(Hand.gameObject);
                        }
                        else if (handOriginal.slot_name != null)
                        {
                            slot = GameObject.Find(handOriginal.slot_name).GetComponent<Slot_s>();
                            slot.Change_Count(1);
                            slot.Block_Name = Hand.name;
                            Destroy(Hand.gameObject);
                            handOriginal.slot_name = null;
                        }
                        else
                        {
                            Hand.position = handOriginal.position;
                            Hand.rotation = handOriginal.rotation;
                            Hand.GetComponent<Collider>().enabled = true;
                        }
                    }
                }

                /*
                Hand.rotation = Quaternion.Euler(0, 0, 0);
                Hand.transform.SetParent(hit.transform);

                if (Hand.tag == "Block")
                    Hand.GetComponent<Block_s>().On = false;
                */
            }
            else if (hit.transform.tag == "area")
            {
                Hand.position = new Vector3(hit.transform.position.x, 0, hit.transform.position.z);
                Hand.GetComponent<Collider>().enabled = true;
            }
            else
            {
                if (handOriginal.slot_name != null)
                {
                    Slot_s slot = GameObject.Find(handOriginal.slot_name).GetComponent<Slot_s>();
                    slot.Change_Count(1);
                    slot.Block_Name = Hand.name;
                    Destroy(Hand.gameObject);
                    handOriginal.slot_name = null;
                }
                else
                {
                    Hand.position = handOriginal.position;
                    Hand.rotation = handOriginal.rotation;
                    Hand.GetComponent<Collider>().enabled = true;
                }
            }

            Hand = null;
        }
    }
    void Slot_Setting(int num)
    {
        OriginalSlot_Count = Slot_Count;

        if (num > 0)
        {
            for (int i = 0; i < num; i++)
                Slot_List.Add(Instantiate(Slot_Obj, Inventory.transform.position, Quaternion.Euler(0, 0, 0), Inventory.transform));
        }
        else if (num < 0)
        {
            int list_count = Slot_List.Count;
            for (int i = 0; i < -num; i++)
            {
                int index = list_count - i - 1;
                Destroy(Slot_List[index]);
                Slot_List.RemoveAt(index);
            }
        }

        float plus = -(Slot_Interval / 2) * (Slot_List.Count - 1);
        for (int i = 0; i < Slot_List.Count; i++, plus += Slot_Interval)
        {
            Slot_List[i].name = i.ToString();
            Slot_List[i].transform.position = Inventory.transform.position + (Vector3.right * plus);
        }
    } //���� ��ȯ
    public void Mode_Button() //��ư�� ������ ��� ��ȯ
    {
        if (MainGame_Canvas.activeSelf) //���ġ ��� Ű��
        {
            MainGame_Canvas.SetActive(false);
            Relocation_Mode.SetActive(true);
            MainCam_Tr.GetComponent<Camera>().orthographic = Change_Cam.orthographic;
            MainCam_Tr.position = Change_Cam.position;
            MainCam_Tr.rotation = Change_Cam.rotation;

            if (Slot_Count != OriginalSlot_Count)
                Slot_Setting(Slot_Count - OriginalSlot_Count);
        }
        else //���ġ ��� ����
        {
            Relocation_Mode.SetActive(false);
            MainGame_Canvas.SetActive(true);
            MainCam_Tr.GetComponent<Camera>().orthographic = false;
            MainCam_Tr.position = Original_Cam.position;
            MainCam_Tr.rotation = Original_Cam.rotation;
        }
    }
    public void ResetBlock_Button() //��ư�� ������ ��� ��ȯ_������
    {
        for (int i = 0; i < Inventory.transform.childCount; i++)
        {
            Transform block = Player.transform.GetChild(i);
            Transform slot = Inventory.transform.GetChild(i);
        }
    }
}
