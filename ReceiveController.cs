using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Annulaire_Client
{
    internal class ReceiveController
    {
        public ReceiveController() { }

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



        public void PrintConnexionSucces()
        {
            PrintDynamicMessage("Vous êtes désormais connecté en tant qu'administrateur");
        }

        public void PrintConnexionFailed()
        {
            PrintDynamicMessage("Mauvais mot de passe, vous n'êtes pas connecté en tant qu'administrateur.");
        }

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

        public void PrintDemandeSucces(string succesMessage)
        {
            PrintDynamicMessage(succesMessage);
        }

        public void PrintDemandeFailed(string erreur)
        {
            PrintDynamicMessage(erreur);
        }
    }
}
