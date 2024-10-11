using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "GLConfig", menuName = "Configs/GameLoops")]
public class GameLoopsConfig : ScriptableObject
{
    [Header("TickMachimesettings")]
    [SerializeField]
    private bool _tickMachineIsOn = false;
    [ShowIf("_tickMachineIsOn"), SerializeField, Tooltip("Tick interval in seconds.\n ")]
    private float _timeForTickPerSeconds = 1.0F;


    #region [ Getters ]
    
    public float timeForTickPerSeconds => _timeForTickPerSeconds;
    public bool tickMachineIsOn => _tickMachineIsOn;

    #endregion
}
