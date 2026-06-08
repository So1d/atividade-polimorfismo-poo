using System;
using Incidentes.Dominio.Enums;

namespace Incidentes.Dominio.Modelos
{
    /// <summary>
    /// Representa um incidente de Tentativa de Phishing.
    /// </summary>
    public class TentativaPhishing : IncidenteSeguranca
    {
        public string CanalRecebimento { get; }
        public string LinkFalso { get; }
        public int QuantidadeCliques { get; }

        public TentativaPhishing(
            string identificador,
            DateTime dataRegistro,
            string responsavelAnalise,
            string sistemaAfetado,
            NivelSeveridade severidade,
            string canalRecebimento,
            string linkFalso,
            int quantidadeCliques)
            : base(identificador, dataRegistro, responsavelAnalise, sistemaAfetado, severidade)
        {
            if (string.IsNullOrWhiteSpace(canalRecebimento))
                throw new ArgumentException("O canal de recebimento do phishing não pode ser nulo ou vazio.", nameof(canalRecebimento));

            if (string.IsNullOrWhiteSpace(linkFalso))
                throw new ArgumentException("O link falso de phishing não pode ser nulo ou vazio.", nameof(linkFalso));

            if (quantidadeCliques < 0)
                throw new ArgumentException("A quantidade de cliques não pode ser negativa.", nameof(quantidadeCliques));

            CanalRecebimento = canalRecebimento;
            LinkFalso = linkFalso;
            QuantidadeCliques = quantidadeCliques;
        }

        public override string CalcularRisco()
        {
            if (QuantidadeCliques > 10 || Severidade == NivelSeveridade.Critico)
            {
                return $"CRÍTICO - Alto impacto devido a {QuantidadeCliques} cliques registrados no canal {CanalRecebimento}.";
            }
            if (QuantidadeCliques > 0)
            {
                return $"ALTO - Risco de credenciais vazadas ({QuantidadeCliques} clique(s) detectado(s)).";
            }
            return $"MÉDIO - Tentativa contida. Canal: {CanalRecebimento}. Nenhum clique confirmado.";
        }

        public override string GerarPlanoResposta()
        {
            var plano = $"1. Bloquear o domínio '{LinkFalso}' no firewall corporativo e proxy web.\n" +
                        $"2. Adicionar o remetente do canal '{CanalRecebimento}' à lista de bloqueio do gateway de e-mail/mensageria.";
            
            if (QuantidadeCliques > 0)
            {
                plano += "\n3. ATENÇÃO: Identificar os usuários que clicaram, forçar redefinição imediata de senhas e revogar tokens de sessão ativos.";
            }
            return plano;
        }

        public override string IndicarMedidasPreventivas()
        {
            return "1. Reforçar os filtros de SPAM e proteção de e-mail (SPF/DKIM/DMARC).\n" +
                   "2. Agendar simulação interna de Phishing focado no canal " + CanalRecebimento + " para os colaboradores da área do sistema " + SistemaAfetado + ".";
        }
    }
}
