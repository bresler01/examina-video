using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace proj
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            buttonclear.FlatAppearance.MouseOverBackColor = Color.Red;
            buttonCriarConta.FlatAppearance.MouseOverBackColor = Color.CornflowerBlue;
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

                panel4.Width = this.Size.Width / 2;
                panel4.Left = this.ClientSize.Width - panel4.Width;
                panel5.Width = this.Size.Width / 2;
            }
            else if (this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal;

                panel4.Width = this.Size.Width / 2;
                panel4.Left = this.ClientSize.Width - panel4.Width;
                panel5.Width = this.Size.Width / 2;
            }
        }

        private void textBoxSenha_TextChanged(object sender, EventArgs e)
        {
            textBoxSenha.PasswordChar = '•';
        }

        private void buttonVer_Click(object sender, EventArgs e)
        {
            if (senhaVisivel)
            {
                textBoxSenha.PasswordChar = '•';
                senhaVisivel = false;
            }
            else
            {
                textBoxSenha.PasswordChar = '\0';
                senhaVisivel = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBoxNome.Clear();
            textBoxApelido.Clear();
            textBoxEmail.Clear();
            textBoxSenha.Clear();
        }

        private void label6_Click(object sender, EventArgs e)
        {
            Login log = new Login();
            this.Hide();
            log.Closed += (s, args) => this.Close();

            log.Show();
        }

        private void buttonCriarConta_Click(object sender, EventArgs e)
        {
            string nome = textBoxNome.Text.Trim();
            string apelido = textBoxApelido.Text.Trim();
            string email = textBoxEmail.Text.Trim();
            string passe = textBoxSenha.Text;

            if (string.IsNullOrEmpty(nome) ||
                string.IsNullOrEmpty(apelido) ||
                string.IsNullOrEmpty(email) ||
                string.IsNullOrEmpty(passe))
            {
                MessageBox.Show("Por favor, preencha todos os campos!", "Campos obrigatórios", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            bool emailExiste = false;
            if (System.IO.File.Exists("utilizadores.txt"))
            {
                foreach (var linha in System.IO.File.ReadLines("utilizadores.txt"))
                {
                    DadosLog userExistente = DadosLog.FromFileLine(linha);
                    if (userExistente.GetEmail().Trim().Equals(email, StringComparison.OrdinalIgnoreCase))
                    {
                        emailExiste = true;
                        break;
                    }
                }
            }

            if (emailExiste)
            {
                MessageBox.Show("Este email já está registado! Faça login.", "Conta já existe", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Login loginForm = new Login();
                loginForm.Show();
                this.Hide();
                return;
            }

            string tipo;

            if (radioButtonAluno.Checked)
            {
                tipo = "Examinando";
            }
            else if (radioButtonProfessor.Checked)
            {
                tipo = "Examinador";
            }
            else
            {
                MessageBox.Show("Selecione Examinando ou Examinador!");
                return;
            }

            string palavraPasseEncriptada = Encriptar.Encrypt(passe);

            DadosLog user = new DadosLog();
            user.SetNome(nome);
            user.SetApelido(apelido);
            user.SetEmail(email);
            user.SetPasse(palavraPasseEncriptada);
            user.SetTipo(tipo);

            using (StreamWriter sw = new StreamWriter("utilizadores.txt", true))
            {
                sw.WriteLine(user.ToFileLine());
            }

            MessageBox.Show("Conta criada com sucesso!");

            textBoxNome.Clear();
            textBoxApelido.Clear();
            textBoxEmail.Clear();
            textBoxSenha.Clear();

            Login log = new Login();
            log.Show();
            this.Hide();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
