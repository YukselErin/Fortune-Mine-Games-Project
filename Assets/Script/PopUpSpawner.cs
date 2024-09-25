using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Unity.Mathematics;

public class PopUpSpawner : MonoBehaviour
{
    public ObjectPool<GameObject> pool;
    public GameObject popUpUIElement; void Start()
    {

        pool = new ObjectPool<GameObject>(CreateBullet, OnTakeBulletFromPool, OnReturnBulletToPool, OnDestroyBullet, true, 1, 10);

    }
    private GameObject CreateBullet()
    {
        GameObject bullet = Instantiate(popUpUIElement, new Vector3(0f, 0f, 0f), quaternion.identity);
        bullet.GetComponent<PopUpUIElement>().SetPool(pool);
        return bullet;
    }

    private void OnTakeBulletFromPool(GameObject bullet)
    {
        bullet.gameObject.SetActive(true);
    }
    private void OnReturnBulletToPool(GameObject bullet)
    {
        bullet.gameObject.SetActive(false);

    }
    private void OnDestroyBullet(GameObject bullet)
    {
        Destroy(bullet);
    }
}
