using System ;

// Le message que le joueur va recevoir.
public class NotifierJoueur : EventArgs {

    public string Message {get ;} 

    public NotifierJoueur(string m){

        Message = m ;
    }
}