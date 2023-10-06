using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_Final
{
    public class Jeu
    {
        #region Variables d'instance
        Dictionnaire mondico;
        /// <summary>
        /// Le dictionnaire utilisé pour le jeu
        /// </summary>
        public Dictionnaire Mondico
        {
            get { return mondico; }
        }
        Plateau monplateau;
        /// <summary>
        /// Le plateau utilisé pour le jeu
        /// </summary>
        public Plateau Monplateau
        {
            get { return monplateau; }
        }
        #endregion

        /// <summary>
        /// Définit un jeu à parir d'un dictionnaire, et d'un plateau (ici, ils sont fixés)
        /// </summary>
        public Jeu()
        {
            Dictionnaire mondico = new Dictionnaire("MotsPossibles.txt");
            this.mondico = mondico;
            Plateau monplateau = new Plateau("Des.txt");
            this.monplateau = monplateau;
        }
        
    }
}
