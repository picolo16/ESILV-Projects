using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Projet_Final
{
    /// <summary>
    /// Classe qui définit un dictionnaire
    /// </summary>
    public class Dictionnaire
    {
        #region Variables d'instance
        Dictionary<int, string[]> mots;
        /// <summary>
        /// Un dictionnaire dont les clés sont la taille des mots et les valeurs sont la liste des mots de cette taille
        /// </summary>
        public Dictionary<int, string[]> Mots
        {
            get { return mots; } // à priopris, pas besoin de set
        }
        #endregion

        /// <summary>
        /// Définit un dictionnaire à partir d'un fichier
        /// </summary>
        /// <param name="file"> Fichier à lire (Attention : ne pas oublier .txt)</param>
        public Dictionnaire(string file) // Langue ~ file: le file definira la langue
        {
            this.mots = new Dictionary<int, string[]>();
            try
            {
                string[] tab = File.ReadAllLines(file);
                for(int i =3; i<=15; i++) //Sauter les éléments inutiles ("3","4", etc...) et aussi pour commencer aux mots de taille 3
                {   
                    string contents = tab[(i-3)+i]; // J'ai regardé les suites de nombre et j'ai trouvé la logique qui le vérifait
                    string[] mots = contents.Split(' ');
                    this.mots.Add(i, mots);
                }
            }
                #region Exceptions
                catch (ArgumentException f)
                {
                    Console.WriteLine(f.Message);
                }
                catch (IOException f)
                {
                    Console.WriteLine(f.Message);
                }
                catch (UnauthorizedAccessException f)
                {
                    Console.WriteLine(f.Message);
                }
                catch (NotSupportedException f)
                {
                    Console.WriteLine(f.Message);
                }
                #endregion  
        }

        #region Methodes

        /// <summary>
        /// Décrit le dictionnaire (nombre de mots dans le dico, par taille)
        /// </summary>
        /// <returns> Le message descriptif </returns>
        public string toString()
        {
            string message = "";
            int[] keys = new int[mots.Keys.Count()];
            mots.Keys.CopyTo(keys, 0);
            for(int i =0; i<keys.Length; i++)
            {
                message = message + "Il y a " + mots[keys[i]].Length + " mots de taille " + keys[i] + " dans le dictionnaire\n";
            }
            return message;
        }

        /// <summary>
        /// Cherche si le mot précisé en entrée figure dans le dictionnaire (Algo récursif)
        /// </summary>
        /// <param name="mot">Le mot à tester </param>
        /// <param name="debut"> Index de début de recherche (initialisé à 0)</param>
        /// <param name="fin"> Index de fin de recherche (initialisé à -1, puis directement à la taille du mot) </param>
        /// <returns> Un booléen pour savoir s'il est présent (true) ou non (false) </returns>
        public bool RechDichoRecursif(string mot, int debut = 0, int fin = -1)
        {
            if (mot.Length - 1 >= 2 && mot.Length - 1 <= 15)
            {
                if (fin == -1)
                {
                    fin = mots[mot.Length].Length - 1;
                }
                int m = (debut + fin) / 2;
                if (debut > fin)
                    return false;
                if (mot == mots[mot.Length][m])
                    return true;
                if (Comparatif(mot, mots[mot.Length][m]) == 1)
                    return RechDichoRecursif(mot, m + 1, fin);
                if (Comparatif(mot, mots[mot.Length][m]) == -1)
                    return RechDichoRecursif(mot, debut, m - 1);
                else
                    return false;
            }
            else
                return false;
        }

        /// <summary>
        /// Opérateur de comparaison de 2 mots par leur char. Leur longueur est la même
        /// </summary>
        /// <param name="mot1"></param>
        /// <param name="mot2"></param>
        /// <returns>Si (mot 1) > (mot 2) alors on retourne 1, si (mot 2) > (mot 1) on retourne -1, sinon on retourne 0</returns>
        public static int Comparatif(string mot1, string mot2) 
        {
            int i = 0;
            while(mot1[i]==mot2[i] && i<mot1.Length)
            {
                i++;
            }
            if(i == mot1.Length)
                return 0;
            else
                return mot1[i] > mot2[i] ? 1 : -1;
        }
        #endregion
    }
}
