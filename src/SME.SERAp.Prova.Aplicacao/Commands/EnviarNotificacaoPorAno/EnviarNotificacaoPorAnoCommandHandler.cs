using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using MediatR;
using SME.SERAp.Prova.Infra.EnvironmentVariables;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao.Commands.EnviarNotificacaoPorAno
{
    public class EnviarNotificacaoPorAnoCommandHandler : IRequestHandler<EnviarNotificacaoPorAnoCommand, bool>
    {
        private readonly FireBaseOptions fireBaseOptions;

        public EnviarNotificacaoPorAnoCommandHandler(FireBaseOptions fireBaseOptions)
        {
            this.fireBaseOptions = fireBaseOptions ?? throw new ArgumentNullException(nameof(fireBaseOptions));
        }
        public async Task<bool> Handle(EnviarNotificacaoPorAnoCommand request, CancellationToken cancellationToken)
        {
            var firebaseToken = fireBaseOptions.Token;
            var firebaseCredential = GoogleCredential.FromJson(firebaseToken);
            FirebaseApp app = FirebaseApp.DefaultInstance;

            if (app == null)
            {
                app = FirebaseApp.Create(new AppOptions()
                {
                    Credential = firebaseCredential,
                    ProjectId = fireBaseOptions.ProjectId
                });
            }

            var mensagemFirebase = new Message();
            mensagemFirebase.Data = MontarDataNotificacao(request.Mensagem);

            mensagemFirebase.Topic = request.Ano.ToString();


            var resultado = await FirebaseMessaging.DefaultInstance.SendAsync(mensagemFirebase);
            return resultado != null;

        }
        private static Dictionary<string, string> MontarDataNotificacao(string mensagem)
        {
            var mensagemFormatada = Regex.Replace(mensagem, @"<(.|\n)*?>", string.Empty);
            return new Dictionary<String, String>
            {
                ["Titulo"] = "Nova prova disponível para download.",
                ["Mensagem"] = mensagemFormatada.Length > 256 ? mensagemFormatada.Substring(0, 256) : mensagemFormatada,
                ["CriadoEm"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffffff"),
                ["click_action"] = "FLUTTER_NOTIFICATION_CLICK",
            };
        }
    }
}
