using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;

namespace ProjetINTM1
{
    /// <summary>
    /// Regroupement de plusieurs compte bancaires avec capacité de traiter des transactions.
    /// </summary>
    class Banque
    {
        private readonly Dictionary<uint, Compte> _comptes;

        /// <summary>
        /// Chargement des comptes bancaires d'après un fichier formatté.
        /// </summary>
        /// <param name="comptesFilePath">Chemin vers le fichier contenant les comptes décrient ligne par ligne avec ';' comme séparateur, 
        /// le premier élément est l'identifiant du compte et optionnellement le deuxième son solde. </param>
        /// <exception cref="FileNotFoundException"></exception>
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

                    // Vérification des champs du fichier
                    if (ligneFichier.Length != 2)
                    {
                        continue;
                    }
                    // Tentative de conversion de l'identifiant et vérification de son unicité et que ce n'est pas l'identifiant réservé à la console.
                    if (!uint.TryParse(ligneFichier[0], out uint identifiant) || _comptes.ContainsKey(identifiant) || identifiant == 0)
                    {
                        continue;
                    }
                    // Si le champ correspondant au solde est vide, on utilise la valeur par défaut du solde.
                    if (ligneFichier[1] == string.Empty)
                    {
                        _comptes.Add(identifiant, new Compte(identifiant));
                    }
                    // Tentative de conversion du solde en autorisant les virgules et en considérant le point comme le séparateur décimal, on vérifie également
                    // que le solde est positif.
                    else if (!decimal.TryParse(ligneFichier[1], NumberStyles.AllowDecimalPoint, CultureInfo.GetCultureInfo("en-US"), out decimal solde) 
                             || solde < 0)
                    {
                        continue;
                    }
                    else
                    {
                        try
                        {
                            _comptes.Add(identifiant, new Compte(identifiant, solde));
                        }
                        catch (ArgumentException)
                        {
                            continue;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Traitement d'une ou plusieurs transactions décritent dans un fichier formatté et impression du résultat de chaque transaction
        /// dans un fichier de sortie.
        /// </summary>
        /// <param name="transactionsFilePath">Chemin vers le fichier des transactions.</param>
        /// <param name="outFilePath">Chemin vers le fichier de sortie.</param>
        /// <exception cref="FileNotFoundException"></exception>
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
                // Liste des identifiants déjà vu dans le fichier.
                List<uint> identifiantsTransactions = new List<uint>();
                
                while (!lecTransaction.EndOfStream)
                {
                    string[] ligneFichier = lecTransaction.ReadLine().Split(';');

                    // Vérification des champs du fichier.
                    if (ligneFichier.Length != 4)
                    {
                        swout.WriteLine("Transaction illisible, mauvais nombre d'éléments;KO");
                        continue;
                    }
                    // On sépare le test d'unicité et le test de conversion car on affiche pas la même valeur en sortie selon l'erreur
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
                    // Tentative de conversion du montant de la transaction en autorisant les virgules et en considérant le point comme
                    // le séparateur décimal, on vérifie également que le montant est strictement positif.
                    if (!decimal.TryParse(ligneFichier[1], NumberStyles.AllowDecimalPoint, CultureInfo.GetCultureInfo("en-US"), out decimal montant) 
                        || montant <= 0)
                    {
                        swout.WriteLine(identifiant + ";KO");
                        continue;
                    }
                    if (!uint.TryParse(ligneFichier[2], out uint expediteur) || !ExistenceCompte(expediteur)) 
                    {
                        swout.WriteLine(identifiant + ";KO");
                        continue;
                    }
                    if (!uint.TryParse(ligneFichier[3], out uint destinataire) || !ExistenceCompte(destinataire)) 
                    {
                        swout.WriteLine(identifiant + ";KO");
                        continue;
                    }

                    Transaction transaction;
                    // Le constructeur de transaction peut remonter une exception si le montant est nul.
                    try
                    {
                        transaction = new Transaction(identifiant, montant, expediteur, destinataire);
                    }
                    catch (ArgumentException)
                    {
                        swout.WriteLine(identifiant + ";KO");
                        continue;
                    }

                    if (PrelevementSansExpediteur(transaction))
                    {
                        _comptes[destinataire].Prelevement(montant);
                        swout.WriteLine(identifiant + ";OK");
                        identifiantsTransactions.Add(identifiant);
                    }
                    else if (VirementSansDestinataire(transaction)) 
                    {
                        _comptes[expediteur].Virement(montant);
                        swout.WriteLine(identifiant + ";OK");
                        identifiantsTransactions.Add(identifiant);
                    }
                    else if (TransactionEntreDeuxComptes(transaction))
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

        /// <summary>
        /// Vérifie qu'un compte existe bien dans le dictionnaire, on considère le compte 0 comme existant.
        /// </summary>
        /// <param name="identifiant">Identifiant de compte à vérifier.</param>
        /// <returns>True si le compte existe (ou est 0) et False sinon.</returns>
        private bool ExistenceCompte(uint identifiant)
        {
            return identifiant == 0 || _comptes.ContainsKey(identifiant);
        }

        /// <summary>
        /// Vérifie que l'on se trouve dans un cas de prélèvement sans expéditeur (expéditeur = 0).
        /// </summary>
        /// <param name="transaction">Transaction à traiter.</param>
        /// <returns>True si on se trouve dans ce cas et False sinon.</returns>
        private bool PrelevementSansExpediteur(Transaction transaction)
        {
            return transaction.Expediteur == 0 && transaction.Destinataire != 0 && _comptes[transaction.Destinataire].PrelevementVerif(transaction);
        }

        /// <summary>
        /// Vérifie que l'on se trouve dans un cas de virement sans destinataire (destinataire = 0).
        /// </summary>
        /// <param name="transaction">Transaction à traiter.</param>
        /// <returns>True si on se trouve dans ce cas et False sinon.</returns>
        private bool VirementSansDestinataire(Transaction transaction)
        {
            return transaction.Expediteur != 0 && transaction.Destinataire == 0 && _comptes[transaction.Expediteur].VirementVerif(transaction);
        }

        /// <summary>
        /// Vérifie que l'on se trouve dans un cas d'une transaction classique entre deux comptes.
        /// </summary>
        /// <param name="transaction">Transaction à traiter.</param>
        /// <returns>True si on se trouve dans ce cas et False sinon.</returns>
        private bool TransactionEntreDeuxComptes(Transaction transaction)
        {
            return transaction.Expediteur != 0 && transaction.Destinataire != 0 && _comptes[transaction.Expediteur].VirementVerif(transaction) && _comptes[transaction.Destinataire].PrelevementVerif(transaction);
        }
    }
}
