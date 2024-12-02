using System.ComponentModel.Design;
using System.Net.Sockets;

namespace Annulaire_Client
{
    internal class Menu
    {
        private ActionController aController;
        private static List<Option> options = new();
        private bool isAdmin = false; // Ajouter un indicateur pour vérifier si l'utilisateur est un administrateur

        public Menu(Socket SocketClient)
        {
            this.aController = new ActionController(SocketClient);
            InitializeMenu();
        }

        // Initialiser le menu avec les options disponibles
        private void InitializeMenu()
        {
            options = new List<Option>
            {
                new Option(isAdmin ? "Déconnecter administrateur" : "Connecter administrateur", isAdmin ? LogoutAdmin : MenuConnexionAdmin),
                new Option("Demande de requête", MenuDemandeRequete),
                new("Quitter", () => Environment.Exit(0)),
            };
        }

        // Afficher le menu principal
        public void MenuPrincipal()
        {
            int index = 0;
            bool inMenu = true;
            WriteMenu(options, options[index]);

            Console.CursorVisible = false; // Cacher le curseur

            while (true)
            {
                if (inMenu)
                {
                    ConsoleKeyInfo keyinfo = Console.ReadKey(true);
                    if (keyinfo.Key == ConsoleKey.DownArrow)
                    {
                        do
                        {
                            index = (index + 1) % options.Count;
                        } while (!isAdmin && options[index].Name.Contains("(Admin)"));
                        WriteMenu(options, options[index]);
                    }
                    else if (keyinfo.Key == ConsoleKey.UpArrow)
                    {
                        do
                        {
                            index = (index - 1 + options.Count) % options.Count;
                        } while (!isAdmin && options[index].Name.Contains("(Admin)"));
                        WriteMenu(options, options[index]);
                    }
                    else if (keyinfo.Key == ConsoleKey.Enter)
                    {
                        if (!isAdmin && options[index].Name.Contains("(Admin)"))
                        {
                            Console.WriteLine("Accès refusé. Administrateur seulement.");
                            Thread.Sleep(1000);
                            WriteMenu(options, options[index]);
                        }
                        else
                        {
                            inMenu = false;
                            options[index].Selected.Invoke();
                            index = 0;
                            InitializeMenu(); // Re-initialize the menu to update the options
                            WriteMenu(options, options[index]);
                            inMenu = true;
                        }
                    }
                    else if (keyinfo.Key == ConsoleKey.X)
                    {
                        break;
                    }
                }
            }

            Console.CursorVisible = true; // Afficher le curseur
        }

        // Écrire le menu à l'écran
        private void WriteMenu(List<Option> options, Option selectedOption)
        {
            Console.Clear();
            int width = Console.WindowWidth;
            string border = new('-', width);

            Console.WriteLine(border);
            Console.WriteLine(CenterText("***********************************", width));
            Console.WriteLine(CenterText("*****     DEVOIR 3 : Annulaire     *****", width));
            Console.WriteLine(CenterText("***********************************", width));
            Console.WriteLine(border);

            // Indiquer si l'utilisateur est un administrateur
            string adminStatus = isAdmin ? "Statut: Administrateur" : "Statut: Utilisateur";
            Console.WriteLine(CenterText(adminStatus, width));
            Console.WriteLine(border);

            foreach (Option option in options)
            {
                string optionText = option == selectedOption ? $"> {option.Name}" : $"  {option.Name}";
                if (!isAdmin && option.Name.Contains("(Admin)"))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                else if (option == selectedOption && (option.Name == "Déconnecter administrateur" || option.Name == "Quitter" || option.Name == "Retour"))
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                }
                else if (option == selectedOption)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                }
                else
                {
                    Console.ResetColor();
                }

