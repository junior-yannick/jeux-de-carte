using System ;
using Jambo.Cartes ;
using Jambo.PilePioche ;
using Jambo.PileDepot ;

namespace Jambo.TableJeu {

    public class TableDeJeu
    {
        public PileDePioche Pioche { get; private set; }

        public PileDeDepot Depot { get; private set; }

        public TableDeJeu(List<Carte> cartes)
        {
            Pioche = new PileDePioche(cartes);

            Depot = new PileDeDepot();
        }

        // Méthode pour recharger la pile de pioche quand elle est vide.
        public void RechargerPioche()
        {
            List<Carte> cartes = Depot.RecupererCartesPourRemelanger();
            
            Pioche.Recharger(cartes);
        }

        // Obtenir la dernière carte de la pile de depot sur la table.
        public Carte DerniereCartePDD(){

            return Depot.DerniereCarte() ;
        }
    }

}
