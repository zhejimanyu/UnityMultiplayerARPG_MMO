﻿namespace MultiplayerARPG.MMO
{
    public struct UserCharacterData
    {
        public string id;
        public string userId;
        public string characterName;
        public int dataId;
        public short level;
        public int partyId;
        public int guildId;
        public int currentHp;
        public int maxHp;
        public int currentMp;
        public int maxMp;

        public SocialCharacterData ToSocialCharacterData()
        {
            var result = new SocialCharacterData();

            result.id = id;
            result.characterName = characterName;
            result.dataId = dataId;
            result.level = level;
            result.currentHp = currentHp;
            result.maxHp = maxHp;
            result.currentMp = currentMp;
            result.maxMp = maxMp;

            return result;
        }
    }
}