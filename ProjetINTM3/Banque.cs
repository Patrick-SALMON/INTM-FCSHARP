using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;

namespace ProjetINTM3
{
    /// <summary>
    /// Regroupement de plusieurs compte bancaires avec capacité de traiter des transactions.
    /// </summary>
    class Banque
    {
        private readonly Dictionary<uint, Gestionnaire> _gestionnaires;
        private readonly Dictionary<uint, uint> _liaisonComptesGestionnaires;
        private readonly Dictionary<uint, Compte> _comptes;
        private int _compteurCompteCree;
        private int _compteurTransactions;
        private int _compteurReussite;
        private int _compteurEchec;
        private decimal _totalReussite;

        /// <summary>
        /// Chargement des gestionnaires et initialisation des attributs de Banque.
        /// </summary>
        /// <param name="gestionnairesFilePath">Chemin vers le fichier contenant les informations des gestionnaires.</param>
        /// <exception cref="FileNotFoundException"></exception>
        public Banque(string gestionnairesFilePath)
        {
            _comptes = new Dictionary<uint, Compte>();
            _liaisonComptesGestionnaires = new Dictionary<uint, uint>();
            _gestionnaires = new Dictionary<uint, Gestionnaire>();
            _compteurCompteCree = 0;
            _compteurTransactions = 0;
            _compteurReussite = 0;
            _compteurEchec = 0;
            _totalReussite = 0;

            if (!File.Exists(gestionnairesFilePath))
            {
                throw new FileNotFoundException("Le fichier des gestionnaires n'a pas été trouvé", gestionnairesFilePath);
            }
            
            using(FileStream fs = File.OpenRead(gestionnairesFilePath))
            using(StreamReader lecGestionnaires = new StreamReader(fs))
            {
                while (!lecGestionnaires.EndOfStream)
                {
                    string[] ligneFichier = lecGestionnaires.ReadLine().Split(';');

                    // Vérification des champs du fichier
                    if (ligneFichier.Length != 3)
                    {
                        continue;
                    }
                    // Tentative de conversion de l'identifiant et vérification de son unicité.
                    if (!uint.TryParse(ligneFichier[0], out uint identifiant) || _gestionnaires.ContainsKey(identifiant))
                    {
                        continue;
                    }
                    // Vérification du type du gestionnaire.
                    if (ligneFichier[1] != "Particulier" && ligneFichier[1] != "Entreprise")
                    {
                        continue;
                    }
                    // Tentative de conversion et vérification que le nombre de transactions à considérer pour le retrait max est strictement positif.
                    if (!int.TryParse(ligneFichier[2], out int nbTransRetMax) || nbTransRetMax < 0)
                    {
                        continue;
                    }
                    try
                    {
                        nbTransRetMax++;
                        //lignefichier[1] contient le type du gestionnaire.
                        _gestionnaires.Add(identifiant, new Gestionnaire(identifiant, ligneFichier[1], nbTransRetMax));
                    }
                    catch (ArgumentException)
                    {
                        continue;
                    }
                }
            }
        }

