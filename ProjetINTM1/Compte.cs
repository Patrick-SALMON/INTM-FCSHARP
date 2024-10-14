using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetINTM1
{
    /// <summary>
    /// Définit un compte bancaire selon son identifiant unique et strictement positif et son solde positif.
    /// </summary>
    class Compte
    {
        public uint Identifiant { get; private set; }
        public decimal Solde { get; private set; }
        private const decimal _retraitMax = 1000;
        private readonly List<decimal> _historiqueVirement;

        /// <summary>
        /// Définition d'un nouveau compte bancaire.
        /// </summary>
        /// <param name="identifiant">Identifiant unique et strictement positif à donner au compte.</param>
        /// <param name="solde">Solde initial positif du compte.</param>
        /// <exception cref="ArgumentException"></exception>
        public Compte(uint identifiant, decimal solde = 0)
        {
            if (identifiant == 0)
            {
                throw new ArgumentException("L'identifiant d'un compte ne peut pas être 0", "identifiant");
            }

            Identifiant = identifiant;

            if (solde < 0)
            {
                Solde = 0;
            }
            else
            {
                Solde = solde;
            }
            _historiqueVirement = new List<decimal>();
        }

        /// <summary>
        /// Vérifie qu'un prélèvement est possible.
        /// </summary>
        /// <param name="transaction">Transaction à vérifier.</param>
        /// <returns>True si le prélèvement est autorisé et False sinon.</returns>
        public bool PrelevementVerif(Transaction transaction)
        {
            if (transaction.Montant <= 0)
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
        public void Virement(decimal montant)
        {
            Solde -= montant;
            AjoutAHistorique(montant);
        }

        /// <summary>
        /// Vérifie si la limite de retrait a été atteinte ou non.
        /// </summary>
        /// <param name="valVirement">Montant qu'on essaye de virer.</param>
        /// <returns>True si la limite a été atteinte et False sinon.</returns>
        private bool RetraitMaxAtteint(decimal valVirement)
        {
            decimal totalVirements = valVirement;

            for (int i = 0; i < _historiqueVirement.Count(); i++)
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
        /// Rajoute le montant d'une transaction de virement à l'historique des virements. La taille maximale de l'historique est de 9.
        /// </summary>
        /// <param name="valVirement">Valeur du virement à rajouter.</param>
        private void AjoutAHistorique(decimal valVirement)
        {
            if (_historiqueVirement.Count() < 9)
            {
                _historiqueVirement.Add(valVirement);
            }
            else
            {
                _historiqueVirement.RemoveAt(0);
                _historiqueVirement.Add(valVirement);
            }
        }

        /// <summary>
        /// Vérifie qu'un virement est possible
        /// </summary>
        /// <param name="transaction">Transaction correspondant au virement à vérifier.</param>
        /// <returns>True si le virement est autorisé et False sinon.</returns>
        private bool VirementPossible(Transaction transaction)
        {
            return transaction.Montant > 0 && transaction.Montant <= Solde && !RetraitMaxAtteint(transaction.Montant);
        }
    }
}
