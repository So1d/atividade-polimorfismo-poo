using System;
using Incidentes.Dominio.Enums;
using Incidentes.Dominio.Modelos;
using Incidentes.Dominio.Servicos;

namespace Incidentes.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("=================================================================================");
            Console.WriteLine("          SISTEMA DE GESTÃO DE INCIDENTES DE SEGURANÇA DA INFORMAÇÃO             ");
            Console.WriteLine("                       Desenvolvido para Luis Felipe                             ");
            Console.WriteLine("=================================================================================\n");

            // 1. Demostração de Validações no Construtor
            Console.WriteLine(">>> 1. TESTANDO VALIDAÇÕES DOS CONSTRUTORES (PROVA DE CONSISTÊNCIA)");
            Console.WriteLine("---------------------------------------------------------------------------------");
            
            try
            {
                Console.WriteLine("Tentando registrar um incidente com Identificador vazio...");
                var incidenteInvalido = new TentativaPhishing(
                    identificador: "", // Inválido
                    dataRegistro: DateTime.Now,
                    responsavelAnalise: "Luis Felipe",
                    sistemaAfetado: "ERP Financeiro",
                    severidade: NivelSeveridade.Medio,
                    canalRecebimento: "E-mail",
                    linkFalso: "http://banco-atualizacao.net",
                    quantidadeCliques: 0
                );
            }
            catch (ArgumentException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[Erro Capturado com Sucesso]: {ex.Message}");
                Console.ResetColor();
            }

            try
            {
                Console.WriteLine("\nTentando registrar um incidente com Data de Registro no futuro...");
                var incidenteFuturo = new VazamentoDados(
                    identificador: "INC-099",
                    dataRegistro: DateTime.Now.AddDays(5), // Inválido
                    responsavelAnalise: "Luis Felipe",
                    sistemaAfetado: "Banco de Dados CRM",
                    severidade: NivelSeveridade.Critico,
                    registrosAfetados: 1500,
                    tipoDadosExpostos: "Dados Pessoais (LGPD)"
                );
            }
            catch (ArgumentException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[Erro Capturado com Sucesso]: {ex.Message}");
                Console.ResetColor();
            }
            
            Console.WriteLine("---------------------------------------------------------------------------------\n");

            // 2. Inicializando o serviço gerenciador e cadastrando incidentes de forma polimórfica
            Console.WriteLine(">>> 2. CADASTRANDO INCIDENTES NO GERENCIADOR...");
            var gerenciador = new GerenciadorIncidentes();

            try
            {
                // Incidente 1: Phishing Sem Cliques (Risco Médio)
                gerenciador.RegistrarIncidente(new TentativaPhishing(
                    identificador: "INC-001",
                    dataRegistro: DateTime.Now.AddHours(-12),
                    responsavelAnalise: "Rodrigo Santos",
                    sistemaAfetado: "Intranet Corporativa",
                    severidade: NivelSeveridade.Medio,
                    canalRecebimento: "WhatsApp Corporativo",
                    linkFalso: "http://intranet-atualizar-cadastro.xyz/login",
                    quantidadeCliques: 0
                ));

                // Incidente 2: Phishing com Vários Cliques (Risco Alto/Crítico)
                gerenciador.RegistrarIncidente(new TentativaPhishing(
                    identificador: "INC-002",
                    dataRegistro: DateTime.Now.AddHours(-8),
                    responsavelAnalise: "Ana Beatriz",
                    sistemaAfetado: "E-mail Corporativo",
                    severidade: NivelSeveridade.Alto,
                    canalRecebimento: "E-mail",
                    linkFalso: "http://promocao-milhas-gratis.com/ganhar",
                    quantidadeCliques: 15
                ));

                // Incidente 3: Vazamento de Dados Grande (Risco Crítico)
                gerenciador.RegistrarIncidente(new VazamentoDados(
                    identificador: "INC-003",
                    dataRegistro: DateTime.Now.AddHours(-6),
                    responsavelAnalise: "Gabriel Alencar",
                    sistemaAfetado: "Servidor PostgreSQL de Produção",
                    severidade: NivelSeveridade.Critico,
                    registrosAfetados: 8500,
                    tipoDadosExpostos: "Dados Pessoais e Financeiros (Cartões de Crédito)"
                ));

                // Incidente 4: Acesso Não Autorizado com Conta Admin (Risco Crítico)
                gerenciador.RegistrarIncidente(new AcessoNaoAutorizado(
                    identificador: "INC-004",
                    dataRegistro: DateTime.Now.AddHours(-3),
                    responsavelAnalise: "Luis Felipe",
                    sistemaAfetado: "Console Cloud AWS",
                    severidade: NivelSeveridade.Critico,
                    origemAcesso: "IP 185.220.101.4 (Rússia - Tor Exit Node)",
                    usuarioTentouAcesso: "admin_superuser"
                ));

                // Incidente 5: Malware Detectado - Ransomware (Risco Crítico)
                gerenciador.RegistrarIncidente(new MalwareDetectado(
                    identificador: "INC-005",
                    dataRegistro: DateTime.Now.AddHours(-1),
                    responsavelAnalise: "Carlos Eduardo",
                    sistemaAfetado: "Servidor de Arquivos (FileShare)",
                    severidade: NivelSeveridade.Alto,
                    nomeMalware: "WannaCry.Decryptor.v2",
                    categoriaMalware: "Ransomware",
                    caminhoArquivo: @"C:\Compartilhado\Financeiro\planilha_lucros.xlsx.locked"
                ));

                // Incidente 6: Malware Detectado - Adware (Risco Médio)
                gerenciador.RegistrarIncidente(new MalwareDetectado(
                    identificador: "INC-006",
                    dataRegistro: DateTime.Now.AddMinutes(-30),
                    responsavelAnalise: "Carlos Eduardo",
                    sistemaAfetado: "Estação de Trabalho EST-14",
                    severidade: NivelSeveridade.Baixo,
                    nomeMalware: "SuperSearchHelper.dll",
                    categoriaMalware: "Adware/PUP",
                    caminhoArquivo: @"C:\Users\colaborador\AppData\Local\Temp\searchhelper.dll"
                ));

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Todos os incidentes foram cadastrados com sucesso no Gerenciador!");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Erro inesperado ao cadastrar incidentes: {ex.Message}");
                Console.ResetColor();
                return;
            }

            Console.WriteLine("---------------------------------------------------------------------------------\n");

            // 3. Geração do Relatório Polimórfico (Sem ifs ou switches com base no tipo concreto do objeto)
            Console.WriteLine(">>> 3. RELATÓRIO DE RESPOSTA A INCIDENTES (POLIMORFISMO EM AÇÃO)");
            Console.WriteLine("=================================================================================");

            var incidentesCadastrados = gerenciador.ObterTodos();
            int contador = 1;

            foreach (IncidenteSeguranca incidente in incidentesCadastrados)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"--- INCIDENTE #{contador:D2} ---");
                Console.ResetColor();

                // 3.1. Exibindo Descrição Base (Método concreto herdado)
                Console.WriteLine($"Descrição Base      : {incidente.GerarDescricaoBase()}");

                // 3.2. Exibindo Risco Calculado (Método polimórfico sobrescrito)
                string risco = incidente.CalcularRisco();
                Console.Write("Risco Calculado     : ");
                ImprimirComDestaqueDeRisco(risco);

                // 3.3. Exibindo Plano de Resposta (Método polimórfico sobrescrito)
                Console.WriteLine("Plano de Resposta   :");
                Console.WriteLine(IdentarTexto(incidente.GerarPlanoResposta(), "  "));

                // 3.4. Exibindo Medidas Preventivas (Método polimórfico sobrescrito)
                Console.WriteLine("Medidas Preventivas :");
                Console.WriteLine(IdentarTexto(incidente.IndicarMedidasPreventivas(), "  "));

                Console.WriteLine("---------------------------------------------------------------------------------");
                contador++;
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\nFim do relatório. Total de incidentes analisados de forma polimórfica: {incidentesCadastrados.Count}");
            Console.ResetColor();
            Console.WriteLine("=================================================================================");
        }

        /// <summary>
        /// Colore a saída do risco no console para facilitar a visualização de acordo com o nível detectado.
        /// </summary>
        private static void ImprimirComDestaqueDeRisco(string risco)
        {
            if (risco.Contains("CRÍTICO"))
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
            }
            else if (risco.Contains("ALTO"))
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Blue;
            }
            Console.WriteLine(risco);
            Console.ResetColor();
        }

        /// <summary>
        /// Função auxiliar para formatar blocos de texto no relatório do console.
        /// </summary>
        private static string IdentarTexto(string texto, string identacao)
        {
            if (string.IsNullOrEmpty(texto)) return texto;
            var linhas = texto.Split('\n');
            for (int i = 0; i < linhas.Length; i++)
            {
                linhas[i] = identacao + linhas[i];
            }
            return string.Join('\n', linhas);
        }
    }
}
