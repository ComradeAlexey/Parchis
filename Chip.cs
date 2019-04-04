using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Chip : MonoBehaviour {
    public int idPlayer;//ай ди игрока которому принадлежит фишка
    public int indexChip;//индекс фишки
    public int indexCell;//индекс ячейки на которой мы торчим
    public int indexEnemyPlayer;//индекс игрока у которого фишка в плену
    public Material materialChip;
    public Material materialSelectedChip;
    public MeshRenderer meshRenderer;
    public Transform baseCoordinates;
    public bool isOnField = false;//на поле?
    public bool isOnFinalCell;//на финальной ячейке?
    public bool isOnEnemyBase;//в плену?
    public bool isXorZAxesSpawnBase;//в какую сторону передвигаются фишки при спавне на базе, фолс по Х, тру по Z
    public bool isMinusOrPlusAxes;
    public EventSystem eventSystem;
    public Player player;
    public GameManager gameManager;

    public Cell thisCell;
    public void CreateChip()
    {
        if (!isXorZAxesSpawnBase)
            transform.position = baseCoordinates.position + new Vector3((isMinusOrPlusAxes ? -0.5f : 0.5f) * indexChip, 0, 0);
        else
            transform.position = baseCoordinates.position + new Vector3(0, 0, (isMinusOrPlusAxes ? -0.5f : 0.5f) * indexChip);
        gameObject.SetActive(true);
    }
    public void ChipSelected()
    {
        EditMaterial();
        ShowGhostCells();
        
    }
    bool CheckThisAngleCellForMove(int[] Scores, Cell cellPlayer, Cell enemyCell)
    {
        foreach (int scr in Scores)
        {
            if (enemyCell != null)
            {
                for (int i = 0; i < 2; i++)
                {
                    if (cellPlayer.cellsAngle[i].idCell == enemyCell.idCell)
                    {
                        switch (scr)
                        {
                            case 1:
                                return true;
                            case 3:
                                return true;
                        }
                    }

                }

            }

            else if (thisCell == null)
            {
                return false;
            }
        }
        return false;
    }

    void DeleteScore(Cell cell)
    {
        for (int i = 0; i < player.Scores.Length; i++)
        {
            if (cell.isFinalCell)
            {
                player.DeleteScore(i);
                return;
            }
            else
            { 
            if (cell.idCell - (player.Scores[i] + indexCell) == 0 & !cell.isStartCell)
            {
                player.DeleteScore(i);
                return;
            }
            else if (player.Scores[i] + indexCell >= gameManager.cells.Length)
            {
                if (cell.idCell - (player.Scores[i] + indexCell - gameManager.cells.Length) == 0 & !cell.isStartCell)
                {
                    player.DeleteScore(i);
                    return;
                }
            }
            else if (cell.idCell - player.Scores[i] == 0 & !cell.isStartCell)
            {
                player.DeleteScore(i);
                return;
            }
            else if (thisCell != null & cell.isAngleCell)
            {
                if (thisCell.isAngleCell)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        if (thisCell.cellsAngle[j] == cell)
                        {
                            if (player.Scores[i] == 1 & j == 0)
                            {
                                player.DeleteScore(i);
                                return;
                            }
                            else if (player.Scores[i] == 3 & j == 1)
                            {
                                player.DeleteScore(i);
                                return;
                            }
                        }
                    }
                }
            }
            else if (player.Scores[i] == 6 & cell.isStartCell)
            {
                player.DeleteScore(i);
                return;
            }
            else if (player.Scores[i] == 6 & isOnEnemyBase)
            {
                player.DeleteScore(i);
                gameManager.players[indexEnemyPlayer].AddScore(6);
                return;
            }
            else if ((cell.idCell + 1) - (player.Scores[i] + indexCell) == 0 & !cell.isStartCell & cell.isHomeCell)
            {
                player.DeleteScore(i);
                return;
            }
        }
        }
    }

    public bool MoveToGhostChip(Cell cell)
    {
        if (!player.IsSixScores() & !isOnField)
            return false;
        if (thisCell != null)
        {
            thisCell.isOccupied = false;
            thisCell.idPlayer = -1;
        }
        
        gameManager.HideGhostChip();
        DeleteScore(cell);
        if(cell.isFinalCell)
        {
            isOnFinalCell = true;
            isOnField = false;
        }
        if(isOnEnemyBase)
        {
            isOnEnemyBase = false;
        }
        else if(!isOnField & !isOnFinalCell)
        {
            isOnField = true;
        }
        thisCell = cell;
        indexCell = thisCell.idCell;
        transform.position = thisCell.GhostChip.transform.position;
        thisCell.GhostChip.transform.gameObject.SetActive(false);
        thisCell.isOccupied = true;
        thisCell.idPlayer = idPlayer;
        return true;
    }

    public bool MoveToEnemyBase(Chip enemyChip, Chip playerChip, Cell cell)
    {
        if (!isOnField)
            return false;
        if (playerChip.thisCell.isAngleCell & enemyChip.thisCell.isAngleCell)
        {
            if (!CheckThisAngleCellForMove(playerChip.player.Scores, playerChip.thisCell, enemyChip.thisCell))
                return false;
        }
        foreach (Player.EnemyCoordBase eCB in gameManager.players[playerChip.idPlayer].enemyCoordBases)
        {
            if (eCB.indexPlayer == enemyChip.idPlayer)
            {
                if (thisCell != null)
                {
                    thisCell.isOccupied = false;
                }
                if (!gameManager.players[playerChip.idPlayer].chips[0].isXorZAxesSpawnBase)
                    transform.position = eCB.coord.position + new Vector3((gameManager.players[playerChip.idPlayer].chips[0].isMinusOrPlusAxes ? -0.5f : 0.5f) * indexChip, 0, 0);
                else
                    transform.position = eCB.coord.position + new Vector3(0, 0, (gameManager.players[playerChip.idPlayer].chips[0].isMinusOrPlusAxes ? -0.5f : 0.5f) * indexChip);
                isOnEnemyBase = true;
                isOnField = false;
                thisCell = null;
                indexCell = 0;
                indexEnemyPlayer = playerChip.idPlayer;
                playerChip.MoveToGhostChip(cell);
                return true;
            }
        }
        return false;
    }

    void EditMaterial()
    {
        materialSelectedChip.color = materialChip.color;
        meshRenderer.material = materialSelectedChip;
    }

    void ShowGhostCells()
    {
        switch (CheckOnIsField())
        {
            case true:
                {
                    switch (CheckOnIsAngleCell())
                    {
                        case true:
                            for (int i = 0; i < ReturnScorer().Length; i++)
                            {
                                switch (ReturnScorer()[i])
                                {
                                    case 1:
                                        thisCell.cellsAngle[0].GhostChip.gameObject.SetActive(true);
                                        break;
                                    case 3:
                                        thisCell.cellsAngle[1].GhostChip.gameObject.SetActive(true);
                                        break;
                                }
                                int sum = ReturnScorer()[i] + indexCell;

                                if (sum > gameManager.cells.Length)
                                    sum -= gameManager.cells.Length;
                                int sumChip = 0;
                                if (ReturnScorer()[i] + indexCell > gameManager.cells.Length)
                                {
                                    for (int j = indexCell; j <= gameManager.cells.Length - 1; j++)
                                    {
                                        if (gameManager.cells[j].idPlayer != -1)
                                        {
                                            sumChip++;
                                        }
                                    }
                                    for (int j = 0; j < sum - 1; j++)
                                    {
                                        if (gameManager.cells[j].idPlayer != -1)
                                        {
                                            sumChip++;
                                        }

                                    }
                                }
                                else
                                {
                                    for (int j = indexCell; j < sum - 1; j++)
                                    {
                                        if (gameManager.cells[j].idPlayer != -1)
                                        {
                                            sumChip++;
                                        }

                                    }
                                }
                                if (sumChip == 0)
                                {
                                    if (gameManager.cells[sum - 1].rightCellHome)
                                    {
                                        if (!gameManager.cells[sum - 1].home.isOccupied)
                                            gameManager.cells[sum - 1].home.GhostChip.gameObject.SetActive(true);
                                    }
                                    gameManager.cells[sum - 1].GhostChip.gameObject.SetActive(true);
                                }
                            }
                            break;
                        case false:
                            for (int i = 0; i < ReturnScorer().Length; i++)
                            {
                                int sum = ReturnScorer()[i] + indexCell;
                                if (sum > gameManager.cells.Length)
                                    sum -= gameManager.cells.Length;
                                int sumChip = 0;
                                if (ReturnScorer()[i] + indexCell > gameManager.cells.Length)
                                {
                                    for (int j = indexCell; j <= gameManager.cells.Length - 1; j++)
                                    {
                                        if (gameManager.cells[j].idPlayer != -1)
                                        {
                                            sumChip++;
                                        }
                                    }
                                    for (int j = 0; j < sum - 1; j++)
                                    {
                                        if (gameManager.cells[j].idPlayer != -1)
                                        {
                                            sumChip++;
                                        }

                                    }
                                }
                                else
                                {
                                    for (int j = indexCell; j < sum - 1; j++)
                                    {
                                        if (gameManager.cells[j].idPlayer != -1)
                                        {
                                            sumChip++;
                                        }

                                    }
                                }
                                if (sumChip == 0)
                                {   if (indexCell < player.idCellToFinalCell & ReturnScorer()[i] + indexCell >= player.idCellToFinalCell)
                                    {
                                        player.finalCells[indexChip].GhostChip.gameObject.SetActive(true);
                                    }
                                    else
                                    {
                                        if(gameManager.cells[sum-1].rightCellHome)
                                        {
                                            if (!gameManager.cells[sum - 1].home.isOccupied)
                                                gameManager.cells[sum - 1].home.GhostChip.gameObject.SetActive(true);
                                        }
                                        gameManager.cells[sum - 1].GhostChip.gameObject.SetActive(true);
                                    }
                                }
                            }
                            break;
                    }

                }
                break;
            case false:
                {
                            if (player.IsSixScores() & !isOnEnemyBase)
                                player.StartCell.GhostChip.transform.gameObject.SetActive(true);
                            else if (player.IsSixScores() & isOnEnemyBase)
                                player.BaseCells[indexChip].GhostChip.transform.gameObject.SetActive(true);
                }
                break;
        }
    }

    bool CheckOnIsField()
    {
        return isOnField;
    }

    bool CheckOnIsEnemyBase()
    {
        return isOnEnemyBase;
    }

    bool CheckOnIsAngleCell()
    {
        return thisCell.isAngleCell;
    }

    bool IsOccupiedCell(Cell cell)
    {
        if (cell.isOccupied)
            return true;
        else
            return false;
    }

    bool CheckScores()
    {
        if(player.Scores != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public int[] ReturnScorer()
    {
        return player.Scores;
    }

    public void ChipUnSelected()
    {
        meshRenderer.material = materialChip;
        int numIsFinal = 0;
        foreach (var item in player.chips)
        {
            if (item.isOnFinalCell)
                numIsFinal++;
        }
        if(numIsFinal == 5)
        {
            gameManager.winPanel.SetActive(true);
            gameManager.winText.text = "" + player.NamePlayer;
        }
    }

    
}