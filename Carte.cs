using System ;

namespace Jambo.Cartes {

    // Struct pour Carte
    public struct Carte
    {
        public Valeur Valeur { get; }

        public Couleur Couleur { get; }

        public Carte(Valeur valeur, Couleur couleur)
        {
            Valeur = valeur;

            Couleur = couleur;
        }

        public override string ToString()
        {
            return $"{Valeur} de {Couleur}";
        }
    }
}
