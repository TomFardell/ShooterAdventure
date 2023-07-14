using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Scriptable Enemy", menuName = "Scriptable Enemy")]
public class ScriptableEnemy : ScriptableObject {
    [SerializeField] private EnemyData _data;
    public EnemyData Data => _data;
}

[Serializable]
public struct EnemyData {
    public float MoveSpeed;
    public int Damage;
    public int MaxHealth;

    public Color DefaultColor;
    public Color HitColor;
}
