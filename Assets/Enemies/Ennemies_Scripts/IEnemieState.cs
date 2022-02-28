using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemieState 
{
    int CalculateDamage(int attackPower);
    int Calculate_DamageReceive(int attackPower);
    int CalculatePath(bool isChassing);

}
