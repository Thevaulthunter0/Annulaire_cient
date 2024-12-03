using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.ComponentModel.Design;

namespace Annulaire_Client
{
    internal class Paquet
    {
        public int intInfo { get; set; }
        public bool boolInfo { get; set; }
        public int idClient { get; set; }
        public TypePaquet type { get; set; }
        public List<List<String>> donnee { get; set; }

        //Creer un paquet a partir de byte
        //Utiliser quand recoit.
        public Paquet(byte[] bytePaquet)
        {
            string jsonStr = Encoding.UTF8.GetString(bytePaquet);
            jsonStr = jsonStr.Replace("\0", "");
            var paquet = JsonSerializer.Deserialize<Paquet>(jsonStr);
            if (paquet != null)
            {
                this.intInfo = paquet.intInfo;
                this.boolInfo = paquet.boolInfo;
                this.idClient = paquet.idClient;
                this.type = paquet.type;
                this.donnee = paquet.donnee;
            }
        }

        public Paquet()
        {
            // Default constructor (parameterless)
        }

        //Creer un paquet a partir de nouvelle propriete
        //Puis utiliser la fonciton bytes pour la transformer en byte.
        public Paquet(int intInfo, int idClient, TypePaquet type, List<List<String>> donnee, bool boolInfo)
        {
            this.intInfo = intInfo;
            this.idClient = idClient;
            this.type = type;
            this.donnee = donnee;
            this.boolInfo = boolInfo;
        }

        //Transformer this. en forme json byte
        public byte[] bytes()
        {
            string jsonStr = JsonSerializer.Serialize(this);
            return Encoding.UTF8.GetBytes(jsonStr);
        }
    }

    public enum TypePaquet
    {
        Connexion,
        Demande,
        Deconnexion
    }
}