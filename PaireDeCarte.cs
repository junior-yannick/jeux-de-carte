using System ;
using Jambo.Cartes ;

// Implémentation de la paire de carte.

namespace Jambo.PaireDeCarte{

    public class PaireDeCartes
    {
        private List<Carte> _cartes = new List<Carte>();

        // Initialise les 52 cartes, en prenant chaque Couleur et Valeur respectivement.
        public PaireDeCartes()
        {
            foreach (Valeur valeur in Enum.GetValues(typeof(Valeur)))
            {
                foreach (Couleur couleur in Enum.GetValues(typeof(Couleur)))
                {
                    _cartes.Add(new Carte(valeur, couleur));
                }
            }
        }


        // Distribution de n cartes aux joueurs.
        public List<Carte> DistribuerCartes(int nombre)
        {
            List<Carte> Partage = new List<Carte>();

                for (int i = 1 ; i <= nombre ; i ++){

                    Carte temp = _cartes[0] ;

                    Partage.Add(temp);

                    _cartes.RemoveAt(0);
                }

                return Partage ;
        }

        // Méthode qui va mélanger les cartes, en utilisant le hasard.
        public void BoxerCartes(){

                Random rng = new Random();

                int n = _cartes.Count();

                while (n > 1){

                    n -- ;

                    int k = rng.Next(n+1);

                    Carte temp = _cartes[k];

                    _cartes[k] = _cartes[n];

                    _cartes[n] = temp ;

                    

                }    

        }

        // Va afficher le paire de carte.
        public void AfficherPaireDeCarte(){

            int i = 1 ;
            
                foreach(Carte carte in _cartes){

                    Console.WriteLine(i + " " + carte.Valeur + " de " + carte.Couleur) ;
                    
                    i++ ;
                }
                
            }
        
        // Méthodes Pour obtenir les cartes.
        public List<Carte> GetCartes()
        {
            return _cartes;
        }
    }
}
