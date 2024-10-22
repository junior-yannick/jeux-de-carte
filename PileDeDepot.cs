using System ;
using System.Collections.Generic ;
using Jambo.Cartes ;

namespace Jambo.PileDepot{

    public class PileDeDepot
    {
        private Stack<Carte> _depot = new Stack<Carte>();


        // Méthode pour ajouter dans la pile de depôt.
        public void AjouterCarte(Carte carte)
        {
            _depot.Push(carte);
        }

        // Cette méthode nous permet d'obtnenir la dernière carte de la pile de depôt sans la rétirer.
        public Carte DerniereCarte()
        {
            if (_depot.Count > 0)
            {
                return _depot.Peek();
            }
            else
            {
                throw new InvalidOperationException("La pile de dépôt est vide.");
            }
        }

        // Recupération des cartes de la pile de depot pour remélanger sauf la première carte.
        public List<Carte> RecupererCartesPourRemelanger()
        {
            // On garde la première carte.
            Carte derniereCarte = _depot.Pop() ;

            //  Liste pour stocker les cartes de la pile de depôt.
            List<Carte> cartesEnlevees = new List<Carte>();

            while (_depot.Count > 0)
            {
                // Ajout des cartes dans cette liste de cartes qui sera la nouvelle pile de pioche.
                cartesEnlevees.Add(_depot.Pop()) ;
            }

            // On ajoute la dernière carte dans notre pile de depôt, qui ne contient maintenant qu'une seule carte.
            _depot.Push(derniereCarte) ;

            return cartesEnlevees ;
        }

        // On obtient la pile de depôt avec cette méthode.
        public Stack<Carte> GetPileDD(){

            return _depot ;
        }
    }
    
}