        /// <summary>
        /// Traite séquentiellement et selon l'ordre chronologique le fichier contenant les opérations sur les comptes et le fichier contenant
        /// les transactions. Les fichiers sont supposés trié sur leur date d'opération et on écrit leur statut sur deux fichiers de sortie séparés.
        /// </summary>
        /// <param name="comptesFilePath">Chemin vers le fichier contenant les opérations sur les comptes.</param>
        /// <param name="transactionsFilePath">Chemin vers le fichier contenant les transactions.</param>
        /// <param name="comptesStatutFilePath">Chemin vers le fichier à écrire du statut des opérations sur les comptes.</param>
        /// <param name="transactionsStatutFilePath">Chemin vers le fichier à écrire du statut des transactions.</param>
        /// <exception cref="FileNotFoundException"></exception>
        public void TraiterOperations(string comptesFilePath, string transactionsFilePath, string comptesStatutFilePath, string transactionsStatutFilePath)
        {
            string[] ligneCompte = new string[0];
            string[] ligneTransaction = new string[0];
            DateTime dateOperationCompte = new DateTime();
            DateTime dateOperationTransaction = new DateTime();
            bool ligneCompteTraitee = true;
            bool ligneTransactionTraitee = true;

            if (!File.Exists(comptesFilePath))
            {
                throw new FileNotFoundException("Le fichier des opérations sur les comptes n'a pas été trouvé", comptesFilePath);
            }
            if (!File.Exists(transactionsFilePath))
            {
                throw new FileNotFoundException("Le fichier des transactions n'a pas été trouvé", transactionsFilePath);
            }

            using (FileStream fsComptes = File.OpenRead(comptesFilePath))
            using (StreamReader lecComptes = new StreamReader(fsComptes))

            using (FileStream fsTransactions = File.OpenRead(transactionsFilePath))
            using (StreamReader lecTransactions = new StreamReader(fsTransactions))

            using (FileStream fsStatutComptes = File.Create(comptesStatutFilePath))
            using (StreamWriter swComptes = new StreamWriter(fsStatutComptes))

            using (FileStream fsStatutTransactions = File.Create(transactionsStatutFilePath))
            using (StreamWriter swTransactions = new StreamWriter(fsStatutTransactions))
            {
                // Aucun traitement ne peut être fait si le fichier des comptes est vide.
                if (lecComptes.EndOfStream)
                {
                    return;
                }
                if (lecTransactions.EndOfStream)
                {
                    ligneTransactionTraitee = false;
                    dateOperationTransaction = DateTime.MaxValue;
                }
                while (!lecComptes.EndOfStream || !lecTransactions.EndOfStream || !ligneCompteTraitee || !ligneTransactionTraitee)
                {
                    // Lecture de la prochaine ligne du fichier des comptes.
                    if (!lecComptes.EndOfStream && ligneCompteTraitee)
                    {
                        ligneCompte = lecComptes.ReadLine().Split(';');
                        // On continue à lire tant que la ligne n'est pas correcte.
                        while (ligneCompte.Length != 7 || !DateTime.TryParseExact(ligneCompte[2], "d", CultureInfo.GetCultureInfo("fr-FR"), DateTimeStyles.None, out dateOperationCompte))
                        {
                            swComptes.WriteLine("Ligne illisible;KO");
                            if (lecComptes.EndOfStream)
                            {
                                dateOperationCompte = DateTime.MaxValue;
                                break;
                            }
                            ligneCompte = lecComptes.ReadLine().Split(';');
                        }
                    }
                    // Lecture de la prochaine ligne du fichier des transactions.
                    if (!lecTransactions.EndOfStream && ligneTransactionTraitee)
                    {
                        ligneTransaction = lecTransactions.ReadLine().Split(';');
                        // On continue à lire tant que la ligne n'est pas correcte.
                        while (ligneTransaction.Length != 5 || !DateTime.TryParseExact(ligneTransaction[1], "d", CultureInfo.GetCultureInfo("fr-FR"), DateTimeStyles.None, out dateOperationTransaction))
                        {
                            swTransactions.WriteLine("Ligne illisible;KO");
                            if (lecTransactions.EndOfStream)
                            {
                                dateOperationTransaction = DateTime.MaxValue;
                                break;
                            }
                            ligneTransaction = lecTransactions.ReadLine().Split(';');
                        }
                    }

                    // L'opération sur le compte est plus ancienne donc on l'exécute en premier.
                    if (dateOperationCompte < dateOperationTransaction)
                    {
                        ligneCompteTraitee = true;
                        ligneTransactionTraitee = false;
                        swComptes.WriteLine(TraitementLigneCompte(ligneCompte));
                    }
                    // La transaction est plus ancienne donc on l'exécute en premier
                    else if (dateOperationCompte > dateOperationTransaction)
                    {
                        ligneCompteTraitee = false;
                        ligneTransactionTraitee = true;
                        swTransactions.WriteLine(TraitementLigneTransaction(ligneTransaction));
                    }
                    // On exécute d'abord l'opération sur les comptes puis la transaction.
                    else if (dateOperationCompte == dateOperationTransaction)
                    {
                        ligneCompteTraitee = true;
                        ligneTransactionTraitee = true;
                        if (dateOperationCompte != DateTime.MaxValue && dateOperationTransaction != DateTime.MaxValue)
                        {
                            swComptes.WriteLine(TraitementLigneCompte(ligneCompte));

                            swTransactions.WriteLine(TraitementLigneTransaction(ligneTransaction));
                        }
                    }

                    // On passe les dates à la valeur max si on est à la fin du fichier et que la dernière ligne a été traitée.
                    if (lecComptes.EndOfStream && ligneCompteTraitee)
                    {
                        dateOperationCompte = DateTime.MaxValue;
                    }
                    if (lecTransactions.EndOfStream && ligneTransactionTraitee)
                    {
                        dateOperationTransaction = DateTime.MaxValue;
                    }
                }
            }
        }

