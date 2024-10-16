using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetINTM2
{
    /// <summary>
    /// Correspond à une transaction monétaire entre deux comptes ou avec la console si l'un des comptes est 0.
    /// </summary>
    class Transaction
    {
        public uint Identifiant { get; private set; }
        public decimal Montant { get; private set; }
        public uint Expediteur { get; private set; }
        public uint Destinataire { get; private set; }
        public DateTime DateEffet { get; private set; }

        /// <summary>
        /// Définition d'une nouvelle transaction. montant doit être non nul. Si montant est négatif, sa valeur absolue est utilisée.
        /// </summary>
        /// <param name="identifiant">Identifiant de la transaction.</param>
        /// <param name="montant">Montant de la transaction.</param>
        /// <param name="expediteur">L'expéditeur de la transaction, c'est un identifiant de compte.</param>
        /// <param name="destinataire">Le destinataire de la transaction, c'est un identifiant de compte.</param>
        /// <exception cref="ArgumentException"></exception>
        public Transaction(uint identifiant, DateTime dateEffet, decimal montant, uint expediteur, uint destinataire)
        {
            Identifiant = identifiant;
            DateEffet = dateEffet;

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
