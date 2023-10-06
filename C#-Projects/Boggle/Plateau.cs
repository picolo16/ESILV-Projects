using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_Final
{
    /// <summary>
    /// Classe qui défini un plateau
    /// </summary>
    public class Plateau
    {
        #region Variables d'instance

        //Pas nécessaire d'y acceder ailleurs (on a déjà des fonctions qui nous permmettent de les visionner)
        private De[,] dices;
        private char[,] dicesManip;
        #endregion

        /// <summary>
        /// Défini un plateau (c'est à dire un tableau de dés) à partir d'un fichier 
        /// </summary>
        /// <param name="file">Fichier à lire (Attention : ne pas oublier .txt) </param>
        public Plateau(string file)
        {
            De[,] dices = new De[4, 4];
            Random r = new Random();
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {

                    dices[i, j] = new De(4 * i + j, file);//4*i+j permet d'aller jusqua 15
                    dices[i, j].Lance(r);

                }
            }
            this.dices = dices;
            char[,] dicesManip = new char[dices.GetLength(0), dices.GetLength(1)];
            for(int i = 0; i<dices.GetLength(0);i++)
            {
                for(int j =0; j<dices.GetLength(1);j++)
                {
                    dicesManip[i,j] = dices[i, j].Face;
                }
            }
            this.dicesManip = dicesManip;
        }

        #region Methodes

        /// <summary>
        /// Permet d'afficher le plateau comme une matrice (visuellement)
        /// </summary>
        /// <returns> Le message "sous forme" de tableau (attention, ce n'est pas un tableau)</returns>
        public string toString()
        {
            string message="";
            if(dices==null)
            {
                message = "Le tableau de dé est null";
            }
            else
            {
                for(int i =0; i<4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        message = message + dices[i,j].Face + ((j == 3 )? "\n" : " ");
                    }
                }
            }
            return message;
        }
        /// <summary>
        /// Verifie si le mot précisé en entrée se trouve dans le plateau
        /// </summary>
        /// <param name="mot"> Mot cherché</param>
        /// <param name="coord">Liste de coordonnées que va prendre les differents char du mot</param>
        /// <param name="index"> Index du mot (on commence évidement à 0)</param>
        /// <param name="decr"> Permet quand le chemin est faux de garder le mauvais chemin en mémoire pour ne pas refaire la même erreur </param>
        /// <returns></returns>
        public bool Test_Plateau(string mot, List<int[]> coord, int index = 0, int decr = 0)// newCoord dans les arguments pour ne pas en créer un à chaque fois
        {
            if (index<= mot.Length-1 && index>=0)
            {
                if (decr == 0)
                {
                    for (int i = (index == 0 || coord[coord.Count() - (1 + decr)][0] - 1 <= 0 ? 0 : coord[coord.Count() - (1 + decr)][0] - 1); (index == 0 || coord[coord.Count() - (1 + decr)][0] + 1 >= dices.GetLength(0) ? i < dices.GetLength(0) : i <= coord[coord.Count() - (1 + decr)][0] + 1); i++)
                    {
                        for (int j = (index == 0 || coord[coord.Count() - (1 + decr)][1] - 1 <= 0 ? 0 : coord[coord.Count() - (1 + decr)][1] - 1); (index == 0 || coord[coord.Count() - (1 + decr)][1] + 1 >= dices.GetLength(1) ? j < dices.GetLength(1) : j <= coord[coord.Count() - (1 + decr)][1] + 1); j++)
                        {
                            if (mot[index] == dicesManip[i, j] && Contient(coord, i, j) == false) //&& (index == 0 || PositionValide(coord, i, j, index)))
                            {
                                if (decr == 1)
                                    coord.RemoveAt(coord.Count() - 1);

                                int[] newCoord = { i, j };
                                coord.Add(newCoord);
                                return index == mot.Length - 1 ? true : Test_Plateau(mot, coord, index + 1, 0);
                            }
                        }
                    }
                    if (decr == 1)
                        coord.RemoveAt(coord.Count() - 1);

                    return index == 0 ? false : Test_Plateau(mot, coord, index - 1, 1); //Si le premier chemin est faux, c'est pas forcé que tous le soient
                }
                else
                {
                    for (int i = coord[coord.Count()-1][0]; i<dicesManip.GetLength(0); i++)
                    {
                        for (int j = coord[coord.Count() - 1][1]+1; j < dicesManip.GetLength(0); j++)
                        {
                            if(coord[coord.Count() - 1][1] + 1>=dices.GetLength(1))//retour à la ligne
                            {
                                i = coord[coord.Count() - 1][0] + 1;
                                j = 0;
                            }
                            if (mot[index] == dicesManip[i, j] && Contient(coord, i, j) == false) //&& (index == 0 || PositionValide(coord, i, j, index)))
                            {
                                if (decr == 1)
                                    coord.RemoveAt(coord.Count() - 1);

                                int[] newCoord = { i, j };
                                coord.Add(newCoord);
                                return index == mot.Length - 1 ? true : Test_Plateau(mot, coord, index + 1, 0);
                            }
                        }
                    }
                    return index == 0 ? false : Test_Plateau(mot, coord, index - 1, 1);
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Vérifie si une coordonnée est présente dans notre liste de coordonnées
        /// </summary>
        /// <param name="coord"> La liste de coordonnées </param>
        /// <param name="ligne"> dimension 0 de la coordonnée à vérifier</param>
        /// <param name="colonne">dimension 1 de la coordonnée à vérifier</param>
        /// <returns></returns>
        public static bool Contient(List<int[]> coord, int ligne, int colonne)
        {
            int[] test = { ligne, colonne };
            bool contient = coord.Contains(test);
            return contient;
        }
        #endregion

    }
}
