using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Annulaire_Client
{
    internal class ActionController
    {
        Socket socketClient;
        public ActionController(Socket nSocketClient) { this.socketClient = nSocketClient; }

        //Requete de verifier le mot de passe administrateur
        public void VerifierMdp(string mdp)
        {
            List<List<String>> nMdp = new List<List<String>>();
            nMdp.Add(new List<String> { mdp });
            Paquet p = new Paquet(0, 0, TypePaquet.Connexion, nMdp, false);
            EnvoyerPaquet(p);
        }

        //Requete num 1
        public void ListerMembreCategorie(string categorie)
        {
            List<List<String>> nCategorie = new List<List<String>>();
            nCategorie.Add(new List<String> { categorie });
            Paquet p = new Paquet(1, 0, TypePaquet.Demande, nCategorie, false);
            EnvoyerPaquet(p);
        }

        //Requete num 2
        public void ListerProfesseurDomaine(string domaine)
        {
            List<List<String>> nDomaine = new List<List<String>>();
            nDomaine.Add(new List<String> { domaine });
            Paquet p = new Paquet(2, 0, TypePaquet.Demande, nDomaine, false);
            EnvoyerPaquet(p);
        }

        //Requete num 3
        public void RechercherMembre(string Nom)
        {
            List<List<String>> nNom = new List<List<String>>();
            nNom.Add(new List<String> { Nom });
            Paquet p = new Paquet(3, 0, TypePaquet.Demande, nNom, false);
            EnvoyerPaquet(p);
        }

        //Requete num 4
        public void AjouterMembre(string nom, string prenom, string categorie, string matricule, string email, string telephone, string listeRouge, string domaine)
        {
            List<List<String>> nMembre = new List<List<String>>();
            nMembre.Add(new List<String> { nom, prenom, categorie, matricule, email, telephone, listeRouge, domaine });
            Paquet p = new Paquet(4, 0, TypePaquet.Demande, nMembre, false);
            EnvoyerPaquet(p);
        }

        //Requete num 5
        public void SupprimerMembre(string num)
        {
            List<List<String>> nNum = new List<List<String>>();
            nNum.Add(new List<String> { num });
            Paquet p = new Paquet(5, 0, TypePaquet.Demande, nNum, false);
            EnvoyerPaquet(p);
        }

        //Requete num 6
        public void ModifierMembre(string num, string nom, string prenom, string categorie, string matricule, string email, string telephone, string listeRouge, string domaine)
        {
            List<List<String>> nNum = new List<List<String>>();
            nNum.Add(new List<String> { num, nom, prenom, categorie, matricule, email, telephone, listeRouge, domaine });
            Paquet p = new Paquet(6, 0, TypePaquet.Demande, nNum, false);
            EnvoyerPaquet(p);
        }

        //Requete num 7
        public void AjouterMembreListeRouge(string num)
        {
            List<List<String>> nNum = new List<List<String>>();
            nNum.Add(new List<String> { num });
            Paquet p = new Paquet(7, 0, TypePaquet.Demande, nNum, false);
            EnvoyerPaquet(p);
        }

        //Requete num 8
        public void EnleverMembreListRouge(string num)
        {
            List<List<String>> nNum = new List<List<String>>();
            nNum.Add(new List<String> { num });
            Paquet p = new Paquet(8, 0, TypePaquet.Demande, nNum, false);
            EnvoyerPaquet(p);
        }
        //Envoi du paquet de deconnexion
        public void Exit()
        {
            Paquet paquetDec = new Paquet(0, 0, TypePaquet.Deconnexion, new List<List<String>>(), false);
            EnvoyerPaquet(paquetDec);
            Environment.Exit(0);
        }

        private async void EnvoyerPaquet(Paquet paquet)
        {
            byte[] buffer = paquet.bytes();
            await socketClient.SendAsync(new ArraySegment<byte>(buffer), SocketFlags.None);
        }
    }
}
