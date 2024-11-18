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

        public void PrintConnexionSucces()
        {
            Console.WriteLine("=====\nVous êtes désormais connecté en tant qu'administrateur\n=====");
        }

        public void PrintConnexionFailed()
        {
            Console.WriteLine("=====\nMauvais mot de passe, vous n'êtes pas connecté en tant qu'administrateur.\n=====");
        }

        public void PrintMembre(List<List<string>> membres)
        {
            Console.WriteLine("----------------------------------------");
            membres.ForEach(membre => { membre.ForEach(p => Console.Write(p + " | ")); 
                Console.WriteLine("\n----------------------------------------"); });
        }

        public void PrintDemandeSucces(string succesMessage)
        {
            Console.WriteLine("=====\n" + succesMessage + "\n=====");
        }

        public void PrintDemandeFailed(string erreur)
        {
            Console.WriteLine("=====\n" + erreur + "\n=====");
        }
    }
}
