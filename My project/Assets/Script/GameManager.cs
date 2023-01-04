using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("- 기본 필수 설정 -")]
    public Transform Player;

    [Header("- 재배치 모드 변수 변경 -")]
    [Range(0, 20)]
    [Tooltip("슬롯 개수")]
    public int Slot_Count = 5;
    //기존 슬롯 개수
    int OriginalSlot_Count;
    [Range(0, 5)]
    [Tooltip("슬롯 간격")]
    public float Slot_Interval = 5;
    //카메라 움직이는 위치 설정
    [System.Serializable]
    public struct Change_Main_Camera
    {
        [Tooltip("재배치 모드시, 변경되는 위치")]
        public Vector3 position;
        [Tooltip("재배치 모드 시, 변경되는 회전값")]
        public Quaternion rotation;
        [Space(20)]
        [Tooltip("원근감 제거 여부")]
        public bool orthographic;
    }
    public Change_Main_Camera Change_Cam;

    [Header("- 재배치 모드 필수 설정 -")]
    [Tooltip("메인 캔버스 오브젝트")]
    public GameObject MainGame_Canvas;
    [Tooltip("재배치 모드 캔버스 오브젝트")]
    public GameObject Relocation_Mode;
    [Tooltip("인벤토리 오브젝트")]
    public GameObject Inventory;
    [Tooltip("인벤토리 슬롯 오브젝트")]
    public GameObject Slot_Obj;
    [Tooltip("인벤토리에 넣거나 꺼낼 수 있는 오브젝트들")]
    public GameObject[] Objs = new GameObject[1];


    //소환된 슬롯 오브젝트 리스트
    List<GameObject> Slot_List = new List<GameObject>();
    //현재 잡고 있는 오브젝트
    Transform Hand;
    //현재 잡고 있는 오브젝트의 원래 위치
    struct HandOriginal
    {
        public Vector3 position;
        public Quaternion rotation;
        public string slot_name;
    }
    HandOriginal handOriginal;
    //원래 카메라 위치, 회전값
    struct Original_Main_Camera
    {
        public Vector3 position;
        public Quaternion rotation;
    }
    Original_Main_Camera Original_Cam;
    //카메라
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
        if (Physics.Raycast(mouseRay, out hit, 1000) && Input.GetMouseButtonUp(0) && Hand != null)//영역에 놓기
        {
            if (hit.transform.tag == "Slot")//슬롯에 넣기
            {
                if (Hand.tag == "Turret" || Hand.tag == "Block")
                {
                    Slot_s slot = hit.transform.GetComponent<Slot_s>();
                    if (slot.Block_Name == Hand.name)
                    {
                        slot.Change_Count(1);
                        Destroy(Hand.gameObject);
                    }
                    else//설치 영역
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
    } //슬롯 소환
    public void Mode_Button() //버튼을 누르면 모드 전환
    {
        if (MainGame_Canvas.activeSelf) //재배치 모드 키기
        {
            MainGame_Canvas.SetActive(false);
            Relocation_Mode.SetActive(true);
            MainCam_Tr.GetComponent<Camera>().orthographic = Change_Cam.orthographic;
            MainCam_Tr.position = Change_Cam.position;
            MainCam_Tr.rotation = Change_Cam.rotation;

            if (Slot_Count != OriginalSlot_Count)
                Slot_Setting(Slot_Count - OriginalSlot_Count);
        }
        else //재배치 모드 끄기
        {
            Relocation_Mode.SetActive(false);
            MainGame_Canvas.SetActive(true);
            MainCam_Tr.GetComponent<Camera>().orthographic = false;
            MainCam_Tr.position = Original_Cam.position;
            MainCam_Tr.rotation = Original_Cam.rotation;
        }
    }
    public void ResetBlock_Button() //버튼을 누르면 모드 전환_수정중
    {
        for (int i = 0; i < Inventory.transform.childCount; i++)
        {
            Transform block = Player.transform.GetChild(i);
            Transform slot = Inventory.transform.GetChild(i);
        }
    }
}
