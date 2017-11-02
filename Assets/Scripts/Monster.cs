using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

namespace Monster
{
    public class Monster : MonoBehaviour
    {
        int Damage;
        int HP;
        string MonsterName;
        public Monster(string Name, int MonsterDamage, int MonsterHP)
        {
            MonsterName = Name;
            Damage = MonsterDamage;
            HP = MonsterHP;
        }
        public int GetHP()
        {
            return HP;
        }
        public void SetHP(int newHP)
        {
            HP = newHP;
        }
        public void Attack(ref Player.Player CurrentPlayer)
        {
            int newHP = CurrentPlayer.GetHP() - Damage;
            CurrentPlayer.SetHP(newHP);
        }

    }
}
