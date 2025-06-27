using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StockDataSO", menuName = "Scriptable Objects/StockDataSO")]
public class StockDataSO : ScriptableObject
{
    public string stockName;
    public float basePrice;
    public List<SectorSO> sectors;
}