        /// <summary>
        /// Réalise l'opération sur les comptes donnée en entrée et renvoie l'identifiant du compte et son statut en sortie.
        /// </summary>
        /// <param name="ligneCompte">Ligne du fichier des opérations sur les comptes à tester.</param>
        /// <returns>Une chaîne de caractères contenant l'identifiant du compte et le statut de l'opération (OK ou KO).</returns>
        private string TraitementLigneCompte(string[] ligneCompte)
        {
            if (TraiterOperationCompte(ligneCompte))
            {
                return $"{ligneCompte[0]};OK";
            }
            else
            {
                if (uint.TryParse(ligneCompte[0], out uint identifiant))
                {
                    return $"{identifiant};KO";
                }
                else
                {
                    return "Identifiant illisible;KO";
                }
            }
        }

        /// <summary>
        /// Réalise la transaction donnée en entrée et renvoie l'identifiant de la transaction et son statut en sortie.
        /// </summary>
        /// <param name="ligneTransaction">Ligne du fichier des transactions à tester.</param>
        /// <returns>Une chaîne de caractères contenant l'identifiant de la transaction et son statut (OK ou KO).</returns>
        private string TraitementLigneTransaction(string[] ligneTransaction)
        {
            if (TraiterOperationTransaction(ligneTransaction))
            {
                return $"{ligneTransaction[0]};OK";
            }
            else
            {
                if (uint.TryParse(ligneTransaction[0], out uint identifiant))
                {
                    return $"{identifiant};KO";
                }
                else
                {
                    return "Identifiant illisible;KO";
                }
            }
        }

        /// <summary>
        /// Traitement d'une opération sur les comptes décrite en entrée.
        /// </summary>
        /// <param name="ligneFichier">Ligne du fichier des opérations sur les comptes à exécuter.</param>
        /// <returns>True si l'opération s'est bien passée et False sinon.</returns>
        private bool TraiterOperationCompte(string[] ligneFichier)
        {
            // Vérification des champs du fichier
            if (ligneFichier.Length != 7)
            {
                return false;
            }
            // Tentative de conversion de l'identifiant et vérification que ce n'est pas l'identifiant réservé à la console.
            if (!uint.TryParse(ligneFichier[0], out uint identifiant) || identifiant == 0)
            {
                return false;
            }
            // Tentative de conversion de la date.
            if (!DateTime.TryParseExact(ligneFichier[2], "d", CultureInfo.GetCultureInfo("fr-FR"), DateTimeStyles.None, out DateTime dateOperation))
            {
                return false;
            }
            decimal solde;
            // Si le champ correspondant au solde est vide, on utilise la valeur par défaut du solde.
            if (ligneFichier[3] == string.Empty)
            {
                solde = 0;
            }
            // Tentative de conversion du solde en autorisant les virgules et en considérant le point comme le séparateur décimal, on vérifie également
            // que le solde est positif.
            else if (!decimal.TryParse(ligneFichier[3], NumberStyles.AllowDecimalPoint, CultureInfo.GetCultureInfo("en-US"), out solde)
                     || solde < 0)
            {
                return false;
            }

            uint gestionnaireEntree;
            uint gestionnaireSortie;
            // Vérification du gestionnaire d'entrée et de sortie et détermination de l'opération à réaliser.
            // Cas sans signification.
            if (ligneFichier[5] == string.Empty && ligneFichier[6] == string.Empty)
            {
                return false;
            }
            // Cas de création de compte.
            else if (ligneFichier[5] != string.Empty && ligneFichier[6] == string.Empty && uint.TryParse(ligneFichier[5], out gestionnaireEntree))
            {
                int age = 0;
                if (ligneFichier[1] == string.Empty || ligneFichier[1] != "C" && ligneFichier[1] != "J" && ligneFichier[1] != "L" && ligneFichier[1] != "T")
                {
                    return false;
                }
                if (ligneFichier[1] == "J" && (!int.TryParse(ligneFichier[4], out age) || age < 8 || age > 17)) 
                {
                    return false;
                }
                if (CreationCompte(gestionnaireEntree, identifiant, char.Parse(ligneFichier[1]), dateOperation, solde, age))
                {
                    _compteurCompteCree++;
                    return true;
                }
                return false;
            }
            // Cas de cloture de compte.
            else if (ligneFichier[6] != string.Empty && ligneFichier[5] == string.Empty && uint.TryParse(ligneFichier[6], out gestionnaireSortie))
            {
                if (_comptes.ContainsKey(identifiant) && _comptes[identifiant] is CompteTerme compteTerme && compteTerme.Age < 5) 
                {
                    return false;
                }
                return ClotureCompte(gestionnaireSortie, identifiant, dateOperation);
            }
            // Cas de transfert de compte.
            else if (ligneFichier[5] != string.Empty && ligneFichier[6] != string.Empty && uint.TryParse(ligneFichier[5], out gestionnaireEntree)
                                                                                        && uint.TryParse(ligneFichier[6], out gestionnaireSortie))
            {
                return TransfertCompte(gestionnaireEntree, gestionnaireSortie, identifiant, dateOperation);
            }
            return false;
        }

