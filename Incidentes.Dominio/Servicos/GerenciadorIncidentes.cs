using System;
using System.Collections.Generic;
using Incidentes.Dominio.Modelos;

namespace Incidentes.Dominio.Servicos
{
    /// <summary>
    /// Classe de serviço responsável por gerenciar e armazenar os incidentes de segurança
    /// em uma coleção polimórfica.
    /// </summary>
    public class GerenciadorIncidentes
    {
        private readonly List<IncidenteSeguranca> _incidentes = new List<IncidenteSeguranca>();

        /// <summary>
        /// Adiciona um incidente à coleção polimórfica.
        /// </summary>
        public void RegistrarIncidente(IncidenteSeguranca incidente)
        {
            if (incidente == null)
                throw new ArgumentNullException(nameof(incidente), "O incidente não pode ser nulo.");

            _incidentes.Add(incidente);
        }

        /// <summary>
        /// Retorna a lista contendo todos os incidentes armazenados sob a abstração de IncidenteSeguranca.
        /// </summary>
        public IReadOnlyList<IncidenteSeguranca> ObterTodos()
        {
            return _incidentes.AsReadOnly();
        }
    }
}
