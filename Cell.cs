using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour {
    public bool isOccupied;//занята ли ячейка?
    public int idPlayer;//id игрока фишка которого расположена на этой клетке.
    public int idCell;
    public bool isStartCell;//стартовая ли ячейка?
    public bool isAngleCell;//угловая ли ячейка?
    public bool isBaseCell;//база?
    public bool isHomeCell;
    public bool rightCellHome;//фишка справа - дом
    public bool isFinalCell;//финальная ячейка?
    public Cell[] cellsAngle;
    public Cell home;
    public GhostChip GhostChip;
}