using System;
using System.Collections.Generic;
using SerenBot.Models;

namespace SerenBot.Discord.Ultils
{
    public static class ByteExtensions
    {
        public static byte AddVos(this byte storeByte, VoiceOfSeren vos) {
            byte mask = (byte)(1 << (int)vos);

            return storeByte |= mask;
        }

        public static byte AddVos(this byte storeByte, IEnumerable<VoiceOfSeren> vos) {
            foreach (VoiceOfSeren voice in vos)
            {
                storeByte = AddVos(storeByte, voice);
            }

            return storeByte;
        }

        public static VoiceOfSeren[] GetVoiceOfSerens(this byte storeByte) {
            List<VoiceOfSeren> vos = new List<VoiceOfSeren>();

            foreach (VoiceOfSeren voice in (VoiceOfSeren[]) Enum.GetValues(typeof(VoiceOfSeren)))
            {
                if ((storeByte & (1 << (int)voice)) != 0) {
                    vos.Add(voice);
                }
            }

            return vos.ToArray();
        }

        public static byte RemoveVos(this byte storeByte, VoiceOfSeren vos) {
            byte mask = (byte)(1 << (int)vos);

            return storeByte &= (byte)~mask;
        }

        public static byte RemoveVos(this byte storeByte, IEnumerable<VoiceOfSeren> vos) {
            foreach (VoiceOfSeren voice in vos)
            {
                storeByte = RemoveVos(storeByte, voice);
            }

            return storeByte;
        }

        public static byte ResetVos(this byte storeByte) {
            return 0xFF;
        }
    }
}