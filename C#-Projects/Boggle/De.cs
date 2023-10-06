using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Projet_Final
{
    /// <summary>
    /// Classe qui définit un dé
    /// </summary>
    public class De
    {
        #region Variables d'instance
        private char[] lettres;
        /// <summary>
        /// Tableau comprenant le contenu du dé (toutes les faces)
        /// </summary>
        public char[] Lettres // Même pas forcement utile car le toStrong fait l'affaire
        {
            get { return lettres; }
        }
        private char face;
        /// <summary>
        /// Face utilisée du dé
        /// </summary>
        public char Face
        {
            get { return face; }
        }
        #endregion

        /// <summary>
        /// Définit un dé à 6 faces à partir d'un fichier, chaque numéro à des faces prédéfinis
        /// </summary>
        /// <param name="numero"> Le numéro du dé (compris entre 1 et 16) </param>
        /// <param name="file">Fichier à lire (Attention : ne pas oublier .txt)</param>
        public De(int numero, string file)
        {
            if (numero >= 0 && numero <= 15) // Numéro compris entre 0 et 15
            {
                try
                {
                    string[] contents = File.ReadAllLines(file);
                    string[] possib = contents[numero].Split(';');// possib donne les 6 faces (une par index) que peut prendre le dé numéro
                    char[] tab = new char[6];
                    for (int i = 0; i < 6; i++)
                    {
                        tab[i] = Convert.ToChar(possib[i]);
                    }
                    this.lettres = tab;
                    this.face = tab[0]; //Par defaut première face
                    //Question : Si l'on fait le random ici, il ne prend que 1 ou 2 valeurs max pour 16 dés : pourquoi ? dans le try ou dans une classe ?
                }

                #region Exceptions
                catch (IOException f)
                {
                    Console.WriteLine(f.Message);
                }
                catch (ArgumentException f)
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
            else
            {
                this.lettres = null;
                this.face = ' ';
            }
        }

        #region Methodes
        /// <summary>
        /// Permet de "lancer le dé", la face utilisée du dé est alors modifiée
        /// </summary>
        /// <param name="z">Variable aléatoire qui va nous permettre de prendre un nombre entre 0 et 5</param>
        public void Lance(Random z)
        {
            int val = z.Next(0, 6);
            this.face = lettres[val];
        }

        /// <summary>
        /// Permet de voir toutes les faces du dés
        /// </summary>
        /// <returns> Retourne les faces possibles du dé, les uns à la suite des autres</returns>
        public string toString()
        {
            string faces = "";
            for (int i = 0; i < lettres.Length; i++)
            {
                faces = faces + lettres[i] + (i==lettres.Length-1 ?"": ";");
            }
            return faces;
        }
        #endregion
    }
 }
