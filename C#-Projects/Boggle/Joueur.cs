using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_Final
{
    /// <summary>
    /// Classe qui définit un joueur
    /// </summary>
    public class Joueur
    {
        #region Variables d'instance

        string nom;

        /// <summary>
        /// Nom du joueur
        /// </summary>
        public string Nom
        {
            get { return nom; } // Non modifiable par la suite
        }

        int score;

        /// <summary>
        /// Score du joueur
        /// </summary>
        public int Score
        {
            get { return score; }
            set { score = value; }
        }

        List<string> mots;

        /// <summary>
        /// Liste des mots que le joueur a trouvé
        /// </summary>
        public List<string> Mots
        {
            get { return mots; }
            set { mots = value; }
        }
        #endregion

        /// <summary>
        /// Définit un joueur avec son nom, ses mots trouvés et son score
        /// </summary>
        /// <param name="nom"> Nom à donner au joueur</param>
        public Joueur(string nom)
        {
            if(nom != null)
            {
                this.nom = nom.ToUpper();
                this.score = 0;
                this.mots = new List<string>() ;
            }
            else
            {
                this.nom = null;
                this.score = 0;
                this.mots = null;
                Console.WriteLine("Erreur le nom ne convient pas");
            }
        }

        #region Methodes

        /// <summary>
        /// Vérifie si le mot en paramètre est déja inclus dans la liste des mots trouvés d'un joueur
        /// </summary>
        /// <param name="mot"> Mot à vérifier </param>
        /// <returns> Un booléen pour savoir si elle le contient (true) ou non (false)</returns>
        public bool Countain(string mot) //Fct countains 
        {
            /* Sans la fonction de List:
            bool test = false;
            for (int i = 0; i < mots.Count(); i++)
            {
                if(mot == mots[i])
                {
                    test = true;
                    break;
                }
            }
            return true;*/
            return mots.Contains(mot);
        }

        /// <summary>
        /// Permet d'ajouter un mot dans la liste des mots trouvés d'un joueur (si il n'est pas déjà dedans)
        /// </summary>
        /// <param name="mot"> Le mot à ajouter dans la liste </param>
        public void Add_Mot(string mot)
        {
            if (Countain(mot) == false)
            {
                mots.Add(mot);
            }
        }

        /// <summary>
        /// Permet d'avoir les infos d'un joueur
        /// </summary>
        /// <returns> Description détaillée du joueur </returns>
        public string toString()
        {
            string listemot = "";
            for(int i = 0; i< mots.Count(); i++)
            {
                listemot = listemot + mots[i] + " ";
            }
            return nom + " a " + score + " points. Il a trouvé les mots suivants : "+ listemot;
        }
        #endregion
    }
}
