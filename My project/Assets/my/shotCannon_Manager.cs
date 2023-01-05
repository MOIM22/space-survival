using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shotCannon_Manager : MonoBehaviour
{
    public float GameTime = 0;

    public int Lv = 1;


    [SerializeField] GameObject Bullet_Obj;
    [SerializeField] float speed = 10f;

    [Header("Bullet_Pos")]
    [SerializeField] Transform StartBullet_Pos1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameTime += Time.deltaTime;
        ShotCannon();
    }

    private void ShotCannon()
    {
        if (GameTime > 1)
        {
            var Ball1 = Bullet_Obj;
            switch (Lv)
            {
                case 1:
                    StartBullet_Pos1.Rotate(transform.forward + new Vector3(0, 0, 0));
                    Ball1 = Instantiate(Bullet_Obj, StartBullet_Pos1.position, StartBullet_Pos1.rotation);
                    Ball1.GetComponent<Rigidbody>().velocity = StartBullet_Pos1.forward * speed;
                    StartBullet_Pos1.rotation = transform.rotation;
                    Destroy(Ball1.gameObject, 5.0f);
                    GameTime = 0;
                    break;
                    /*case 2:
                        for (int i = 0; i < 3; i++)
                        {
                            Ball = Instantiate(Bullet_Obj, StartBullet_Pos.position, transform.rotation);
                        }
                        break;
                    case 3:
                        for (int i = 0; i < 4; i++)
                        {
                            Ball = Instantiate(Bullet_Obj, StartBullet_Pos.position, transform.rotation);
                        }
                        break;
                    case 4:
                        for (int i = 0; i < 5; i++)
                        {
                            Ball = Instantiate(Bullet_Obj, StartBullet_Pos.position, transform.rotation);
                        }
                        break;
                    case 5:
                        for (int i = 0; i < 5; i++)
                        {
                            Ball = Instantiate(Bullet_Obj, StartBullet_Pos.position, transform.rotation);
                        }
                        break;*/

            }
        }
    }
}
