using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetINTM3
{
    /// <summary>
    /// Particulier ou Entreprise possédant un ou plusieurs comptes dans la banque.
    /// </summary>
    class Gestionnaire
    {
        public uint Identifiant { get; private set; }
        public string Type { get; private set; }
        public int NombreTransactionsRetraitMax { get; private set; }
        public decimal TotalFraisGestion { get; set; }
        
        private readonly Dictionary<uint, Compte> _comptes;

        /// <summary>
        /// Initialisation du gestionnaire.
        /// </summary>
        /// <param name="identifiant">Identifiant unique du gestionnaire.</param>
        /// <param name="type">Type du gestionnaire, "Particulier" ou "Entreprise".</param>
        /// <param name="nbTransRetMax">Nombre de transactions que l'on considère pour la limite de retrait maximale.</param>
        /// <exception cref="ArgumentException"></exception>
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

        /// <summary>
        /// Ajout d'un compte dans le gestionnaire.
        /// </summary>
        /// <param name="compte">Objet compte à créer dans le gestionnaire.</param>
        /// <returns>True si la création a réussi et False sinon.</returns>
        public bool CreationCompte(Compte compte)
        {
            if (_comptes.ContainsKey(compte.Identifiant))
            {
                return false;
            }
            _comptes.Add(compte.Identifiant, compte);
            return true;
        }

        /// <summary>
        /// Cloturation d'un compte dans le gestionnaire.
        /// </summary>
        /// <param name="identifiant">Identifiant du compte à cloturer.</param>
        /// <param name="dateCloture">Date de cloture.</param>
        /// <returns>True si la cloturation a réussi et False sinon.</returns>
        public bool ClotureCompte(uint identifiant, DateTime dateCloture)
        {
            if (!CompteExistantEtActif(identifiant, dateCloture))
            {
                return false;
            }
            _comptes[identifiant].DateResiliation = dateCloture;
            if (_comptes[identifiant] is Livret livret)
            {
                livret.DateDerniereTransaction = dateCloture;
            }
            if (_comptes[identifiant] is CompteTerme compteTerme)
            {
                compteTerme.DateDerniereTransaction = dateCloture;
            }
            return true;
        }

        public bool RemplacerCompte(uint identifiant, Compte compte)
        {
            if (!_comptes.ContainsKey(identifiant)) 
            {
                return false;
            }
            _comptes[identifiant] = compte;
            return true;
        }

        /// <summary>
        /// Vérifie que le compte donné existe bien dans le gestionnaire et qu'il est encore actif.
        /// </summary>
        /// <param name="identifiant">Identifiant du compte à tester.</param>
        /// <param name="dateOperation">Date de l'opération qu'on traite.</param>
        /// <returns>True si le compte est existant et actif et False sinon.</returns>
        private bool CompteExistantEtActif(uint identifiant, DateTime dateOperation)
        {
            return _comptes.ContainsKey(identifiant) && dateOperation >= _comptes[identifiant].DateCreation && dateOperation < _comptes[identifiant].DateResiliation;
        }
    }
}
