using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetINTM1
{
    class Transaction
    {
        public uint Identifiant { get; private set; }
        public decimal Montant { get; private set; }
        public uint Expediteur { get; private set; }
        public uint Destinataire { get; private set; }

        public Transaction(uint identifiant, decimal montant, uint expediteur, uint destinataire)
        {
            Identifiant = identifiant;
            if (montant < 0)
            {
                Montant = -montant;
            }
            else if (montant == 0)
            {
                throw new ArgumentException("Le montant d'une transaction ne peut pas etre nul et doit etre strictement positif", "montant");
            }
            else
            {
                Montant = montant;
            }
            Expediteur = expediteur;
            Destinataire = destinataire;
        }
    }
}
