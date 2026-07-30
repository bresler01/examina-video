using AxWMPLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Windows.Forms;

namespace proj
{
    public partial class Examinador : Form
    {
        private string nomeUsuario;
        private string apelidoUsuario;
        private string emailUsuarioo;
        private string SenhaUsuario;

        private Dictionary<string, LocTemp> pontosPorVideo = new Dictionary<string, LocTemp>();
        private string pastaVideos;
        private string arquivoDadosUsuario;
        private LocTemp pontoAtual = null;
        private bool modoEdicao = false;



        public Examinador(string nome, string apelido, string email, string passe)
        {
            InitializeComponent();

            emailOriginal = email;

            nomeUsuario = nome;
            apelidoUsuario = apelido;
            emailUsuarioo = email;
            SenhaUsuario = passe;




            pastaVideos = Path.Combine(Application.StartupPath, $"Videos_{emailUsuarioo}");
            Directory.CreateDirectory(pastaVideos);
            arquivoDadosUsuario = $"dados_{emailUsuarioo}.txt";

            axWindowsMediaPlayer1.Visible = true;
            button6.Visible = true;
            button7.Visible = true;
            listBoxBiblioteca.Visible = false;
            label0.Visible = false;
            label4.Visible = false;
            label5.Visible = false;
            label8.Visible = false;
            label9.Visible = false;
            label11.Visible = false;
            textBoxNome.Visible = false;
            textBoxApelido.Visible = false;
            textBoxEmail.Visible = false;
            textBoxSenha.Visible = false;
            pictureBox2.Visible = false;
            pictureBox3.Visible = false;
            pictureBox4.Visible = false;
            pictureBox5.Visible = false;
            button5.Visible = false;
            button3.Visible = false;
            listBoxTesteAlunos.Visible = false;

            buttonVer.Visible = false;

            axWindowsMediaPlayer1.ClickEvent += axWindowsMediaPlayer1_ClickEvent;
            this.Load += Examinador_Load;
        }

        private void Examinador_Load(object sender, EventArgs e)
        {
            label3.Text = $"{nomeUsuario} {apelidoUsuario}";
            textBoxSenha.PasswordChar = '•';
            senhaVisivel = false;
            CarregarBiblioteca();
            CarregarHistoricoProfessor();
            AtualizarListaAlunosEVideos();
        }

