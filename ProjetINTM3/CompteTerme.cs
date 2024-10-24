using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetINTM3
{
    class CompteTerme : Compte
    {
        public DateTime DateDernierCalculInteret { get; private set; }
        public TimeSpan PeriodeAvantCalculInteret { get; private set; }
        public decimal SoldeCalculInteret { get; private set; }
        public decimal Interets { get; private set; }
        public DateTime DateDerniereTransaction { get; set; }
        public int Age { get; private set; }
        public DateTime DateDernierAnniversaire { get; private set; }
        public TimeSpan PeriodeAvantAnniversaire { get; private set; }
        private const decimal _interetAnnuel = 5;

        public CompteTerme(uint identifiant, DateTime dateCreation, int nbTransRetMax, decimal solde = 0) : base(identifiant, dateCreation, nbTransRetMax, solde)
        {
            if (solde < 200)
            {
                throw new ArgumentException("La création d'un compte à terme nécessite un solde initial supérieur ou égal à 200", "solde");
            }
            solde -= 200;
            Solde = solde;
            DateDernierCalculInteret = dateCreation;
            PeriodeAvantCalculInteret = TimeSpan.FromDays(DateTime.DaysInMonth(dateCreation.Year, dateCreation.Month) - dateCreation.Day + 1);
            SoldeCalculInteret = solde;
            DateDerniereTransaction = DateResiliation;
            Interets = 0;
            Age = 0;
            DateDernierAnniversaire = dateCreation;
            PeriodeAvantAnniversaire = DateDernierAnniversaire.AddYears(1) - DateDernierAnniversaire;
        }

        public CompteTerme(CompteTerme compteTerme) : base(compteTerme)
        {
            DateDernierCalculInteret = compteTerme.DateDernierCalculInteret;
            PeriodeAvantCalculInteret = compteTerme.PeriodeAvantCalculInteret;
            SoldeCalculInteret = compteTerme.SoldeCalculInteret;
            DateDerniereTransaction = compteTerme.DateDerniereTransaction;
            Interets = compteTerme.Interets;
            Age = compteTerme.Age;
            DateDernierAnniversaire = compteTerme.DateDernierAnniversaire;
            PeriodeAvantAnniversaire = compteTerme.PeriodeAvantAnniversaire;
        }

        public override bool PrelevementVerif(Transaction transaction)
        {
            if (transaction.DateEffet >= DateCreation && transaction.DateEffet < DateResiliation)
            {
                DateDerniereTransaction = transaction.DateEffet;

                while (DateDernierCalculInteret.CompareTo(transaction.DateEffet) < 0 && transaction.DateEffet - DateDernierCalculInteret >= PeriodeAvantCalculInteret)
                {
                    CalculerInteret();
                }
                while (DateDernierAnniversaire.CompareTo(transaction.DateEffet) < 0 && transaction.DateEffet - DateDernierAnniversaire >= PeriodeAvantAnniversaire)
                {
                    IncrementerAge();
                }
            }
            return base.PrelevementVerif(transaction);
        }

        public override bool VirementVerif(Transaction transaction)
        {
            if (transaction.DateEffet >= DateCreation && transaction.DateEffet < DateResiliation)
            {
                DateDerniereTransaction = transaction.DateEffet;

                while (DateDernierCalculInteret.CompareTo(transaction.DateEffet) < 0 && transaction.DateEffet - DateDernierCalculInteret >= PeriodeAvantCalculInteret)
                {
                    CalculerInteret();
                }
                while (DateDernierAnniversaire.CompareTo(transaction.DateEffet) < 0 && transaction.DateEffet - DateDernierAnniversaire >= PeriodeAvantAnniversaire)
                {
                    IncrementerAge();
                }
            }
            if (Age < 5)
            {
                return false;
            }
            return base.VirementVerif(transaction);
        }

        private void CalculerInteret()
        {
            decimal prorata = 1;
            if (DateDernierCalculInteret.Day != 1)
            {
                int numberOfDays = DateTime.DaysInMonth(DateDernierCalculInteret.Year, DateDernierCalculInteret.Month);
                prorata = Math.Round((decimal)(numberOfDays - DateDernierCalculInteret.Day + 1) / numberOfDays, 2);
            }
            decimal interet = Math.Round(SoldeCalculInteret * Math.Round(_interetAnnuel / 12, 2) / 100 * prorata, 2);

            Solde += interet;
            SoldeCalculInteret = Solde;
            Interets += interet;

            DateDernierCalculInteret += PeriodeAvantCalculInteret;
            PeriodeAvantCalculInteret = DateDernierCalculInteret.AddMonths(1) - DateDernierCalculInteret;
        }

        public void CalculerInteretCloture()
        {
            decimal prorata = 1;
            while (DateDerniereTransaction.Month != DateDernierCalculInteret.Month)
            {
                CalculerInteret();
            }
            if (DateDerniereTransaction.Day != 1 && DateDernierCalculInteret.Day == 1)
            {
                int numberOfDays = DateTime.DaysInMonth(DateDerniereTransaction.Year, DateDerniereTransaction.Month);
                prorata = Math.Round((decimal)(DateDerniereTransaction.Day - 1) / numberOfDays, 2);
            }
            else if (DateDerniereTransaction.Day != 1 && DateDernierCalculInteret.Day != 1)
            {
                int numberofDays = DateTime.DaysInMonth(DateDerniereTransaction.Year, DateDerniereTransaction.Month);
                prorata = Math.Round((decimal)((DateDerniereTransaction - DateDernierCalculInteret).Days - 1) / numberofDays, 2);
            }
            decimal interet = Math.Round(SoldeCalculInteret * Math.Round(_interetAnnuel / 12, 2) / 100 * prorata, 2);

            Solde += interet;
            SoldeCalculInteret = Solde;
            Interets += interet;

            DateDernierCalculInteret = DateDerniereTransaction;
            PeriodeAvantCalculInteret = TimeSpan.FromDays(DateTime.DaysInMonth(DateDerniereTransaction.Year, DateDerniereTransaction.Month) - DateDerniereTransaction.Day + 1);
        }

        private void IncrementerAge()
        {
            Age++;
            if (Age == 5)
            {
                decimal interetSpecial = Solde * 0.1m;
                Solde += interetSpecial;
                Interets += interetSpecial;
            }

            DateDernierAnniversaire += PeriodeAvantAnniversaire;
            PeriodeAvantAnniversaire = DateDernierAnniversaire.AddYears(1) - DateDernierAnniversaire;
        }
    }
}
