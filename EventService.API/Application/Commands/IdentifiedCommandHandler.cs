using EventService.Infrastructure.Idempotency;
using MediatR;

namespace EventService.API.Application.Commands {
    public abstract class IdentifiedCommandHandler<T, R> : IRequestHandler<IdentifiedCommand<T, R>, R>
    where T : IRequest<R> {
        private readonly IMediator _mediator;
        private readonly IRequestManager _requestManager;
        private readonly ILogger<IdentifiedCommandHandler<T, R>> _logger;

        public IdentifiedCommandHandler(
            IMediator mediator,
            IRequestManager requestManager,
            ILogger<IdentifiedCommandHandler<T, R>> logger) {
            ArgumentNullException.ThrowIfNull(logger);
            _mediator = mediator;
            _requestManager = requestManager;
            _logger = logger;
        }

        protected abstract R CreateResultForDuplicateRequest();

        public async Task<R> Handle(IdentifiedCommand<T, R> message, CancellationToken cancellationToken) {
            var alreadyExists = await _requestManager.ExistAsync(message.Id);
            if (alreadyExists) {
                return CreateResultForDuplicateRequest();
            } else {
                await _requestManager.CreateRequestForCommandAsync<T>(message.Id);
                try {
                    var command = message.Command;
                    var commandId = message.Id;

                    _logger.LogInformation(
                        "Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})",
                        command.GetType().Name,
                        nameof(commandId),
                        commandId,
                        command);

                    // Send the embedded business command to mediator so it runs its related CommandHandler 
                    var result = await _mediator.Send(command, cancellationToken);

                    _logger.LogInformation(
                        "Command result: {@Result} - {CommandName} - {IdProperty}: {CommandId} ({@Command})",
                        result,
                        command.GetType().Name,
                        nameof(commandId),
                        commandId,
                        command);

                    return result;
                } catch {
                    return default!;
                }
            }
        }
    }
}