                Console.WriteLine(CenterText(optionText, width));
            }
            Console.ResetColor();
            Console.WriteLine(border);
        }

        // Centrer le texte dans la console
        private string CenterText(string text, int width)
        {
            int padding = (width - text.Length) / 2;
            return new string(' ', padding) + text;
        }

        // Menu pour la connexion administrateur
        private void MenuConnexionAdmin()
        {
            Console.Clear();
            Console.CursorVisible = true; // Afficher le curseur
            ReceiveController.PrintDynamicMessage("Entrer le mot de passe administrateur.");
            string Entree = "";
            while (true)
            {
                ConsoleKeyInfo keyinfo = Console.ReadKey(true);
                if (keyinfo.Key == ConsoleKey.Enter)
                {
                    if (!string.IsNullOrEmpty(Entree))
                    {
                        aController.VerifierMdp(Entree);
                        if (Entree == "12345") // FIX POUR USE SERVER INSTEAD
                            isAdmin = true; // TO DO, DEMANDER MATH COMMENT CHERCHER SI MDP EST VRAI ADMIN
                        else
                        {
                            isAdmin = false;
                        }
                        break;

                    }
                }
                else if (keyinfo.Key == ConsoleKey.Backspace && Entree.Length > 0)
                {
                    Entree = Entree.Substring(0, Entree.Length - 1);
                    Console.Write("\b \b");
                }
                else if (!char.IsControl(keyinfo.KeyChar))
                {
                    Entree += keyinfo.KeyChar;
                    Console.Write("*");
                }
            }
            Console.CursorVisible = false; // Cacher le curseur
        }

        // Déconnexion administrateur
        private void LogoutAdmin()
        {
            isAdmin = false;
            ReceiveController.PrintDynamicMessage("Vous êtes maintenant déconnecté en tant qu'administrateur.");
            Thread.Sleep(2000);
            InitializeMenu();
            MenuPrincipal();
        }

        // Menu pour les demandes de requêtes
        private void MenuDemandeRequete()
        {
            int index = 0;
            bool inMenu = true;
            List<Option> requestOptions = new List<Option>
            {
                new Option("Lister membre catégorie", () =>
                {
                    List<string> listVerifCate = new List<string>() { "Etudiant", "Professeur" };
                    string categorie = SelectOption("catégorie", listVerifCate);
                    aController.ListerMembreCategorie(categorie);
                    ReceiveController.PrintDynamicMessage("Table des membres pour:" + categorie);
                    ReceiveController.PrintTableHeaders();
                    Console.CursorVisible = true; // Afficher le curseur
                    Console.ReadLine();
                    Console.CursorVisible = false; // Cacher le curseur
                }),
                new Option("Lister professeur domaine", () =>
                {
                    List<string> listVerifDom = new List<string>() { "Logiciel", "Science des données", "Web et mobile", "Cybersécurité" };
                    string domaine = SelectOption("domaine", listVerifDom);
                    aController.ListerProfesseurDomaine(domaine);
                    ReceiveController.PrintDynamicMessage("Table des membres pour:" + domaine);
                    ReceiveController.PrintTableHeaders();
                    Console.CursorVisible = true; // Afficher le curseur
                    Console.ReadLine();
                    Console.CursorVisible = false; // Cacher le curseur
                }),
                new Option("Rechercher un membre", () =>
                {
                    ReceiveController.PrintDynamicMessage("Entrer le nom du membre que vous recherchez.");
                    string nom = SingleInput("nom");
                    aController.RechercherMembre(nom);
                    ReceiveController.PrintDynamicMessage("Table des membres pour:" + nom);
                    ReceiveController.PrintTableHeaders();
                    Console.CursorVisible = true; // Afficher le curseur
                    Console.ReadLine();
                    Console.CursorVisible = false; // Cacher le curseur
                }),
                new Option("Ajouter un membre (Admin)", () =>
                {
                    if (!isAdmin)
                    {
                        Console.WriteLine("Accès refusé. Administrateur seulement.");
                        Thread.Sleep(1000);
                        return;
                    }
                    List<string> p = MenuAjoutMembre();
                    aController.AjouterMembre(p[0], p[1], p[2], p[3], p[4], p[5], p[6], p[7]);
                    Console.WriteLine("Appuyez sur Entrée pour revenir...");
                    Console.CursorVisible = true; // Afficher le curseur
                    Console.ReadLine();
                    Console.CursorVisible = false; // Cacher le curseur
                }),
                new Option("Supprimer un membre (Admin)", () =>
                {
                    if (!isAdmin)
                    {
                        Console.WriteLine("Accès refusé. Administrateur seulement.");
                        Thread.Sleep(1000);
                        return;
                    }
                    ReceiveController.PrintDynamicMessage("Entrer le numéro dont vous voulez supprimer de la BD.");
                    string supNum = SingleInput("numéro");
                    aController.SupprimerMembre(supNum);
                    Console.WriteLine("Appuyez sur Entrée pour revenir...");
                    Console.CursorVisible = true; // Afficher le curseur
                    Console.ReadLine();
                    Console.CursorVisible = false; // Cacher le curseur
                }),
                new Option("Modifier un membre (Admin)", () =>
                {
                    if (!isAdmin)
                    {
                        Console.WriteLine("Accès refusé. Administrateur seulement.");
                        Thread.Sleep(1000);
                        return;
                    }
                    ReceiveController.PrintDynamicMessage("Entrer le numéro dont vous voulez modifier dans la BD.");
                    string modifyNum = SingleInput("numéro");
                    List<string> d = MenuModifierMembre(modifyNum);
                    aController.ModifierMembre(d[0], d[1], d[2], d[3], d[4], d[5], d[6], d[7], d[8]);
                    Console.WriteLine("Appuyez sur Entrée pour revenir...");
                    Console.CursorVisible = true; // Afficher le curseur
                    Console.ReadLine();
                    Console.CursorVisible = false; // Cacher le curseur
                }),
                new Option("Mettre un membre sur Liste Rouge (Admin)", () =>
                {
                    if (!isAdmin)
                    {
                        Console.WriteLine("Accès refusé. Administrateur seulement.");
                        Thread.Sleep(1000);
                        return;
                    }
                    ReceiveController.PrintDynamicMessage("Entrer le numéro dont vous voulez ajouter à la Liste Rouge.");
                    string ajoutLRNum = SingleInput("numéro");
                    aController.AjouterMembreListeRouge(ajoutLRNum);
                    Console.WriteLine("Appuyez sur Entrée pour revenir...");
                    Console.CursorVisible = true; // Afficher le curseur
                    Console.ReadLine();
                    Console.CursorVisible = false; // Cacher le curseur
                }),
                new Option("Enlever un membre sur Liste Rouge (Admin)", () =>
                {
                    if (!isAdmin)
                    {
                        Console.WriteLine("Accès refusé. Administrateur seulement.");
                        Thread.Sleep(1000);
                        return;
                    }
                    ReceiveController.PrintDynamicMessage("Entrer le numéro dont vous voulez enlever de la Liste Rouge.");
                    string removeLRNum = SingleInput("numéro");
                    aController.EnleverMembreListRouge(removeLRNum);
                    Console.WriteLine("Appuyez sur Entrée pour revenir...");
                    Console.CursorVisible = true; // Afficher le curseur
                    Console.ReadLine();
                    Console.CursorVisible = false; // Cacher le curseur
                }),
                new Option("Retour", () => { })
            };

            WriteMenu(requestOptions, requestOptions[index]);

            Console.CursorVisible = false; // Cacher le curseur

            while (true)
            {
                if (inMenu)
                {
                    ConsoleKeyInfo keyinfo = Console.ReadKey(true);
                    if (keyinfo.Key == ConsoleKey.DownArrow)
                    {
                        do
                        {
                            index = (index + 1) % requestOptions.Count;
                        } while (!isAdmin && requestOptions[index].Name.Contains("(Admin)"));
                        WriteMenu(requestOptions, requestOptions[index]);
                    }
                    else if (keyinfo.Key == ConsoleKey.UpArrow)
                    {
                        do
                        {
                            index = (index - 1 + requestOptions.Count) % requestOptions.Count;
                        } while (!isAdmin && requestOptions[index].Name.Contains("(Admin)"));
                        WriteMenu(requestOptions, requestOptions[index]);
                    }
                    else if (keyinfo.Key == ConsoleKey.Enter)
                    {
                        if (!isAdmin && requestOptions[index].Name.Contains("(Admin)"))
                        {
                            Console.WriteLine("Accès refusé. Administrateur seulement.");
                            Thread.Sleep(1000);
                            WriteMenu(requestOptions, requestOptions[index]);
                        }
                        else
                        {
                            inMenu = false;
                            requestOptions[index].Selected.Invoke();
                            if (index == requestOptions.Count - 1) // Si "Retour" est sélectionné
                            {
                                break;
                            }
                            index = 0;
                            WriteMenu(requestOptions, requestOptions[index]);
                            inMenu = true;
                        }
                    }
                    else if (keyinfo.Key == ConsoleKey.X)
                    {
                        break;
                    }
                }
            }

            Console.CursorVisible = true; // Afficher le curseur
        }

        // Menu pour modifier un membre
        private List<string> MenuModifierMembre(string num)
        {
            List<string> listVerifCate = new List<string>() { "Etudiant", "Professeur" };
            List<string> listVerifDom = new List<string>() { "Logiciel", "Science des données", "Web et mobile", "Cybersécurité" };
            List<string> listVerifLR = new List<string>() { "true", "false" };
            List<string> list = new List<string>();
            list.Add(num);
            ReceiveController.PrintDynamicMessage("Entrer le nom.");
            list.Add(SingleInput("nom"));
            ReceiveController.PrintDynamicMessage("Entrer le prénom.");
            list.Add(SingleInput("prenom"));
            ReceiveController.PrintDynamicMessage("Entrer une catégorie entre \"Etudiant\" et \"Professeur\"");
            list.Add(SelectOption("catégorie", listVerifCate));
            if (list[3] == "Etudiant")
            {
                ReceiveController.PrintDynamicMessage("Entrer le matricule.");
                list.Add(SingleInput("matricule"));
            }
            else { list.Add("Null"); }
            ReceiveController.PrintDynamicMessage("Entrer le email.");
            list.Add(SingleInput("email"));
            if (list[3] == "Professeur")
            {
                ReceiveController.PrintDynamicMessage("Entrer le téléphone.");
                list.Add(SingleInput("téléphone"));
            }
            else { list.Add("Null"); }
            ReceiveController.PrintDynamicMessage("Entrer si dans Liste Rouge entre \"true\", \"false\"");
            list.Add(SelectOption("ListeRouge", listVerifLR));
            ReceiveController.PrintDynamicMessage("Entrer un domaine entre \"Logiciel\", \"Science des données\", \"Web et mobile\", \"Cybersécurité\"");
            list.Add(SelectOption("domaine", listVerifDom));
            return list;
        }

        // Menu pour ajouter un membre
        private List<string> MenuAjoutMembre()
        {
            List<string> listVerifCate = new List<string>() { "Etudiant", "Professeur" };
            List<string> listVerifDom = new List<string>() { "Logiciel", "Science des données", "Web et mobile", "Cybersécurité" };
            List<string> listVerifLR = new List<string>() { "true", "false" };
            List<string> list = new List<string>();
            ReceiveController.PrintDynamicMessage("Entrer le nom.");
            list.Add(SingleInput("nom"));
            ReceiveController.PrintDynamicMessage("Entrer le prénom.");
            list.Add(SingleInput("prenom"));
            ReceiveController.PrintDynamicMessage("Entrer une catégorie entre \"Etudiant\" et \"Professeur\"");
            list.Add(SelectOption("catégorie", listVerifCate));
            if (list[2] == "Etudiant")
            {
                ReceiveController.PrintDynamicMessage("Entrer le matricule.");
                list.Add(SingleInput("matricule"));
            }
            else { list.Add("Null"); }
            ReceiveController.PrintDynamicMessage("Entrer le email.");
            list.Add(SingleInput("email"));
            if (list[2] == "Professeur")
            {
                ReceiveController.PrintDynamicMessage("Entrer le téléphone.");
                list.Add(SingleInput("téléphone"));
            }
            else { list.Add("Null"); }
            ReceiveController.PrintDynamicMessage("Entrer si dans Liste Rouge entre \"true\", \"false\"");
            list.Add(SelectOption("ListeRouge", listVerifLR));
            ReceiveController.PrintDynamicMessage("Entrer un domaine entre \"Logiciel\", \"Science des données\", \"Web et mobile\", \"Cybersécurité\"");
            list.Add(SelectOption("domaine", listVerifDom));
            return list;
        }

        // Obtenir une entrée unique de l'utilisateur
        private string SingleInput(string MotCle)
        {
            Console.CursorVisible = true; // Afficher le curseur
            while (true)
            {
                try
                {
                    string input = Console.ReadLine();
                    if (input != null || input != "")
                    {
                        Console.CursorVisible = false; // Cacher le curseur
                        return input;
                    }
                    else
                    {
                        ReceiveController.PrintDynamicMessage($"Veuillez entrer un {MotCle} valide.");
                    }
                }
                catch
                {
                    ReceiveController.PrintDynamicMessage($"Veuillez entrer un {MotCle} valide.");
                }
            }
        }

        // Sélectionner une option parmi une liste
        private string SelectOption(string MotCle, List<string> options)
        {
            int index = 0;
            bool inMenu = true;
            Console.CursorVisible = false; // Cacher le curseur

            while (true)
            {
                Console.Clear();
                ReceiveController.PrintDynamicMessage($"Veuillez sélectionner un {MotCle} valide:");

                for (int i = 0; i < options.Count; i++)
                {
                    if (i == index)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write("> ");
                    }
                    else
                    {
                        Console.ResetColor();
                        Console.Write("  ");
                    }
                    Console.WriteLine(options[i]);
                }
                Console.ResetColor();

                if (inMenu)
                {
                    ConsoleKeyInfo keyinfo = Console.ReadKey(true);
                    if (keyinfo.Key == ConsoleKey.DownArrow)
                    {
                        index = (index + 1) % options.Count;
                    }
                    else if (keyinfo.Key == ConsoleKey.UpArrow)
                    {
                        index = (index - 1 + options.Count) % options.Count;
                    }
                    else if (keyinfo.Key == ConsoleKey.Enter)
                    {
                        Console.CursorVisible = true; // Afficher le curseur
                        return options[index];
                    }
                }
            }
        }
    }

    public class Option
    {
        public string Name { get; }
        public Action Selected { get; }

        public Option(string name, Action selected)
        {
            Name = name;
            Selected = selected;
        }
    }
}
