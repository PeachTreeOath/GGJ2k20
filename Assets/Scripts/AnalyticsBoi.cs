using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnalyticsBoi : MonoBehaviour
{
    List<float> historicalDamageTaken = new List<float>();
    
    public void recordDamageDealt(float amount)
    {
        historicalDamageTaken.Add(amount);    
    }
}
