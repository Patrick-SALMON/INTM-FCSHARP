using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetINTM3
{
    class CompteJeune : Compte
    {
        public int Age { get; set; }

        public CompteJeune(uint identifiant, DateTime dateCreation, int nbTransRetMax, int age, decimal solde = 0) : base(identifiant, dateCreation, nbTransRetMax, solde)
        {
            if (age < 8 || age >= 18)
            {
                throw new ArgumentException("L'age du possesseur d'un compte jeune doit être compris entre 8 et 18", "age");
            }

            Age = age;
        }

        public CompteJeune(CompteJeune compteJeune) : base(compteJeune)
        {
            Age = compteJeune.Age;
        }
    }
}
