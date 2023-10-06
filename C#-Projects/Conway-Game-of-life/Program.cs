using System;
namespace Rapport_algo
{
    class Program
    {
        /* Pour l'étape 2 on considère que la population 1 garde la même notation (#, *, -),
           et la population 2 prendra la notation suivante respectivement (§, ¤, +) */

        static void AfficherMatrice(char[,] matrice)
        // Cette fonction permet, comme son nom l'indique, d'afficher une matrice (non nulle ou vide)
        // Affichage simple (pas de séparation stylisée entre deux éléments)
        {
            if (matrice != null && matrice.Length != 0) // La matrice ne sera affichée que si elle n'est ni nulle ni vide
            {
                for (int i = 0; i < matrice.GetLength(0); i++)
                {
                    for (int j = 0; j < matrice.GetLength(1); j++)
                    {
                        if (j != matrice.GetLength(1) - 1)
                        {
                            Console.Write(matrice[i, j] + " ");
                        }
                        else
                        {
                            Console.WriteLine(matrice[i, j]); // Retour à la ligne à la fin d'une ligne
                        }
                    }
                }
            }
            else
            {
                Console.Write("Erreur : la matrice est nulle ou vide"); // Cas où la matrice est nulle ou vide
            }
        }

        static char[,] CreerPlateau()
        // Permet de créer la matrice qui servira de "plateau de jeu".
        // Les dimensions de cette matrice sont demandées à l'utilisateur (elles doivent être supérieures à 0)
        {
            // Demande le nombre de ligne (tant que la valeur est inf à 0)
            Console.WriteLine("Quelle est le nombre de lignes du plateau ?");
            int ligne;
            do
            {
                ligne = Convert.ToInt32(Console.ReadLine());
            }
            while (ligne < 0);

            // Demande le nombre de colonne (tant que la valeur est inf à 0)
            Console.WriteLine("Quelle est le nombre de colonnes du plateau ?");
            int colonne;
            do
            {
                colonne = Convert.ToInt32(Console.ReadLine());
            }
            while (colonne < 0);

            // Création de la matrice
            char[,] plateau = new char[ligne, colonne];
            return plateau;
        }

        static int Taux(char[,] plateau, int version)
        // Cette fonction sera utilisé pour demander le taux de population en % (et vérifier qu'il est correcte)
        // La valeur en sortie est le nombre de cases à remplir avec des cellules vivantes dans le tableau 
        {
            Console.WriteLine("Quelle est le taux de remplissage de cellules (valeur entre 10% et 90% inclus) ?");
            int taux = Convert.ToInt32(Console.ReadLine());
            while (taux < 10 && taux > 90)                                                                          // On fait une boucle de vérification pour savoir si le taux est bien dans l'intervalle souhaité
            {
                Console.WriteLine("Veuillez saisir un taux global entre 10% et 90% inclus :");
                taux = Convert.ToInt32(Console.ReadLine());
            }
            switch (version)                                                                                        // En fonction de la version, le calcul du taux est différent
            {
                case 1:
                    taux = taux * plateau.Length / 100;
                    break;
                case 2:
                    taux = (taux / 2) * plateau.Length / 100;                                                       // Ici, c'est un taux global, donc divisé en 2 taux égaux pour chacune des populations
                    break;
                default:
                    Console.WriteLine("erreur");
                    break;
            }
            return taux;
        }

        static char[,] Initialisation(char[,] plateau, int taux, int version)
        // On remplit ici le tableau des cellules (mortes puis vivantes) , selon le taux et la version du jeu
        {
            if (plateau != null && plateau.Length != 0)
            {
                // Partie cellule morte :
                for (int i = 0; i < plateau.GetLength(0); i++)
                {
                    for (int j = 0; j < plateau.GetLength(1); j++)
                    {
                        plateau[i, j] = '.';  
                    }
                }

                // Cellulles vivante:
                for (int pop = 1; pop < 3; pop = (version == 1 ? 3 : pop + 1))                             // Si version = 1 alors on ne considérera que la pop 1 sinon, pop = 3 et on sort tout de suite de la boucle
                {
                    for (int n = 0; n <= taux; n++)                                                       // Taux est le nombre de cases à remplir de cellules vivantes. Tant que l'on n'a pas atteint ce nombre, on continue.
                    {
                        Random valeur = new Random();
                        int i = valeur.Next(0, plateau.GetLength(0));                                     // Génération d'une valeur aléatoire comprise entre 0 et (le nombre de ligne - 1)
                        int j = valeur.Next(0, plateau.GetLength(1));
                        if (plateau[i, j] == '#' || plateau[i, j] == '§')                                // On regarde si on est retombé sur une case où la cellule était déjà vivante, si oui on enlève 1 à n pour annuler le tour de boucle
                        {
                            n--;
                        }
                        else
                        {
                            plateau[i, j] = (pop == 1 ? '#' : '§');
                        }
                    }
                }
            }
            else
            {
                Console.Write("La matrice est vide ou null");
            }
            return plateau;
        }

