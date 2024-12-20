using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;
public class GenerateBoxes : MonoBehaviour
{
    // Start is called before the first frame update
    private int _count = 0;
    [SerializeField] GameObject InventoryBox;

    public Task<bool> GenerateInventory(int _Size, int _startX, int _startY, int _increment, int _decrement, GameObject PanelObject, string ScriptTobeAddedForItems, string slotTag="")
    {
        int increment = _startX;
        int decrement = _startY;

        for (int i = 0; i < _Size; i++)
        {
            for (int j = 0; j < _Size; j++)
            {
                Vector3 IncrementalSize = new(increment, decrement);
                GameObject _temp = Instantiate(InventoryBox, IncrementalSize, Quaternion.identity);
                _temp.name = (_count).ToString("0");

                if (ScriptTobeAddedForItems != "")
                {
                    var scriptInstance = _temp.AddComponent(Type.GetType(ScriptTobeAddedForItems)) as ISerializableFeildsHelper; //adds the script (cast it to ISerializableFieldsHelper)

                    if(slotTag!="")
                    {
                        scriptInstance.FieldName = slotTag; //adds the field tag
                        _temp.tag = slotTag;
                        InventoryManagementSystem.Instance.AddToSlot(_temp);
                    }

                }

                _temp.transform.SetParent(PanelObject.transform, false);
                increment += _increment;
                _count++;

            }
            decrement -= _decrement;
            increment = _startX;
        }

        _count = 0;
        return Task.FromResult(true);

    }

}
