using System ;

// Importation des différents namespaces
using Jambo.Cartes ;
using Jambo.PaireDeCarte ;
using Jambo.TableJeu ;
using Jambo.Joueurs ;

namespace Jambo.JeuPeche{

    public class JeuDePeche
    {

        ArbitreJeu arbitre = new ArbitreJeu() ;

        private List<Joueur> _joueurs;

        private TableDeJeu _table;

        private Random _random = new Random();

        public JeuDePeche(List<Joueur> joueurs, List<Carte> cartes)
        {
            _joueurs = joueurs;

            _table = new TableDeJeu(cartes);
        }

        // Méthode pour démarrer le jeu
        public void DemarrerJeu(int nombreCartesParJoueur, PaireDeCartes paireDeCarte)
        {
            foreach (Joueur joueur in _joueurs)
            {
                // Abonnement des joueur à l'arbitre quand le jeu commence.
                joueur.AbonnerJoueur(arbitre) ;
            }

            // Distribution des cartes.
            foreach (Joueur joueur in _joueurs)
            {
                joueur.Main.AddRange(paireDeCarte.DistribuerCartes(nombreCartesParJoueur));
            }

            // Après le partage des cartes, le reste des cartes devient la pile de pioche.
            _table.Pioche.SetPileDePioche(paireDeCarte) ;

            // Affichage des cartes dans les mains des joueurs au debut du jeu.
            foreach (Joueur joueur in _joueurs){

                Console.WriteLine("Le joueur " + joueur + " a en main " +  "\n");

                joueur.AfficherCarte();
            }

            // Choisir le premier joueur aléatoirement.
            int indexPremierJoueur = _random.Next(_joueurs.Count);

            // L'Arbitre notifie le joueur qui commence la partie.
            arbitre.PremierJoueur(_joueurs[indexPremierJoueur]) ;

            // Logique pour jouer la partie.
            JouerPartie(indexPremierJoueur);

            // Une fois la partie terminée, calculer les points.
            CalculerPointsFinaux();
        }

        // Méthode qui va implémenter la logique du jeu.
        private void JouerPartie(int indexJoueur)
        {
            //Sens du jeu.
            bool sensHoraire = true;

            while (true)
            {
                Joueur joueurActuel = _joueurs[indexJoueur] ;

                Carte cartePileDeDepot ;

                // S'il y a aucune carte dans la pile de depot, alors on choisit la première carte de la pile de depot est
                // la première carte dans la main du premier joueur.
                if (_table.Depot.GetPileDD().Count == 0){

                    cartePileDeDepot = joueurActuel.PremiereCarte() ;

                }

                // Sinon, on obtient la dernière carte de la pile de depot.
                else {

                    cartePileDeDepot = _table.Depot.DerniereCarte() ;
                }

                // Logique de jeu : jouer une carte ou piocher
                // PeutJouerCarte() vérifie si un joueur peut jouer ou piocher dependamment de la dernière carte de la pile de depôt.
                if (PeutJouerCarte(joueurActuel, cartePileDeDepot))
                {   

                    // S'il peut jouer, alors il jouera sa carte en fonction de la dernière carte de la pile de depôt.
                    Carte carteJouee = JouerCarte(joueurActuel, cartePileDeDepot);

                    // Mets cette carte dans le pile de dépôt.
                    _table.Depot.AjouterCarte(carteJouee);

                    // Différents cas où on peut avoir des cartes spéciales, comme un Valet, Un As, un Sept et un Dix.
                    switch (carteJouee.Valeur)
                    {

                        case Valeur.Valet:
                            
                            Console.WriteLine($"{joueurActuel} a joué un Valet. Il peut choisir une nouvelle couleur.");

                            break;

                        case Valeur.As:

                            Console.WriteLine($"L'As de {joueurActuel} fait sauter le tour du prochain joueur.");

                            //Si on a un As qui est joué, alors le prochain joueur voit son tour sauté.
                            indexJoueur = ProchainJoueur(indexJoueur, sensHoraire);

                            break;

                        case Valeur.Sept:

                            Console.WriteLine($"{joueurActuel} a joué {carteJouee}. Le joueur suivant doit piocher 2 cartes.");

                            Joueur joueurSuivant = _joueurs[ProchainJoueur(indexJoueur, sensHoraire)];

                            // Si on a un Sept, alors le prochain joueur doit piocher 2 cartes, et passe son tour.
                            // Mais s'il a un Sept, alors il peut contrer cette attaque.
                            if (joueurSuivant.Main.Any(c => c.Valeur == Valeur.Sept))
                            {
                                Carte carteSept = joueurSuivant.Main.FirstOrDefault(c => c.Valeur == Valeur.Sept);

                                joueurSuivant.Main.Remove(carteSept);

                                _table.Depot.AjouterCarte(carteSept);

                                Console.WriteLine($"{joueurSuivant} a contré avec un autre 7, donc l'attaque s'annule.");

                                indexJoueur = ProchainJoueur(indexJoueur, sensHoraire);
                            
                            }
                            else
                            {
                                // Si la dernière carte du joueur était un sept, pour éviter que le prochain joueur ne pioche, on va directement terminer la partie.
                                if (joueurActuel.Main.Count == 0){

                                    break ;
                                }
                                // Sinon, il pioche 2 cartes, et fait passer son tour.
                                joueurSuivant.AjouterCarte(_table.Pioche.PiocherCarte());

                                joueurSuivant.AjouterCarte(_table.Pioche.PiocherCarte());

                                Console.WriteLine($"{joueurSuivant} a pioché 2 cartes.");

                                indexJoueur = ProchainJoueur(indexJoueur, sensHoraire);
                            }
                            
                            break;

                        case Valeur.Dix:
                            
                            Console.WriteLine($"{joueurActuel} a joué {carteJouee}");

                            //Si le joueur a un 10, alors l'arbitre va notifier les autres joueurs d'un changement de direction.
                            arbitre.ChangementDeDirection(joueurActuel) ;

                            //Et le sens s'inverse.
                            sensHoraire = !sensHoraire;

                            break;

                        default :
                            
                            // Sinon, si le joueur n'a aucune carte spéciale, alors il joue une carte admissible.
                            Console.WriteLine($"{joueurActuel} a joué {carteJouee}");

                            break ;
                    }

                    // Si le joueur actuel après son jeu n'a qu'une carte restante alors, les autres joueurs seront notifiés.
                    if (joueurActuel.Main.Count == 1)
                    {
                        //NotifierObservateurs($"{joueurActuel} n'a plus qu'une carte !");
                        arbitre.UneCarteRestante(joueurActuel) ;
                    }

                    // Si un joueur n'a plus de carte dans sa main alors il a gagné le jeu et le jeu se termine.
                    if (joueurActuel.Main.Count == 0)
                    {
                        Console.WriteLine($"\n******************** {joueurActuel} A GAGNÉ LA PARTIE ! ********************\n");

                        break;
                    }
                    
                    // On obtient la dernière carte de la pile de depot avant le tour du prochain joueur.
                    cartePileDeDepot = _table.Depot.DerniereCarte() ;

                    Console.WriteLine("________________________________________________________________") ;

                    Console.WriteLine("La dernière carte de la pile de depot est : " + cartePileDeDepot);

                    Console.WriteLine("________________________________________________________________") ;

                }
                
                // Si un joueur ne peut pas jouer, c.a.d qu'il n'a pas la carte, alors il va devoir piocher.
                else
                {
                    joueurActuel.AjouterCarte(_table.Pioche.PiocherCarte());

                    Console.WriteLine($"{joueurActuel} a pioché une carte.");

                    Console.WriteLine("________________________________________________________________") ;
                }

                // Et son tour passe au prochain joueur.
                indexJoueur = ProchainJoueur(indexJoueur, sensHoraire);

                // Si la pile de pioche est vide, alors l'arbitre va notifier les joueurs et la pile de depot sera remélanger pour la pile de pioche.
                if (_table.Pioche.EstVide())
                {
                    arbitre.FinDePileDePioche() ;
                    
                    _table.RechargerPioche();
                }

                // Retarder les traitements.
                System.Threading.Tasks.Task.Delay(3000).Wait();
            }
        }

