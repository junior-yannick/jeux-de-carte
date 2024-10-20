using System ;
using System.Collections.Generic ;
using Jambo.Cartes ;

namespace Jambo.Joueurs {

    public class Joueur : Personne
    {
        public List<Carte> Main { get; } = new List<Carte>();

        public Joueur(string nom, string prenom, int identifiant) : base(nom, prenom, identifiant) { }


        // Abonnement du joueur à l'arbitre pour être notifié des évènements.
        public void AbonnerJoueur(ArbitreJeu arbitre){

            arbitre.theHandlers += Handler ;

        }

        public void Handler(object sender, NotifierJoueur e){

            Console.WriteLine(Identifiant + " - " + Prenom + " " + Nom +  " a reçu le message : {0}", e.Message );
        }

        // Méthode pour ajouter une carte dans la main du joueur.
        public void AjouterCarte(Carte carte)
        {
            if (Main.Count < 8)
            {
                Main.Add(carte);
            }
        }

        // Méthode pour afficher les cartes dans la main du joeuur.
        public void AfficherCarte(){

            foreach(Carte carte in Main){

                Console.WriteLine(carte);
            }
        }

        // Méthode pour vérifier la carte d'un joueur, s'il a une carte dont la valeur ou la couleur correspond à la dernière qui est sur la pile de depôt, 
        // Ou dont la valeur est un Valet et que la dernière carte de la pile de depôt n'est pas un sept, alors in pourra jouer.
        public bool VerifierCarte(Carte cartePDD){

            foreach (Carte c in Main){

                if (c.Valeur == cartePDD.Valeur || c.Couleur == cartePDD.Couleur || cartePDD.Valeur == Valeur.Valet || (c.Valeur == Valeur.Valet && cartePDD.Valeur != Valeur.Sept)){

                    return true ;
                }
            }
            return false ;
        }

        // Implémente la stratégie pour minimiser le décompte de points à la fin de la partie pour un joueur, et retourner cette carte.
        public Carte StrategieDeJeu(Carte cartePDD){

            // Liste qui va contenir les cartes possibles qu'un joueur pourra jouer.
            List<Carte> cartesPossibles = new List<Carte>() ;

            // Si la dernière carte de la pile de depôt est un Valet, alors les cartes possibles du joueur sera l'ensemble des cartes dans sa main.
            if (cartePDD.Valeur == Valeur.Valet){

                    cartesPossibles = Main ;
            }

            // Sinon, on va parcourir la main du joueur et vérifier si la couleur ou la valeur de chaque carte correspond à la couleur ou la valeur
            // de la dernière carte de la pile de depôt, ou encore si une carte est un valet mais que la dernière carte de la pile de depot est différent de sept.
            // Et lorsqu'on trouve ces cartes, on les ajoute dans la liste des cartes possibles.
            else {

                foreach (Carte c in Main){

                    if (c.Valeur == cartePDD.Valeur || c.Couleur == cartePDD.Couleur || (c.Valeur == Valeur.Valet && cartePDD.Valeur != Valeur.Sept)) {

                        cartesPossibles.Add(c) ;
                        
                    }

                }

            }

            Carte carteAJouer = cartesPossibles[0] ;

            // S'il y a qu'une seule carte possible à joueur, alors le joueur va jouer cette carte.
            if (cartesPossibles.Count == 1){

                return carteAJouer ;
            }
            
            // Sinon, il va parcourir chaque cartes possible et vérifier celle avec le plus de point pour pouvoir la jouer.
            else {

                foreach (Carte c in cartesPossibles){
                    
                    // On utilise le modulo 12 pour les cartes comme le Roi, Dame et Valet dont les valeurs sont de 2.
                    if ((int)c.Valeur % 12 > (int)carteAJouer.Valeur % 12){

                        carteAJouer = c ;
                    }

                }

                return carteAJouer ;
            }

        }

        // Méthode qui permettra au joueur de jouer sa première carte avec la plus grosse valeur.
        public Carte PremiereCarte() {

            Carte premiereCarte = Main[0] ;

            foreach (Carte c in Main){
                    
                    // On utilise le modulo 12 pour les cartes comme le Roi, Dame et Valet dont les valeurs sont de 2.
                    if ((int)c.Valeur % 12 > (int)premiereCarte.Valeur % 12){

                        premiereCarte = c ;
                    }

            }

            return premiereCarte ;

        }

        public override string ToString()
        {
            return $"{Prenom} {Nom} (ID: {Identifiant})";
        }
    }
}