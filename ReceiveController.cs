
namespace Annulaire_Client
{
    internal class ReceiveController
    {
        public ReceiveController() { }

        //Controlleur, annalyse le paquet recu et affiche
        public void HandleReceivedMessage(Paquet paquet, Menu menu)
        {
            switch (paquet.type)
            {
                case TypePaquet.Connexion:
                    if (paquet.boolInfo == true)
                    {
                        PrintConnexionSucces();
                        menu.setIsAdmin(true);
                    }
                    else
                    {
                        PrintConnexionFailed();
                        menu.setIsAdmin(false);
                    }
                    break;

                case TypePaquet.Demande:
                    switch (paquet.intInfo)
                    {
                        case 1:
                            if (paquet.boolInfo == true)
                            {
                                PrintMembre(paquet.donnee);
                            }
                            else { PrintDemandeFailed(paquet.donnee[0][0]); }
                            break;
                        case 2:
                            if (paquet.boolInfo == true)
                            {
                                PrintMembre(paquet.donnee);
                            }
                            else { PrintDemandeFailed(paquet.donnee[0][0]); }
                            break;
                        case 3:
                            if (paquet.boolInfo == true)
                            {
                               PrintMembre(paquet.donnee);
                            }
                            else { PrintDemandeFailed(paquet.donnee[0][0]); }
                            break;
                        case 4:
                            if (paquet.boolInfo == true)
                            {
                                PrintDemandeSucces("Succes, le membre a bien été ajouté");
                            }
                            else { PrintDemandeFailed(paquet.donnee[0][0]); }
                            break;
                        case 5:
                            if (paquet.boolInfo == true)
                            {
                                PrintDemandeSucces("Succes, le membre a bien été supprimé.");
                            }
                            else { PrintDemandeFailed(paquet.donnee[0][0]); }
                            break;
                        case 6:
                            if (paquet.boolInfo == true)
                            {
                                PrintDemandeSucces("Succes, le membre a bien été modifié.");
                            }
                            else { PrintDemandeFailed(paquet.donnee[0][0]); }
                            break;
                        case 7:
                            if (paquet.boolInfo == true)
                            {
                                PrintDemandeSucces("Succes, si le numéro existe le membre a bien été mis sur la liste rouge.");
                            }
                            else { PrintDemandeFailed(paquet.donnee[0][0]); }
                            break;
                        case 8:
                            if (paquet.boolInfo == true)
                            {
                                PrintDemandeSucces("Succes, si le numéro existe le membre a bien été enlever de la liste rouge.");
                            }
                            else { PrintDemandeFailed(paquet.donnee[0][0]); }
                            break;
                    }
                    break;
            }
        }

        //Affiche un message dynamique
        public static void PrintDynamicMessage(string message)
        {
            int consoleWidth = Console.WindowWidth;
            string separator = new string('-', consoleWidth);

            Console.WriteLine(separator);

            string[] lines = message.Split('\n');
            foreach (var originalLine in lines)
            {
                string line = originalLine;
                int contentWidth = line.Length;
                int padding = (consoleWidth - contentWidth - 4) / 2;
                if (padding > 0)
                {
                    line = new string(' ', padding) + line + new string(' ', padding);
                }
                if (line.Length + 4 < consoleWidth)
                {
                    line += " ";
                }
                string formattedLine = "| " + line + " |";

                if (formattedLine.Length > consoleWidth)
                {
                    formattedLine = formattedLine.Substring(0, consoleWidth - 1) + "|";
                }

                Console.WriteLine(formattedLine);
                Console.WriteLine(separator);
            }
        }

        //Pour afficher tout les proprietes d'un membre lorsqu'on affiche un ou plusieurs membres
        public static void PrintTableHeaders()
        {
            // Définir les en-têtes
            List<string> headers = new List<string> { "Nom", "Prenom", "Categorie", "Matricule", "Email", "Telephone", "Liste Rouge", "Domaine" };

            // Largeur totale de la console
            int consoleWidth = Console.WindowWidth;

            // Calculer la largeur totale de la ligne d'en-têtes
            string headerLine = string.Join(" | ", headers); // Utilise un séparateur visible
            int totalHeaderWidth = headerLine.Length;

            // Calculer l'espacement pour centrer
            int leftPadding = Math.Max(0, (consoleWidth - totalHeaderWidth) / 2);

            // Générer la ligne centrée
            string paddedHeaderLine = new string(' ', leftPadding) + headerLine;

            // Afficher les en-têtes
            Console.WriteLine(paddedHeaderLine);
        }

        //Pour afficher une connexion en tant qu'administrateur
        public void PrintConnexionSucces()
        {
            PrintDynamicMessage("Vous êtes désormais connecté en tant qu'administrateur");
        }

        //Pour afficher que le mot de passe entree, etait le mauvais
        public void PrintConnexionFailed()
        {
            PrintDynamicMessage("Mauvais mot de passe, vous n'êtes pas connecté en tant qu'administrateur.");
        }

        //Fonction pour afficher les membres retournees dans le Paquet
        public void PrintMembre(List<List<string>> membres)
        {
            int consoleWidth = Console.WindowWidth;
            string separator = new string('-', consoleWidth);

            Console.WriteLine(separator);
            membres.ForEach(membre =>
            {
                // Construct the content row
                string content = string.Join(" | ", membre);
                int contentWidth = content.Length;
                int padding = (consoleWidth - contentWidth - 4) / 2; // 4 accounts for the borders and spaces ("| ")

                // Ensure the content is centered between the borders
                if (padding > 0)
                {
                    content = new string(' ', padding) + content + new string(' ', padding);
                }

                // Adjust if the width is odd and doesn't perfectly fit
                if (content.Length + 4 < consoleWidth)
                {
                    content += " ";
                }

                // Add borders
                string line = "| " + content + " |";

                // Truncate if line exceeds console width
                if (line.Length > consoleWidth)
                {
                    line = line.Substring(0, consoleWidth - 1) + "|";
                }

                Console.WriteLine(line);
                Console.WriteLine(separator);
            });
        }

        //Custom print message succes
        public void PrintDemandeSucces(string succesMessage)
        {
            PrintDynamicMessage(succesMessage);
        }

        //Custom print message echec
        public void PrintDemandeFailed(string erreur)
        {
            PrintDynamicMessage(erreur);
        }
    }
}
