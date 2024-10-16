using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetINTM2
{
    class Gestionnaire
    {
        public uint Identifiant { get; private set; }
        public string Type { get; private set; }
        public int NombreTransactionsRetraitMax { get; private set; }
        public decimal TotalFraisGestion { get; set; }
        
        private readonly Dictionary<uint, Compte> _comptes;

        public Gestionnaire(uint identifiant, string type, int nbTransRetMax)
        {
            Identifiant = identifiant;

            if (type != "Particulier" && type != "Entreprise")
            {
                throw new ArgumentException("Le type de gestionnaire est incorrect", "type");
            }

            Type = type;

            if (nbTransRetMax <= 0)
            {
                throw new ArgumentException("Le nombre de virements considéré pour le maximum du retrait ne peut pas être 0 ou négatif", "nbTransRetMax");
            }

            NombreTransactionsRetraitMax = nbTransRetMax;

            TotalFraisGestion = 0;
            _comptes = new Dictionary<uint, Compte>();
        }

        public bool CreationCompte(Compte compte)
        {
            if (_comptes.ContainsKey(compte.Identifiant))
            {
                return false;
            }
            _comptes.Add(compte.Identifiant, compte);
            return true;
        }

        public bool ClotureCompte(uint identifiant, DateTime dateCloture)
        {
            if (!CompteExistantEtActif(identifiant, dateCloture))
            {
                return false;
            }
            _comptes[identifiant].DateResiliation = dateCloture;
            return true;
        }

        private bool CompteExistantEtActif(uint identifiant, DateTime dateOperation)
        {
            return _comptes.ContainsKey(identifiant) && dateOperation >= _comptes[identifiant].DateCreation && dateOperation < _comptes[identifiant].DateResiliation;
        }
    }
}
