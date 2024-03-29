﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {
    public Chip[] chips = new Chip[5];
    public GameObject prefabChip;

    public int LocalIdPlayer { get; set; }
    public string NamePlayer { get; set; }

    public Color ColorPlayer { get; set; }

    public GameManager GameManager { get; set; }
    public int[] Scores;
    public int numIsSteps = 0;//дополнительные ходы(если был дубль)
    public int numSteps;//сколько раз кинул кубики игрок
    public int maxSteps = 2;//сколько раз может бросить кубики игрок
    public int indexSixScore;
    public Transform baseCoordinates;
    public List<EnemyCoordBase> enemyCoordBases;

    public Cell StartCell;//стартовая ячейка для данного игрока
    public Cell[] finalCells;//финальные ячейки
    public Cell[] BaseCells;//ячейки базы.

    public int idCellToFinalCell;//
    public int numChipsOnFinalCells;//кол-во фишек закончивших своё путешествие:)
    [Serializable]
    public struct EnemyCoordBase
    {
        public Transform coord;
        public int indexPlayer;
    }
    [Serializable]
    public struct EnemyCoordBaseArray
    {
        public EnemyCoordBase[] enemyCoordBase;
    }

    public void CreatePlayer(int localIdPlayer, string namePlayer, Color colorPlayer, GameManager gameManager, Transform baseCoordinates, EnemyCoordBase[] _enemyCoordBases)
    {
        for(int i = 0; i < 3;i++)
        {
            enemyCoordBases.Add(_enemyCoordBases[i]);
        }
        LocalIdPlayer = localIdPlayer;
        NamePlayer = namePlayer;
        ColorPlayer = colorPlayer;
        GameManager = gameManager;
        this.gameObject.name = NamePlayer;
        bool XOrZAxes, plusOrMinus;
        if (localIdPlayer >= 2)
            XOrZAxes = true;
        else
            XOrZAxes = false;

        if (localIdPlayer == 1 || localIdPlayer == 3)
            plusOrMinus = false;
        else
            plusOrMinus = true;


        for (int i = 0; i < 5;i++)
        {
            GameObject newChip = Instantiate(prefabChip, this.transform);
            newChip.name = NamePlayer + ":Chip#" + i;
            newChip.transform.position = gameManager.spawnPointChip[localIdPlayer].position;
            chips[i] = newChip.GetComponent<Chip>();
            chips[i].materialChip = gameManager.materialsChips[localIdPlayer];
            chips[i].meshRenderer.material = chips[i].materialChip;
            chips[i].materialChip.color = colorPlayer;
            this.baseCoordinates = baseCoordinates;
            chips[i].baseCoordinates = baseCoordinates;
            chips[i].indexChip = i;
            chips[i].idPlayer = LocalIdPlayer;
            chips[i].isXorZAxesSpawnBase = XOrZAxes;
            chips[i].isMinusOrPlusAxes = plusOrMinus;
            chips[i].eventSystem = gameManager.eventSystem;
            chips[i].materialSelectedChip = gameManager.selectedChip;
            chips[i].player = this;
            chips[i].gameManager = gameManager;
            chips[i].CreateChip();
        }
    }
    private void Start()
    {
        
    }

    void GetUserData()
    {

    }

    public IEnumerator _Registration()
    {
        WWW www = new WWW("https://test.ru/php/.php");
        yield return www;
        if (www.error != null)
        {
            Debug.Log("" + www.error);
            yield break;
        }
        else
        {
            Debug.Log(www.text);
        }
    }
    public bool IsSixScores()
    {
        foreach(var scr in Scores)
        {
            if (scr == 6)
                return true;
        }
        return false;
    }
    public bool SearchChipInField()
    {
        for (int i = 0; i < 5; i++)
        {
            if (chips[i].isOnField)
            {
                return true;
            }
        }
        return false;
    }

    public void AddScore(int score)
    {
        //Debug.Break();
        Array.Resize(ref Scores, Scores.Length + 1);
        Scores[Scores.Length - 1] = score;
        
        //scoreGO.transform.localPosition = new scoreGO.transform.localPosition.x, -(Scores.Length * 100 + 50), scoreGO.transform.localPosition.z);
        //Rect rect = GameManager.contentScroll;
        //GameManager.contentScroll.Set(rect.x, rect.y, rect.width, rect.height + 100);
        //GameManager.contentScroll.GetComponent<RectTransform>().rect.Set(rect.x,rect.y,rect.width,rect.height);
    }
    public void DeleteScore(int indexScore)
    {
        
        if (Scores.Length > 1)
        {
            Scores[indexScore] = Scores[Scores.Length - 1];
            Array.Resize(ref Scores, Scores.Length - 1);
        }
        else
        {
            Array.Resize(ref Scores, 0);
        }

        //GameManager.scrollRect.gameObject.SetActive(true);
        
        if(Scores.Length == 0)
        {
            GameManager.scrollRect.gameObject.SetActive(false);
        }
        if (Scores.Length == 1)
        {
            GameManager.scrollRect.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 100);
            GameManager.scrollRect.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 100);
        }
        else if (Scores.Length == 2)
        {
            GameManager.scrollRect.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 100);
            GameManager.scrollRect.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 200);
        }
        else
        {
            GameManager.scrollRect.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 120);
            GameManager.scrollRect.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 200);
        }
        GameManager.scrollRect.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Scores.Length * 100);
        int index = 0;
        foreach (var item in GameManager.scoreObjects)
        {
            Destroy(item);
        }
        GameManager.scoreObjects.Clear();
        foreach (var item in Scores)
        {
            GameObject scoreGO = Instantiate(GameManager.scorePrefab, GameManager.scrollRect.content.gameObject.transform);
            //scoreGO.GetComponent<RectTransform>().anchoredPosition.Set(0,(index++) * 100);

            scoreGO.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, ((index++) * -100) - 100);
            scoreGO.transform.Find("Text").GetComponent<Text>().text = item.ToString();
            GameManager.scoreObjects.Add(scoreGO);
        }
        GameManager.scrollRect.verticalScrollbar.value = 1;
    }

    public void CleanData()
    {

        Scores = new int[0];
        numSteps = 0;
        indexSixScore = 0;
        foreach (var item in GameManager.scoreObjects)
        {
            Destroy(item);
        }
        GameManager.scoreObjects.Clear();
    }
}
