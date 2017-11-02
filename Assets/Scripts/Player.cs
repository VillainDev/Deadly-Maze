using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Item;


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

        //Items
        Item.Item SmallBulletPack = new Item.Item(Item.Item._ItemType.BulletPack);
        Item.Item MediumBulletPack = new Item.Item(Item.Item._ItemType.BulletPack);
        Item.Item LargeBulletPack = new Item.Item(Item.Item._ItemType.BulletPack);
        Item.Item MediumFAK = new Item.Item(Item.Item._ItemType.FirstAidKit);
        Item.Item BigFAK = new Item.Item(Item.Item._ItemType.FirstAidKit);
        Gun PlayerGun = new Gun(); //containing current type of gun and numbers of bullets in each gun (This PlayerGun can change its type)
                                   /*********************/

        #region methods used for player
        public Player()
        {
            HP = 100;
            process = 0;
            //x?,y?,z?
            //sceneName;
        }
        public bool HealthUp(string type)
        {
            if (type == "Medium")
            {
                if (MediumFAK.CurrentFirstAidKitTypeSize() > 0)
                {
                    HP += MediumFAK.CurrentFirstAidKitType();
                    MediumFAK.ChangeFirstAidKitTypeSize(MediumFAK.CurrentFirstAidKitTypeSize() - 1);
                    if (HP > 100)
                        HP = 100;
                    return true;
                }
                else
                    return false;
            }
            else if (type == "Big")
            {
                if (BigFAK.CurrentFirstAidKitTypeSize() > 0)
                {
                    HP += BigFAK.CurrentFirstAidKitType();
                    BigFAK.ChangeFirstAidKitTypeSize(BigFAK.CurrentFirstAidKitTypeSize() - 1);
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
        public void Damage(int dam)
        {
            HP -= dam;
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
                if (currentPack.CurrentBulletPackSize() > 0)
                {
                    PlayerGun.ChangeHandgunBullet(PlayerGun.HandgunBullet() + (int)currentPack.CurrentBulletPack());
                    currentPack.ChangeBulletPackSize(currentPack.CurrentBulletPackSize() - 1);
                    if (PlayerGun.HandgunBullet() > 40)
                        PlayerGun.ChangeHandgunBullet(40);
                    return true;
                }
                else
                    return false;
            }
            else if (type == "Shotgun")
            {
                if (currentPack.CurrentBulletPack() > 0)
                {
                    PlayerGun.ChangeShotgunBullet(PlayerGun.ShotgunBullet() + (int)currentPack.CurrentBulletPack());
                    currentPack.ChangeBulletPackSize(currentPack.CurrentBulletPackSize() - 1);
                    if (PlayerGun.ShotgunBullet() > 40)
                        PlayerGun.ChangeShotgunBullet(40);
                    return true;
                }
                else
                    return false;
            }
            else if (type == "AK")
            {
                if (currentPack.CurrentBulletPack() > 0)
                {
                    PlayerGun.ChangeAKBullet(PlayerGun.AKBullet() + (int)currentPack.CurrentBulletPack());
                    currentPack.ChangeBulletPackSize(currentPack.CurrentBulletPackSize() - 1);
                    if (PlayerGun.AKBullet() > 40)
                        PlayerGun.ChangeAKBullet(40);
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
            int newHP = CurrentMonster.GetHP() - PlayerGun.CurrentPower();
            CurrentMonster.SetHP(newHP);
        }
        public int GetHP()
        {
            return HP;
        }
        public void SetHP(int newHP)
        {
            HP = newHP;
        }
        #endregion
    }
}
