﻿using System.Collections.Generic;
using Mono.Data.Sqlite;

namespace MultiplayerARPG.MMO
{
    public partial class SQLiteDatabase
    {
        private void CreateCharacterItem(int idx, string characterId, InventoryType inventoryType, CharacterItem characterItem)
        {
            ExecuteNonQuery("INSERT INTO characteritem (id, idx, inventoryType, characterId, dataId, level, amount, durability, exp, lockRemainsDuration) VALUES (@id, @idx, @inventoryType, @characterId, @dataId, @level, @amount, @durability, @exp, @lockRemainsDuration)",
                new SqliteParameter("@id", characterId + "_" + (byte)inventoryType + "_" + idx),
                new SqliteParameter("@idx", idx),
                new SqliteParameter("@inventoryType", (byte)inventoryType),
                new SqliteParameter("@characterId", characterId),
                new SqliteParameter("@dataId", characterItem.dataId),
                new SqliteParameter("@level", characterItem.level),
                new SqliteParameter("@amount", characterItem.amount),
                new SqliteParameter("@durability", characterItem.durability),
                new SqliteParameter("@exp", characterItem.exp),
                new SqliteParameter("@lockRemainsDuration", characterItem.lockRemainsDuration));
        }

        private bool ReadCharacterItem(SQLiteRowsReader reader, out CharacterItem result, bool resetReader = true)
        {
            if (resetReader)
                reader.ResetReader();

            if (reader.Read())
            {
                result = new CharacterItem();
                result.dataId = reader.GetInt32("dataId");
                result.level = (short)reader.GetInt32("level");
                result.amount = (short)reader.GetInt32("amount");
                result.durability = reader.GetFloat("durability");
                result.exp = reader.GetInt32("exp");
                result.lockRemainsDuration = reader.GetFloat("lockRemainsDuration");
                return true;
            }
            result = CharacterItem.Empty;
            return false;
        }

        private List<CharacterItem> ReadCharacterItems(string characterId, InventoryType inventoryType)
        {
            var result = new List<CharacterItem>();
            var reader = ExecuteReader("SELECT * FROM characteritem WHERE characterId=@characterId AND inventoryType=@inventoryType ORDER BY idx ASC",
                new SqliteParameter("@characterId", characterId),
                new SqliteParameter("@inventoryType", inventoryType));
            CharacterItem tempInventory;
            while (ReadCharacterItem(reader, out tempInventory, false))
            {
                result.Add(tempInventory);
            }
            return result;
        }

        public EquipWeapons ReadCharacterEquipWeapons(string characterId)
        {
            var result = new EquipWeapons();
            // Right hand weapon
            var reader = ExecuteReader("SELECT * FROM characteritem WHERE characterId=@characterId AND inventoryType=@inventoryType LIMIT 1",
                new SqliteParameter("@characterId", characterId),
                new SqliteParameter("@inventoryType", InventoryType.EquipWeaponRight));
            CharacterItem rightWeapon;
            if (ReadCharacterItem(reader, out rightWeapon))
                result.rightHand = rightWeapon;
            // Left hand weapon
            reader = ExecuteReader("SELECT * FROM characteritem WHERE characterId=@characterId AND inventoryType=@inventoryType LIMIT 1",
                new SqliteParameter("@characterId", characterId),
                new SqliteParameter("@inventoryType", InventoryType.EquipWeaponLeft));
            CharacterItem leftWeapon;
            if (ReadCharacterItem(reader, out leftWeapon))
                result.leftHand = leftWeapon;
            return result;
        }

        public void CreateCharacterEquipWeapons(string characterId, EquipWeapons equipWeapons)
        {
            CreateCharacterItem(0, characterId, InventoryType.EquipWeaponRight, equipWeapons.rightHand);
            CreateCharacterItem(0, characterId, InventoryType.EquipWeaponLeft, equipWeapons.leftHand);
        }

        public void CreateCharacterEquipItem(int idx, string characterId, CharacterItem characterItem)
        {
            CreateCharacterItem(idx, characterId, InventoryType.EquipItems, characterItem);
        }

        public List<CharacterItem> ReadCharacterEquipItems(string characterId)
        {
            return ReadCharacterItems(characterId, InventoryType.EquipItems);
        }

        public void CreateCharacterNonEquipItem(int idx, string characterId, CharacterItem characterItem)
        {
            CreateCharacterItem(idx, characterId, InventoryType.NonEquipItems, characterItem);
        }

        public List<CharacterItem> ReadCharacterNonEquipItems(string characterId)
        {
            return ReadCharacterItems(characterId, InventoryType.NonEquipItems);
        }

        public void DeleteCharacterItems(string characterId)
        {
            ExecuteNonQuery("DELETE FROM characteritem WHERE characterId=@characterId", new SqliteParameter("@characterId", characterId));
        }
    }
}
