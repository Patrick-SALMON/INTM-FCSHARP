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
        /// Renvoie le nombre de lettres d'un code morse.
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
        // On va utiliser un stack pour le traitement, à chaque caractère ouvrant qu'on trouve il faut qu'on trouve le caractère fermant correspondant
        // en premier donc le stack est bien adapté à notre problème. On utilisera aussi un dictionnaire pour faire la liaision des membres ouvrant avec les membres fermant

        /// <summary>
        /// Vérifie dans une chaine de caractères que tous les caractères ouvrant se ferment bien.
        /// </summary>
        /// <param name="sentence">Texte à vérifier.</param>
        /// <returns>True si la chaine est correcte et False sinon.</returns>
        public static bool BracketsControls(string sentence)
        {
            if (sentence == null || sentence.Length == 0)
            {
                return true;
            }

            Stack<char> pile = new Stack<char>();
            Dictionary<char, char> dictOuvrantFermant = new Dictionary<char, char>
            {
                {'(',')'},
                {'[',']'},
                {'{','}'}
            };

            for (int i = 0; i < sentence.Length; i++)
            {
                if (dictOuvrantFermant.ContainsKey(sentence[i]))
                {
                    pile.Push(sentence[i]);
                }
                else if (dictOuvrantFermant.ContainsValue(sentence[i]))
                {
                    if (pile.Count() == 0)
                    {
                        return false;
                    }

                    if (sentence[i] != dictOuvrantFermant[pile.Pop()])
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        // Exercice III - Liste des contacts téléphoniques
        // On va utiliser un dictionnaire pour associer les numéros aux contacts car on peut assurer l'unicité du numéro de téléphone en l'utilisant
        // comme clé du dictionnaire
        
        /// <summary>
        /// Structure représentant un annuaire.
        /// </summary>
        public struct PhoneBook
        {
            public static Dictionary<string, string> dictPhoneBook = new Dictionary<string, string>();

            /// <summary>
            /// Vérifie que le numéro de téléphone donné est dans un format valide
            /// </summary>
            /// <param name="phoneNumber">Numéro de téléphone à vérifier.</param>
            /// <returns>True si le numéro est valide et False sinon.</returns>
            public bool IsValidPhoneNumber(string phoneNumber)
            {
                if (phoneNumber == null || phoneNumber.Length != 10)
                {
                    return false;
                }
                else if (phoneNumber[0] != '0' || phoneNumber[1] == 0 )
                {
                    return false;
                }
                return true;
            }

            /// <summary>
            /// Vérifie si le numéro de téléphone donné existe dans notre dictionnaire.
            /// </summary>
            /// <param name="phoneNumber">Numéro de téléphone à vérifier.</param>
            /// <returns>True si le numéro existe dans le dictionnaire et False si le numéro est invalide ou s'il n'existe pas dans le dictionnaire.</returns>
            public bool ContainsPhoneNumber(string phoneNumber)
            {
                if (!IsValidPhoneNumber(phoneNumber))
                {
                    return false;
                }
                else if (!dictPhoneBook.ContainsKey(phoneNumber))
                {
                    return false;
                }

                return true;
            }

            /// <summary>
            /// Recherche le nom du contact associé à un numéro de téléphone donné.
            /// </summary>
            /// <param name="phoneNumber">Numéro de téléphone à rechercher.</param>
            /// <returns>Le nom du contact dans le dictionnaire.</returns>
            /// <exception cref="KeyNotFoundException"></exception>
            public string PhoneContact(string phoneNumber)
            {
                if (!ContainsPhoneNumber(phoneNumber))
                {
                    throw new KeyNotFoundException("Le format du numéro recherché est invalide ou il n'existe pas dans l'annuaire.");
                }
                return dictPhoneBook[phoneNumber];
            }

            /// <summary>
            /// Ajoute un numéro de téléphone accompagné d'un nom de contact à l'annuaire.
            /// </summary>
            /// <param name="phoneNumber">Numéro de téléphone à rajouter.</param>
            /// <param name="name">Nom du contact associé.</param>
            /// <returns>True si l'ajout a réussi et False sinon.</returns>
            public bool AddPhoneNumber(string phoneNumber, string name)
            {
                if (ContainsPhoneNumber(phoneNumber) || !IsValidPhoneNumber(phoneNumber) || name == null || name.Length == 0)
                {
                    return false;
                }

                dictPhoneBook.Add(phoneNumber, name);
                return true;
            }

            /// <summary>
            /// Supprime un numéro de téléphone de l'annuaire.
            /// </summary>
            /// <param name="phoneNumber">Numéro de téléphone à supprimer.</param>
            /// <returns>True si le numéro a bien été supprimé et False sinon.</returns>
            /// <exception cref="KeyNotFoundException"></exception>
            public bool DeletePhoneNumber(string phoneNumber)
            {
                if (!ContainsPhoneNumber(phoneNumber))
                {
                    throw new KeyNotFoundException("Le format du numéro à supprimer est invalide ou il n'existe pas dans l'annuaire.");
                }

                return dictPhoneBook.Remove(phoneNumber);
            }

            /// <summary>
            /// Affiche la liste des numéros de téléphones et leurs contacts associés.
            /// </summary>
            public void DisplayPhoneBook()
            {
                if (dictPhoneBook.Count() != 0)
                {
                    Console.WriteLine("Annuaire téléphonique :");
                    Console.WriteLine("-----------------------");
                    foreach (KeyValuePair<string,string> keyValuePair in dictPhoneBook)
                    {
                        Console.WriteLine($"{keyValuePair.Key} : {keyValuePair.Value}");
                    }
                    Console.WriteLine("-----------------------");
                }
            }

            // Exercice IV - Emploi du temps professionnel
        }

    }
}
