using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;

namespace ProjetINTM1
{
    class Banque
    {
        private readonly Dictionary<uint, Compte> _comptes;

        public Banque(string comptesFilePath)
        {
            _comptes = new Dictionary<uint, Compte>();

            if (!File.Exists(comptesFilePath))
            {
                throw new FileNotFoundException("Le fichier des comptes n'a pas été trouvé", comptesFilePath);
            }

            using (FileStream fs = File.OpenRead(comptesFilePath))
            using (StreamReader lecComptes = new StreamReader(fs))
            {
                while (!lecComptes.EndOfStream)
                {
                    string[] ligneFichier = lecComptes.ReadLine().Split(';');
                    if (ligneFichier.Length != 2)
                    {
                        continue;
                    }
                    if (!uint.TryParse(ligneFichier[0], out uint identifiant) || _comptes.ContainsKey(identifiant))
                    {
                        continue;
                    }
                    if (ligneFichier[1] == string.Empty)
                    {
                        _comptes.Add(identifiant, new Compte(identifiant));
                    }
                    else if (!decimal.TryParse(ligneFichier[1], NumberStyles.AllowDecimalPoint, CultureInfo.GetCultureInfo("en-US"), out decimal solde) 
                             || solde < 0)
                    {
                        continue;
                    }
                    else
                    {
                        _comptes.Add(identifiant, new Compte(identifiant, solde));
                    }
                }
            }
        }

        public void TraiterTransactions(string transactionsFilePath, string outFilePath)
        {
            if (!File.Exists(transactionsFilePath))
            {
                throw new FileNotFoundException("Le fichier des transactions n'a pas été trouvé", transactionsFilePath);
            }

            using (FileStream fsout = File.Create(outFilePath))
            using (StreamWriter swout = new StreamWriter(fsout))
            using (FileStream fsin = File.OpenRead(transactionsFilePath))
            using (StreamReader lecTransaction = new StreamReader(fsin))
            {
                List<uint> identifiantsTransactions = new List<uint>();
                
                while (!lecTransaction.EndOfStream)
                {
                    string[] ligneFichier = lecTransaction.ReadLine().Split(';');

                    if (ligneFichier.Length != 4)
                    {
                        swout.WriteLine("Transaction illisible, mauvais nombre d'éléments;KO");
                        continue;
                    }
                    if (!uint.TryParse(ligneFichier[0], out uint identifiant)) 
                    {
                        swout.WriteLine("Transaction illisible, mauvais format d'identifiant;KO");
                        continue;
                    }
                    if (identifiantsTransactions.Contains(identifiant))
                    {
                        swout.WriteLine(identifiant + ";KO");
                        continue;
                    }
                    if (!decimal.TryParse(ligneFichier[1], NumberStyles.AllowDecimalPoint, CultureInfo.GetCultureInfo("en-US"), out decimal montant) 
                        || montant <= 0)
                    {
                        swout.WriteLine(identifiant + ";KO");
                        continue;
                    }
                    if (!uint.TryParse(ligneFichier[2], out uint expediteur) || (expediteur != 0 && !_comptes.ContainsKey(expediteur))) 
                    {
                        swout.WriteLine(identifiant + ";KO");
                        continue;
                    }
                    if (!uint.TryParse(ligneFichier[3], out uint destinataire) || (destinataire != 0 && !_comptes.ContainsKey(destinataire))) 
                    {
                        swout.WriteLine(identifiant + ";KO");
                        continue;
                    }

                    Transaction transaction;
                    try
                    {
                        transaction = new Transaction(identifiant, montant, expediteur, destinataire);
                    }
                    catch (ArgumentException)
                    {
                        swout.WriteLine(identifiant + ";KO");
                        continue;
                    }

                    if (expediteur == 0 && destinataire != 0 && _comptes[destinataire].PrelevementVerif(transaction))
                    {
                        _comptes[destinataire].Prelevement(montant);
                        swout.WriteLine(identifiant + ";OK");
                        identifiantsTransactions.Add(identifiant);
                    }
                    else if (expediteur != 0 && destinataire == 0 && _comptes[expediteur].VirementVerif(transaction)) 
                    {
                        _comptes[expediteur].Virement(montant);
                        swout.WriteLine(identifiant + ";OK");
                        identifiantsTransactions.Add(identifiant);
                    }
                    else if (expediteur != 0 && destinataire != 0 && _comptes[expediteur].VirementVerif(transaction) && _comptes[destinataire].PrelevementVerif(transaction))
                    {
                        _comptes[expediteur].Virement(montant);
                        _comptes[destinataire].Prelevement(montant);
                        swout.WriteLine(identifiant + ";OK");
                        identifiantsTransactions.Add(identifiant);
                    }
                    else
                    {
                        swout.WriteLine(identifiant + ";KO");
                    }
                }
            }
        }
    }
}
