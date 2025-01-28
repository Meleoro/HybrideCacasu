using UnityEngine;

public class GenericSingletonClass<T> : MonoBehaviour where T : Component 
{
    public static T Instance;

    public virtual void Awake()
    {
        if (Instance == null)
            Instance = this as T;
        
        else
            Destroy(gameObject);
    }
}
