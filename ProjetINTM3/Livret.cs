using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetINTM3
{
    class Livret : Compte
    {
        public DateTime DateDernierCalculInteret { get; private set; }
        public TimeSpan PeriodeAvantCalculInteret { get; private set; }
        public decimal SoldeCalculInteret { get; private set; }
        public DateTime DateDerniereTransaction { get; set; }
        private const double _interetAnnuel = 0.02;

        public Livret(uint identifiant, DateTime dateCreation, int nbTransRetMax, decimal solde = 0) : base(identifiant, dateCreation, nbTransRetMax, solde)
        {
            DateDernierCalculInteret = dateCreation;
            PeriodeAvantCalculInteret = new TimeSpan(DateTime.DaysInMonth(dateCreation.Year, dateCreation.Month) - dateCreation.Day, 0, 0, 0);
            SoldeCalculInteret = solde;
            DateDerniereTransaction = new DateTime();
        }

        public Livret(Livret livret) : base(livret)
        {
            DateDernierCalculInteret = livret.DateDernierCalculInteret;
            PeriodeAvantCalculInteret = livret.PeriodeAvantCalculInteret;
            SoldeCalculInteret = livret.SoldeCalculInteret;
            DateDerniereTransaction = livret.DateDerniereTransaction;
        }
    }
}
