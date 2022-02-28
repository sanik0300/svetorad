using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace svetotelegraf
{
    class LetterSettings
    {
        private static Dictionary<char, byte[]> lels = new Dictionary<char, byte[]>
        {
            { 'а', new byte[] { 0, 1 } },
                { 'à', new byte[] { 0, 1, 1, 0, 1} }, { 'å', new byte[] { 0, 1, 1, 0, 1} },
                { 'ä', new byte[] { 0, 1, 0, 1 } }, { 'ą', new byte[] { 0, 1, 0, 1 } }, { 'æ', new byte[] { 0, 1, 0, 1 } },
            { 'б', new byte[] { 1, 0, 0, 0 } }, { 'b', new byte[] { 1, 0, 0, 0 } },
            { 'в', new byte[] { 0, 1, 1 } },
                { 'w', new byte[] { 0, 1, 1 } }, { 'v', new byte[] { 0, 0, 0, 1 } },
            { 'г', new byte[] { 1, 1, 0 } },
                { 'g', new byte[] { 1, 1, 0 } }, { 'ґ', new byte[] { 1, 1, 0 } }, { 'ĝ', new byte[] { 1, 1, 0, 1, 0 } },
            { 'д', new byte[] { 1, 0, 0 } },
                { 'd', new byte[] { 1, 0, 0 } }, {'đ', new byte[] { 0, 0, 1, 0, 0 } }, {'ð', new byte[] { 0, 0, 1, 1, 0 } },
            { 'е', new byte[] {0} },
                { 'e', new byte[] {0} }, {'ё', new byte[] {0} }, {'é', new byte[] {0, 0, 1, 0, 0} },
                {'ę', new byte[] {0, 0, 1, 0, 0} }, {'è', new byte[]{0, 1, 0, 0, 1 } },
            { 'ж', new byte[] {0, 0, 0, 1 } },
            { 'з', new byte[] { 1, 1, 0, 0} },
                { 'z', new byte[] { 1, 1, 0, 0} }, { 'ź', new byte[] { 1, 1, 0, 0, 1, 0} }, { 'ż', new byte[] { 1, 1, 0, 0, 1 } },
            { 'и', new byte[] { 0, 0} },
                { 'i', new byte[] { 0, 0} }, {'ї', new byte[] {0,1,1,1,0 } },
            { 'й', new byte[] { 0, 1, 1, 1} },
                { 'j', new byte[] { 0, 1, 1, 1} }, {'ĵ', new byte[] {0, 1, 1, 1, 0 } },
            { 'к', new byte[] {1,0,1 } }, { 'k', new byte[] {1, 0, 1} },
            { 'л', new byte[]{0, 1, 0, 0} },
                { 'l', new byte[]{0, 1, 0, 0} }, {'ł', new byte[]{0,1,0,0,1} },
            { 'м', new byte[] {1, 1 } }, { 'm', new byte[] {1, 1 } },
            { 'н', new byte[] { 1, 0} },
                { 'n', new byte[] { 1, 0} }, { 'ń', new byte[] {1, 1, 0, 1, 1 } }, { 'ñ', new byte[] {1, 1, 0, 1, 1 } },
            { 'о', new byte[] {1, 1, 1 } },
                 { 'o', new byte[] {1, 1, 1 } }, { 'ó', new byte[] {1, 1, 1, 0} }, { 'ö', new byte[] {1, 1, 1, 0} }, { 'ø', new byte[] {1, 1, 1, 0} },
            {'п' , new byte[] { 0, 1, 1, 0} },
                {'p' , new byte[] { 0, 1, 1, 0} }, {'þ', new byte[] {0, 1,1,0,0 } },
            {'р', new byte[]{0,1,0} }, {'r', new byte[]{0,1,0} },
            {'с', new byte[]{0,0,0} },
                {'s', new byte[]{0,0,0} }, {'š', new byte[] {1, 1, 1, 1 } },
                {'ś', new byte[]{ 0,0,0,1,0,0,0} }, { 'ŝ', new byte[]{0,0,0,1,0 } },
            {'т', new byte[]{1} }, {'t', new byte[]{1} },
            {'у', new byte[] { 0,0,1} },
                {'u', new byte[] {0,0,1} }, {'ü', new byte[] {0,0,1,1} }, {'ŭ', new byte[] {0,0,1,1} },
            {'ф', new byte[]{ 0,0,1,0} }, {'f', new byte[]{ 0,0,1,0} },
            {'х', new byte[]{ 0,0,0,0} }, {'h', new byte[]{ 0,0,0,0 } },
            {'ц', new byte[]{ 1,0,1,0} },
                {'c', new byte[]{ 1,0,1,0} }, {'ć', new byte[]{ 1,0,1,0,0} },
                {'ĉ', new byte[]{ 1,0,1,0,0} }, {'ç', new byte[]{ 1,0,1,0,0} },
            { 'ч', new byte[]{1,1,1,0} },
            { 'ш', new byte[]{1,1,1,1} },
            { 'щ', new byte[]{1,1,0,1} },
            { 'ъ', new byte[]{0,1,1,0,1,0} },
            { 'ы', new byte[]{1,0,1,1} },
            { 'ь', new byte[]{1,0,0,1} },
            { 'э', new byte[]{0,0,1,0,0} }, {'є', new byte[]{ 0, 0, 1, 0,0 } },
            { 'ю', new byte[]{0,0,1,1} }, { 'q', new byte[]{1,1,0,1} },
            { 'я', new byte[]{0,1,0,1} },
            
            { '1', new byte[]{0,1,1,1,1} },
            { '2', new byte[]{0,0,1,1,1} },
            { '3', new byte[]{0,0,0,1,1} },
            { '4', new byte[]{0,0,0,0,1} },
            { '5', new byte[]{0,0,0,0,0} },
            { '6', new byte[]{1,0,0,0,0} },
            { '7', new byte[]{1,1,0,0,0} },
            { '8', new byte[]{1,1,1,0,0} },
            { '9', new byte[]{1,1,1,1,1} },
            { '0', new byte[]{1,1,1,1,1} }
        };

        private static double AvgLetterLen;
        /// <summary>
        /// Длительность 1 точки (минимального отрезка) в мс
        /// </summary>
        public int interval;
        public readonly string Txt;
        public static void WeightedAvg()
        {
            ArrayList frequencies = new ArrayList(10);
            foreach (KeyValuePair<char, byte[]> kvp in lels)
            {
                while (frequencies.Count <= kvp.Value.Length)
                    frequencies.Add(0);

                frequencies[kvp.Value.Length] = (int)frequencies[kvp.Value.Length] + 1;
            }
            double multiplieds = 0;
            double sumweights = 0;
            foreach (var kvpp in lels) 
            {
                multiplieds += kvpp.Value.Length * (int)frequencies[kvpp.Value.Length];
                sumweights += (int)frequencies[kvpp.Value.Length];
            }
            AvgLetterLen = multiplieds / sumweights;     
            
        }
        public static Dictionary<char, byte[]> letters { get { return lels; } }
        public bool OnPlay = false;       
        
        private bool BEEPING = false;
        public LetterSettings(string txt, int hz, bool beeeep)
        {
            Txt = txt;
            BEEPING = beeeep;
            OnPlay = true;
            interval = (int)Math.Round(60000 /(AvgLetterLen*hz));
        }
        public uint position = 0;
    }
}
       
