using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICell{
    bool GetIsOccupied();
    void SetIsOccupied(bool value);
}
