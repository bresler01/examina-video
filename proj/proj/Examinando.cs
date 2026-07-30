using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace proj
{
    public partial class Examinando : Form
    {
        private string nomeUsuario;
        private string apelidoUsuario;
        private string emailUsuarioo;
        private string SenhaUsuario;
        private string emailOriginal;
        public Examinando(string nome, string apelido, string email, string passe)
        {
            InitializeComponent();

            axWindowsMediaPlayer1.ClickEvent += axWindowsMediaPlayer1_ClickEvent;

            listBoxHist.Visible = false;
            axWindowsMediaPlayer1.Visible = false;
            listBoxTestes.Visible = true;
            labela.Visible = false;
            labelb.Visible = false;
            buttoneditar.Visible = false;
            buttonenviar.Visible = false;
            label5.Visible = false;
            label0.Visible = false;
            label4.Visible = false;
            label8.Visible = false;
            label9.Visible = false;
            label11.Visible = false;
            textBoxNome.Visible = false;
            textBoxApelido.Visible = false;
            textBoxEmail.Visible = false;
            textBoxSenha.Visible = false;
            buttonVer.Visible = false;
            button7.Visible = false;
            button6.Visible = false;
            pictureBox2.Visible = false;
            pictureBox3.Visible = false;
            pictureBox4.Visible = false;
            pictureBox5.Visible = false;

            emailOriginal = email;

            nomeUsuario = nome;
            apelidoUsuario = apelido;
            emailUsuarioo = email;
            SenhaUsuario = passe;
        }

        private void buttonConfig_Click(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.Visible = false;
            listBoxHist.Visible = false;
            listBoxTestes.Visible = false;
            labela.Visible = false;
            labelb.Visible = false;
            buttoneditar.Visible = false;
            buttonenviar.Visible = false;
            label5.Visible = true;
            label0.Visible = true;
            label4.Visible = true;
            label8.Visible = true;
            label9.Visible = true;
            label11.Visible = true;
            textBoxNome.Visible = true;
            textBoxApelido.Visible = true;
            textBoxEmail.Visible = true;
            textBoxSenha.Visible = true;
            buttonVer.Visible = true;
            button7.Visible = true;
            button6.Visible = true;
            pictureBox2.Visible = true;
            pictureBox3.Visible = true;
            pictureBox4.Visible = true;
            pictureBox5.Visible = true;

            textBoxNome.ReadOnly = true;
            textBoxApelido.ReadOnly = true;
            textBoxEmail.ReadOnly = true;
            textBoxSenha.ReadOnly = true;

            textBoxNome.Text = nomeUsuario;
            textBoxApelido.Text = apelidoUsuario;
            textBoxEmail.Text = emailUsuarioo;
            textBoxSenha.Text = SenhaUsuario;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            textBoxNome.ReadOnly = false;
            textBoxApelido.ReadOnly = false;
            textBoxEmail.ReadOnly = false;
            textBoxSenha.ReadOnly = false;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string novoNome = textBoxNome.Text.Trim();
            string novoApelido = textBoxApelido.Text.Trim();
            string novoEmail = textBoxEmail.Text.Trim();
            string novaSenha = textBoxSenha.Text;

            if (string.IsNullOrEmpty(novoNome) ||
                string.IsNullOrEmpty(novoApelido) ||
                string.IsNullOrEmpty(novoEmail) ||
                string.IsNullOrEmpty(novaSenha))
            {
                MessageBox.Show("Por favor, preencha todos os campos!", "Campos obrigatórios", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var linhas = System.IO.File.ReadAllLines("utilizadores.txt").ToList();
            for (int i = 0; i < linhas.Count; i++)
            {
                DadosLog user = DadosLog.FromFileLine(linhas[i]);
                if (user.GetEmail().Equals(emailOriginal, StringComparison.OrdinalIgnoreCase))
                {
                    user.SetNome(novoNome);
                    user.SetApelido(novoApelido);
                    user.SetEmail(novoEmail);
                    user.SetPasse(Encriptar.Encrypt(novaSenha));

                    linhas[i] = user.ToFileLine();
                    break;
                }
            }

            System.IO.File.WriteAllLines("utilizadores.txt", linhas);

            nomeUsuario = novoNome;
            apelidoUsuario = novoApelido;
            emailUsuarioo = novoEmail;
            SenhaUsuario = novaSenha;
            emailOriginal = novoEmail;

            textBoxNome.ReadOnly = true;
            textBoxApelido.ReadOnly = true;
            textBoxEmail.ReadOnly = true;
            textBoxSenha.ReadOnly = true;

            MessageBox.Show("Dados atualizados com sucesso!");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal)
            {
                this.WindowState = FormWindowState.Maximized;
            }
            else if (this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Examinando_Load(object sender, EventArgs e)
        {
            label3.Text = $"{nomeUsuario} {apelidoUsuario}";
            textBoxSenha.PasswordChar = '•';
            senhaVisivel = false;

            CarregarHistorico();

            string arquivoTestes = $"testes_{emailUsuarioo}.txt";
            if (File.Exists(arquivoTestes))
            {
                var linhas = File.ReadAllLines(arquivoTestes);
                listBoxTestes.Items.Clear();
                foreach (var linha in linhas)
                {
                    if (!string.IsNullOrWhiteSpace(linha))
                    {
                        listBoxTestes.Items.Add(linha.Split(';')[0]);
                        nomeProfessor = linha.Split(';')[1];
                    }
                }
            }
        }

        private bool senhaVisivel;
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

        private void label11_Click(object sender, EventArgs e)
        {
            var resultado = MessageBox.Show("Tem a certeza que deseja apagar a sua conta?", "Confirmação de Apagar Conta", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

            if (resultado == DialogResult.OK)
            {
                ApagarConta();
            }
        }

        private void ApagarConta()
        {
            try
            {
                var linhas = System.IO.File.ReadAllLines("utilizadores.txt").ToList();
                var novasLinhas = linhas
                    .Where(linha =>
                    {
                        var user = DadosLog.FromFileLine(linha);
                        return !user.GetEmail().Equals(emailUsuarioo, StringComparison.OrdinalIgnoreCase);
                    })
                    .ToList();
                System.IO.File.WriteAllLines("utilizadores.txt", novasLinhas);

                string[] pathsToDelete = {
            Path.Combine(Application.StartupPath, $"Videos_{emailUsuarioo}"),
            Path.Combine(Application.StartupPath, $"dados_{emailUsuarioo}.txt"),
            Path.Combine(Application.StartupPath, $"historico_{emailUsuarioo}.txt"),
            Path.Combine(Application.StartupPath, $"testes_{emailUsuarioo}.txt")
        };

                foreach (var path in pathsToDelete)
                {
                    try
                    {
                        if (Directory.Exists(path))
                        {
                            Directory.Delete(path, true); 
                        }
                        else if (File.Exists(path))
                        {
                            File.Delete(path); 
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Erro ao apagar {path}: {ex.Message}");
                    }
                }

                
                if (listBoxTestes != null)
                {
                    for (int i = listBoxTestes.Items.Count - 1; i >= 0; i--)
                    {
                        if (listBoxTestes.Items[i].ToString().Contains(emailUsuarioo))
                            listBoxTestes.Items.RemoveAt(i);
                    }
                }
                if (listBoxHist != null)
                {
                    for (int i = listBoxHist.Items.Count - 1; i >= 0; i--)
                    {
                        if (listBoxHist.Items[i].ToString().Contains(emailUsuarioo))
                            listBoxHist.Items.RemoveAt(i);
                    }
                }

                MessageBox.Show("Conta apagada com sucesso!");

                Form1 reg = new Form1();
                reg.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao apagar conta: {ex.Message}");
            }
        }

        private void buttonSair_Click(object sender, EventArgs e)
        {
            this.Hide();

            var reg = new Form1();
            reg.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            listBoxHist.Visible = true;
            axWindowsMediaPlayer1.Visible = false;
            listBoxTestes.Visible = false;
            labela.Visible = false;
            labelb.Visible = false;
            label5.Visible = false;
            label0.Visible = false;
            label4.Visible = false;
            label8.Visible = false;
            label9.Visible = false;
            label11.Visible = false;
            buttoneditar.Visible = false;
            buttonenviar.Visible = false;
            textBoxNome.Visible = false;
            textBoxApelido.Visible = false;
            textBoxEmail.Visible = false;
            textBoxSenha.Visible = false;
            buttonVer.Visible = false;
            button7.Visible = false;
            button6.Visible = false;
            pictureBox2.Visible = false;
            pictureBox3.Visible = false;
            pictureBox4.Visible = false;
            pictureBox5.Visible = false;





        }

        private void button1_Click(object sender, EventArgs e)
        {
            listBoxHist.Visible = false;
            axWindowsMediaPlayer1.Visible = false;
            listBoxTestes.Visible = true;
            labela.Visible = false;
            labelb.Visible = false;
            buttoneditar.Visible = false;
            buttonenviar.Visible = false;
            label5.Visible = false;
            label0.Visible = false;
            label4.Visible = false;
            label8.Visible = false;
            label9.Visible = false;
            label11.Visible = false;
            textBoxNome.Visible = false;
            textBoxApelido.Visible = false;
            textBoxEmail.Visible = false;
            textBoxSenha.Visible = false;
            buttonVer.Visible = false;
            button7.Visible = false;
            button6.Visible = false;
            pictureBox2.Visible = false;
            pictureBox3.Visible = false;
            pictureBox4.Visible = false;
            pictureBox5.Visible = false;
        }
        private bool podeEditar = true;
        private double tempoAluno = 0;
        private double xAluno = 0;
        private double yAluno = 0;
        private string nomeVideoAtual = "";
        private string nomeProfessor = "";
        private double tempoProfessor = 0;
        private double xProfessor = 0;
        private double yProfessor = 0;
        private double raioAceitacao = 50;
        private DateTime inicioTeste;

        private void listBoxTestes_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxTestes.SelectedIndex == -1)
                return;

            listBoxHist.Visible = false;
            axWindowsMediaPlayer1.Visible = true;
            listBoxTestes.Visible = false;
            labela.Visible = true;
            labelb.Visible = true;
            buttoneditar.Visible = true;
            buttonenviar.Visible = true;
            label5.Visible = false;
            label0.Visible = false;
            label4.Visible = false;
            label8.Visible = false;
            label9.Visible = false;
            label11.Visible = false;
            textBoxNome.Visible = false;
            textBoxApelido.Visible = false;
            textBoxEmail.Visible = false;
            textBoxSenha.Visible = false;
            buttonVer.Visible = false;
            button7.Visible = false;
            button6.Visible = false;
            pictureBox2.Visible = false;
            pictureBox3.Visible = false;
            pictureBox4.Visible = false;
            pictureBox5.Visible = false;

            if (listBoxTestes.SelectedItem == null)
                return;

            nomeVideoAtual = listBoxTestes.SelectedItem.ToString();

            string caminhoVideo = Path.Combine(Application.StartupPath, "Videos_" + nomeProfessor, nomeVideoAtual);
            axWindowsMediaPlayer1.URL = caminhoVideo;

            string[] linhas = File.ReadAllLines("dados_" + nomeProfessor + ".txt");
            bool achou = false;
            foreach (var linha in linhas)
            {
                var partes = linha.Split(';');
                if (partes.Length == 4 && partes[0].EndsWith(nomeVideoAtual, StringComparison.OrdinalIgnoreCase))
                {
                    tempoProfessor = double.Parse(partes[1]);
                    xProfessor = double.Parse(partes[2]);
                    yProfessor = double.Parse(partes[3]);
                    achou = true;
                    break;
                }
            }

            if (!achou)
            {
                MessageBox.Show("Ponto do professor não encontrado para este vídeo!");
                return;
            }

            podeEditar = true;
            labela.Text = "Tempo: ";
            labelb.Text = "Localização: ";
            inicioTeste = DateTime.Now;
        }

        private bool _jaMarcou = false;       
        private bool _edicaoPermitida = false; 
        private bool _jaEditou = false;

        private void axWindowsMediaPlayer1_ClickEvent(object sender, AxWMPLib._WMPOCXEvents_ClickEvent e)
        {
            if (string.IsNullOrEmpty(axWindowsMediaPlayer1.URL))
                return;

            if (!_jaMarcou)
            {
                Point pos = axWindowsMediaPlayer1.PointToClient(Cursor.Position);
                xAluno = pos.X;
                yAluno = pos.Y;
                tempoAluno = axWindowsMediaPlayer1.Ctlcontrols.currentPosition;

                labela.Text = $"Tempo: {tempoAluno:F2}s";
                labelb.Text = $"Localização: X={xAluno}, Y={yAluno}";

                _jaMarcou = true;
                MessageBox.Show("Ponto marcado com sucesso!");
            }
            else
            {
                if (_edicaoPermitida && !_jaEditou)
                {
                    Point pos = axWindowsMediaPlayer1.PointToClient(Cursor.Position);
                    xAluno = pos.X;
                    yAluno = pos.Y;
                    tempoAluno = axWindowsMediaPlayer1.Ctlcontrols.currentPosition;

                    labela.Text = $"Tempo: {tempoAluno:F2}s";
                    labelb.Text = $"Localização: X={xAluno}, Y={yAluno}";

                    _edicaoPermitida = false;
                    _jaEditou = true;
                    MessageBox.Show("Ponto editado com sucesso!");
                }
                else if (_jaEditou)
                {
                    MessageBox.Show("Você já editou uma vez. Não é permitido editar novamente.");
                }
                else
                {
                    MessageBox.Show("Clique no botão 'Editar' para permitir nova marcação.");
                }
            }
        }

        

        private void buttoneditar_Click(object sender, EventArgs e)
        {
            if (_jaEditou)
            {
                MessageBox.Show("Você já editou uma vez. Não é permitido editar novamente.");
                return;
            }
            _edicaoPermitida = true;
            MessageBox.Show("Você pode marcar um novo ponto no vídeo.");
        }

        private void buttonenviar_Click(object sender, EventArgs e)
        {
            if (tempoAluno == 0)
            {
                MessageBox.Show("Selecione um ponto no vídeo antes de enviar.");
                return;
            }

            double notaFinal = 0;
            string explicacao = "";

            double diferenca = tempoProfessor - tempoAluno;

            if (diferenca > 3.0)
            {
                notaFinal = 0;
                explicacao =
                    $"Tempo do aluno: {tempoAluno:F2}s\n" +
                    $"Localização do aluno: X={xAluno}, Y={yAluno}\n\n" +
                    $"Tempo do professor: {tempoProfessor:F2}s\n" +
                    $"Localização do professor: X={xProfessor}, Y={yProfessor}\n\n" +
                    $"Nota: {notaFinal}\n\n" +
                    "Nota 0 porque você respondeu muito antes do tempo correto (mais de 3 segundos antes).";
                MessageBox.Show(explicacao, "Resultado do Teste");
                SalvarHistoricoAluno(notaFinal, explicacao);
                RemoverTesteDaLista();
                RemoverTesteDoArquivo();
                ResetarTeste();
                return;
            }
            else if (diferenca >= 1.0 && diferenca <= 3.0)
            {
                explicacao += $"Você respondeu {diferenca:F2} segundos antes do tempo do professor, mas está dentro da tolerância permitida.\n\n";
            }

            double distancia = Math.Sqrt(Math.Pow(xAluno - xProfessor, 2) + Math.Pow(yAluno - yProfessor, 2));

            if (distancia > raioAceitacao)
            {
                notaFinal = 0;
                explicacao +=
                    $"Tempo do aluno: {tempoAluno:F2}s\n" +
                    $"Localização do aluno: X={xAluno}, Y={yAluno}\n\n" +
                    $"Tempo do professor: {tempoProfessor:F2}s\n" +
                    $"Localização do professor: X={xProfessor}, Y={yProfessor}\n\n" +
                    $"Nota: {notaFinal}\n\n" +
                    $"Nota 0 porque você clicou fora do raio de aceitação ({raioAceitacao} pixels) em relação ao ponto do professor (sua distância foi {distancia:F2} pixels).";
                MessageBox.Show(explicacao, "Resultado do Teste");
                SalvarHistoricoAluno(notaFinal, explicacao);
                RemoverTesteDaLista();
                RemoverTesteDoArquivo();
                ResetarTeste();
                return;
            }

            double notaBase = 10.0;

            double tempoLimite = tempoProfessor + 5.0;
            double penalizacaoTempo = 0;

            if (tempoAluno > tempoLimite)
            {
                penalizacaoTempo = tempoAluno - tempoLimite;
            }

            notaFinal = Math.Max(0, notaBase - penalizacaoTempo);
            notaFinal = Math.Round(notaFinal, 1);

            explicacao +=
                $"Tempo do aluno: {tempoAluno:F2}s\n" +
                $"Localização do aluno: X={xAluno}, Y={yAluno}\n\n" +
                $"Tempo do professor: {tempoProfessor:F2}s\n" +
                $"Localização do professor: X={xProfessor}, Y={yProfessor}\n\n" +
                $"Distância do ponto: {distancia:F2} pixels (raio permitido: {raioAceitacao} pixels)\n" +
                $"Tempo limite sem penalização: {tempoLimite:F2} segundos\n" +
                $"Seu tempo de resposta: {tempoAluno:F2} segundos\n\n";

            if (penalizacaoTempo > 0)
                explicacao += $"Você perdeu {penalizacaoTempo:F2} ponto(s) por responder mais de 5 segundos após o tempo do professor.\n";
            else
                explicacao += "Você respondeu dentro do tempo permitido, sem penalização por tempo.\n";

            explicacao += $"Nota final: {notaFinal}";

            MessageBox.Show(explicacao, "Resultado do Teste");

            SalvarHistoricoAluno(notaFinal, explicacao);
            RemoverTesteDaLista();
            RemoverTesteDoArquivo();
            ResetarTeste();
            axWindowsMediaPlayer1.Ctlcontrols.pause();


        }

        private void RemoverTesteDaLista()
        {
            int index = listBoxTestes.Items.IndexOf(nomeVideoAtual);
            if (index >= 0)
                listBoxTestes.Items.RemoveAt(index);
        }

        private void ResetarTeste()
        {
            axWindowsMediaPlayer1.Visible = false;
            listBoxTestes.Visible = true;
            labela.Visible = false;
            labelb.Visible = false;
            buttoneditar.Visible = false;
            buttonenviar.Visible = false;

            tempoAluno = 0;
            xAluno = 0;
            yAluno = 0;
            nomeVideoAtual = "";
        }

        private void SalvarHistoricoAluno(double notaFinal, string explicacao)
        {
            string arquivoHistorico = $"historico_{emailUsuarioo}.txt";
            string linhaHist = $"{nomeVideoAtual};{tempoAluno:F2};{xAluno};{yAluno};{tempoProfessor:F2};{xProfessor};{yProfessor};{notaFinal};{nomeUsuario};{nomeProfessor}";
            File.AppendAllText(arquivoHistorico, linhaHist + Environment.NewLine);

            if (!listBoxHist.Items.Contains(nomeVideoAtual))
                listBoxHist.Items.Add(nomeVideoAtual);
        }

        private void CarregarHistorico()
        {
            listBoxHist.Items.Clear();
            string arquivoHistorico = $"historico_{emailUsuarioo}.txt";
            if (File.Exists(arquivoHistorico))
            {
                var linhas = File.ReadAllLines(arquivoHistorico);
                var nomesVideos = new HashSet<string>();
                foreach (var linha in linhas)
                {
                    var partes = linha.Split(';');
                    if (partes.Length >= 8)
                        nomesVideos.Add(partes[0]);
                }
                foreach (var nome in nomesVideos)
                    listBoxHist.Items.Add(nome.Split(';')[0]);
            }
        }

        private void listBoxHist_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxHist.SelectedItem == null)
                return;

            string nomeVideo = listBoxHist.SelectedItem.ToString();
            string arquivoHistorico = $"historico_{emailUsuarioo}.txt";
            if (!File.Exists(arquivoHistorico))
                return;

            var linhas = File.ReadAllLines(arquivoHistorico);
            foreach (var linha in linhas)
            {
                var partes = linha.Split(';');
                if (partes.Length >= 8 && partes[0] == nomeVideo)
                {
                    string tempoAlunoStr = partes[1];
                    string xAlunoStr = partes[2];
                    string yAlunoStr = partes[3];
                    string tempoProfStr = partes[4];
                    string xProfStr = partes[5];
                    string yProfStr = partes[6];
                    string notaStr = partes[7];

                    string msg = $"Tempo do aluno: {tempoAlunoStr}s\n" +
                                 $"Localização do aluno: X={xAlunoStr}, Y={yAlunoStr}\n\n" +
                                 $"Tempo do professor: {tempoProfStr}s\n" +
                                 $"Localização do professor: X={xProfStr}, Y={yProfStr}\n\n" +
                                 $"Nota: {notaStr}";

                    MessageBox.Show(msg, "Histórico do Teste");
                    break;
                }
            }
        }

        private void RemoverTesteDoArquivo()
        {
            string arquivoTestes = $"testes_{emailUsuarioo}.txt";
            if (!File.Exists(arquivoTestes))
                return;

            var linhas = File.ReadAllLines(arquivoTestes).ToList();
            string linhaParaRemover = $"{nomeVideoAtual};{nomeProfessor}";
            linhas.RemoveAll(l => l.Trim().Equals(linhaParaRemover, StringComparison.OrdinalIgnoreCase));
            File.WriteAllLines(arquivoTestes, linhas);
        }
    }
}
