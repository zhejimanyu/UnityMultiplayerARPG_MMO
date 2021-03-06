﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MySql.Data.MySqlClient;

namespace MultiplayerARPG.MMO
{
    public partial class MySQLDatabase
    {
        private void FillCharacterAttributes(IPlayerCharacterData characterData)
        {
            var connection = NewConnection();
            connection.Open();
            var transaction = connection.BeginTransaction();
            try
            {
                DeleteCharacterAttributes(connection, transaction, characterData.Id);
                var i = 0;
                foreach (var attribute in characterData.Attributes)
                {
                    CreateCharacterAttribute(connection, transaction, i++, characterData.Id, attribute);
                }
                transaction.Commit();
            }
            catch (System.Exception ex)
            {
                Debug.LogError("Transaction, Error occurs while replacing attributes of character: " + characterData.Id);
                Debug.LogException(ex);
                transaction.Rollback();
            }
            transaction.Dispose();
            connection.Close();
        }

        private void FillCharacterBuffs(IPlayerCharacterData characterData)
        {
            var connection = NewConnection();
            connection.Open();
            var transaction = connection.BeginTransaction();
            try
            {
                DeleteCharacterBuffs(connection, transaction, characterData.Id);
                foreach (var buff in characterData.Buffs)
                {
                    CreateCharacterBuff(connection, transaction, characterData.Id, buff);
                }
                transaction.Commit();
            }
            catch (System.Exception ex)
            {
                Debug.LogError("Transaction, Error occurs while replacing buffs of character: " + characterData.Id);
                Debug.LogException(ex);
                transaction.Rollback();
            }
            transaction.Dispose();
            connection.Close();
        }

        private void FillCharacterHotkeys(IPlayerCharacterData characterData)
        {
            var connection = NewConnection();
            connection.Open();
            var transaction = connection.BeginTransaction();
            try
            {
                DeleteCharacterHotkeys(connection, transaction, characterData.Id);
                foreach (var hotkey in characterData.Hotkeys)
                {
                    CreateCharacterHotkey(connection, transaction, characterData.Id, hotkey);
                }
                transaction.Commit();
            }
            catch (System.Exception ex)
            {
                Debug.LogError("Transaction, Error occurs while replacing hotkeys of character: " + characterData.Id);
                Debug.LogException(ex);
                transaction.Rollback();
            }
            transaction.Dispose();
            connection.Close();
        }

        private void FillCharacterItems(IPlayerCharacterData characterData)
        {
            var connection = NewConnection();
            connection.Open();
            var transaction = connection.BeginTransaction();
            try
            {
                DeleteCharacterItems(connection, transaction, characterData.Id);
                CreateCharacterEquipWeapons(connection, transaction, characterData.Id, characterData.EquipWeapons);
                var i = 0;
                foreach (var equipItem in characterData.EquipItems)
                {
                    CreateCharacterEquipItem(connection, transaction, i++, characterData.Id, equipItem);
                }
                i = 0;
                foreach (var nonEquipItem in characterData.NonEquipItems)
                {
                    CreateCharacterNonEquipItem(connection, transaction, i++, characterData.Id, nonEquipItem);
                }
                transaction.Commit();
            }
            catch (System.Exception ex)
            {
                Debug.LogError("Transaction, Error occurs while replacing items of character: " + characterData.Id);
                Debug.LogException(ex);
                transaction.Rollback();
            }
            transaction.Dispose();
            connection.Close();
        }

        private void FillCharacterQuests(IPlayerCharacterData characterData)
        {
            var connection = NewConnection();
            connection.Open();
            var transaction = connection.BeginTransaction();
            try
            {
                DeleteCharacterQuests(connection, transaction, characterData.Id);
                var i = 0;
                foreach (var quest in characterData.Quests)
                {
                    CreateCharacterQuest(connection, transaction, i++, characterData.Id, quest);
                }
                transaction.Commit();
            }
            catch (System.Exception ex)
            {
                Debug.LogError("Transaction, Error occurs while replacing quests of character: " + characterData.Id);
                Debug.LogException(ex);
                transaction.Rollback();
            }
            transaction.Dispose();
            connection.Close();
        }

        private void FillCharacterSkills(IPlayerCharacterData characterData)
        {
            var connection = NewConnection();
            connection.Open();
            var transaction = connection.BeginTransaction();
            try
            {
                DeleteCharacterSkills(connection, transaction, characterData.Id);
                var i = 0;
                foreach (var skill in characterData.Skills)
                {
                    CreateCharacterSkill(connection, transaction, i++, characterData.Id, skill);
                }
                transaction.Commit();
            }
            catch (System.Exception ex)
            {
                Debug.LogError("Transaction, Error occurs while replacing skills of character: " + characterData.Id);
                Debug.LogException(ex);
                transaction.Rollback();
            }
            transaction.Dispose();
            connection.Close();
        }

