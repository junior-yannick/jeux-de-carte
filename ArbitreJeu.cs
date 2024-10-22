using System ;
using Jambo.Joueurs ;

public class ArbitreJeu {

    public delegate void Evenement(object sender , NotifierJoueur e) ;

    public event Evenement theHandlers ;

    // Notifie  le changement de direction quand un 10 est joué.
    public void ChangementDeDirection(Joueur joueur){

        RaiseCustomEvent(new NotifierJoueur($"Le joueur {joueur} a joué un 10, le sens du jeu change.")) ;
    }

    // Notifie quand il va rester une carte.
    public void UneCarteRestante(Joueur joueur){

        RaiseCustomEvent(new NotifierJoueur($"Le joueur {joueur} a une carte restante.")) ;
    }

    // Notifie pour signaler le premier joueur.
    public void PremierJoueur(Joueur joueur){

        RaiseCustomEvent(new NotifierJoueur($"Le joueur {joueur} est le premier à jouer.")) ;
    }

    // Notifie pour signaler la fin de la pile de pioche.
    public void FinDePileDePioche(){

        RaiseCustomEvent(new NotifierJoueur($"La pile de pioche est vide, rechargement de la pile de depot !")) ;
    }

    // Méthode pour activer ces évènements.
    public virtual void RaiseCustomEvent(NotifierJoueur e){

        Evenement handler = theHandlers ;

        if(handler != null){

            handler(this, e) ;
        }
    }

}