        static int CompteCellulesViv(char[,] plateau, int population)
        // Cette fonction à pour but de compter le nombre de cellules vivantes d'une population donnée
        {
            int compt = 0;
            if (plateau != null && plateau.Length != 0)
            {
                for (int i = 0; i < plateau.GetLength(0); i++)
                {
                    for (int j = 0; j < plateau.GetLength(1); j++)
                    {
                        if (population == 1 & plateau[i, j] == '#' || population == 2 && plateau[i, j] == '§')
                        {
                            compt++;
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("La matrice est nulle ou vide");
            }
            return compt;
        }

        static int RestePlateau(char[,] plateau, int index, int dim)
        // Cette fonction permet de garder des index possibles : quand un index dépasse la taille maximale, il revient à la valeur minimale
        // C'est une sorte de fonction modulo
        {

            int modulo = index;                                                                                 // On ne pouvait pas retourner index lui même car il ne respecterait plus les conditions du for (voir dans Cellulesvoisines)
            while (modulo < 0 || modulo >= plateau.GetLength(dim))
            {
                modulo = (modulo < 0 ? modulo + plateau.GetLength(dim) : modulo - plateau.GetLength(dim));
            }
            return modulo;
        }

        static int Cellulesvoisines(char[,] plateau, int ligne, int colonne, int rang, bool newpop)
        // Cette fonction permet de compter les cellules voisines (vivantes) autour d'une cellule donnée
        // On peut choisir à quel rang on s'interesse et à quelle population (newpop = true si c'est la nouvelle population: la 2)
        {
            int celluleviv = 0;
            for (int i = (ligne - rang); i <= (ligne + rang); i++)                                  // Le double for() dépend du rang
            {
                for (int j = (colonne - rang); j <= (colonne + rang); j++)
                {
                    if (i != ligne || j != colonne)                                                 // Permet de ne pas prendre en compte la cellule que nous étudions au centre du carré
                    {
                        int newl = RestePlateau(plateau, i, 0);                                     // En créant de nouvelles variables, i et j ne sont pas impactées
                        int newc = RestePlateau(plateau, j, 1);
                        switch (newpop)                                                             // En fonction de la population, les caractères qui indiquent l'état d'une cellules sont différents
                        {
                            case (false):
                                if (plateau[newl, newc] == '#' || plateau[newl, newc] == '*')       // Si la cellule "va mourir" ('*'), cela signifie qu'elle était vivante et on la considère alors comme cela
                                {
                                    celluleviv++;
                                }
                                break;

                            case (true):
                                if (plateau[newl, newc] == '§' || plateau[newl, newc] == '¤')       // Même raisonnement pour '¤'
                                {
                                    celluleviv++;
                                }
                                break;
                        }
                    }
                    // Si l'on est au rang 2, mais que nous ne sommes ni sur la première, ni la dernière ligne, il nous suffit alors simplement de vérifier le premier et dernier élément de la ligne, le reste appartient au rang 1 (voir sur un schéma)
                    if (rang == 2 && i != ligne - rang && i != ligne + rang && j != colonne + rang) // La dernière condition permet de ne pas indéfiniment redéf j en (colonne + rang) - 1 
                    {
                        j = (colonne + rang) - 1;
                    }
                }
            }
            return celluleviv;
        }

        static void EtatIntermediaire(char[,] plateau, bool afficherinterm, int version)
        // Permet de "préparer" la transition pour la prochaine génération
        // Cette préparation peut être afficher/montrer ou non
        {
            for (int i = 0; i < plateau.GetLength(0); i++)
            {
                for (int j = 0; j < plateau.GetLength(1); j++)
                {
                    bool newpop = (plateau[i, j] == '§'); // Vrai si elle appartient a la nouvelle pop
                    if (plateau[i, j] == '#' || plateau[i, j] == '§')
                    {
                        if (Cellulesvoisines(plateau, i, j, 1, newpop) != 2 && Cellulesvoisines(plateau, i, j, 1, newpop) != 3)//+ test que ce soit pas inf a 0 en haut de la fct
                        {
                            plateau[i, j] = (newpop == true ? '¤' : '*');
                        }
                    }
                    else
                    {
                        if (Cellulesvoisines(plateau, i, j, 1, true) == 3 || Cellulesvoisines(plateau, i, j, 1, false) == 3)
                        {
                            if (Cellulesvoisines(plateau, i, j, 1, false) != 3 || Cellulesvoisines(plateau, i, j, 1, true) != 3)
                            {
                                plateau[i, j] = (Cellulesvoisines(plateau, i, j, 1, false) != 3 ? '+' : '-'); // Si Cellulesvoisines(plateau, i, j, 1, false) != 3 cela signifie qu'à la condition du dessus c'est Cellulesvoisines(plateau, i, j, 1, true) == 3 donc que la cellule sera de la population 2
                            }
                            else
                            {
                                int u = Cellulesvoisines(plateau, i, j, 2, true); // Les voisines au rang 2 de la population 2
                                int v = Cellulesvoisines(plateau, i, j, 2, false); // Les voisines au rang 2 de la population 1
                                if (u > v)
                                {
                                    plateau[i, j] = '+';
                                }
                                else
                                {
                                    if (u < v)
                                    {
                                        plateau[i, j] = '-';
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (afficherinterm == true) // Affichage de l'état intermédiaire
            {
                Console.WriteLine((version == 1 ? "\n" : "\n\n")); // 1 retour à la ligne pour la version 1 et 2 pour la version 2
                AfficherMatrice(plateau);
                System.Threading.Thread.Sleep(1000);
            }
        }

        static bool EtatFin(char[,] plateau)
        // Cette fonction marque la fin d'une génération: les cellules qui doivent mourir meurent, et les cellules qui doivent naître naissent
        // Elle renvoit aussi un booléen qui permet de savoir si il y a eu des modifications/ changements entre 2 générations
        {
            bool changement = false;
            for (int i = 0; i < plateau.GetLength(0); i++)
            {
                for (int j = 0; j < plateau.GetLength(1); j++)
                {
                    if (plateau[i, j] == '*' || plateau[i, j] == '-' || plateau[i, j] == '+' || plateau[i, j] == '¤') // les états de transition vont maintenant être changé en état de fin de génération
                    {
                        changement = true;
                        if (plateau[i, j] == '*' || plateau[i, j] == '¤')
                        {
                            plateau[i, j] = '.'; // Pour les 2 population, l'état mort est le même
                        }
                        else
                        {
                            plateau[i, j] = (plateau[i, j] == '-' ? '#' : '§');
                        }
                    }
                }
            }
            return changement;
        }

        static string AfficheCellule(char[,] plateau, int version)
        // Cette fonction permet un affichage du nombre de cellules vivantes en fonction de la version de jeu
        {
            string phrase = "";
            if (version == 1)
            {
                phrase = "Le nombre de cellules vivantes est de : " + CompteCellulesViv(plateau, 1);
            }
            else
            {
                phrase = "Le nombre de cellules vivantes de la population 1 est de : " + CompteCellulesViv(plateau, 1) + "\nCelui de la population 2 est de : " + CompteCellulesViv(plateau, 2);
            }

            return phrase;
        }

        static void Main()
        {
            char[,] plateau = CreerPlateau(); // Tableau vide

            // Sélectio de la version:
            Console.WriteLine("Sélectionnez la version du jeu que vous souhaitez");
            Console.WriteLine("Tappez 1 pour une version du jeu avec une seule population");
            Console.WriteLine("Tappez 2 pour une version du jeu avec deux populations");
            int version;
            do
            {
                version = Convert.ToInt32(Console.ReadLine()); //obligé 1 ou 2
            }
            while (version != 1 && version != 2);

            // Création de tableau, Assignation:
            int taux = Taux(plateau, version);
            plateau = Initialisation(plateau, taux, version);

            // Choisir avec/sans étapes intermédiaires:
            int interm;
            do
            {
                Console.WriteLine("Avec ou sans étapes intermédiaires ?");
                Console.WriteLine("Tappez 1 pour la version sans changement d'état");
                Console.WriteLine("Tappez 2 pour la version avec changement d'état");
                interm = Convert.ToInt32(Console.ReadLine());
            }
            while (interm < 1 || interm > 2);
            bool afficherinterm = (interm == 2);

            // Déclaration des variables qui permettront de faire différents compteurs :
            int generation = 0;
            bool changement;

            // Lancement du "jeu":
            do
            {
                // Affichage :
                Console.Clear();
                Console.WriteLine("Génération " + (generation));
                Console.WriteLine(AfficheCellule(plateau, version));
                AfficherMatrice(plateau);
                System.Threading.Thread.Sleep(1000);
                Console.Clear();

                // Les modifications : 
                generation = generation + 1;
                EtatIntermediaire(plateau, afficherinterm, version); 
                changement = EtatFin(plateau);
            }
            while (generation < Math.Max(plateau.GetLength(0)+2, plateau.GetLength(1)+2) && (CompteCellulesViv(plateau, 1) > 0 || CompteCellulesViv(plateau, 2) > 0) && changement == true);
            Console.Clear();
            AfficherMatrice(plateau);
            Console.WriteLine("Fin de la simulation");
        }
    }
}