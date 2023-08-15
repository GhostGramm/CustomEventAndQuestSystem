using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestSO", menuName = "ScriptableObjects/QuestSO", order = 1)]
public class QuestSO : ScriptableObject
{
    [field: SerializeField] public string id;

    [Header("General")]
    public string displayName;

    [Header("Requirements")]
    public int levelRequirement;

    public QuestSO[] questPrerequisites;

    [Header("Steps")]
    public GameObject[] questStepPrefabs;

    [Header("Rewards")]
    public int reward;

    private void OnValidate()
    {
        #if UNITY_EDITOR
                id = this.name;
                UnityEditor.EditorUtility.SetDirty(this);
        #endif
    }

}