        // Méthode qui permet au joueur de jouer une carte qu'il a dans sa main, tout en minimisant ses points.
        private Carte JouerCarte(Joueur joueur, Carte cartePDD)
        {
            Carte carteJouee = joueur.StrategieDeJeu(cartePDD) ;

            joueur.Main.Remove(carteJouee);

            return carteJouee;
            
        }

        // Cette méthode vérifie si un joueur peut jouer la carte dans sa main.
        private bool PeutJouerCarte(Joueur joueur, Carte cartePDD)
        {
            
            // Si la pile de depôt est vide, alors il peut jouer.
            if (_table.Depot.GetPileDD().Count == 0){

                return true ;
            }
            
            // Sinon, on va vérifier si la carte qu'il veut jouer et celle sur la pile de depot correspondent.
            else {
    
                return (joueur.VerifierCarte(cartePDD)  && joueur.Main.Count > 0 );
            
            }
        
        }

        // Méthode pour déterminer le prochain joueur en tenant compte de son index et du sens.
        private int ProchainJoueur(int indexActuel, bool sensHoraire)
        {
            if (sensHoraire)
            {
                return (indexActuel + 1) % _joueurs.Count;
            }
            else
            {
                return (indexActuel - 1 + _joueurs.Count) % _joueurs.Count;
            }
        }

        // Calcul des points à la fin de la partie
        private void CalculerPointsFinaux()
        {
            Console.WriteLine("\n*-*-*-*-*-*-*-*-*-*-*-*-* CALCUL DES POINTS *-*-*-*-*-*-*-*-*-*-*-*-* \n");

            foreach (Joueur joueur in _joueurs)
            {
                // Appel de la fonction pour calculer le nombre de points.
                int totalPoints = CalculerPoints(joueur);

                Console.WriteLine($"{joueur} a {totalPoints} points.");

                Console.WriteLine("=====================================") ;
            }
        }

        // Méthode pour calculer les points d'un joueur
        private int CalculerPoints(Joueur joueur)
        {
            int totalPoints = 0;

            if (joueur.Main.Count == 0) {Console.WriteLine($"{joueur} a finalement en main : Aucune carte !") ;}
            
            else 
            {
                Console.WriteLine($"{joueur} a finalement en main : ");

                joueur.AfficherCarte();
            }

            // Si la valeur de la carte est un Valet, Roi ou Dame, alors on ajoute 2 au nombre de point.
            foreach (Carte carte in joueur.Main){

                if (carte.Valeur == Valeur.Valet || carte.Valeur == Valeur.Dame || carte.Valeur == Valeur.Roi)
                {

                    totalPoints += 2 ;
                }
                else {

                    // On va faire une conversion explicite de la valeur de la carte pour calculer son nombre de points.
                    totalPoints += (int)carte.Valeur ;
                    
                }
                
            }

            return totalPoints;
        }
    }
}
