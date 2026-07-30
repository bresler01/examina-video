using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace proj
{
    public class DadosLog
    {
        private string nome, apelido, email, passe, tipo;

        public DadosLog()
        {
            nome = "";
            apelido = "";
            email = "";
            passe = "";
            tipo = "";
        }

        public DadosLog(string nome, string apelido, string email, string passe, string tipo)
        {
            this.nome = nome;
            this.apelido = apelido;
            this.email = email;
            this.passe = passe;
            this.tipo = tipo;
        }

        public void SetNome(string nome)
        {
            this.nome = nome;
        }
        public string GetNome()
        {
            return this.nome;
        }

        public void SetApelido(string apelido)
        {
            this.apelido = apelido;
        }
        public string GetApelido()
        {
            return this.apelido;
        }

        public void SetEmail(string email)
        {
            this.email = email; 
        }
        public string GetEmail()
        {
            return this.email;
        }

        public void SetPasse(string passe)
        {
            this.passe = passe;  
        }
        public string GetPasse()
        {
            return this.passe;
        }

        public void SetTipo(string tipo)
        {
            this.tipo = tipo;
        }
        public string GetTipo()
        {
            return this.tipo;
        }

        public string ToFileLine()
        {
            return $"{GetNome()};;{GetApelido()};;{GetEmail()};;{GetPasse()};;{GetTipo()}";
        }

        public static DadosLog FromFileLine(string line)
        {
            var partes = line.Split(new[] { ";;" }, StringSplitOptions.None);
            DadosLog user = new DadosLog();

            user.SetNome(partes[0]);
            user.SetApelido(partes[1]);
            user.SetEmail(partes[2]);
            user.SetPasse(partes[3]);
            user.SetTipo(partes[4]);
            return user;
        }
    }
}
