using System ;
using System.Collections.Generic ;
using Jambo.Cartes ;
using Jambo.PaireDeCarte ;

namespace Jambo.PilePioche{

    public class PileDePioche
    {
        private Stack<Carte> _pioche;

        public PileDePioche(List<Carte> cartes)
        {
            // Mélange aléatoire des cartes.
            _pioche = new Stack<Carte>(cartes.OrderBy(c => Guid.NewGuid())); 
        }

        // Méthode pour piocher une carte.
        public Carte PiocherCarte()
        {
            if (_pioche.Count > 0)
            {
                return _pioche.Pop();
            }
            else
            {
                throw new InvalidOperationException("La pile de pioche est vide.");
            }
        }

        // Vérifie si la pile de pioche est vide.
        public bool EstVide()
        {
            return _pioche.Count == 0;
        }

        // Va récharger la pile de pioche de la pile de depôt aléatoirement.
        public void Recharger(List<Carte> cartes)
        {
            _pioche = new Stack<Carte>(cartes.OrderBy(c => Guid.NewGuid())); // Mélange aléatoire
        }

        // Méthode qui va initialiser la pile de pioche en passant la PaireDeCarte en paramètre.
        public void SetPileDePioche(PaireDeCartes pdc){
                
            _pioche.Clear() ;

            foreach (Carte carte in pdc.GetCartes()){

                _pioche.Push(carte) ;
            }
                
        }

        // Méthode pour obtenir la pile de pioche.
        public Stack<Carte> GetPileDP(){

            return _pioche ;
        }

        // Méthode qui affiche le pile de pioche.
        public void AfficherPileDePioche(){

            foreach (Carte c in _pioche){

                Console.WriteLine(c) ;
            }
        }

    }
}