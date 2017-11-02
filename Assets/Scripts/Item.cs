using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Item
{
    public class Gun : MonoBehaviour
    {
        public enum _GunType : int { Handgun = 0, Shotgun = 1, AK = 2 };
        public enum _GunPower : int { Weak = 5, Average = 20, Strong = 40 };
        #region properties

        _GunType GunType;
        _GunPower GunPower;
        _GunType currentGun
        {
            get
            {
                return GunType;
            }
            set
            {
                GunType = value;
                if (value == _GunType.Handgun)
                    GunPower = _GunPower.Weak;
                else if (value == _GunType.Shotgun)
                    GunPower = _GunPower.Average;
                else if (value == _GunType.AK)
                    GunPower = _GunPower.Strong;
            }
        }
        _GunPower currentPower
        {
            get
            {
                return GunPower;
            }
        }
        int NoOfHandgunBullet;
        int NoOfShotgunBullet;
        int NoOfAKBullet;
        /***************************/
        #endregion
        #region Method for struct Gun
        public void ChangeHandgunBullet(int bullet)
        {
            NoOfHandgunBullet = bullet;
        }
        public void ChangeShotgunBullet(int bullet)
        {
            NoOfShotgunBullet = bullet;
        }
        public void ChangeAKBullet(int bullet)
        {
            NoOfAKBullet = bullet;
        }
        public int HandgunBullet()
        {
            return NoOfHandgunBullet;
        }
        public int ShotgunBullet()
        {
            return NoOfShotgunBullet;
        }
        public int AKBullet()
        {
            return NoOfAKBullet;
        }
        public void ChangeGun(_GunType newGunType)
        {
            currentGun = newGunType;
        }
        public string CurrentType()
        {
            if (GunType == _GunType.Handgun)
                return "Handgun";
            else if (GunType == _GunType.Shotgun)
                return "Shotgun";
            else
                return "AK";
        }
        public _GunType CurrentTypeInEnum()
        {
            return GunType;
        }
        public _GunPower CurrentPowerInEnum()
        {
            return currentPower;
        }
        public int CurrentPower()
        {
            return (int)currentPower;
        }
        #endregion
    }
    public class Item : MonoBehaviour
    {
        const int MAX_BULLET = 40;
        #region THE ONLY THING THAT YOU NEED TO KNOW IN THIS PART IS ENUM
        /******************/

        //BulletPack
        public enum _BulletPackType: int { small = 10, medium = 20, large = 40};
        _BulletPackType BulletPackType;
        int BPSize;
        /*****************/

        //First-aids kits
        public enum _FirstAidKitType: int { medium = 20, big = 50};
        _FirstAidKitType FirstAidKitType;
        int FAKSize;
        /****************/

        //item state
        public enum _ItemType : int { Gun = 0, BulletPack = 1, FirstAidKit = 2, Treasure = 3};
        bool[] ItemType = { false, false, false, false }; //Determine existence of an item in Object Item
        /****************/

        //treasure
        const int TreasureSize = 1;
        /****************/

        //constructor
        public Item(params _ItemType[] ItemTypeList)
        {
            for (int i = 0; i < ItemTypeList.Length; i++)
                ItemType[(int)ItemTypeList[i]] = true;
            if (ItemType[(int)_ItemType.Gun])
            {
                G.ChangeHandgunBullet(MAX_BULLET);
                G.ChangeShotgunBullet(MAX_BULLET);
                G.ChangeAKBullet(MAX_BULLET);
            }
            FAKSize = 0;
            BPSize = 0;
        }
        /****************/
        #endregion

        #region  Methods for status of items
        public static string ItemName(_ItemType i) //return item name
        {
            if (i == _ItemType.Gun)
                return "Gun";
            else if (i == _ItemType.BulletPack)
                return "Bullet Pack";
            else if (i == _ItemType.FirstAidKit)
                return "First-Aids Kit";
            else
                return "Treasure";
        }
        public bool CheckType(_ItemType i) //check the existence of an item
        {
            return ItemType[(int)i];
        }
        public void AddItem(_ItemType i) // add an item
        {
            if(ItemType[(int)i])
                Debug.LogWarning("There already exists an item named " + ItemName(i) + "!");
            else
                ItemType[(int)i] = true;
        }
        public void RemoveItem(_ItemType i) //remove an item
        {
            if (!ItemType[(int)i])
                Debug.LogWarning("There is no item named " + ItemName(i) + " exists in Object Item!");
            else
                ItemType[(int)i] = false;
        }
        #endregion
        #region Methods for Bullet Pack
        public void ChangeBulletPack(_BulletPackType i)//Change the type of bullet pack
        {
            if (ItemType[(int)_ItemType.BulletPack])
                BulletPackType = i;
            else
                Debug.LogError("Bullet Pack does not exist in current Object Item!");
        }
        public int CurrentBulletPack()//bullets added in gun
        {
            return (int)BulletPackType;
        }
        public void ChangeBulletPackSize(int size)//Change the number of bullet packs
        {
            if (ItemType[(int)_ItemType.BulletPack])
                BPSize = size;
            else
                Debug.LogError("Bullet Pack does not exist in current Object Item!");
            
        }
        public int CurrentBulletPackSize()
        {
            if (ItemType[(int)_ItemType.BulletPack])
                return BPSize;
            else
            {
                Debug.LogError("Bullet Pack does not exist in current Object Item!");
                return 0;
            }
            
        }
        #endregion
        #region Methods for First Aid Kit
        public void ChangeFirstAidKitType(_FirstAidKitType i)//Change the type of first aid kit
        {
            if (ItemType[(int)_ItemType.FirstAidKit])
                FirstAidKitType = i;
            else
                Debug.LogError("First Aid Kit does not exist in current Object Item!");
        }
        public int CurrentFirstAidKitType()
        {
            return (int)FirstAidKitType;
        } //energy recover from FAK
        public void ChangeFirstAidKitTypeSize(int size)
        {
            if(ItemType[(int)_ItemType.FirstAidKit])
                FAKSize = size;
            else
                Debug.LogError("First Aid Kit does not exist in current Object Item!");
        } //change current FAK type size
        public int CurrentFirstAidKitTypeSize()//show current first aid kit type size
        {
            if (ItemType[(int)_ItemType.FirstAidKit])
                return FAKSize;
            else
            {
                Debug.LogError("First Aid Kit does not exist in current Object Item!");
                return 0;
            }
            
        }
        #endregion
        //no methods for treasure!
    }
}
