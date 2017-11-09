using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Item;
using Monster;


namespace Player
{
    [Serializable]
    public class Player : MonoBehaviour
    {

        ConsoleSetting Key; //User's Console Setting
        int HP; //min:0, max: 100
        int process; //min:0%, max:100%
        string sceneName; //current name of the scene
        //position must not be collided with enemy area
        float x, y, z;

        #region  backpacks for player
        Item.Item SmallBulletPack = new Item.Item(Item.Item._Item.BulletPack);
        Item.Item MediumBulletPack = new Item.Item(Item.Item._Item.BulletPack);
        Item.Item LargeBulletPack = new Item.Item(Item.Item._Item.BulletPack);
        Item.Item MediumFAK = new Item.Item(Item.Item._Item.FirstAidKit);
        Item.Item BigFAK = new Item.Item(Item.Item._Item.FirstAidKit);
        Item.Item Pole = new Item.Item(Item.Item._Item.Pole);
        Gun PlayerGun = new Gun(); //containing current type of gun and numbers of bullets in each gun (This PlayerGun can change its type)
                                   /*********************/
        #endregion
        #region contructor
        public Player()
        {
            HP = 100;
            process = 0;
            SmallBulletPack.SetBulletPack(Item.Item._BulletPack.small);
            MediumBulletPack.SetBulletPack(Item.Item._BulletPack.medium);
            LargeBulletPack.SetBulletPack(Item.Item._BulletPack.large);
            MediumFAK.SetFirstAidKit(Item.Item._FirstAidKit.medium);
            BigFAK.SetFirstAidKit(Item.Item._FirstAidKit.big);
            //x?,y?,z?
            //sceneName;
        }
        #endregion
        #region methods used for player
        public bool HealthUp(string type) //type == "Medium" or type == "Big"
        {
            if (type == "Medium")
            {
                if (MediumFAK.GetFirstAidKitSize() > 0)
                {
                    HP += MediumFAK.GetFirstAidKitSize();
                    MediumFAK.SetFirstAidKitSize(MediumFAK.GetFirstAidKitSize() - 1);
                    if (HP > 100)
                        HP = 100;
                    return true;
                }
                else
                    return false;
            }
            else if (type == "Big")
            {
                if (BigFAK.GetFirstAidKitSize() > 0)
                    {
                        HP += BigFAK.GetFirstAidKit();
                        BigFAK.SetFirstAidKitSize(BigFAK.GetFirstAidKitSize() - 1);
                        if (HP > 100)
                            HP = 100;
                        return true;
                    }
                else
                        return false;
            }
            else
            {
                Debug.Log("First aid kit type not recognized!");
                return false;
            }
        } //two type: "Big" and "Medium", return true if sucessfully increased health, false otherwise
        public void MonsterDamage(Monster.Monster currentMonster)
        {
            //TODO
        }
        public enum PlayerStatus { Healthy = 0, Normal = 1, Bad = 2 };
        public PlayerStatus ShowStatus()
        {
            if (HP >= 80)
                return PlayerStatus.Healthy;
            else if (HP >= 50)
                return PlayerStatus.Normal;
            else
                return PlayerStatus.Bad;

        } //show current player status
        public bool BulletUp(string type, string size)// 3 types: "Handgun","Shotgun" and "AK"; 3 size: "Small", "Medium","Large"
        {
            Item.Item currentPack;
            if (size == "Small")
                currentPack = SmallBulletPack;
            else if (size == "Medium")
                currentPack = MediumBulletPack;
            else if (size == "Large")
                currentPack = LargeBulletPack;
            else
            {
                Debug.LogError(size + " type of bullet packs does not exist!");
                return false;
            }
            if (type == "Handgun")
            {
                if (currentPack.GetBulletPackSize() > 0)
                {
                    PlayerGun.SetHGBullet(PlayerGun.GetHGBullet() + (int)currentPack.GetBulletPack());
                    currentPack.SetBulletPackSize(currentPack.GetBulletPackSize() - 1);
                    if (PlayerGun.GetHGBullet() > 40)
                        PlayerGun.SetHGBullet(40);
                    return true;
                }
                else
                    return false;
            }
            else if (type == "Shotgun")
            {
                if (currentPack.GetBulletPackSize() > 0)
                {
                    PlayerGun.SetSGBullet(PlayerGun.GetSGBullet() + (int)currentPack.GetBulletPack());
                    currentPack.SetBulletPackSize(currentPack.GetBulletPackSize() - 1);
                    if (PlayerGun.GetSGBullet() > 40)
                        PlayerGun.SetSGBullet(40);
                    return true;
                }
                else
                    return false;
            }
            else if (type == "AK")
            {
                if (currentPack.GetBulletPackSize() > 0)
                {
                    PlayerGun.SetAKBullet(PlayerGun.GetAKBullet() + (int)currentPack.GetBulletPack());
                    currentPack.SetBulletPackSize(currentPack.GetBulletPackSize() - 1);
                    if (PlayerGun.GetAKBullet() > 40)
                        PlayerGun.SetAKBullet(40);
                    return true;
                }
                else
                    return false;
            }
            else
            {
                Debug.LogError("Gun type does not exist!");
                return false;
            }
        }
        public void Shoot(ref Monster.Monster CurrentMonster)//3 types: "Handgun","Shotgun" and "AK". No need for type, since PlayerGun is the gun player is using.
        {
            int newHP = CurrentMonster.GetHP() - (int)PlayerGun.CurrentPower;
            CurrentMonster.SetHP(newHP);
        }
        public int GetHP()
        {
            return HP;
        } //return player's HP
        public void SetHP(int newHP)
        {
            HP = newHP;
        } //set player's HP
        public void AddPoletoMap()
        {
            Pole.SetPole(Pole.GetPole() - 1);
        }
        #endregion
    }
}
