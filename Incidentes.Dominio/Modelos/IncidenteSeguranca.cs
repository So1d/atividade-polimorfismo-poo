using System;
using Incidentes.Dominio.Enums;

namespace Incidentes.Dominio.Modelos
{
    /// <summary>
    /// Classe base abstrata que representa um Incidente de Segurança da Informação.
    /// Define as propriedades comuns a todos os incidentes e estabelece o contrato polimórfico.
    /// </summary>
    public abstract class IncidenteSeguranca
    {
        public string Identificador { get; }
        public DateTime DataRegistro { get; }
        public string ResponsavelAnalise { get; }
        public string SistemaAfetado { get; }
        public NivelSeveridade Severidade { get; }

        /// <summary>
        /// Construtor da classe base que realiza as validações obrigatórias para garantir consistência.
        /// </summary>
        protected IncidenteSeguranca(string identificador, DateTime dataRegistro, string responsavelAnalise, string sistemaAfetado, NivelSeveridade severidade)
        {
            if (string.IsNullOrWhiteSpace(identificador))
                throw new ArgumentException("O identificador do incidente não pode ser nulo ou vazio.", nameof(identificador));

            if (dataRegistro == default)
                throw new ArgumentException("A data de registro fornecida é inválida.", nameof(dataRegistro));

            if (dataRegistro > DateTime.Now.AddMinutes(5)) // Tolerância pequena para fusos horários/sincronia
                throw new ArgumentException("A data de registro não pode estar no futuro.", nameof(dataRegistro));

            if (string.IsNullOrWhiteSpace(responsavelAnalise))
                throw new ArgumentException("O responsável pela análise não pode ser nulo ou vazio.", nameof(responsavelAnalise));

            if (string.IsNullOrWhiteSpace(sistemaAfetado))
                throw new ArgumentException("O sistema afetado não pode ser nulo ou vazio.", nameof(sistemaAfetado));

            Identificador = identificador;
            DataRegistro = dataRegistro;
            ResponsavelAnalise = responsavelAnalise;
            SistemaAfetado = sistemaAfetado;
            Severidade = severidade;
        }

        /// <summary>
        /// Método concreto reutilizado pelas classes derivadas para obter a descrição base do incidente.
        /// </summary>
        public string GerarDescricaoBase()
        {
            return $"[ID: {Identificador}] | Registrado em: {DataRegistro:dd/MM/yyyy HH:mm} | Sistema: {SistemaAfetado} | Responsável: {ResponsavelAnalise} | Severidade Inicial: {Severidade}";
        }

        /// <summary>
        /// Método abstrato para calcular o risco do incidente com base em suas características específicas.
        /// </summary>
        public abstract string CalcularRisco();

        /// <summary>
        /// Método abstrato para gerar o plano de resposta/ação adequado ao tipo de incidente.
        /// </summary>
        public abstract string GerarPlanoResposta();

        /// <summary>
        /// Método abstrato para sugerir as medidas preventivas adequadas que evitem a reincidência do incidente.
        /// </summary>
        public abstract string IndicarMedidasPreventivas();
    }
}
