using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public Player[] players;
    public GameObject playerPrefab;
    public GameObject playersPoint;
    public int numPlayers;
    public Transform[] spawnPointChip;
    public Color[] colors;//цвета игроков
    public Material[] materialsChips;
    public Material selectedChip;
    public new Camera camera;
    public EventSystem eventSystem;
    public GameObject cubePrefab;
    public Cube[] cubes;
    public Transform[] spawnPointsCubes;
    public GameObject buttonStepOne;
    public GameObject buttonStepTwo;
    public GameObject[] startCellPlayers;//стартовые позиции фишек
    public Image[] imagesStartCellPlayers;//картинки стартовых позиций фишек
    public Image[] imagesColorCellPlayers;//картинки стартовых позиций фишек
    public Text[] ScoresBoards;
    public int idThisPlayer;//номер текущего игрока
    public Transform[] basesPlayersCoordinates;
    public Transform[] startPointsPlayers;
    public float tForLerp = 0;
    public Text namePlayer;
    public Cell[] cells;
    public Cell[] startCells;
    public Cell[] angleCells;
    public Cell[] spawnCellsOne;
    public Cell[] spawnCellsTwo;
    public Cell[] spawnCellsThree;
    public Cell[] spawnCellsFour;
    public int AllCubesSleep;//спят ли кубики?
    public float timeDeleteCubes = 3;
    RaycastHit hit;
    public GameObject GhostChip;
    Chip thisSelectedChip;
    public LayerMask layerMask;

    

    public Player.EnemyCoordBase[] enemyCoordBasesOnePlayer = new Player.EnemyCoordBase[3];
    public Player.EnemyCoordBase[] enemyCoordBasesTwoPlayer = new Player.EnemyCoordBase[3];
    public Player.EnemyCoordBase[] enemyCoordBasesThreePlayer = new Player.EnemyCoordBase[3];
    public Player.EnemyCoordBase[] enemyCoordBasesFourPlayer = new Player.EnemyCoordBase[3];
    public Player.EnemyCoordBaseArray[] enemyCoordBaseArrays = new Player.EnemyCoordBaseArray[4];

     public Cell[] finalCells1;//финальные ячейки
   
    public Cell[] finalCells2;//финальные ячейки
    public Cell[] finalCells3;//финальные ячейки
    public Cell[] finalCells4;//финальные ячейки
    public int[] idCellsToFinalCells;

    public Cell[] homeCells;

    public GameObject winPanel;
    public Text winText;
    void Start()
    {
        foreach (Cell cell in homeCells)
        {
            cell.idPlayer = -1;
            cell.GhostChip = Instantiate(GhostChip, cell.transform).GetComponent<GhostChip>();
            cell.GhostChip.transform.position = cell.transform.position;
        }
        enemyCoordBaseArrays[0].enemyCoordBase = enemyCoordBasesOnePlayer;
        enemyCoordBaseArrays[1].enemyCoordBase = enemyCoordBasesTwoPlayer;
        enemyCoordBaseArrays[2].enemyCoordBase = enemyCoordBasesThreePlayer;
        enemyCoordBaseArrays[3].enemyCoordBase = enemyCoordBasesFourPlayer;
        idThisPlayer = 0;
        buttonStepOne.SetActive(true);
        buttonStepTwo.SetActive(false);
        players = new Player[numPlayers];
        for (int i = 0; i < cells.Length; i++)
        {
            cells[i].idCell = i + 1;
            cells[i].idPlayer = -1;
        }
        foreach(Cell cell in cells)
        {
            cell.GhostChip = Instantiate(GhostChip, cell.transform).GetComponent<GhostChip>();
            cell.GhostChip.transform.position = cell.transform.position;
        }

        foreach (Cell cell in startCells)
        {
            cell.GhostChip = Instantiate(GhostChip, cell.transform).GetComponent<GhostChip>();
            cell.GhostChip.transform.position = cell.transform.position;
        }
        for (int i = 0; i < numPlayers; i++)
        {
            GameObject player = Instantiate(playerPrefab, playersPoint.transform);
            players[i] = player.GetComponent<Player>();
            players[i].CreatePlayer(i, "Player#" + i, colors[i], this, basesPlayersCoordinates[i], enemyCoordBaseArrays[i].enemyCoordBase);
            players[i].StartCell = startCells[i];
            players[i].idCellToFinalCell = idCellsToFinalCells[i];
            imagesStartCellPlayers[i].color = colors[i];
            imagesColorCellPlayers[i].color = colors[i];
        }
        foreach (Cell cell in spawnCellsOne)
        {
            cell.GhostChip = Instantiate(GhostChip, cell.transform).GetComponent<GhostChip>();
            cell.GhostChip.transform.position = cell.transform.position;
        }
        
        players[0].BaseCells = spawnCellsOne;
        players[0].finalCells = finalCells1;
        foreach (Cell cell in players[0].finalCells)
        {
            cell.GhostChip = Instantiate(GhostChip, cell.transform).GetComponent<GhostChip>();
            cell.GhostChip.transform.position = cell.transform.position;
        }
        foreach (Cell cell in spawnCellsTwo)
        {
            cell.GhostChip = Instantiate(GhostChip, cell.transform).GetComponent<GhostChip>();
            cell.GhostChip.transform.position = cell.transform.position;
        }
        players[1].BaseCells = spawnCellsTwo;
        players[1].finalCells = finalCells2;
        foreach (Cell cell in players[1].finalCells)
        {
            cell.GhostChip = Instantiate(GhostChip, cell.transform).GetComponent<GhostChip>();
            cell.GhostChip.transform.position = cell.transform.position;
        }
        foreach (Cell cell in spawnCellsThree)
        {
            cell.GhostChip = Instantiate(GhostChip, cell.transform).GetComponent<GhostChip>();
            cell.GhostChip.transform.position = cell.transform.position;
        }
        players[2].BaseCells = spawnCellsThree;
        players[2].finalCells = finalCells3;
        foreach (Cell cell in players[2].finalCells)
        {
            cell.GhostChip = Instantiate(GhostChip, cell.transform).GetComponent<GhostChip>();
            cell.GhostChip.transform.position = cell.transform.position;
        }
        foreach (Cell cell in spawnCellsFour)
        {
            cell.GhostChip = Instantiate(GhostChip, cell.transform).GetComponent<GhostChip>();
            cell.GhostChip.transform.position = cell.transform.position;
        }
        players[3].BaseCells = spawnCellsFour;
        players[3].finalCells = finalCells4;
        foreach (Cell cell in players[3].finalCells)
        {
            cell.GhostChip = Instantiate(GhostChip, cell.transform).GetComponent<GhostChip>();
            cell.GhostChip.transform.position = cell.transform.position;
        }


    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit,float.MaxValue,layerMask))
        {
            if (hit.collider.tag == "Chip")
            {

                Chip hitChip = hit.collider.GetComponent<Chip>();
                if (hitChip.idPlayer == idThisPlayer & !hitChip.isOnFinalCell)
                {
                    if (thisSelectedChip != null)
                    {
                        thisSelectedChip.ChipUnSelected();
                        thisSelectedChip = null;
                        HideGhostChip();
                    }

                    thisSelectedChip = hitChip;

                    thisSelectedChip.ChipSelected();
                }
                else if (hitChip.idPlayer != idThisPlayer & thisSelectedChip != null)
                {
                    Chip thisDeletedChip = hitChip;
                    if(thisDeletedChip.thisCell == null)
                    {
                        if (thisSelectedChip != null)
                        {
                            thisSelectedChip.ChipUnSelected();
                            thisSelectedChip = null;
                            HideGhostChip();
                            
                        }
                        return;
                    }
                    if (thisDeletedChip.thisCell.GhostChip.gameObject.activeInHierarchy == true)
                    {
                        if (thisDeletedChip.MoveToEnemyBase(thisDeletedChip, thisSelectedChip, thisDeletedChip.thisCell))
                        {
                            if (IsEndRaundOneStep())
                            {
                                if (IsEndRaundTwoStep())
                                {
                                    NextPlayer();
                                }
                                else
                                {
                                    StepOne();
                                }
                            }
                            if (thisSelectedChip != null)
                            {
                                thisSelectedChip.ChipUnSelected();
                                thisSelectedChip = null;
                                HideGhostChip();
                            }
                        }
                    }
                    else
                    {
                        if (thisSelectedChip != null)
                        {
                            thisSelectedChip.ChipUnSelected();
                            thisSelectedChip = null;
                            HideGhostChip();
                        }
                    }
                }
            }
            else if (hit.collider.tag == "Cell")
            {
                Cell hitCell = hit.collider.GetComponent<Cell>();
                if (thisSelectedChip != null & !hitCell.isOccupied)
                {
                    if (hitCell.GhostChip.gameObject.activeInHierarchy == true)
                    {
                        if (thisSelectedChip.MoveToGhostChip(hitCell))
                        {
                            if (IsEndRaundOneStep())
                            {
                                if (IsEndRaundTwoStep())
                                {
                                    NextPlayer();
                                }
                                else
                                {
                                    StepOne();
                                }
                            }
                            thisSelectedChip.ChipUnSelected();
                            thisSelectedChip = null;
                        }
                        else
                        {
                            Debug.Log("CheckChipsOnField() = " + CheckChipsOnField());
                            if (CheckChipsOnField() == false & CheckPlayersScoreIsSixScore() == false)
                                NextPlayer();
                            HideGhostChip();
                            thisSelectedChip.ChipUnSelected();
                            thisSelectedChip = null;
                        }
                    }
                    else
                    {
                        Debug.Log("CheckChipsOnField() = " + CheckChipsOnField());
                        if (CheckChipsOnField() == false & CheckPlayersScoreIsSixScore() == false)
                            NextPlayer();
                        HideGhostChip();
                        thisSelectedChip.ChipUnSelected();
                        thisSelectedChip = null;
                    }
                }
                else
                {
                    if (thisSelectedChip != null)
                    {
                        Debug.Log("CheckChipsOnField() = " + CheckChipsOnField());
                        if (CheckChipsOnField() == false & CheckPlayersScoreIsSixScore() == false)
                            NextPlayer();
                        HideGhostChip();
                        thisSelectedChip.ChipUnSelected();
                        thisSelectedChip = null;
                    }
                }

            }
            Debug.Log(hit.collider.name);
        }
        else if(Input.GetMouseButtonDown(0))
        {
            if (thisSelectedChip != null)
            {
                thisSelectedChip.ChipUnSelected();
                thisSelectedChip = null;
                HideGhostChip();
            }

        }
    }
    
    
    public bool IsEndRaundOneStep()
    {
        if (thisSelectedChip.ReturnScorer().Length > 0)
            return false;
        else
            return true;
    }

    public bool IsEndRaundTwoStep()
    {
        if (thisSelectedChip.player.numIsSteps > 0 & thisSelectedChip.player.numSteps < thisSelectedChip.player.maxSteps)
            return false;
        else
            return true;
    }
    public void HideGhostChip()
    {
        foreach (Cell cell in cells)
        {
            cell.GhostChip.gameObject.SetActive(false);
        }
        foreach (Cell cell in startCells)
        {
            cell.GhostChip.gameObject.SetActive(false);
        }
        foreach(Player player in players)
        {
            foreach(Cell cell in player.BaseCells)
            {
                cell.GhostChip.gameObject.SetActive(false);
            }
        }
        foreach (Cell cell in homeCells)
        {
            cell.GhostChip.gameObject.SetActive(false);
        }
    }

    public void DeleteCubes()
    {
        
        
        //CheckSix();
        Destroy(cubes[0].gameObject);
        Destroy(cubes[1].gameObject);
        if (!CheckChipsOnField() & !players[idThisPlayer].IsSixScores() & players[idThisPlayer].numIsSteps == 0)
        {
            NextPlayer();
            return;
        }
        else if (!CheckChipsOnField() & !players[idThisPlayer].IsSixScores() & players[idThisPlayer].numIsSteps != 0)
        {
            StepOne();
            return;
        }
        buttonStepTwo.SetActive(true);
    }

    public IEnumerator MoveChipFromTo(Transform chip, Transform from, Transform to, Chip _chip)
    {
        while(chip.position != to.position)
        {
            chip.position = Vector3.Lerp(from.position, to.position, tForLerp);
            tForLerp += .05f;
            yield return new WaitForSeconds(.001f);
        }
        _chip.isOnField = true;
        tForLerp = 0;
    }
    public void StepOne()
    {
        players[idThisPlayer].indexSixScore = 0;
        AllCubesSleep = 0;
        namePlayer.text = "Player #" + idThisPlayer;
        cubes = new Cube[2];
        for (int i = 0; i < 2; i++)
        {
            GameObject gO = Instantiate(cubePrefab);
            cubes[i] = gO.GetComponent<Cube>();
            cubes[i].idCube = i;
            cubes[i].scoreBoard = ScoresBoards[i];
            cubes[i].gameManager = this;
            gO.transform.position = spawnPointsCubes[i].position;
        }
        buttonStepOne.SetActive(false);
        if (players[idThisPlayer].numIsSteps > 0)
            players[idThisPlayer].numIsSteps--;
        players[idThisPlayer].numSteps++;
        //buttonStepTwo.SetActive(false);
    }

    public void NextPlayer()
    {
        players[idThisPlayer].CleanData();
        if (idThisPlayer < 3)
            idThisPlayer++;
        else
            idThisPlayer = 0;
        buttonStepOne.SetActive(true);
        buttonStepTwo.SetActive(false);
        AllCubesSleep = 0;
    }

    public void SkipStep()
    {
        if(players[idThisPlayer].numIsSteps > 0 & players[idThisPlayer].numSteps < players[idThisPlayer].maxSteps)
        {
            StepOne();
        }
        else
            NextPlayer();
    }

    bool CheckChipsOnField()
    {
        foreach(Chip chip in players[idThisPlayer].chips)
        {
            if (chip.isOnField)
                return true;
        }
        return false;
    }

    bool CheckPlayersScoreIsSixScore()
    {
        foreach (int score in players[idThisPlayer].Scores)
        {
            if (score == 6)
                return true;
        }
        return false;
    }
}