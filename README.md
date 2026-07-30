# ExaminaVídeo

Aplicação desktop em C# / Windows Forms para criação e realização de testes de precisão baseados em vídeo, desenvolvida como projeto final da unidade curricular de **Programação Orientada a Objetos** (2.º semestre, 1.º ano).

O objetivo proposto foi criar uma aplicação a ser utilizada por professores de cursos de **formação de nadadores-salvadores**, para treinar e avaliar a capacidade dos alunos identificarem, em vídeo, o momento e o local exatos em que uma pessoa está a afogar-se — uma competência fundamental de deteção rápida em contexto real.

> O professor (Examinador) marca, num vídeo de treino, o ponto exato e o instante em que ocorre o afogamento. O aluno (Examinando) assiste ao mesmo vídeo e tenta clicar no local certo, no momento certo, tal como faria em vigilância real. O sistema compara as duas marcações e calcula automaticamente a nota, considerando a distância ao ponto correto e o desvio de tempo de deteção.

## Índice

- [Sobre o projeto](#sobre-o-projeto)
- [Funcionalidades](#funcionalidades)
- [Tecnologias utilizadas](#tecnologias-utilizadas)
- [Estrutura do projeto](#estrutura-do-projeto)
- [Como executar](#como-executar)
- [Capturas de ecrã](#capturas-de-ecrã)
- [Notas académicas](#notas-académicas)
- [Autor](#autor)

## Sobre o projeto

O **ExaminaVídeo** simula um pequeno sistema de avaliação com dois tipos de utilizador:

- **Examinador (professor)**: gere uma biblioteca de vídeos de treino, define para cada vídeo um "ponto de referência" (posição X/Y no ecrã + instante temporal) correspondente ao momento e local do afogamento, cria testes a partir desses vídeos e aplica-os a alunos específicos. Pode também consultar o histórico de resultados de cada aluno.
- **Examinando (aluno, futuro nadador-salvador)**: acede aos testes que lhe foram atribuídos, assiste ao vídeo e marca o local e o momento em que identifica a pessoa a afogar-se, enviando depois a resposta. A aplicação calcula a nota com base na distância euclidiana entre o clique do aluno e o ponto marcado pelo professor, aplicando também uma penalização relacionada com o tempo de resposta — simulando a importância da rapidez de deteção numa situação real. O aluno pode consultar o seu histórico de testes realizados.

O projeto foi pensado como exercício de aplicação prática dos conceitos de POO (classes, encapsulamento, herança de `Form`, composição de objetos) e não como um produto final, servindo essencialmente fins académicos.

## Funcionalidades

- Registo e autenticação de utilizadores (aluno ou professor), com palavras-passe encriptadas (AES).
- Gestão de uma biblioteca de vídeos por parte do professor.
- Marcação de um ponto de referência (posição + tempo) num vídeo.
- Criação e atribuição de testes a alunos específicos.
- Reprodução do vídeo dentro da aplicação (Windows Media Player).
- Submissão de respostas pelo aluno, com cálculo automático da nota.
- Histórico de testes realizados, consultável por alunos e por professores.
- Edição e eliminação de conta.
- Interface personalizada (janela sem moldura padrão, com botões próprios de minimizar/maximizar/fechar).

## Tecnologias utilizadas

- **C#**
- **.NET 8 (Windows Forms)**
- **Visual Studio 2022**
- **Windows Media Player (COM / AxWMPLib)** para reprodução de vídeo
- **System.Security.Cryptography (AES)** para encriptação de palavras-passe
- Persistência de dados em ficheiros de texto (sem base de dados)

## Estrutura do projeto

```
proj/
├── proj.sln                  # Solução do Visual Studio
└── proj/
    ├── Program.cs             # Ponto de entrada da aplicação
    ├── Form1.cs                # Ecrã de registo
    ├── Login.cs                 # Ecrã de autenticação
    ├── Examinador.cs         # Ecrã e lógica do professor
    ├── Examinando.cs        # Ecrã e lógica do aluno
    ├── DadosLog.cs             # Modelo dos dados de utilizador
    ├── LocTemp.cs               # Modelo do ponto de referência (URL, tempo, X, Y)
    └── Encriptar.cs             # Encriptação/desencriptação AES das palavras-passe
```

## Como executar

**Pré-requisitos:**

- Windows (a aplicação usa Windows Forms e o controlo COM do Windows Media Player)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) com a carga de trabalho **.NET desktop development**
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

**Passos:**

1. Clonar o repositório:
   ```bash
   git clone https://github.com/bresler01/examina-video.git
   ```
2. Abrir o ficheiro `proj/proj.sln` no Visual Studio 2022.
3. Compilar e executar (F5).

Na primeira execução, a aplicação cria automaticamente um ficheiro `utilizadores.txt` para guardar as contas registadas.

## Capturas de ecrã

Abaixo, uma breve demonstração da aplicação em funcionamento:

https://github.com/bresler01/examina-video/blob/main/assets/demonstracao.mp4

> Nota: o vídeo também está disponível na pasta [`assets/demonstracao.mp4`](./assets/demonstracao.mp4) deste repositório.

## Notas académicas

Este projeto foi desenvolvido em contexto académico, com foco na aplicação dos conceitos de Programação Orientada a Objetos lecionados na unidade curricular. Como tal, algumas decisões (como a persistência em ficheiros de texto simples em vez de uma base de dados) foram tomadas para manter o âmbito adequado ao trabalho pedido, e não representam necessariamente as melhores práticas para uma aplicação em produção.

Algumas partes do código foram desenvolvidas com o apoio do ChatGPT, utilizado como ferramenta de suporte ao longo do desenvolvimento.

## Autor

Desenvolvido por **Gabrielly Bresler**, no âmbito da unidade curricular de Programação Orientada a Objetos.

Data: Junho de 2025 · Instituição: IPMAIA

---

Todos os direitos reservados.
