using UnityEngine;

namespace Framework
{
	public abstract class UIBase : MonoBehaviour, IWindow
	{
        public abstract UILayer Layer { get; }

        public virtual void Close(bool destory)
        {
            gameObject.SetActive(false);
            if (destory)
            {
                Destroy(gameObject);
            }
        }

        public virtual void Open(params object[] args)
        {
            gameObject.SetActive(true);
            transform.SetAsLastSibling();
            Init();
        }

        protected virtual void Init() { }
        
        public virtual void CloseSelf(bool destory = true)
        {
            UIManager.Instance.Close(GetType().Name, destory);
        }
	}
}