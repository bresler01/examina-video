using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;

namespace proj
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
            buttonclear.FlatAppearance.MouseOverBackColor = Color.Red;
            buttonLogin.FlatAppearance.MouseOverBackColor = Color.CornflowerBlue;
        }

        private bool senhaVisivel = false;

        private void buttonclose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void buttonminimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void buttonfullscreen_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal)
            {
                this.WindowState = FormWindowState.Maximized;

                panel3.Width = this.Size.Width / 2;
                panel3.Left = this.ClientSize.Width - panel3.Width;
                panel2.Width = this.Size.Width / 2;
            }
            else if (this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal;

                panel3.Width = this.Size.Width / 2;
                panel3.Left = this.ClientSize.Width - panel3.Width;
                panel2.Width = this.Size.Width / 2;
            }
        }

        private void buttonclear_Click(object sender, EventArgs e)
        {
            textBoxEmailLog.Clear();
            textBoxLog.Clear();
        }

        private void buttonCriarConta_Click(object sender, EventArgs e)
        {
            string email = textBoxEmailLog.Text.Trim();
            string palavraPasse = textBoxLog.Text;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(palavraPasse))
            {
                labelmensagem.Text = "Por favor, preencha todos os campos!";
                return;
            }


            bool emailExiste = false;
            bool credenciaisCorretas = false;

            DadosLog userEncontrado = null;

            if (!File.Exists("utilizadores.txt"))
            {
                labelmensagem.Text = "Nenhum utilizador registado!";
                return;
            }

            foreach (var linha in File.ReadLines("utilizadores.txt"))
            {
                DadosLog user = DadosLog.FromFileLine(linha);
                if (user.GetEmail().Trim().Equals(email, StringComparison.OrdinalIgnoreCase))
                {
                    emailExiste = true;
                    string palavraPasseDesencriptada = Encriptar.Decrypt(user.GetPasse());
                    if (palavraPasseDesencriptada == palavraPasse)
                    {
                        credenciaisCorretas = true;
                        userEncontrado = user;
                        break;
                    }
                }
            }

            if (emailExiste && credenciaisCorretas)
            {
                if (userEncontrado.GetTipo() == "Examinando")
                {
                    Examinando Alu = new Examinando(userEncontrado.GetNome(), userEncontrado.GetApelido(), userEncontrado.GetEmail(), Encriptar.Decrypt(userEncontrado.GetPasse()));
                    Alu.Show();
                }
                else if (userEncontrado.GetTipo() == "Examinador")
                {
                    Examinador Pro = new Examinador(userEncontrado.GetNome(), userEncontrado.GetApelido(), userEncontrado.GetEmail(), Encriptar.Decrypt(userEncontrado.GetPasse()));
                    Pro.Show();
                }
                this.Hide();
            }
            else if (emailExiste)
            {
                labelmensagem.Text = "Palavra-passe incorreta!";
            }
            else
            {
                MessageBox.Show("Utilizador não existe. Por favor, registe-se!", "Conta não encontrada", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Form1 reg = new Form1();
                reg.Show();
                this.Hide();
            }
        }

        private void textBoxLog_TextChanged(object sender, EventArgs e)
        {
            textBoxLog.PasswordChar = '•';
        }

        private void buttonVer_Click(object sender, EventArgs e)
        {
            if (senhaVisivel)
            {
                textBoxLog.PasswordChar = '•';
                senhaVisivel = false;
            }
            else
            {
                textBoxLog.PasswordChar = '\0';
                senhaVisivel = true;
            }
        }

        private void labelVoltar_Click(object sender, EventArgs e)
        {
            Form1 reg = new Form1();
            reg.Show();
            this.Hide();
        }
    }
}
