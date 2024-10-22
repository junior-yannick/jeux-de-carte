using System ;

// Classe qui va représenter une personne.
public class Personne
{
    public string Nom { get; }

    public string Prenom { get; }

    public int Identifiant { get; } 

    public Personne(string nom, string prenom, int identifiant)
    {
        Nom = nom;

        Prenom = prenom;

        Identifiant = identifiant;
    }
}