using System;
using Incidentes.Dominio.Enums;

namespace Incidentes.Dominio.Modelos
{
    /// <summary>
    /// Representa um incidente de Vazamento de Dados.
    /// </summary>
    public class VazamentoDados : IncidenteSeguranca
    {
        public int RegistrosAfetados { get; }
        public string TipoDadosExpostos { get; }

        public VazamentoDados(
            string identificador,
            DateTime dataRegistro,
            string responsavelAnalise,
            string sistemaAfetado,
            NivelSeveridade severidade,
            int registrosAfetados,
            string tipoDadosExpostos)
            : base(identificador, dataRegistro, responsavelAnalise, sistemaAfetado, severidade)
        {
            if (registrosAfetados < 0)
                throw new ArgumentException("A quantidade de registros afetados não pode ser negativa.", nameof(registrosAfetados));

            if (string.IsNullOrWhiteSpace(tipoDadosExpostos))
                throw new ArgumentException("O tipo de dados expostos não pode ser nulo ou vazio.", nameof(tipoDadosExpostos));

            RegistrosAfetados = registrosAfetados;
            TipoDadosExpostos = tipoDadosExpostos;
        }

        public override string CalcularRisco()
        {
            if (RegistrosAfetados > 5000 || Severidade == NivelSeveridade.Critico)
            {
                return $"CRÍTICO - {RegistrosAfetados} registros expostos de dados do tipo '{TipoDadosExpostos}'. Risco severo de multas da LGPD/ANPD.";
            }
            if (RegistrosAfetados > 100 || TipoDadosExpostos.Contains("Financeiro", StringComparison.OrdinalIgnoreCase) || TipoDadosExpostos.Contains("Senha", StringComparison.OrdinalIgnoreCase))
            {
                return $"ALTO - Vazamento de dados sensíveis '{TipoDadosExpostos}' para {RegistrosAfetados} usuários.";
            }
            return $"MÉDIO - Vazamento de dados limitado ({RegistrosAfetados} registro(s) do tipo '{TipoDadosExpostos}').";
        }

        public override string GerarPlanoResposta()
        {
            return $"1. Bloquear o vazamento revogando credenciais e isolando a origem da fuga de dados no sistema '{SistemaAfetado}'.\n" +
                   $"2. Notificar imediatamente o Encarregado pelo Tratamento de Dados Pessoais (DPO) e a equipe jurídica.\n" +
                   $"3. Iniciar a elaboração do Relatório de Impacto à Proteção de Dados (RIPD) e avaliar a necessidade de notificar a ANPD e os titulares em até 72 horas.";
        }

        public override string IndicarMedidasPreventivas()
        {
            return "1. Adotar mecanismos de DLP (Data Loss Prevention) nas saídas de rede corporativas.\n" +
                   "2. Criptografar dados sensíveis do tipo '" + TipoDadosExpostos + "' em trânsito e em repouso.";
        }
    }
}
