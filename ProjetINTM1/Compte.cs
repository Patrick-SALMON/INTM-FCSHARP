using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetINTM1
{
    class Compte
    {
        public uint Identifiant { get; private set; }
        public decimal Solde { get; private set; }
        private const decimal _retraitMax = 1000;
        private readonly List<decimal> _historiqueVirement;

        public Compte(uint identifiant, decimal solde = 0)
        {
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

        public bool PrelevementVerif(Transaction transaction)
        {
            if (transaction.Montant <= 0)
            {
                return false;
            }
            return true;
        }

        public void Prelevement(decimal montant)
        {
            Solde += montant;
        }

        public bool VirementVerif(Transaction transaction)
        {
            if (transaction.Montant <= 0 || transaction.Montant > Solde || RetraitMaxAtteint(transaction.Montant))
            {
                return false;
            }
            return true;
        }

        public void Virement(decimal montant)
        {
            Solde -= montant;
            AjoutAHistorique(montant);
        }

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
    }
}
