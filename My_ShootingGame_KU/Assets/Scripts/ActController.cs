using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ActController : MonoBehaviour
{
    public Transform character;
    public List<GameObject> bullet = new List<GameObject>();
    private int bulletNum = 0;
    public TMP_Text bullet_Text;
    private List<GameObject> bulletList = new List<GameObject>();

    public Transform shootPoint;
    public float speed;
    public float destoryDelay;
    private Vector3 rotateVec;

    public GameObject enemy;
    private List<GameObject> enemyList = new List<GameObject>();
    public Transform spawnPoint;

    public float startTime;
    public float endTime;
    Animator animator;

    public bool aniMode = false;

    public void MoveEnemy()
    {
        if(enemyList.Count == 0)
        {
            return;
        }

        for (int i = 0; i < enemyList.Count; i++)
        {
            if (enemyList[i])
            {
                enemyList[i].transform.Translate(-speed * Time.deltaTime, 0, 0);
                enemyList[i].transform.Rotate(rotateVec * 2);
            }            
        }
    }

    public void MoveBullet()
    {
        if (bulletList.Count == 0)
        {
            return;
        }

        for (int i = 0; i < bulletList.Count; i++)
        {
            if (bulletList[i])
                bulletList[i].transform.Translate(0, 0, -speed * Time.deltaTime);
        }
    }

    public void ChangeBullet()
    {
        bulletNum = bulletNum == 0 ? 1 : 0;
        bullet_Text.text = bulletNum == 0 ? "Bullet" : "Sphere";
    }

    private Vector3 Get_RandomPosition()
    {
        Vector3 ran = Vector3.zero;

        ran.x = UnityEngine.Random.Range(-50, 50);
        ran.z = UnityEngine.Random.Range(-50, 50);

        return ran;
    }

    private void SpawnEnemy()
    {
        GameObject _enemy = Instantiate(enemy);
        _enemy.transform.SetParent(spawnPoint);
        _enemy.transform.position = spawnPoint.position + Get_RandomPosition();        
        enemyList.Add(_enemy);

        Destroy(_enemy, destoryDelay);
    }

    private void Shoot_Bullet()
    {
        GameObject _bullet = Instantiate(bullet[bulletNum]);
        _bullet.transform.SetParent(character);
        _bullet.transform.position = shootPoint.position;
        _bullet.transform.rotation = bullet[bulletNum].transform.rotation;
        bulletList.Add(_bullet);

        Destroy(_bullet, destoryDelay);
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = character.GetComponent<Animator>();        
        animator.Play("WAIT00");

        InvokeRepeating("SpawnEnemy", startTime, endTime);

        rotateVec = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            Shoot_Bullet();
        }
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            ChangeBullet();
        }

        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        character.transform.Translate(x * speed * Time.deltaTime, 0, y * speed * Time.deltaTime);
        rotateVec.y = x;

        MoveEnemy();
        MoveBullet();

        character.transform.Rotate(rotateVec);

        if (aniMode)
        {
            if (x == 0 && y == 0)
            {
                animator.Play("WAIT00");
            }
            else if (x == 0)
            {
                animator.Play("RUN00_F");
            }
            else
            {
                if (x > 0)
                    animator.Play("RUN00_R");
                else if (x < 0)
                    animator.Play("RUN00_L");
            }
        }        
    }
}
