using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MoveCrystal : MonoBehaviour
{
    // Start is called before the first frame update
    private bool _isMoving = false;
    private RectTransform _diamondUILocation;
    private Vector3 _diamondUILocaitonConverted, LocalPos;
    public static bool increaseValue = false;
    void Start()
    {
        _diamondUILocation=GameObject.FindWithTag("Diamond").GetComponent<RectTransform>(); 

    }

    // Update is called once per frame
    void Update()
    {
       

        if (transform!=null &&_isMoving)
        {
            _diamondUILocaitonConverted = Camera.main.ScreenToWorldPoint(_diamondUILocation.position); //converts UI position to world position
            LocalPos= _diamondUILocaitonConverted;
            LocalPos.z = 0;
            LocalPos.x = LocalPos.x - 1f;
            transform.DOMove(LocalPos, .090f).SetEase(Ease.InFlash);
            transform.GetComponent<BoxCollider2D>().enabled = false;
           

        }

        OnDestroy();

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _isMoving= true;
        }
    }


    private void OnDestroy()
    {
        if (transform!=null && ((int)transform.position.x == (int)LocalPos.x))
        {
            increaseValue = true;
            Destroy(gameObject);
        }
    }
}