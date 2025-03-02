using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Configuration;

/// <summary>
/// Questo bot gestisce le richieste inviate a Teams.
/// </summary>
public class TeamsBot : ActivityHandler
{
    private readonly OpenAIHelper _aiHelper;
    private readonly SQLHelper _sqlHelper;

    public TeamsBot(IConfiguration configuration)
    {
        _aiHelper = new OpenAIHelper();
        _sqlHelper = new SQLHelper();
    }

    protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
    {
        string userQuery = turnContext.Activity.Text;

        // Usa OpenAI per generare la query SQL
        string sqlQuery = await _aiHelper.CallOpenAIToGenerateSQL(userQuery);

        // Esegue la query su SQL Server
        string result = await _sqlHelper.ExecuteSQLQuery(sqlQuery);

        // Invia la risposta a Teams
        await turnContext.SendActivityAsync(MessageFactory.Text($"Risultato SQL: {result}"), cancellationToken);
    }
}
