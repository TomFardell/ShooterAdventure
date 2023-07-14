using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Scriptable Player", menuName = "Scriptable Player")]
public class ScriptablePlayer : ScriptableObject {
    [SerializeField] private PlayerData _data;
    public PlayerData Data => _data;
}

[Serializable]
public struct PlayerData {
    public float MoveSpeed;

    public float RollCooldown;
    public float RollDuration;
    public float RollSpeed;

    public float BulletSpeed;
    public int BulletDamage;
    public float ShootInterval;
    public float WeaponRecoil;

    public int MaxHealth;
    public float TimeInvincibleAfterHit;

    public Color DefaultColor;
    public Color RollColor;
    public Color RollReadyColor;
    public Color InvincibleColor;
}
