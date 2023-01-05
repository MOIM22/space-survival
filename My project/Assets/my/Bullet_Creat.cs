using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Creat : MonoBehaviour
{
    public float GameTime = 0;

    public int Lv = 1;

    //¾È³ç

    [SerializeField] GameObject Bullet_Obj;
    [SerializeField] Transform StartBullet_Pos;
    [SerializeField] float speed = 1f;

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
           var Ball = Bullet_Obj;
            switch(Lv)
            {
                case 1:
                    for (int i = 0; i < 1; i++)
                    {
                        Ball = Instantiate(Bullet_Obj, StartBullet_Pos.position, StartBullet_Pos.rotation);
                    }
                    Rigidbody BulletRigid = Ball.GetComponent<Rigidbody>();
                    BulletRigid.velocity = StartBullet_Pos.forward * speed;
                    Destroy(Ball.gameObject, 3.0f);
                    GameTime = 0;
                    break;
                case 2:
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
                    break;

            }
            
            
        }
    }

    private void Fire_Bullet()
    {
        


    }
}
