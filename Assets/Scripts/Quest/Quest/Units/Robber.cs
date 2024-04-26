using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region RobberEnums
public enum RobberType
{
    Mountain = 1,
    Sea,
}
public enum ArmingExtent
{
    Strong = 1,
    Normal,
    Weak,
}
#endregion

public class Robber : MonoBehaviour
{
    [SerializeField] int robberId;
    [SerializeField] RobberType type;
    [SerializeField] int number;
    [SerializeField] ArmingExtent armingExtent;
    [SerializeField] Province province;
    [Space(10)]
    [SerializeField] string robberName;
    
    #region Properties
    public int RobberId => robberId;
    public RobberType Type => type;
    public int Number => number;
    public ArmingExtent ArmingExtent => armingExtent;
    public Province Province => province;
    public string RobberName { get { return robberName; } set { robberName = value; } }
    #endregion

    public void Setup(RobberUnit robberUnit)
    {
        robberId = robberUnit.RobberID;
        type = (RobberType)robberUnit.Type;
        number = robberUnit.Number;
        armingExtent = (ArmingExtent)robberUnit.Arming;
        province = (Province)robberUnit.Province;
    }

}