        private void FillCharacterSkillUsages(IPlayerCharacterData characterData)
        {
            var connection = NewConnection();
            connection.Open();
            var transaction = connection.BeginTransaction();
            try
            {
                DeleteCharacterSkillUsages(connection, transaction, characterData.Id);
                foreach (var skillUsage in characterData.SkillUsages)
                {
                    CreateCharacterSkillUsage(connection, transaction, characterData.Id, skillUsage);
                }
                transaction.Commit();
            }
            catch (System.Exception ex)
            {
                Debug.LogError("Transaction, Error occurs while replacing skill usages of character: " + characterData.Id);
                Debug.LogException(ex);
                transaction.Rollback();
            }
            transaction.Dispose();
            connection.Close();
        }

        private void FillCharacterSummons(IPlayerCharacterData characterData)
        {
            var connection = NewConnection();
            connection.Open();
            var transaction = connection.BeginTransaction();
            try
            {
                DeleteCharacterSummons(connection, transaction, characterData.Id);
                foreach (var skillUsage in characterData.Summons)
                {
                    CreateCharacterSummon(connection, transaction, characterData.Id, skillUsage);
                }
                transaction.Commit();
            }
            catch (System.Exception ex)
            {
                Debug.LogError("Transaction, Error occurs while replacing skill usages of character: " + characterData.Id);
                Debug.LogException(ex);
                transaction.Rollback();
            }
            transaction.Dispose();
            connection.Close();
        }

        private void FillCharacterRelatesData(IPlayerCharacterData characterData)
        {
            FillCharacterAttributes(characterData);
            FillCharacterBuffs(characterData);
            FillCharacterHotkeys(characterData);
            FillCharacterItems(characterData);
            FillCharacterQuests(characterData);
            FillCharacterSkills(characterData);
            FillCharacterSkillUsages(characterData);
            FillCharacterSummons(characterData);
        }

        public override void CreateCharacter(string userId, IPlayerCharacterData characterData)
        {
            ExecuteNonQuery("INSERT INTO characters " +
                "(id, userId, dataId, entityId, characterName, level, exp, currentHp, currentMp, currentStamina, currentFood, currentWater, statPoint, skillPoint, gold, currentMapName, currentPositionX, currentPositionY, currentPositionZ, respawnMapName, respawnPositionX, respawnPositionY, respawnPositionZ) VALUES " +
                "(@id, @userId, @dataId, @entityId, @characterName, @level, @exp, @currentHp, @currentMp, @currentStamina, @currentFood, @currentWater, @statPoint, @skillPoint, @gold, @currentMapName, @currentPositionX, @currentPositionY, @currentPositionZ, @respawnMapName, @respawnPositionX, @respawnPositionY, @respawnPositionZ)",
                new MySqlParameter("@id", characterData.Id),
                new MySqlParameter("@userId", userId),
                new MySqlParameter("@dataId", characterData.DataId),
                new MySqlParameter("@entityId", characterData.EntityId),
                new MySqlParameter("@characterName", characterData.CharacterName),
                new MySqlParameter("@level", characterData.Level),
                new MySqlParameter("@exp", characterData.Exp),
                new MySqlParameter("@currentHp", characterData.CurrentHp),
                new MySqlParameter("@currentMp", characterData.CurrentMp),
                new MySqlParameter("@currentStamina", characterData.CurrentStamina),
                new MySqlParameter("@currentFood", characterData.CurrentFood),
                new MySqlParameter("@currentWater", characterData.CurrentWater),
                new MySqlParameter("@statPoint", characterData.StatPoint),
                new MySqlParameter("@skillPoint", characterData.SkillPoint),
                new MySqlParameter("@gold", characterData.Gold),
                new MySqlParameter("@currentMapName", characterData.CurrentMapName),
                new MySqlParameter("@currentPositionX", characterData.CurrentPosition.x),
                new MySqlParameter("@currentPositionY", characterData.CurrentPosition.y),
                new MySqlParameter("@currentPositionZ", characterData.CurrentPosition.z),
                new MySqlParameter("@respawnMapName", characterData.RespawnMapName),
                new MySqlParameter("@respawnPositionX", characterData.RespawnPosition.x),
                new MySqlParameter("@respawnPositionY", characterData.RespawnPosition.y),
                new MySqlParameter("@respawnPositionZ", characterData.RespawnPosition.z));
            FillCharacterRelatesData(characterData);
            this.InvokeInstanceDevExtMethods("CreateCharacter", userId, characterData);
        }

