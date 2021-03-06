﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// 控制游戏流程的单例类
/// </summary>
public class GameManager : Manager<GameManager>
{
    
    [NonSerialized]
    public int teamCount = 2;
    [NonSerialized]
    public int playerTeam = 1;
    [NonSerialized]
    public int curRound = 0;
    //游戏回合流程的事件分发
    public UnityEvent eRoundStart = new UnityEvent();
    public UnityEvent eRoundEnd = new UnityEvent();
    public UnityEvent ePlayerTurnEnd = new UnityEvent();
    public UnityEvent eGameStart = new UnityEvent();
    public UnityEvent eGameEnd = new UnityEvent();
    public GameObject winUI;
    public ParticleSystem snowWeatherParticle;
    public Color BrightColor;
    public Light sun;
    protected override void Awake()
    {
        base.Awake();
    }
    public void Start()
    {
        GameStart();
    }
    protected void GameStart()
    {
        eGameStart.Invoke();
        RoundStart();
    }
    protected void RoundStart()
    {
        curRound++;
        eRoundStart.Invoke();
        AIPreTurnStart();
    }
    protected void AIPreTurnStart ()
    {
        Debug.Log("GameState:AIPreTurnStart");
        StartCoroutine(GridFunctionUtility.InvokeNextFrame(AIManager.instance.PreTurn));
    }

    public void AIPreTurnEnd()
    {
        Debug.Log("GameState:AIPreTurnEnd");

        EnvironmentPreTurnStart();    
    }
    protected void EnvironmentPreTurnStart()
    {
        //Debug.Log("GameState:PlayerTurnStart");
        EnvironmentManager.instance.PreTurn();

    }
    public void EnvironmentPreTurnEnd()
    {
        PlayerTurnStart();        
    }
    protected void PlayerTurnStart()
    {
        Debug.Log("GameState:PlayerTurnStart");
        PlayerControlManager.instance.PlayerTurnEnter();
    }
    public void PlayerTurnEnd()
    {
        Debug.Log("GameState:PlayerTurnEnd");
        PlayerControlManager.instance.PlayerTurnExit();
        ePlayerTurnEnd.Invoke();
        EnvironmentPostTurnStart();
    }
    protected void EnvironmentPostTurnStart()
    {
        EnvironmentManager.instance.PostTurn();
    }
    public void EnvironmentPostTurnEnd()
    {
        AIPostTurnStart();
    }
    protected void AIPostTurnStart()
    {
        Debug.Log("GameState:AIPostTurnStart");
        StartCoroutine(GridFunctionUtility.InvokeNextFrame(AIManager.instance.PostTurn));
    }

    public void AIPostTurnEnd()
    {
        Debug.Log("GameState:AIPostTurnEnd");
        RoundEnd();
    }
    void RoundEnd()
    {
        eRoundEnd.Invoke();
        RoundStart();
    }
    public void GameWin()
    {
        winUI.SetActive(true);
        snowWeatherParticle.Stop();
        StartCoroutine(WatherClear());
    }
    IEnumerator WatherClear()
    {
        Color originColor = Camera.main.backgroundColor;
        float originLight = sun.intensity;
        float t = 0;
        while(t<3)
        {
            Camera.main.backgroundColor = Color.Lerp(originColor, BrightColor,t);
            sun.intensity = Mathf.Lerp(originLight, 1.6f, t);
            t += Time.deltaTime / 3;
            yield return null;
        }
    }
    void GameEnd()
    {
        eGameEnd.Invoke();
    }

}
