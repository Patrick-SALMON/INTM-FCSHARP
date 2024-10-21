using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetINTM3
{
    class CompteJeune : Compte
    {
        public int Age { get; private set; }
        public DateTime DateDernierAnniversaire { get; private set; }
        public TimeSpan PeriodeAvantAnniversaire { get; private set; }

        public CompteJeune(uint identifiant, DateTime dateCreation, int nbTransRetMax, int age, decimal solde = 0) : base(identifiant, dateCreation, nbTransRetMax, solde)
        {
            if (age < 8 || age >= 18)
            {
                throw new ArgumentException("L'age du possesseur d'un compte jeune doit être compris entre 8 et 18", "age");
            }

            Age = age;
            Solde += 10 * Age;
            _retraitMax *= (decimal)Age / 18;
            _retraitMaxTemporelle *= (decimal)Age / 18;
            DateDernierAnniversaire = dateCreation;
            PeriodeAvantAnniversaire = DateDernierAnniversaire.AddYears(1) - DateDernierAnniversaire;
        }

        public CompteJeune(CompteJeune compteJeune) : base(compteJeune)
        {
            Age = compteJeune.Age;
            DateDernierAnniversaire = compteJeune.DateDernierAnniversaire;
            PeriodeAvantAnniversaire = compteJeune.PeriodeAvantAnniversaire;
        }

        public override bool PrelevementVerif(Transaction transaction)
        {
            while (Age < 18 && DateDernierAnniversaire.CompareTo(transaction.DateEffet) < 0 && transaction.DateEffet - DateDernierAnniversaire >= PeriodeAvantAnniversaire)
            {
                IncrementerAge();
            }
            return base.PrelevementVerif(transaction);
        }

        public override bool VirementVerif(Transaction transaction)
        {
            while (Age < 18 && DateDernierAnniversaire.CompareTo(transaction.DateEffet) < 0 && transaction.DateEffet - DateDernierAnniversaire >= PeriodeAvantAnniversaire)
            {
                IncrementerAge();
            }
            return base.VirementVerif(transaction);
        }

        private void IncrementerAge()
        {
            _retraitMax /= (decimal)Age / 18;
            _retraitMaxTemporelle /= (decimal)Age / 18;
            Age++;
            _retraitMax *= (decimal)Age / 18;
            _retraitMaxTemporelle *= (decimal)Age / 18;

            DateDernierAnniversaire += PeriodeAvantAnniversaire;
            PeriodeAvantAnniversaire = DateDernierAnniversaire.AddYears(1) - DateDernierAnniversaire;
        }
    }
}
