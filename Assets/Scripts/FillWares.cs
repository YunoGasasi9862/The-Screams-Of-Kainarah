using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class FillWares : MonoBehaviour
{
    [Header("Fill Wares with Items")]
    [SerializeField] List<GameObject> wareObjects;
    private List<GameObject> freeslots;
    private int wareCounter = 0;
    [SerializeField] GameObject panel;
    private float scaleSize = .9f;
    void Start()
    {
        freeslots=new List<GameObject>();
        StartCoroutine(FillSlots(panel));
        StartCoroutine(FillUpWares());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator FillUpWares()
    {
        yield return new WaitForSeconds(.7f);
        for (int i=0; i<freeslots.Count; i++)
        {
            if (Possible(i))
            {
                Sprite _sprite = wareObjects[wareCounter].GetComponent<Image>().sprite;
                AssignTagsandSprite(freeslots[i], scaleSize, _sprite, wareObjects[wareCounter].tag);
                wareCounter++;
            }
        }

    }

    IEnumerator  FillSlots(GameObject panel)
    {
        yield return new WaitForSeconds(.5f);
        for(int i=0; i<panel.transform.childCount; i++)
        {
            freeslots.Add(panel.transform.GetChild(i).gameObject);
        }
       
    }

    public bool Possible(int index)
    {
        return wareObjects.Count!= wareCounter && freeslots[index].transform.childCount==0;
    }

    public GameObject AssignTagsandSprite(GameObject _freeslot,float scaleSize, Sprite sprite, string name)
    {
        GameObject temp=new GameObject(name);
        temp.transform.localScale = new Vector3(scaleSize, scaleSize, scaleSize);
        temp.transform.tag = _freeslot.transform.tag;
        temp.AddComponent<Image>();
        temp.GetComponent<Image>().sprite = sprite;
        temp.transform.SetParent(_freeslot.transform, false);
        return temp;
    }
}