        private bool ReadCharacter(MySQLRowsReader reader, out PlayerCharacterData result, bool resetReader = true)
        {
            if (resetReader)
                reader.ResetReader();

            if (reader.Read())
            {
                result = new PlayerCharacterData();
                result.Id = reader.GetString("id");
                result.DataId = reader.GetInt32("dataId");
                result.EntityId = reader.GetInt32("entityId");
                result.CharacterName = reader.GetString("characterName");
                result.Level = (short)reader.GetInt32("level");
                result.Exp = reader.GetInt32("exp");
                result.CurrentHp = reader.GetInt32("currentHp");
                result.CurrentMp = reader.GetInt32("currentMp");
                result.CurrentStamina = reader.GetInt32("currentStamina");
                result.CurrentFood = reader.GetInt32("currentFood");
                result.CurrentWater = reader.GetInt32("currentWater");
                result.StatPoint = (short)reader.GetInt32("statPoint");
                result.SkillPoint = (short)reader.GetInt32("skillPoint");
                result.Gold = reader.GetInt32("gold");
                result.PartyId = reader.GetInt32("partyId");
                result.GuildId = reader.GetInt32("guildId");
                result.GuildRole = (byte)reader.GetInt32("guildRole");
                result.SharedGuildExp = reader.GetInt32("sharedGuildExp");
                result.CurrentMapName = reader.GetString("currentMapName");
                result.CurrentPosition = new Vector3(reader.GetFloat("currentPositionX"), reader.GetFloat("currentPositionY"), reader.GetFloat("currentPositionZ"));
                result.RespawnMapName = reader.GetString("respawnMapName");
                result.RespawnPosition = new Vector3(reader.GetFloat("respawnPositionX"), reader.GetFloat("respawnPositionY"), reader.GetFloat("respawnPositionZ"));
                result.LastUpdate = (int)(reader.GetDateTime("updateAt").Ticks / System.TimeSpan.TicksPerMillisecond);
                return true;
            }
            result = null;
            return false;
        }

        public override PlayerCharacterData ReadCharacter(
            string userId,
            string id,
            bool withEquipWeapons = true,
            bool withAttributes = true,
            bool withSkills = true,
            bool withSkillUsages = true,
            bool withBuffs = true,
            bool withEquipItems = true,
            bool withNonEquipItems = true,
            bool withSummons = true,
            bool withHotkeys = true,
            bool withQuests = true)
        {
            var reader = ExecuteReader("SELECT * FROM characters WHERE id=@id AND userId=@userId LIMIT 1",
                new MySqlParameter("@id", id),
                new MySqlParameter("@userId", userId));
            var result = new PlayerCharacterData();
            if (ReadCharacter(reader, out result))
            {
                this.InvokeInstanceDevExtMethods("ReadCharacter",
                    userId,
                    id,
                    withEquipWeapons,
                    withAttributes,
                    withSkills,
                    withSkillUsages,
                    withBuffs,
                    withEquipItems,
                    withNonEquipItems,
                    withSummons,
                    withHotkeys,
                    withQuests);
                if (withEquipWeapons)
                    result.EquipWeapons = ReadCharacterEquipWeapons(id);
                if (withAttributes)
                    result.Attributes = ReadCharacterAttributes(id);
                if (withSkills)
                    result.Skills = ReadCharacterSkills(id);
                if (withSkillUsages)
                    result.SkillUsages = ReadCharacterSkillUsages(id);
                if (withBuffs)
                    result.Buffs = ReadCharacterBuffs(id);
                if (withEquipItems)
                    result.EquipItems = ReadCharacterEquipItems(id);
                if (withNonEquipItems)
                    result.NonEquipItems = ReadCharacterNonEquipItems(id);
                if (withSummons)
                    result.Summons = ReadCharacterSummons(id);
                if (withHotkeys)
                    result.Hotkeys = ReadCharacterHotkeys(id);
                if (withQuests)
                    result.Quests = ReadCharacterQuests(id);
                return result;
            }
            return null;
        }

        public override List<PlayerCharacterData> ReadCharacters(string userId)
        {
            var result = new List<PlayerCharacterData>();
            var reader = ExecuteReader("SELECT id FROM characters WHERE userId=@userId ORDER BY updateAt DESC", new MySqlParameter("@userId", userId));
            while (reader.Read())
            {
                var characterId = reader.GetString("id");
                result.Add(ReadCharacter(userId, characterId, true, true, true, false, false, true, false, false, false, false));
            }
            return result;
        }

