﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : Manager<EnvironmentManager>
{
    
    public int stormNum;
    public GameObject prefabStorm;
    public int stormAroundDistance=1;
    List<Vector2Int> stormLocation = new List<Vector2Int>();
    List<GameObject> stormParticle = new List<GameObject>();
    public void PreTurn()
    {
        StartCoroutine(PreEnvironmentTurn());
    }
    public void PostTurn()
    {
        StartCoroutine(PostEnvironmentTurn());
    }
    IEnumerator PreEnvironmentTurn()
    {
        Vector2Int yRange = GridFunctionUtility.GetPlayerChessyRange();
        yRange.x = Mathf.Max(0, yRange.x - stormAroundDistance);
        yRange.y = Mathf.Min(GridManager.instance.size.y - 1, yRange.y + stormAroundDistance);
        for (int i = 0; i < stormNum; i++)
        {
            Vector2Int location;
            do
            {
                 location= GridFunctionUtility.GetRandomLocation(yRange);
            }
            while (stormLocation.Contains(location));
            stormLocation.Add(location);
        }
        foreach (var location in stormLocation)
        {
            GameObject t = GridFunctionUtility.CreateParticleAt(prefabStorm, location);
            Material mat = t.GetComponent<MeshRenderer>().material;
            Color color = mat.GetColor("_Color");
            mat.SetColor("_Color", Color.yellow);
            stormParticle.Add(t);
            yield return new WaitForSeconds(1f);
            mat.SetColor("_Color", color);
        }
        GameManager.instance.EnvironmentPreTurnEnd();
    }
    IEnumerator PostEnvironmentTurn()
    {
        for (int i = 0; i < stormLocation.Count; i++)
        {
            Vector2Int location = stormLocation[i];
            GChess t=GridManager.instance.GetChess(location);
            if(t!=null)
            {
                t.ElementReaction(Element.Ice);
            }
            stormParticle[i].GetComponent<MeshRenderer>().material.SetColor("_Color", Color.yellow);
            Destroy(stormParticle[i],1f);
            yield return new WaitForSeconds(1f);
        }
        stormLocation.Clear();
        stormParticle.Clear();
        GameManager.instance.EnvironmentPostTurnEnd();
    }
    public List<IGetInfo> GetInfos(Vector2Int location)
    {
        List<IGetInfo> list= new List<IGetInfo>();
        if(stormLocation.Contains(location))
        {
            list.Add(new Information("冰风暴","留在这里的任何角色都会受到低温"));
        }
        return list;
    }
    
}
