using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace INTM.Serie4
{
    public static class Serie4
    {
        // Exercice I - Code Morse

        // On utilise un dictionnaire pour aisément faire les conversions des caractères en code morse et vice-versa
        private readonly static Dictionary<char, string> morseToChar = new Dictionary<char, string>
        {
            {'A',"=.==="},
            {'B',"===.=.=.="},
            {'C',"===.=.===.="},
            {'D',"===.=.="},
            {'E',"="},
            {'F',"=.=.===.="},
            {'G',"===.===.="},
            {'H',"=.=.=.="},
            {'I',"=.="},
            {'J',"=.===.===.==="},
            {'K',"===.=.==="},
            {'L',"=.===.=.="},
            {'M',"===.==="},
            {'N',"===.="},
            {'O',"===.===.==="},
            {'P',"=.===.===.="},
            {'Q',"===.===.=.==="},
            {'R',"=.===.="},
            {'S',"=.=.="},
            {'T',"==="},
            {'U',"=.=.==="},
            {'V',"=.=.=.==="},
            {'W',"=.===.==="},
            {'X',"===.=.=.==="},
            {'Y',"===.=.===.==="},
            {'Z',"===.===.=.="}
        };

        /// <summary>
        /// Renvoie le nombre d'un lettres d'un code morse.
        /// </summary>
        /// <param name="code">Code morse.</param>
        /// <returns>Entier correspondant au nombre de lettres du code.</returns>
        public static int LettersCount(string code)
        {
            if (code == null || code.Length == 0)
            {
                return 0;
            }

            int nbLetters = 0;
            string[] mots = code.Split(new string[] { "....." }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string mot in mots)
            {
                nbLetters += mot.Split(new string[] { "..." }, StringSplitOptions.RemoveEmptyEntries).Length;
            }
            
            return nbLetters;
        }

        /// <summary>
        /// Renvoie le nombre de mots dans un code morse.
        /// </summary>
        /// <param name="code">Code morse.</param>
        /// <returns>Entier correspondant au nombre de mots du code.</returns>
        public static int WordsCount(string code)
        {
            if (code == null || code.Length == 0) 
            {
                return 0;
            }

            return code.Split(new string[] { "....." }, StringSplitOptions.RemoveEmptyEntries).Length;
        }

        /// <summary>
        /// Traduit un code morse en une chaine de caractères lisible.
        /// </summary>
        /// <param name="code">Code morse à traduire.</param>
        /// <returns>Une chaine de caractères correspondant à la traduction du code morse reçu en entrée.</returns>
        /// <exception cref="ArgumentException"></exception>
        public static string MorseTranslation(string code)
        {
            if (code == null || code.Length == 0)
            {
                return "";
            }

            int nbMots = WordsCount(code);
            int nbLettres = LettersCount(code);
            int charTotal = nbLettres + nbMots - 1;
            char[] chars = new char[charTotal];
            int iterChar = 0;
            string[] mots = code.Split(new string[] { "....." }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string mot in mots)
            {
                string[] lettres = mot.Split(new string[] { "..." }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string lettre in lettres)
                {
                    if (morseToChar.ContainsValue(lettre))
                    {
                        chars[iterChar] = morseToChar.FirstOrDefault(x => x.Value == lettre).Key;
                        iterChar++;
                    }
                    else
                    {
                        for (int c = 0; c < lettre.Length; c++)
                        {
                            if (lettre[c] != '=' && lettre[c] != '.') 
                            {
                                throw new ArgumentException("Le code morse en entrée contient un ou plusieurs caractères invalides", "code");
                            }
                        }
                        chars[iterChar] = '+';
                        iterChar++;
                    }
                }
                if (iterChar < charTotal) 
                {
                    chars[iterChar] = ' ';
                    iterChar++;
                }
            }
            return new string(chars);
        }

        /// <summary>
        /// Renvoie le nombre d'un lettres d'un code morse en considérant les imperfections possibles.
        /// </summary>
        /// <param name="code">Code morse.</param>
        /// <returns>Entier correspondant au nombre de lettres du code.</returns>
        public static int EfficientLettersCount(string code)
        {
            if (code == null || code.Length == 0)
            {
                return 0;
            }

            int nbLetters = 0;
            string[] mots = Regex.Split(code, @"\.{5,}").Where(s => s != null && s.Length != 0).ToArray();

            foreach (string mot in mots)
            {
                nbLetters += Regex.Split(mot, @"\.{3,4}").Where(s => s != null && s.Length != 0).Count();
            }

            return nbLetters;
        }

        /// <summary>
        /// Renvoie le nombre de mots dans un code morse en considérant les imperfections possibles.
        /// </summary>
        /// <param name="code">Code morse.</param>
        /// <returns>Entier correspondant au nombre de mots du code.</returns>
        public static int EfficientWordsCount(string code)
        {
            if (code == null || code.Length == 0)
            {
                return 0;
            }

            return Regex.Split(code, @"\.{5,}").Where(s => s != null && s.Length != 0).Count();
        }

        /// <summary>
        /// Traduit un code morse en une chaine de caractères lisible en considérant les possibles imperfections.
        /// </summary>
        /// <param name="code">Code morse à traduire.</param>
        /// <returns>Une chaine de caractères correspondant à la traduction du code morse reçu en entrée.</returns>
        /// <exception cref="ArgumentException"></exception>
        public static string EfficientMorseTranslation(string code)
        {
            if (code == null || code.Length == 0)
            {
                return "";
            }

            int nbMots = WordsCount(code);
            int nbLettres = LettersCount(code);
            int charTotal = nbLettres + nbMots - 1;
            char[] chars = new char[charTotal];
            int iterChar = 0;
            string[] mots = Regex.Split(code, @"\.{5,}").Where(s => s != null && s.Length != 0).ToArray();

            foreach (string mot in mots)
            {
                string[] lettres = Regex.Split(mot, @"\.{3,4}").Where(s => s != null && s.Length != 0).ToArray();

                foreach (string lettre in lettres)
                {
                    string sLettre = lettre;
                    sLettre = sLettre.Replace("..", ".");

                    if (morseToChar.ContainsValue(sLettre))
                    {
                        chars[iterChar] = morseToChar.FirstOrDefault(x => x.Value == sLettre).Key;
                        iterChar++;
                    }
                    else
                    {
                        for (int c = 0; c < sLettre.Length; c++)
                        {
                            if (sLettre[c] != '=' && sLettre[c] != '.')
                            {
                                throw new ArgumentException("Le code morse en entrée contient un ou plusieurs caractères invalides", "code");
                            }
                        }
                        chars[iterChar] = '+';
                        iterChar++;
                    }
                }
                if (iterChar < charTotal)
                {
                    chars[iterChar] = ' ';
                    iterChar++;
                }
            }
            return new string(chars);
        }

        /// <summary>
        /// Convertir une chaine de texte en code morse.
        /// </summary>
        /// <param name="sentence">Texte à convertir.</param>
        /// <returns>Une chaine de morse correspondant au texte d'entrée.</returns>
        public static string MorseEncryption(string sentence)
        {
            if (sentence == null || sentence.Length == 0)
            {
                return "";
            }

            string res = "";

            for (int s = 0; s < sentence.Length; s++)
            {
                char c = sentence[s];

                if (c == ' ')
                {
                    res += ".....";
                }
                else
                {
                    res += morseToChar[c];
                }

                if (s < sentence.Length - 1 && sentence[s] != ' ' && sentence[s + 1] != ' ') 
                {
                    res += "...";
                }
            }

            return res;
        }

        // Exercice II - Contrôle des parenthèses


    }
}