        private void CarregarBiblioteca()
        {
            listBoxBiblioteca.Items.Clear();
            pontosPorVideo.Clear();

            if (File.Exists(arquivoDadosUsuario))
            {
                foreach (var linha in File.ReadLines(arquivoDadosUsuario))
                {
                    var ponto = LocTemp.FromString(linha);
                    if (ponto != null)
                    {
                        string nomeArquivo = Path.GetFileName(ponto.UrlVideo);
                        if (!listBoxBiblioteca.Items.Contains(nomeArquivo))
                        {
                            listBoxBiblioteca.Items.Add(nomeArquivo);
                            pontosPorVideo[nomeArquivo] = ponto;
                        }
                    }
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Arquivos de Vídeo|*.mp4;*.avi;*.wmv";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string nomeArquivo = Path.GetFileName(openFileDialog.FileName);
                string caminhoDestino = Path.Combine(pastaVideos, nomeArquivo);

                if (listBoxBiblioteca.Items.Contains(nomeArquivo))
                {
                    MessageBox.Show("Este vídeo já está na biblioteca.");
                    return;
                }

                if (!File.Exists(caminhoDestino))
                    File.Copy(openFileDialog.FileName, caminhoDestino);

                axWindowsMediaPlayer1.URL = caminhoDestino;
                axWindowsMediaPlayer1.Visible = true;
                button6.Visible = true;
                button7.Visible = true;
                listBoxBiblioteca.Visible = false;
                label0.Visible = false;
                label4.Visible = false;
                label5.Visible = false;
                label8.Visible = false;
                label9.Visible = false;
                label11.Visible = false;
                textBoxNome.Visible = false;
                textBoxApelido.Visible = false;
                textBoxEmail.Visible = false;
                textBoxSenha.Visible = false;
                pictureBox2.Visible = false;
                pictureBox3.Visible = false;
                pictureBox4.Visible = false;
                pictureBox5.Visible = false;
                button5.Visible = false;
                button3.Visible = false;
                buttonVer.Visible = false;
                listBoxTesteAlunos.Visible = false;

            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(axWindowsMediaPlayer1.URL))
            {
                MessageBox.Show("Nenhum vídeo em reprodução.");
                return;
            }

            string nomeArquivo = Path.GetFileName(axWindowsMediaPlayer1.URL);

            if (pontosPorVideo.ContainsKey(nomeArquivo))
            {
                MessageBox.Show("Já existe um ponto salvo para este vídeo. Use Editar para modificar.");
                return;
            }

            if (pontoAtual == null)
            {
                MessageBox.Show("Clique no vídeo para definir o ponto.");
                modoEdicao = false;
                cliquePermitido = true;
                return;
            }

            using (StreamWriter sw = File.AppendText(arquivoDadosUsuario))
            {
                sw.WriteLine(pontoAtual.ToString());
            }

            listBoxBiblioteca.Items.Add(nomeArquivo);
            pontosPorVideo[nomeArquivo] = pontoAtual;

            axWindowsMediaPlayer1.Ctlcontrols.currentPosition = axWindowsMediaPlayer1.currentMedia.duration;
            axWindowsMediaPlayer1.Ctlcontrols.pause();

            MessageBox.Show("Ponto guardado com sucesso!");
            pontoAtual = null;
            cliquePermitido = false;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(axWindowsMediaPlayer1.URL))
            {
                MessageBox.Show("Nenhum vídeo em reprodução.");
                return;
            }

            string nomeArquivo = Path.GetFileName(axWindowsMediaPlayer1.URL);

            if (!pontosPorVideo.ContainsKey(nomeArquivo))
            {
                MessageBox.Show("Nenhum ponto salvo para este vídeo.", "Aviso");
            }

            modoEdicao = true;
            cliquePermitido = true;
            axWindowsMediaPlayer1.Ctlcontrols.currentPosition = 0;
            MessageBox.Show("Clique no vídeo para atualizar o ponto.");
        }

        private bool cliquePermitido = true;


        private void axWindowsMediaPlayer1_ClickEvent(object sender, AxWMPLib._WMPOCXEvents_ClickEvent e)
        {
            if (cliquePermitido)
            {
                if (string.IsNullOrEmpty(axWindowsMediaPlayer1.URL))
                {
                    MessageBox.Show("URL do vídeo não está definida!");
                    return;
                }

                Point pos = axWindowsMediaPlayer1.PointToClient(Cursor.Position);
                double x = pos.X;
                double y = pos.Y;

                TimeSpan tempo;
                try
                {
                    tempo = TimeSpan.FromSeconds(axWindowsMediaPlayer1.Ctlcontrols.currentPosition);
                }
                catch
                {
                    tempo = TimeSpan.Zero;
                }

                if (pontoAtual == null || !modoEdicao)
                {
                    if (!modoEdicao)
                    {
                        pontoAtual = new LocTemp(axWindowsMediaPlayer1.URL, tempo, x, y);
                        label6.Text = $"Tempo: {tempo.ToString(@"hh\:mm\:ss\.fff")}";
                        label7.Text = $"Localização: X={x}, Y={y}";
                        MessageBox.Show("Ponto marcado. Clique em Salvar para guardar.");
                    }
                    else
                    {
                        pontoAtual = new LocTemp(axWindowsMediaPlayer1.URL, tempo, x, y);
                        label6.Text = $"Tempo: {tempo.ToString(@"hh\:mm\:ss\.fff")}";
                        label7.Text = $"Localização: X={x}, Y={y}";
                        MessageBox.Show("Ponto marcado. Clique novamente para atualizar.");
                    }
                }
                else
                {
                    pontoAtual.SetTempo(tempo);
                    pontoAtual.SetX(x);
                    pontoAtual.SetY(y);
                    pontoAtual.SetUrl(axWindowsMediaPlayer1.URL);
                    label6.Text = $"Tempo: {tempo.ToString(@"hh\:mm\:ss\.fff")}";
                    label7.Text = $"Localização: X={x}, Y={y}";
                }

                if (modoEdicao)
                {
                    AtualizarPontoSalvo();
                    modoEdicao = false;
                    cliquePermitido = false;
                    MessageBox.Show("Ponto atualizado!");
                }
                else
                {
                    cliquePermitido = false;
                }
            }
        }


