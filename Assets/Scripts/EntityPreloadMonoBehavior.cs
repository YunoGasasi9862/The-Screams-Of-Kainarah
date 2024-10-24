using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
public abstract class EntityPreloadMonoBehavior: MonoBehaviour, IEntityPreload, IEntityType
{
    private EntityType m_entityType;
    public EntityType EntityIdentifier { get => m_entityType; set => m_entityType = EntityType.MonoBehavior; }
    public abstract Task<Tuple<EntityType, dynamic>> EntityPreload(AssetReference assetReference, EntityType entityType, Preloader preloader);
}