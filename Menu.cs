using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Annulaire_Client
{
    internal class Menu
    {
        ActionController aController;
        bool isAdmin = false;

        public Menu(Socket SocketClient) {
            this.aController = new ActionController(SocketClient);
        }

        public void setIsAdmin(bool set) { this.isAdmin = set; }

        public void MenuPrincipal()
        {
            bool Running = true;
            while (Running)
            {
                Console.WriteLine("***** DEVOIR 3 : Annulaire *****\n" +
                                  " /\\_/\\  \r\n( o.o )\r\n > ^ <\n" +
                                  "1 - Connecter administrateur\n" +
                                  "2 - Demande de requete\n" +
                                  "0 - Quitter");
                try
                {
                    String Entre = Console.ReadLine();
                    if (Entre != null)
                    {
                        int choix = int.Parse(Entre);
                        switch (choix)
                        {
                            //Connexion admin
                            case 1:
                                MenuConnexionAdmin();
                                Thread.Sleep(1000);
                                break;
                            //Demande de requete
                            case 2:
                                MenuDemandeRequete();
                                break;
                            //Quitter l'application
                            case 0:
                                Console.WriteLine("Vous avez quitté l'application.");
                                Running = false;
                                break;
                            default:
                                Console.WriteLine("Entrer un choix valide.");
                                break;
                        }
                    }
                }
                catch
                {
                    Console.WriteLine("Entrer un choix valide.");
                }
            }
        }

        private void MenuConnexionAdmin()
        {
            bool Running = true;
            string Entree = "";
            Console.WriteLine("Entrer le mot de passe administrateur.");
            while (Running)
            {
                try
                {
                    Entree = Console.ReadLine();
                    if(Entree != "")
                    {
                        aController.VerifierMdp(Entree);
                        Running = false;
                        break;
                    }
                    else 
                    {
                        Console.WriteLine("Entrer un mot de passe.");
                    }
                }
                catch
                {
                    Console.WriteLine("Entrer un mot de passe.");
                }
            }
        }

        private void MenuDemandeRequete()
        {
            bool running = true;
            int choix;
            Console.WriteLine("***** Choisisez une requête. *****");
            while (running)
            {
                try
                {
                    Console.WriteLine("1-Lister membre catégorie \n2-Lister professeur domaine \n3-Rechercher un membre \n4-Ajouter un membre(Admin) \n" +
                        "5-Supprimer un membre(Admin) \n6-Modifier un membre(Admin) \n7-Mettre un membre sur Liste Rouge(Admin) \n8-Enlever un membre sur Liste Rouge(Admin) \n0- Retour");
                    choix = int.Parse(Console.ReadLine());
                    if(choix >= 0 && choix < 9 )
                    {
                        switch(choix)
                        {
                            case 0 :
                                running = false;
                                break;
                            case 1 :
                                Console.WriteLine("Entrer une categorie entre \"Etudiant\" et \"Professeur\"");
                                List<string> listVerifCate = new List<string>() { "Etudiant", "Professeur" };
                                string categorie = SingleInputVerif("categorie", listVerifCate);
                                aController.ListerMembreCategorie(categorie);
                                Thread.Sleep(1000);
                                break;
                            case 2 :
                                Console.WriteLine("Entrer un domaine entre \"Logiciel\", \"Science des données\", \"Web et mobile\", \"Cybersécurité\"");
                                List<string> listVerifDom = new List<string>() { "Logiciel" , "Science des données", "Web et mobile", "Cybersécurité" };
                                string domaine = SingleInputVerif("domaine", listVerifDom);
                                aController.ListerProfesseurDomaine(domaine);
                                Thread.Sleep(1000);
                                break;
                            case 3 :
                                Console.WriteLine("Entrer le nom du membre que vous recherchez.");
                                string Nom = SingleInput("nom");
                                aController.RechercherMembre(Nom);
                                Thread.Sleep(1000);
                                break;
                            case 4 :
                                List<string> p = MenuAjoutMembre();
                                aController.AjouterMembre(p[0], p[1], p[2], p[3], p[4], p[5], p[6], p[7]);
                                Thread.Sleep(1000);
                                break;
                            case 5 :
                                Console.WriteLine("Entrer le numero dont vous voulez supprimer de la bd.");
                                string supNum = SingleInput("numero");
                                aController.SupprimerMembre(supNum);
                                Thread.Sleep(1000);
                                break;
                            case 6 :
                                Console.WriteLine("Entrer le numero dont vous voulez supprimer de la bd.");
                                string modifyNum = SingleInput("numero");
                                List<string> d = MenuModifierMembre(modifyNum); 
                                aController.ModifierMembre(d[0], d[1], d[2], d[3], d[4], d[5], d[6], d[7], d[8]);
                                Thread.Sleep(1000);
                                break;
                            case 7 :
                                Console.WriteLine("Entrer le numero dont vous voulez ajouter à la Liste Rouge");
                                string AjoutLRNum = SingleInput("numero");
                                aController.AjouterMembreListeRouge(AjoutLRNum);
                                Thread.Sleep(1000);
                                break;
                            case 8 :
                                Console.WriteLine("Entrer le numero dont vous voulez ajouter à la Liste Rouge");
                                string RemoveLRNum = SingleInput("numero");
                                aController.EnleverMembreListRouge(RemoveLRNum);
                                Thread.Sleep(1000);
                                break;
                            default:
                                Console.WriteLine("Entrer un choix valide.");
                                break;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Entrer un choix valide entre 0 et 8.");
                    }
                }
                catch
                {
                    Console.WriteLine("Entrer un choix valide entre 0 et 8.");
                }
            }

        }

        private List<string> MenuModifierMembre(string num)
        {
            List<string> listVerifCate = new List<string>() { "Etudiant", "Professeur" };
            List<string> listVerifDom = new List<string>() { "Logiciel", "Science des données", "Web et mobile", "Cybersécurité" };
            List<string> listVerifLR = new List<string>() { "true", "false" };
            List<string> list = new List<string>();
            list.Add(num);
            Console.WriteLine("Entrer le nom.");
            list.Add(SingleInput("nom"));
            Console.WriteLine("Entrer le prenom.");
            list.Add(SingleInput("prenom"));
            Console.WriteLine("Entrer une categorie entre \"Etudiant\" et \"Professeur\"");
            list.Add(SingleInputVerif("categorie", listVerifCate));
            if (list[3] == "Etudiant")
            {
                Console.WriteLine("Entrer le matricule.");
                list.Add(SingleInput("matricule"));
            }
            else { list.Add("Null"); }
            Console.WriteLine("Entrer le email.");
            list.Add(SingleInput("email"));
            if (list[3] == "Professeur")
            {
                Console.WriteLine("Entrer le téléphone.");
                list.Add(SingleInput("téléphone"));
            }
            else { list.Add("Null"); }
            Console.WriteLine("Entrer si dans Liste Rouge entre \"true\", \"false\"");
            list.Add(SingleInputVerif("ListeRouge", listVerifLR));
            Console.WriteLine("Entrer un domaine entre \"Logiciel\", \"Science des données\", \"Web et mobile\", \"Cybersécurité\"");
            list.Add(SingleInputVerif("domaine", listVerifDom));
            return list;
        }

        private List<string> MenuAjoutMembre()
        {
            List<string> listVerifCate = new List<string>() { "Etudiant", "Professeur" };
            List<string> listVerifDom = new List<string>() { "Logiciel", "Science des données", "Web et mobile", "Cybersécurité" };
            List<string> listVerifLR = new List<string> () { "true", "false" };
            List<string> list = new List<string>();
            Console.WriteLine("Entrer le nom.");
            list.Add(SingleInput("nom"));
            Console.WriteLine("Entrer le prenom.");
            list.Add(SingleInput("prenom"));
            Console.WriteLine("Entrer une categorie entre \"Etudiant\" et \"Professeur\"");
            list.Add(SingleInputVerif("categorie", listVerifCate));
            if (list[2] == "Etudiant")
            {
                Console.WriteLine("Entrer le matricule.");
                list.Add(SingleInput("matricule"));
            }
            else { list.Add("Null"); }
            Console.WriteLine("Entrer le email.");
            list.Add(SingleInput("email"));
            if (list[2] == "Professeur")
            {
                Console.WriteLine("Entrer le téléphone.");
                list.Add(SingleInput("téléphone"));
            }
            else { list.Add("Null"); }
            Console.WriteLine("Entrer si dans Liste Rouge entre \"true\", \"false\"");
            list.Add(SingleInputVerif("ListeRouge", listVerifLR));
            Console.WriteLine("Entrer un domaine entre \"Logiciel\", \"Science des données\", \"Web et mobile\", \"Cybersécurité\"");
            list.Add(SingleInputVerif("domaine", listVerifDom));
            return list;
        }

        private string SingleInput(string MotCle)
        {
            while(true)
            {
                try
                {
                    string input = Console.ReadLine();
                    if(input != null || input != "")
                    {
                        return input;
                    }
                    else
                    {
                        Console.WriteLine($"Veuillez entrer un {MotCle} valide.");
                    }
                }
                catch
                {
                    Console.WriteLine($"Veuillez entrer un {MotCle} valide.");
                }
            }
        }

        private string SingleInputVerif(string MotCle, List<string> verification)
        {
            while (true)
            {
                try
                {
                    string input = Console.ReadLine();
                    if (input != null || input != "")
                    {
                        foreach (string line in verification)
                        {
                            if(input == line)
                            {
                                return input;
                            }
                        }
                        Console.WriteLine($"Veuillez entrer un {MotCle} valide.");
                    }
                    else
                    {
                        Console.WriteLine($"Veuillez entrer un {MotCle} valide.");
                    }
                }
                catch
                {
                    Console.WriteLine($"Veuillez entrer un {MotCle} valide.");
                }
            }
        }
    }
}
