using System.Collections.Generic;
using Discord;
using SerenBot.Entities.Models;
using SerenBot.Models;

namespace SerenBot.Discord.Ultils
{
    public static class VosExtenstions
    {
        public static Color GetColorByVos(this VoiceOfSeren vos) {
            switch (vos) {
                case VoiceOfSeren.Amlodd:
                    return new Color(92, 120, 214); // (92, 120, 214) avg (37, 48, 86)
                case VoiceOfSeren.Cadarn:
                    return new Color(124, 207, 127); // (124, 207, 127) avg (47, 78, 48)
                case VoiceOfSeren.Crwys:
                    return new Color(176, 212, 103); // (176, 212, 103) avg (70, 84, 41)
                case VoiceOfSeren.Hefin:
                    return new Color(199, 55, 66); // (199, 55, 66) avg (72, 20, 24)
                case VoiceOfSeren.Iorwerth:
                    return new Color(156, 161, 161); // (156, 161, 161) avg (32, 33, 33)
                case VoiceOfSeren.Ithell:
                    return new Color(137, 148, 179); // (137, 148, 179) avg (40, 43, 52)
                case VoiceOfSeren.Meilyr:
                    return new Color(70, 199, 134); // (70, 199, 134) avg (25, 71, 48)
                case VoiceOfSeren.Trahaearn:
                    return new Color(207, 85, 159); // (207, 85, 159) avg (78, 32, 60)
                default:
                    return Color.Default;
            }
        }

        public static Guild SetRoleIdByVos(this Guild guild, VoiceOfSeren vos, ulong roleId) {
            switch (vos) {
                case VoiceOfSeren.Amlodd:
                    guild.AmloddRoleId = roleId;
                    break;
                case VoiceOfSeren.Cadarn:
                    guild.CadarnRoleId = roleId;
                    break;
                case VoiceOfSeren.Crwys:
                    guild.CrwysRoleId = roleId;
                    break;
                case VoiceOfSeren.Hefin:
                    guild.HefinRoleId = roleId;
                    break;
                case VoiceOfSeren.Iorwerth:
                    guild.IorwerthRoleId = roleId;
                    break;
                case VoiceOfSeren.Ithell:
                    guild.IthellRoleId = roleId;
                    break;
                case VoiceOfSeren.Meilyr:
                    guild.MeilyrRoleId = roleId;
                    break;
                case VoiceOfSeren.Trahaearn:
                    guild.TrahaearnRoleId = roleId;
                    break;
            }
            return guild;
        }

        public static ulong GetRoleIdByVos(this Guild guild, VoiceOfSeren vos) {
            switch (vos) {
                case VoiceOfSeren.Amlodd:
                    return guild.AmloddRoleId;
                case VoiceOfSeren.Cadarn:
                    return guild.CadarnRoleId;
                case VoiceOfSeren.Crwys:
                    return guild.CrwysRoleId;
                case VoiceOfSeren.Hefin:
                    return guild.HefinRoleId;
                case VoiceOfSeren.Iorwerth:
                    return guild.IorwerthRoleId;
                case VoiceOfSeren.Ithell:
                    return guild.IthellRoleId;
                case VoiceOfSeren.Meilyr:
                    return guild.MeilyrRoleId;
                case VoiceOfSeren.Trahaearn:
                    return guild.TrahaearnRoleId;
            }
            return 0;
        }

        public static Guild SetEmojiIdByVos(this Guild guild, VoiceOfSeren vos, ulong emojiId) {
            switch (vos) {
                case VoiceOfSeren.Amlodd:
                    guild.AmloddEmojiId = emojiId;
                    break;
                case VoiceOfSeren.Cadarn:
                    guild.CadarnEmojiId = emojiId;
                    break;
                case VoiceOfSeren.Crwys:
                    guild.CrwysEmojiId = emojiId;
                    break;
                case VoiceOfSeren.Hefin:
                    guild.HefinEmojiId = emojiId;
                    break;
                case VoiceOfSeren.Iorwerth:
                    guild.IorwerthEmojiId = emojiId;
                    break;
                case VoiceOfSeren.Ithell:
                    guild.IthellEmojiId = emojiId;
                    break;
                case VoiceOfSeren.Meilyr:
                    guild.MeilyrEmojiId = emojiId;
                    break;
                case VoiceOfSeren.Trahaearn:
                    guild.TrahaearnEmojiId = emojiId;
                    break;
            }
            return guild;
        }

        public static ulong GetEmojiIdByVos(this Guild guild, VoiceOfSeren vos) {
            switch (vos) {
                case VoiceOfSeren.Amlodd:
                    return guild.AmloddEmojiId;
                case VoiceOfSeren.Cadarn:
                    return guild.CadarnEmojiId;
                case VoiceOfSeren.Crwys:
                    return guild.CrwysEmojiId;
                case VoiceOfSeren.Hefin:
                    return guild.HefinEmojiId;
                case VoiceOfSeren.Iorwerth:
                    return guild.IorwerthEmojiId;
                case VoiceOfSeren.Ithell:
                    return guild.IthellEmojiId;
                case VoiceOfSeren.Meilyr:
                    return guild.MeilyrEmojiId;
                case VoiceOfSeren.Trahaearn:
                    return guild.TrahaearnEmojiId;
            }
            return 0;
        }

        public static VoiceOfSeren? GetVosByRoleId(this Guild guild, ulong roleId) {
            Dictionary<ulong, VoiceOfSeren> vosDictionary = new Dictionary<ulong, VoiceOfSeren>() {
                {guild.AmloddRoleId, VoiceOfSeren.Amlodd},
                {guild.CadarnRoleId, VoiceOfSeren.Cadarn},
                {guild.CrwysRoleId, VoiceOfSeren.Crwys},
                {guild.HefinRoleId, VoiceOfSeren.Hefin},
                {guild.IorwerthRoleId, VoiceOfSeren.Iorwerth},
                {guild.IthellRoleId, VoiceOfSeren.Ithell},
                {guild.MeilyrRoleId, VoiceOfSeren.Meilyr},
                {guild.TrahaearnRoleId, VoiceOfSeren.Trahaearn}
            };

            if (vosDictionary.ContainsKey(roleId)) {
                return vosDictionary[roleId];
            }
            return null;
        }
    }
}