        private void AtualizarPontoSalvo()
        {
            string nomeArquivo = Path.GetFileName(axWindowsMediaPlayer1.URL);
            if (!pontosPorVideo.ContainsKey(nomeArquivo)) return;

            var linhas = File.ReadAllLines(arquivoDadosUsuario).ToList();
            for (int i = 0; i < linhas.Count; i++)
            {
                var ponto = LocTemp.FromString(linhas[i]);
                if (ponto != null && Path.GetFileName(ponto.UrlVideo) == nomeArquivo)
                {
                    linhas[i] = pontoAtual.ToString();
                    break;
                }
            }
            File.WriteAllLines(arquivoDadosUsuario, linhas);
            pontosPorVideo[nomeArquivo] = pontoAtual;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.Visible = false;
            button6.Visible = false;
            button7.Visible = false;
            listBoxBiblioteca.Visible = true;
            label0.Visible = false;
            label4.Visible = false;
            label5.Visible = false;
            label8.Visible = false;
            label9.Visible = false;
            label11.Visible = false;
            label6.Visible = false;
            label7.Visible = false;
            textBoxNome.Visible = false;
            textBoxApelido.Visible = false;
            textBoxEmail.Visible = false;
            textBoxSenha.Visible = false;
            pictureBox2.Visible = false;
            pictureBox3.Visible = false;
            pictureBox4.Visible = false;
            pictureBox5.Visible = false;
            button5.Visible = false;
            button3.Visible = false;
            buttonVer.Visible = false;
            listBoxTesteAlunos.Visible = false;

        }

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
            }
            else if (this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal;
            }
        }

        private void buttonSair_Click(object sender, EventArgs e)
        {
            this.Hide();

            var reg = new Form1();
            reg.Show();
        }

        private void buttonConfig_Click(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.Visible = false;
            button6.Visible = false;
            button7.Visible = false;
            listBoxBiblioteca.Visible = false;
            label0.Visible = true;
            label4.Visible = true;
            label5.Visible = true;
            label8.Visible = true;
            label9.Visible = true;
            label11.Visible = true;
            label6.Visible = false;
            label7.Visible = false;
            textBoxNome.Visible = true;
            textBoxApelido.Visible = true;
            textBoxEmail.Visible = true;
            textBoxSenha.Visible = true;
            pictureBox2.Visible = true;
            pictureBox3.Visible = true;
            pictureBox4.Visible = true;
            pictureBox5.Visible = true;
            button5.Visible = true;
            button3.Visible = true;
            buttonVer.Visible = true;
            listBoxTesteAlunos.Visible = false;


            textBoxNome.ReadOnly = true;
            textBoxApelido.ReadOnly = true;
            textBoxEmail.ReadOnly = true;
            textBoxSenha.ReadOnly = true;

            textBoxNome.Text = nomeUsuario;
            textBoxApelido.Text = apelidoUsuario;
            textBoxEmail.Text = emailUsuarioo;
            textBoxSenha.Text = SenhaUsuario;
        }

        private bool senhaVisivel = false;
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

        private void button3_Click(object sender, EventArgs e)
        {
            textBoxNome.ReadOnly = false;
            textBoxApelido.ReadOnly = false;
            textBoxEmail.ReadOnly = false;
            textBoxSenha.ReadOnly = false;
        }

        private string emailOriginal;
        private void button5_Click(object sender, EventArgs e)
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

            textBoxNome.ReadOnly = true;
            textBoxApelido.ReadOnly = true;
            textBoxEmail.ReadOnly = true;
            textBoxSenha.ReadOnly = true;

            nomeUsuario = novoNome;
            apelidoUsuario = novoApelido;
            emailUsuarioo = novoEmail;
            SenhaUsuario = novaSenha;
            emailOriginal = novoEmail;

            MessageBox.Show("Dados atualizados com sucesso!");

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


                if (listBoxBiblioteca != null)
                {
                    for (int i = listBoxBiblioteca.Items.Count - 1; i >= 0; i--)
                    {
                        if (listBoxBiblioteca.Items[i].ToString().Contains(emailUsuarioo))
                            listBoxBiblioteca.Items.RemoveAt(i);
                    }
                }
                if (listBoxTesteAlunos != null)
                {
                    for (int i = listBoxTesteAlunos.Items.Count - 1; i >= 0; i--)
                    {
                        if (listBoxTesteAlunos.Items[i].ToString().Contains(emailUsuarioo))
                            listBoxTesteAlunos.Items.RemoveAt(i);
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

        private void label11_Click(object sender, EventArgs e)
        {
            var resultado = MessageBox.Show("Tem a certeza que deseja apagar a sua conta?", "Confirmação de Apagar Conta", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

            if (resultado == DialogResult.OK)
            {
                ApagarConta();
            }
        }

        private void listBoxBiblioteca_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                int index = listBoxBiblioteca.IndexFromPoint(e.Location);
                if (index != ListBox.NoMatches)
                {
                    listBoxBiblioteca.SelectedIndex = index;
                    contextMenuStrip1.Show(Cursor.Position);
                }
            }
        }

        private void removerToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            if (listBoxBiblioteca.SelectedItem == null)
            {
                MessageBox.Show("Selecione um vídeo para remover.");
                return;
            }

            string nomeArquivo = listBoxBiblioteca.SelectedItem.ToString();
            string caminhoVideo = Path.Combine(pastaVideos, nomeArquivo);

            if (File.Exists(caminhoVideo))
            {
                File.Delete(caminhoVideo);
                listBoxBiblioteca.Items.Remove(nomeArquivo);
                pontosPorVideo.Remove(nomeArquivo);

                var linhas = File.ReadAllLines(arquivoDadosUsuario).ToList();
                linhas.RemoveAll(l => Path.GetFileName(LocTemp.FromString(l)?.UrlVideo) == nomeArquivo);
                File.WriteAllLines(arquivoDadosUsuario, linhas);

                MessageBox.Show("Vídeo e ponto removidos com sucesso!");
            }
        }

        private void editarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.Visible = true;
            button6.Visible = true;
            button7.Visible = true;
            label6.Visible = true;
            label7.Visible = true;
            listBoxBiblioteca.Visible = false;
            listBoxTesteAlunos.Visible = false;


            if (listBoxBiblioteca.SelectedItem == null)
            {
                MessageBox.Show("Selecione um vídeo para editar.");
                return;
            }

            string nomeArquivo = listBoxBiblioteca.SelectedItem.ToString();

            string caminhoVideo = Path.Combine(pastaVideos, nomeArquivo);
            axWindowsMediaPlayer1.URL = caminhoVideo;

            if (!pontosPorVideo.ContainsKey(nomeArquivo))
            {
                MessageBox.Show("Nenhum ponto salvo para este vídeo.");
                return;
            }

            modoEdicao = true;

            pontoAtual = pontosPorVideo[nomeArquivo];

            label6.Text = $"Tempo: {pontoAtual.GetTempo()}";
            label7.Text = $"Localização: X={pontoAtual.GetX()}, Y={pontoAtual.GetY()}";

            MessageBox.Show("Clique no vídeo para atualizar o ponto.");
        }

        private List<(string Nome, string Email)> alunos = new List<(string Nome, string Email)>();
        private void aplicarToolStripMenuItem_Click(object sender, EventArgs e)
        {

            alunos.Clear();

            var linhas = File.ReadAllLines("utilizadores.txt");

            foreach (var linha in linhas)
            {
                var partes = linha.Split(';');
                if (partes.Length > 8 && partes[8].Trim().Equals("Examinando", StringComparison.OrdinalIgnoreCase))
                {
                    alunos.Add((partes[0].Trim(), partes[4].Trim()));
                }
            }

            if (alunos.Count == 0)
            {
                MessageBox.Show("Nenhum aluno encontrado!");
                return;
            }

            Form selecao = new Form();
            selecao.Text = "Selecionar alunos para aplicar o teste";
            selecao.Width = 350;
            selecao.Height = 400;
            selecao.StartPosition = FormStartPosition.CenterScreen;

            CheckedListBox checkedList = new CheckedListBox();
            checkedList.Dock = DockStyle.Fill;
            foreach (var aluno in alunos)
                checkedList.Items.Add($"{aluno.Nome} ({aluno.Email})");

            Button btnOk = new Button();
            btnOk.Text = "Aplicar";
            btnOk.Dock = DockStyle.Bottom;

            selecao.Controls.Add(checkedList);
            selecao.Controls.Add(btnOk);

            bool confirmado = false;
            btnOk.Click += (s, ev) => { confirmado = true; selecao.Close(); };

            selecao.ShowDialog();

            if (confirmado && checkedList.CheckedItems.Count > 0)
            {
                string teste = listBoxBiblioteca.SelectedItem?.ToString();
                if (string.IsNullOrEmpty(teste))
                {
                    MessageBox.Show("Selecione um teste para aplicar.");
                    return;
                }

                foreach (var item in checkedList.CheckedItems)
                {
                    string texto = item.ToString();
                    int ini = texto.IndexOf('(');
                    int fim = texto.IndexOf(')');
                    if (ini >= 0 && fim > ini)
                    {
                        string email = texto.Substring(ini + 1, fim - ini - 1);
                        var alunoSelecionado = alunos.First(a => a.Email == email);
                        string arquivoTestes = $"testes_{alunoSelecionado.Email}.txt";
                        File.AppendAllText(arquivoTestes, teste + ";" + emailUsuarioo + Environment.NewLine);
                    }
                }

                MessageBox.Show("Teste aplicado apenas aos alunos selecionados!");
            }
            else if (confirmado)
            {
                MessageBox.Show("Nenhum aluno selecionado.");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            label6.Visible = false;
            label7.Visible = false;
            button6.Visible = false;
            button7.Visible = false;
            listBoxTesteAlunos.Visible = true;
        }

        private void CarregarHistoricoProfessor()
        {
            listBoxTesteAlunos.Items.Clear();

            var arquivosHistoricos = Directory.GetFiles(Directory.GetCurrentDirectory(), "historico_*.txt");
            foreach (var arquivo in arquivosHistoricos)
            {
                var linhas = File.ReadAllLines(arquivo);
                foreach (var linha in linhas)
                {
                    var partes = linha.Split(';');
                    if (partes.Length >= 10)
                    {
                        string nomeVideo = partes[0];
                        string nomeAluno = partes[8];
                        string emailDoProfessorQueAplicou = partes[9];

                        if (emailDoProfessorQueAplicou == emailUsuarioo)
                        {
                            string item = $"{nomeAluno} - {nomeVideo}";
                            if (!listBoxTesteAlunos.Items.Contains(item))
                                listBoxTesteAlunos.Items.Add(item);
                        }
                    }
                }
            }
        }

        private void listBoxTesteAlunos_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int index = listBoxTesteAlunos.IndexFromPoint(e.Location);
            if (index == ListBox.NoMatches)
                return;

            string selecionado = listBoxTesteAlunos.Items[index].ToString();

            string[] partes = selecionado.Split('|');
            if (partes.Length != 2)
            {
                MessageBox.Show("Formato do item inválido.");
                return;
            }
            string info = partes[0]; 
            string email = partes[1].Trim();

            int separadorIndex = info.IndexOf(" - ");
            if (separadorIndex == -1)
            {
                MessageBox.Show("Formato do item inválido.");
                return;
            }
            string nomeAluno = info.Substring(0, separadorIndex).Trim();
            string nomeVideo = info.Substring(separadorIndex + 3).Trim();

            var alunoInfo = alunos.FirstOrDefault(a => a.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
            if (string.IsNullOrEmpty(alunoInfo.Email))
            {
                MessageBox.Show("Não foi possível encontrar o e-mail do aluno selecionado.");
                return;
            }

            string arquivoHistorico = Path.Combine(Directory.GetCurrentDirectory(), $"historico_{alunoInfo.Email}.txt");
            if (!File.Exists(arquivoHistorico))
            {
                MessageBox.Show("Histórico não encontrado para esse aluno.");
                return;
            }

            var linhas = File.ReadAllLines(arquivoHistorico);
            foreach (var linha in linhas)
            {
                var partesLinha = linha.Split(';');
                if (partesLinha.Length >= 10)
                {
                    string video = partesLinha[0].Trim();
                    string tempoAlunoStr = partesLinha[1];
                    string xAlunoStr = partesLinha[2];
                    string yAlunoStr = partesLinha[3];
                    string tempoProfStr = partesLinha[4];
                    string xProfStr = partesLinha[5];
                    string yProfStr = partesLinha[6];
                    string notaStr = partesLinha[7];
                    string aluno = partesLinha[8].Trim();
                    string emailDoProfessorQueAplicou = partesLinha[9].Trim();

                    if (video.Equals(nomeVideo, StringComparison.OrdinalIgnoreCase) &&
                        aluno.Equals(nomeAluno, StringComparison.OrdinalIgnoreCase) &&
                        emailDoProfessorQueAplicou.Equals(emailUsuarioo.Trim(), StringComparison.OrdinalIgnoreCase))
                    {
                        string msg = $"Tempo do aluno: {tempoAlunoStr}s\n" +
                                     $"Localização do aluno: X={xAlunoStr}, Y={yAlunoStr}\n\n" +
                                     $"Tempo do professor: {tempoProfStr}s\n" +
                                     $"Localização do professor: X={xProfStr}, Y={yProfStr}\n\n" +
                                     $"Nota: {notaStr}";

                        MessageBox.Show(msg, $"Histórico de {nomeAluno}");
                        return;
                    }
                }
            }

            MessageBox.Show("Nenhum histórico encontrado para esse teste/aluno.");
        }

        private void AtualizarListaAlunosEVideos()
        {
            alunos.Clear();
            listBoxTesteAlunos.Items.Clear();

            if (!File.Exists("utilizadores.txt"))
                return;

            var linhas = File.ReadAllLines("utilizadores.txt");
            foreach (var linha in linhas)
            {
                var partes = linha.Split(';');
                if (partes.Length > 8 && partes[8].Trim().Equals("Examinando", StringComparison.OrdinalIgnoreCase))
                {
                    string nome = partes[0].Trim();
                    string email = partes[4].Trim();
                    alunos.Add((nome, email));
                }
            }

            foreach (var aluno in alunos)
            {
                foreach (var item in listBoxBiblioteca.Items)
                {
                    string nomeVideo = item.ToString();
                    listBoxTesteAlunos.Items.Add($"{aluno.Nome} - {nomeVideo} | {aluno.Email}");
                }
            }
        }
    }
}
