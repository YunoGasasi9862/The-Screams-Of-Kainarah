
using System.Threading.Tasks;
using UnityEngine;

public class EnemyHittableManager : MonoBehaviour
{
    public static Task<bool> isEntityAnAttackObject(Collider2D collider, EnemyHittableObjects objects)
    {
        for (int i = 0; i < objects.elements.Length; i++)
        {

            if (collider.tag == objects.elements[i].ObjectTag) //scriptable Object
            {
                return Task.FromResult(true);
            }
        }

        return Task.FromResult(false);
    }
}