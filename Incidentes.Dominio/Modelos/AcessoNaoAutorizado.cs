using System;
using Incidentes.Dominio.Enums;

namespace Incidentes.Dominio.Modelos
{
    /// <summary>
    /// Representa um incidente de Acesso Não Autorizado.
    /// </summary>
    public class AcessoNaoAutorizado : IncidenteSeguranca
    {
        public string OrigemAcesso { get; }
        public string UsuarioTentouAcesso { get; }

        public AcessoNaoAutorizado(
            string identificador,
            DateTime dataRegistro,
            string responsavelAnalise,
            string sistemaAfetado,
            NivelSeveridade severidade,
            string origemAcesso,
            string usuarioTentouAcesso)
            : base(identificador, dataRegistro, responsavelAnalise, sistemaAfetado, severidade)
        {
            if (string.IsNullOrWhiteSpace(origemAcesso))
                throw new ArgumentException("A origem do acesso não pode ser nula ou vazia.", nameof(origemAcesso));

            if (string.IsNullOrWhiteSpace(usuarioTentouAcesso))
                throw new ArgumentException("O usuário alvo do acesso não pode ser nulo ou vazio.", nameof(usuarioTentouAcesso));

            OrigemAcesso = origemAcesso;
            UsuarioTentouAcesso = usuarioTentouAcesso;
        }

        public override string CalcularRisco()
        {
            var isPrivileged = UsuarioTentouAcesso.Contains("admin", StringComparison.OrdinalIgnoreCase) || 
                               UsuarioTentouAcesso.Contains("root", StringComparison.OrdinalIgnoreCase) || 
                               UsuarioTentouAcesso.Contains("diretoria", StringComparison.OrdinalIgnoreCase);

            if (isPrivileged || Severidade == NivelSeveridade.Critico)
            {
                return $"CRÍTICO - Login não autorizado na conta administrativa '{UsuarioTentouAcesso}' a partir de '{OrigemAcesso}'.";
            }
            if (Severidade == NivelSeveridade.Alto)
            {
                return $"ALTO - Acesso não autorizado na conta '{UsuarioTentouAcesso}' a partir da origem '{OrigemAcesso}'.";
            }
            return $"MÉDIO - Tentativa de acesso anômalo a partir de '{OrigemAcesso}' na conta '{UsuarioTentouAcesso}'.";
        }

        public override string GerarPlanoResposta()
        {
            return $"1. Bloquear imediatamente o endereço/IP de origem '{OrigemAcesso}' no firewall corporativo.\n" +
                   $"2. Desativar temporariamente o usuário '{UsuarioTentouAcesso}' no Active Directory/provedor de identidade.\n" +
                   $"3. Auditar a trilha de logs no sistema '{SistemaAfetado}' para rastrear todas as ações executadas pelo invasor.";
        }

        public override string IndicarMedidasPreventivas()
        {
            return $"1. Ativar obrigatoriamente Autenticação Multifator (MFA) para o usuário '{UsuarioTentouAcesso}'.\n" +
                   $"2. Restringir logins administrativos a faixas IP internas da VPN e habilitar geobloqueio para a origem '{OrigemAcesso}'.";
        }
    }
}