        public override void UpdateCharacter(IPlayerCharacterData character)
        {
            ExecuteNonQuery("UPDATE characters SET " +
                "dataId=@dataId, " +
                "entityId=@entityId, " +
                "characterName=@characterName, " +
                "level=@level, " +
                "exp=@exp, " +
                "currentHp=@currentHp, " +
                "currentMp=@currentMp, " +
                "currentStamina=@currentStamina, " +
                "currentFood=@currentFood, " +
                "currentWater=@currentWater, " +
                "statPoint=@statPoint, " +
                "skillPoint=@skillPoint, " +
                "gold=@gold, " +
                "currentMapName=@currentMapName, " +
                "currentPositionX=@currentPositionX, " +
                "currentPositionY=@currentPositionY, " +
                "currentPositionZ=@currentPositionZ, " +
                "respawnMapName=@respawnMapName, " +
                "respawnPositionX=@respawnPositionX, " +
                "respawnPositionY=@respawnPositionY, " +
                "respawnPositionZ=@respawnPositionZ " +
                "WHERE id=@id",
                new MySqlParameter("@dataId", character.DataId),
                new MySqlParameter("@entityId", character.EntityId),
                new MySqlParameter("@characterName", character.CharacterName),
                new MySqlParameter("@level", character.Level),
                new MySqlParameter("@exp", character.Exp),
                new MySqlParameter("@currentHp", character.CurrentHp),
                new MySqlParameter("@currentMp", character.CurrentMp),
                new MySqlParameter("@currentStamina", character.CurrentStamina),
                new MySqlParameter("@currentFood", character.CurrentFood),
                new MySqlParameter("@currentWater", character.CurrentWater),
                new MySqlParameter("@statPoint", character.StatPoint),
                new MySqlParameter("@skillPoint", character.SkillPoint),
                new MySqlParameter("@gold", character.Gold),
                new MySqlParameter("@currentMapName", character.CurrentMapName),
                new MySqlParameter("@currentPositionX", character.CurrentPosition.x),
                new MySqlParameter("@currentPositionY", character.CurrentPosition.y),
                new MySqlParameter("@currentPositionZ", character.CurrentPosition.z),
                new MySqlParameter("@respawnMapName", character.RespawnMapName),
                new MySqlParameter("@respawnPositionX", character.RespawnPosition.x),
                new MySqlParameter("@respawnPositionY", character.RespawnPosition.y),
                new MySqlParameter("@respawnPositionZ", character.RespawnPosition.z),
                new MySqlParameter("@id", character.Id));
            FillCharacterRelatesData(character);
            this.InvokeInstanceDevExtMethods("UpdateCharacter", character);
        }

        public override void DeleteCharacter(string userId, string id)
        {
            var result = ExecuteScalar("SELECT COUNT(*) FROM characters WHERE id=@id AND userId=@userId",
                new MySqlParameter("@id", id),
                new MySqlParameter("@userId", userId));
            var count = result != null ? (long)result : 0;
            if (count > 0)
            {
                var connection = NewConnection();
                connection.Open();
                var transaction = connection.BeginTransaction();
                try
                {
                    ExecuteNonQuery(connection, transaction, "DELETE FROM characters WHERE id=@characterId", new MySqlParameter("@characterId", id));
                    DeleteCharacterAttributes(connection, transaction, id);
                    DeleteCharacterBuffs(connection, transaction, id);
                    DeleteCharacterHotkeys(connection, transaction, id);
                    DeleteCharacterItems(connection, transaction, id);
                    DeleteCharacterQuests(connection, transaction, id);
                    DeleteCharacterSkills(connection, transaction, id);
                    DeleteCharacterSkillUsages(connection, transaction, id);
                    DeleteCharacterSummons(connection, transaction, id);
                    transaction.Commit();
                }
                catch (System.Exception ex)
                {
                    Debug.LogError("Transaction, Error occurs while deleting character: " + id);
                    Debug.LogException(ex);
                    transaction.Rollback();
                }
                transaction.Dispose();
                connection.Close();
                this.InvokeInstanceDevExtMethods("DeleteCharacter", userId, id);
            }
        }

        public override long FindCharacterName(string characterName)
        {
            var result = ExecuteScalar("SELECT COUNT(*) FROM characters WHERE characterName LIKE @characterName",
                new MySqlParameter("@characterName", characterName));
            return result != null ? (long)result : 0;
        }
    }
}