        /// <summary>
        /// Crée un compte dans le gestionnaire donné.
        /// </summary>
        /// <param name="idGestionnaire">Identifiant du gestionnaire où le compte va être créer.</param>
        /// <param name="idCompte">Identifiant du compte à créer.</param>
        /// <param name="dateCreation">Date de création du compte.</param>
        /// <param name="solde">Solde initial du compte.</param>
        /// <returns>True si la création a réussi et False sinon.</returns>
        private bool CreationCompte(uint idGestionnaire, uint idCompte, char typeCompte, DateTime dateCreation, decimal solde, int ageCompte)
        {
            if (_comptes.ContainsKey(idCompte) || !_gestionnaires.ContainsKey(idGestionnaire))
            {
                return false;
            }

            Compte compte;

            if (typeCompte == 'C')
            {
                compte = new Compte(idCompte, dateCreation, _gestionnaires[idGestionnaire].NombreTransactionsRetraitMax, solde);
            }
            else if (typeCompte == 'J')
            {
                compte = new CompteJeune(idCompte, dateCreation, _gestionnaires[idGestionnaire].NombreTransactionsRetraitMax, ageCompte, solde);
            }
            else if (typeCompte == 'L')
            {
                compte = new Livret(idCompte, dateCreation, _gestionnaires[idGestionnaire].NombreTransactionsRetraitMax, solde);
            }
            else if (typeCompte == 'T')
            {
                compte = new CompteTerme(idCompte, dateCreation, _gestionnaires[idGestionnaire].NombreTransactionsRetraitMax, solde);
            }
            else
            {
                return false;
            }

            if (!_gestionnaires[idGestionnaire].CreationCompte(compte))
            {
                return false;
            }

            _comptes.Add(idCompte, compte);
            _liaisonComptesGestionnaires.Add(idCompte, idGestionnaire);
            return true;
        }

