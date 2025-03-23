using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Domain.Filters
{
    public class ApiExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<ApiExceptionFilter> _logger;

        public ApiExceptionFilter(ILogger<ApiExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            _logger.LogError(context.Exception.Message, "Ocorreu uma exceção");

            context.Result = new ObjectResult("Ocorreu um problema ao tratar sua solicitação: Dentro da controller ou da Action ")
            {
                StatusCode = StatusCodes.Status500InternalServerError,
            };
        }
    }
}



////  Ordem de Execução
//Middleware:
//Middleware são executados antes e depois do pipeline da requisição. Eles interceptam todas as requisições e podem capturar exceções, processar dados e realizar outras operações.

//Filtros:
//Filtros são executados dentro do pipeline do MVC, ou seja, depois que a requisição passa pelos middlewares e chega no controller. Os Filtros de Exceção (IExceptionFilter) atuam apenas quando uma exceção é lançada dentro dos controllers ou actions.

// Fluxo Completo
//Middleware de Tratamento Global:

//Se ocorrer uma exceção em qualquer ponto do pipeline (antes do controller ser chamado), o middleware pode capturá-la.
//Controller e Actions:

//A requisição chega ao controller e executa a lógica da action.
//Filtro de Exceção:

//Se ocorrer uma exceção dentro da action ou do controller, o filtro de exceção será acionado antes de voltar para o middleware.

