using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    [SerializeField] private float speed;
    [SerializeField] private GameObject prefab;

    private GameObject player;

    void Awake()
    {
        player = GameObject.Find("Player");
    }

    private void Move(Vector3 direction)
    {

    }
}