        /// <summary>
        /// Cloture un compte donné dans son gestionnaire correspondant.
        /// </summary>
        /// <param name="idGestionnaire">Identifiant du gestionnaire où le compte va être cloturé.</param>
        /// <param name="idCompte">Identifiant du compte à cloturer.</param>
        /// <param name="dateCloture">Date de cloturation du compte.</param>
        /// <returns>True si la cloture a réussi et False sinon.</returns>
        private bool ClotureCompte(uint idGestionnaire, uint idCompte, DateTime dateCloture)
        {
            if (!_comptes.ContainsKey(idCompte) || !_gestionnaires.ContainsKey(idGestionnaire))
            {
                return false;
            }
            if (!_gestionnaires[idGestionnaire].ClotureCompte(idCompte,dateCloture))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Réalise l'échange d'un compte entre deux gestionnaires.
        /// </summary>
        /// <param name="idGestionnaireEntree">Identifiant du gestionnaire possédant le compte.</param>
        /// <param name="idGestionnaireSortie">Identifiant du gestionnaire recevant le compte.</param>
        /// <param name="idCompte">Identifiant du compte à échanger</param>
        /// <param name="dateEchange">Date de l'échange du compte.</param>
        /// <returns>True si l'échange a réussi et False sinon.</returns>
        private bool TransfertCompte(uint idGestionnaireEntree, uint idGestionnaireSortie, uint idCompte, DateTime dateEchange)
        {
            if (!_comptes.ContainsKey(idCompte) || !_gestionnaires.ContainsKey(idGestionnaireEntree) 
                                                || !_gestionnaires.ContainsKey(idGestionnaireSortie)
                                                || _liaisonComptesGestionnaires[idCompte] != idGestionnaireEntree) 
            {
                return false;
            }
            Compte compte = new Compte(_comptes[idCompte]);
            compte.DateCreation = dateEchange;
            compte.NombreTransactionsRetraitMax = _gestionnaires[idGestionnaireSortie].NombreTransactionsRetraitMax;
            if (!_gestionnaires[idGestionnaireEntree].ClotureCompte(idCompte, dateEchange) || !_gestionnaires[idGestionnaireSortie].CreationCompte(compte)) 
            {
                return false;
            }
            _liaisonComptesGestionnaires[idCompte] = idGestionnaireSortie;
            _comptes[idCompte] = compte;
            return true;
        }

        /// <summary>
        /// Traitement d'une transaction décrite en entrée.
        /// </summary>
        /// <param name="ligneFichier">Ligne du fichier des transactions à exécuter.</param>
        /// <returns>True si l'opération s'est bien passée et False sinon.</returns>
        private bool TraiterOperationTransaction(string[] ligneFichier)
        {
            // Liste des identifiants déjà vu dans le fichier.
            List<uint> identifiantsTransactions = new List<uint>();

            // Vérification des champs du fichier.
            if (ligneFichier.Length != 5)
            {
                _compteurTransactions++;
                _compteurEchec++;
                return false;
            }
            // On sépare le test d'unicité et le test de conversion car on affiche pas la même valeur en sortie selon l'erreur
            if (!uint.TryParse(ligneFichier[0], out uint identifiant))
            {
                _compteurTransactions++;
                _compteurEchec++;
                return false;
            }
            if (identifiantsTransactions.Contains(identifiant))
            {
                _compteurTransactions++;
                _compteurEchec++;
                return false;
            }
            // Tentative de conversion de la date d'effet de la transaction.
            if (!DateTime.TryParseExact(ligneFichier[1], "d", CultureInfo.GetCultureInfo("fr-FR"), DateTimeStyles.None, out DateTime dateEffet))
            {
                _compteurTransactions++;
                _compteurEchec++;
                return false;
            }
            // Tentative de conversion du montant de la transaction en autorisant les virgules et en considérant le point comme
            // le séparateur décimal, on vérifie également que le montant est strictement positif.
            if (!decimal.TryParse(ligneFichier[2], NumberStyles.AllowDecimalPoint, CultureInfo.GetCultureInfo("en-US"), out decimal montant)
                || montant <= 0)
            {
                _compteurTransactions++;
                _compteurEchec++;
                return false;
            }
            if (!uint.TryParse(ligneFichier[3], out uint expediteur) || !ExistenceCompte(expediteur))
            {
                _compteurTransactions++;
                _compteurEchec++;
                return false;
            }
            if (!uint.TryParse(ligneFichier[4], out uint destinataire) || !ExistenceCompte(destinataire))
            {
                _compteurTransactions++;
                _compteurEchec++;
                return false;
            }

            Transaction transaction;
            // Le constructeur de transaction peut remonter une exception si le montant est nul.
            try
            {
                transaction = new Transaction(identifiant, dateEffet, montant, expediteur, destinataire);
            }
            catch (ArgumentException)
            {
                _compteurTransactions++;
                _compteurEchec++;
                return false;
            }

            if (PrelevementSansExpediteur(transaction))
            {
                if (_comptes[destinataire] is CompteJeune compteJeune && compteJeune.Age >= 18)
                {
                    ConvertirCompteJeune(destinataire);
                }
                _comptes[destinataire].Prelevement(montant);
                identifiantsTransactions.Add(identifiant);
                _totalReussite += montant;
                _compteurTransactions++;
                _compteurReussite++;
                return true;
            }
            else if (VirementSansDestinataire(transaction))
            {
                if (_comptes[expediteur] is CompteJeune compteJeune && compteJeune.Age >= 18)
                {
                    ConvertirCompteJeune(expediteur);
                }
                if (_comptes[expediteur] is Livret)
                {
                    _compteurTransactions++;
                    _compteurEchec++;
                    return false;
                }
                if (_comptes[expediteur] is CompteTerme compteTerme && compteTerme.Age < 5)
                {
                    _compteurTransactions++;
                    _compteurEchec++;
                    return false;
                }
                _comptes[expediteur].Virement(montant, dateEffet);
                identifiantsTransactions.Add(identifiant);
                _totalReussite += montant;
                _compteurTransactions++;
                _compteurReussite++;
                return true;
            }
            else if (TransactionEntreDeuxComptes(transaction))
            {
                if (_comptes[destinataire] is CompteJeune compteJeuneDest && compteJeuneDest.Age >= 18)
                {
                    ConvertirCompteJeune(destinataire);
                }
                if (_comptes[expediteur] is CompteJeune compteJeuneExpe && compteJeuneExpe.Age >= 18)
                {
                    ConvertirCompteJeune(expediteur);
                }
                if (_comptes[expediteur] is CompteTerme compteTerme && compteTerme.Age < 5)
                {
                    _compteurTransactions++;
                    _compteurEchec++;
                    return false;
                }
                decimal frais = CalculFrais(expediteur, destinataire, montant);
                if (frais >= montant)
                {
                    _compteurTransactions++;
                    _compteurEchec++;
                    return false;
                }
                _gestionnaires[_liaisonComptesGestionnaires[expediteur]].TotalFraisGestion += frais;
                _comptes[expediteur].Virement(montant, dateEffet);
                _comptes[destinataire].Prelevement(montant - frais);
                identifiantsTransactions.Add(identifiant);
                _totalReussite += montant;
                _compteurTransactions++;
                _compteurReussite++;
                return true;
            }
            else
            {
                _compteurTransactions++;
                _compteurEchec++;
                return false;
            }
        }

        private void ConvertirCompteJeune(uint compteId)
        {
            Compte compte = new Compte(_comptes[compteId]);
            _comptes[compteId] = compte;
            _gestionnaires[_liaisonComptesGestionnaires[compteId]].RemplacerCompte(compteId, compte);
        }

        /// <summary>
        /// Imprime dans un fichier le résumé des opérations et transactions réalisées jusqu'ici.
        /// </summary>
        /// <param name="statFilePath">Chemin vers le fichier de sortie contenant le résumé.</param>
        public void ImprimerCompteurs(string statFilePath)
        {
            using (FileStream fs = File.Create(statFilePath))
            using (StreamWriter sw = new StreamWriter(fs))
            {
                sw.WriteLine("Statistiques :");
                sw.WriteLine($"Nombre de comptes : {_compteurCompteCree}");
                sw.WriteLine($"Nombre de transactions : {_compteurTransactions}");
                sw.WriteLine($"Nombre de réussites : {_compteurReussite}");
                sw.WriteLine($"Nombre d'échecs : {_compteurEchec}");
                sw.WriteLine($"Montant total des réussites : {_totalReussite:F2} euros");
                sw.WriteLine();
                sw.WriteLine("Frais de gestions :");

                foreach (Gestionnaire gestionnaire in _gestionnaires.Values)
                {
                    sw.WriteLine($"{gestionnaire.Identifiant} : {gestionnaire.TotalFraisGestion:F2} euros");
                }

                if (_comptes.Values.OfType<Livret>().Count() != 0 || _comptes.Values.OfType<CompteTerme>().Count() != 0)
                {
                    sw.WriteLine();
                    sw.WriteLine("Intérêts :");

                    foreach (Compte compte in _comptes.Values) 
                    {
                        if (compte is Livret livret)
                        {
                            livret.CalculerInteretCloture();
                            sw.WriteLine($"{compte.Identifiant} : {livret.Interets:F2} euros");
                        }
                        else if (compte is CompteTerme compteTerme)
                        {
                            compteTerme.CalculerInteretCloture();
                            sw.WriteLine($"{compte.Identifiant} : {compteTerme.Interets:F2} euros");
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Pour un virement entre deux comptes, détermine si des frais de gestion sont à payer.
        /// </summary>
        /// <param name="idCompteEntree">Identifiant du compte expéditeur.</param>
        /// <param name="idCompteSortie">Identifiant du compte destinataire.</param>
        /// <param name="montant">Valeur du virement.</param>
        /// <returns>Un nombre correspondant aux frais de gestion à payer ou 0 si la transaction n'en cause aucun.</returns>
        private decimal CalculFrais(uint idCompteEntree, uint idCompteSortie, decimal montant)
        {
            if (idCompteEntree == 0 || _liaisonComptesGestionnaires[idCompteEntree] == _liaisonComptesGestionnaires[idCompteSortie])
            {
                return 0;
            }
            if (_gestionnaires[_liaisonComptesGestionnaires[idCompteEntree]].Type == "Particulier")
            {
                return montant / 100;
            }
            if (_gestionnaires[_liaisonComptesGestionnaires[idCompteEntree]].Type == "Entreprise")
            {
                return 10;
            }
            return -1;
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
