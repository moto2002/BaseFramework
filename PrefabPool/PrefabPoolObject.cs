using UnityEngine;

namespace BaseFramework.PrefabPool
{
    public class PrefabPoolObject : MonoBehaviour
    {
        public PrefabPool OwnerPool {
            get { return m_pxOwnerPool; }
            set { AssignToPool( value ); }
        }
        
        public void Spawn( Vector3 pxPosition, Quaternion pxRotation  )
        {
            m_pxTransform.position = pxPosition;
            m_pxTransform.rotation = pxRotation;
            
            m_pxGameObject.SetActive( true );
        }
        
        public void ReturnToPool()
        {
            m_pxGameObject.SetActive( false );
            m_pxOwnerPool.SendMessage( "ReturnObjectToPool", this );
        }
        
        protected virtual void Awake()
        {
            m_pxGameObject = gameObject;
            m_pxTransform = transform;
        }
        
        private void AssignToPool( PrefabPool pxPool )
        {
            m_pxOwnerPool = pxPool;
            ReturnToPool();
        }
        
        private Transform m_pxTransform;
        private GameObject m_pxGameObject;
        private PrefabPool m_pxOwnerPool;
    }
}

