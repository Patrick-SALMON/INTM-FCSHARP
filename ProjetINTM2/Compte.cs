using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetINTM2
{
    /// <summary>
    /// Définit un compte bancaire selon son identifiant unique et strictement positif et son solde positif.
    /// </summary>
    class Compte
    {
        public uint Identifiant { get; private set; }
        public decimal Solde { get; private set; }
        public DateTime DateCreation { get; set; }
        public DateTime DateResiliation { get; set; }
        public int NombreTransactionsRetraitMax { get; set; }

        private const decimal _retraitMax = 1000;
        // La période est fixée à 7 jours donc une semaine.
        private readonly TimeSpan _periodeRetraitMaxTemporelle = new TimeSpan(7, 0, 0, 0);
        private const decimal _retraitMaxTemporelle = 2000;
        private readonly List<decimal> _historiqueVirement;
        private readonly List<KeyValuePair<decimal,DateTime>> _historiqueTemporel;

        /// <summary>
        /// Définition d'un nouveau compte bancaire.
        /// </summary>
        /// <param name="identifiant">Identifiant unique et strictement positif à donner au compte.</param>
        /// <param name="solde">Solde initial positif du compte.</param>
        /// <exception cref="ArgumentException"></exception>
        public Compte(uint identifiant, DateTime dateCreation, int nbTransRetMax, decimal solde = 0)
        {
            if (identifiant == 0)
            {
                throw new ArgumentException("L'identifiant d'un compte ne peut pas être 0", "identifiant");
            }

            Identifiant = identifiant;

            DateCreation = dateCreation;

            // On considère que le compte existera pour toujours tant qu'on n'a pas spécifié de date de résiliation.
            DateResiliation = DateTime.MaxValue;

            if (nbTransRetMax <= 0)
            {
                throw new ArgumentException("Le nombre de virements considéré pour le maximum du retrait ne peut pas être 0 ou négatif", "nbTransRetMax");
            }

            NombreTransactionsRetraitMax = nbTransRetMax;

            if (solde < 0)
            {
                Solde = 0;
            }
            else
            {
                Solde = solde;
            }

            _historiqueVirement = new List<decimal>();
            _historiqueTemporel = new List<KeyValuePair<decimal,DateTime>>();
        }

        /// <summary>
        /// Constructeur de copie servant à l'échange de compte.
        /// </summary>
        /// <param name="compte">Compte à copier.</param>
        public Compte(Compte compte)
        {
            Identifiant = compte.Identifiant;
            Solde = compte.Solde;
            DateCreation = compte.DateCreation;
            DateResiliation = compte.DateResiliation;
            NombreTransactionsRetraitMax = compte.NombreTransactionsRetraitMax;
            _historiqueVirement = compte._historiqueVirement;
            _historiqueTemporel = compte._historiqueTemporel;
        }

        /// <summary>
        /// Vérifie qu'un prélèvement est possible.
        /// </summary>
        /// <param name="transaction">Transaction à vérifier.</param>
        /// <returns>True si le prélèvement est autorisé et False sinon.</returns>
        public bool PrelevementVerif(Transaction transaction)
        {
            if (transaction.Montant <= 0 || transaction.DateEffet >= DateResiliation || transaction.DateEffet < DateCreation)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Effectue un prélèvement d'un certain montant.
        /// </summary>
        /// <param name="montant">Montant à prélever.</param>
        public void Prelevement(decimal montant)
        {
            Solde += montant;
        }

        /// <summary>
        /// Vérifie qu'un virement peut bien être fait.
        /// </summary>
        /// <param name="transaction">Transaction à vérifier.</param>
        /// <returns>True si le virement est autorisé et False sinon.</returns>
        public bool VirementVerif(Transaction transaction)
        {
            if (!VirementPossible(transaction))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Effectue un virement d'un certain montant.
        /// </summary>
        /// <param name="montant">Montant à virer.</param>
        public void Virement(decimal montant, DateTime dateEffet)
        {
            Solde -= montant;
            AjoutAHistorique(montant);
            AjoutAHistoriqueTemporel(montant, dateEffet);
        }

        /// <summary>
        /// Vérifie si la limite de retrait a été atteinte ou non.
        /// </summary>
        /// <param name="valVirement">Montant qu'on essaye de virer.</param>
        /// <returns>True si la limite a été atteinte et False sinon.</returns>
        private bool RetraitMaxAtteint(decimal valVirement)
        {
            decimal totalVirements = valVirement;

            for (int i = 0; i < NombreTransactionsRetraitMax - 1 && i < _historiqueVirement.Count(); i++) 
            {
                totalVirements += _historiqueVirement[i];
            }

            if (totalVirements > _retraitMax)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Vérifie si la limite de retrait temporelle a été atteinte ou non.
        /// </summary>
        /// <param name="valVirement">Montant qu'on essayer de virer.</param>
        /// <param name="dateEffet">Date du virement.</param>
        /// <returns>True si la limite a été atteinte et False sinon.</returns>
        private bool RetraitMaxTemporelAtteint(decimal valVirement, DateTime dateEffet)
        {
            decimal totalVirements = valVirement;

            foreach (KeyValuePair<decimal,DateTime> virement in _historiqueTemporel)
            {
                if (virement.Value <= dateEffet && dateEffet - virement.Value < _periodeRetraitMaxTemporelle &&
                    virement.Value >= DateCreation && virement.Value < DateResiliation)
                {
                    totalVirements += virement.Key;
                }
            }

            if (totalVirements > _retraitMaxTemporelle)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Rajoute le montant d'une transaction de virement à l'historique des virements. La taille maximale de l'historique est de 9.
        /// </summary>
        /// <param name="valVirement">Valeur du virement à rajouter.</param>
        private void AjoutAHistorique(decimal valVirement)
        {
            if (NombreTransactionsRetraitMax != 1)
            {
                // -1 car on considère toujours le virement actuel quand on vérifie si le maximum est atteint.
                if (_historiqueVirement.Count() < NombreTransactionsRetraitMax - 1)
                {
                    _historiqueVirement.Add(valVirement);
                }
                else if (_historiqueVirement.Count() == NombreTransactionsRetraitMax - 1)
                {
                    _historiqueVirement.RemoveAt(0);
                    _historiqueVirement.Add(valVirement);
                }
                else
                {
                    while (_historiqueVirement.Count() >= NombreTransactionsRetraitMax - 1)
                    {
                        _historiqueVirement.RemoveAt(0);
                    }
                    _historiqueVirement.Add(valVirement);
                }
            }
        }

        /// <summary>
        /// Rajoute un virement à l'historique temporel.
        /// </summary>
        /// <param name="valVirement">Valeur du virement à rajouter.</param>
        /// <param name="dateEffet">Date d'effet du virement.</param>
        private void AjoutAHistoriqueTemporel(decimal valVirement, DateTime dateEffet)
        {
            _historiqueTemporel.Add(new KeyValuePair<decimal, DateTime>(valVirement, dateEffet));
        }

        /// <summary>
        /// Vérifie qu'un virement est possible
        /// </summary>
        /// <param name="transaction">Transaction correspondant au virement à vérifier.</param>
        /// <returns>True si le virement est autorisé et False sinon.</returns>
        private bool VirementPossible(Transaction transaction)
        {
            return transaction.Montant > 0 && transaction.Montant <= Solde && !RetraitMaxAtteint(transaction.Montant) 
                                                                           && !RetraitMaxTemporelAtteint(transaction.Montant,transaction.DateEffet)
                                                                           && transaction.DateEffet < DateResiliation && transaction.DateEffet >= DateCreation;
        }
    }
}
