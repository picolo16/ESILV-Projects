using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Projet_Final
{
    /// <summary>
    /// Programme principal
    /// </summary>
    public class Program
    {
        static void Main(string[] args)
        {
            // Création des Joueurs:
            string name;

            Console.WriteLine("Joueur 1, sélectionnez votre nom :");
            name = Console.ReadLine();
            Joueur j1 = new Joueur(name);
            Console.WriteLine("Bonjour "+ j1.Nom + "\nAppuyer sur une touche");
            Console.ReadKey();
            Console.Clear();

            Console.WriteLine("Joueur 2, sélectionnez votre nom :");
            name = Console.ReadLine();
            Joueur j2 = new Joueur(name);//vérifié qu'il ne dépasse pas le temps
            Console.WriteLine("Bonjour " + j2.Nom + "\nAppuyer sur une touche");
            Console.ReadKey();
            Console.Clear();

            // Création du jeu:
            Jeu game = new Jeu();
            
            // Création du timer:
            Console.WriteLine("Sélectionnez le temps total de jeu (en minutes)");
            int duree;
            do
            {
                duree = (Int32.TryParse(Console.ReadLine(), out int result) ? result : -1);
            }
            while (duree  <6);
            TimeSpan timer;
            timer = new TimeSpan(0, duree, 0);
            DateTime debut = DateTime.Now;
            DateTime fin = debut.Add(timer);

            int tour = 2; //Prend les valeurs 1 et 2 l'un après l'autre
            List<int[]> coord = new List<int[]>();
            while(DateTime.Now < fin)
            {

                tour = (tour == 2 ? 1 : 2);
                Console.WriteLine("C'est au tour de " + (tour == 1 ? j1.Nom : j2.Nom));
                Console.ReadKey();
                DateTime debutTour = DateTime.Now;
                DateTime finTour = debutTour.AddMinutes(1);
                
                while (DateTime.Now < finTour)
                {
                    Console.Clear();
                    Console.WriteLine(game.Monplateau.toString());
                    string mot = Console.ReadLine().ToUpper();
                    try
                    {
                        if (game.Mondico.RechDichoRecursif(mot) == true && game.Monplateau.Test_Plateau(mot, coord)==true)
                        {
                            if (tour == 1 && j1.Countain(mot) == false)
                            {
                                Console.WriteLine("Mot ajouté");
                                j1.Add_Mot(mot);
                            }
                            else
                            {
                                if (tour == 2 && j2.Countain(mot) == false)
                                {
                                    Console.WriteLine("Mot ajouté");
                                    j2.Add_Mot(mot);
                                }
                                else
                                {
                                    Console.WriteLine("Mot déjà trouvé");
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("Le mot n'est pas validé");
                        }
                    }
                    catch (OverflowException f)
                    {
                        Console.WriteLine(f.Message);
                    }
                    coord.Clear();
                    System.Threading.Thread.Sleep(500);
                }
            }

            //Calcul des scores
            for(int joueur = 1; joueur<=2; joueur++)
            {
                for(int i= 0; (joueur==1 ? i<j1.Mots.Count(): i<j2.Mots.Count()); i++)
                {
                    if(joueur==1)
                    {
                        j1.Score += (j1.Mots[i].Length); // Baèrème : taille du mot = nombre de points
                    }
                }
            }
            Console.WriteLine("Fin de partie");
            Console.WriteLine((j1.Score > j2.Score ? j1.Nom : j2.Nom) + " a gagné avec " + (j1.Score > j2.Score ? j1.Score : j2.Score) + " points");

        }

    }
}
