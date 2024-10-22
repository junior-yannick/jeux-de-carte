using System ;
using Jambo.Joueurs ;
using Jambo.Cartes ;
using Jambo.PaireDeCarte ;
using Jambo.JeuPeche ;

public class Program
{
    public static void Main(string[] args)
    {
        // Créer les joueurs
        Joueur joueur1 = new Joueur("Yannick", "Junior", 1);

        Joueur joueur2 = new Joueur("Lyblassa", "Archange", 2);

        Joueur joueur3 = new Joueur("Roy", "Mac", 3);

        // Créer une paire de cartes et distribuer les cartes
        PaireDeCartes paireDeCartes = new PaireDeCartes();

        List<Carte> jeuComplet = paireDeCartes.GetCartes();

        //paireDeCartes.AfficherPaireDeCarte();
        paireDeCartes.BoxerCartes();
        //paireDeCartes.AfficherPaireDeCarte();

        // Initialiser le jeu avec 3 joueurs et les cartes
        List<Joueur> joueurs = new List<Joueur> { joueur1, joueur2, joueur3 };

        JeuDePeche jeuDePeche = new JeuDePeche(joueurs, jeuComplet);

        // Démarrer le jeu avec distribution aléatoire de 7 cartes par joueur
        Console.WriteLine("-------------------------- Demarrage du jeu de Pêche -----------------------------") ;
        
        jeuDePeche.DemarrerJeu(8, paireDeCartes);

        // Afficher la fin du programme
        Console.WriteLine("************************** La partie est terminée ! ******************************");
    }
}