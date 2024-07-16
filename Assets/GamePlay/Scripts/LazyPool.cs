using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazyPool : Singleton<LazyPool>
{
    private Dictionary<GameObject, List<GameObject>> _poolObjt = new Dictionary<GameObject, List<GameObject>>();
    
    public GameObject GetObj(GameObject objKey)
    {
        if (!_poolObjt.ContainsKey(objKey))
        {
            _poolObjt.Add(objKey, new List<GameObject>());
        }

        foreach (GameObject g in _poolObjt[objKey])
        {
            if (g.activeSelf)
                continue;
            g.SetActive(true);
            return g;
        }

        GameObject g2 = Instantiate(objKey, transform);
        _poolObjt[objKey].Add(g2);
        return g2;
        
    }

    public T GetObj<T>(GameObject objKey) where T : Component
    {
        return this.GetObj(objKey).GetComponent<T>();
    }

    public void AddObjectToPool(GameObject obj)
    {
        obj.transform.SetParent(transform); 
        obj.SetActive(false);
    }
}