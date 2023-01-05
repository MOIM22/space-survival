using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Creat : MonoBehaviour
{
    public float GameTime = 0;

    public int Lv = 1;

    private float random_number = Random.Range(-45, 45);

    [SerializeField] GameObject Bullet_Obj;
    [SerializeField] float speed = 1f;

    [Header("Bullet_Pos")]
    [SerializeField] Transform StartBullet_Pos1;
    [SerializeField] Transform StartBullet_Pos2;
    [SerializeField] Transform StartBullet_Pos3;
    [SerializeField] Transform StartBullet_Pos4;
    [SerializeField] Transform StartBullet_Pos5;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        GameTime += Time.deltaTime;
        Creat_Bullet();
    }

    private void Creat_Bullet()
    {
        if(GameTime > 1)
        {
            var Ball1 = Bullet_Obj;
            var Ball2 = Bullet_Obj;
            switch (Lv)
            {
                case 1:
                    Ball1 = Instantiate(Bullet_Obj, StartBullet_Pos1.position, StartBullet_Pos1.rotation);
                    Ball2 = Instantiate(Bullet_Obj, StartBullet_Pos2.position, StartBullet_Pos2.rotation);
                    Rigidbody BulletRigid1 = Ball1.GetComponent<Rigidbody>();
                    Rigidbody BulletRigid2 = Ball2.GetComponent<Rigidbody>();
                    BulletRigid1.velocity = StartBullet_Pos1.forward * speed;
                    BulletRigid2.velocity = StartBullet_Pos2.forward * speed;
                    Destroy(Ball1.gameObject, 3.0f);
                    Destroy(Ball2.gameObject, 3.0f);
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

    private void Fire_Bullet()
    {
        


    }
}
