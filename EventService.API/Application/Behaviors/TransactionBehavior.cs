﻿using EventService.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class TransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse> {
    private readonly ILogger<TransactionBehavior<TRequest, TResponse>> _logger;
    private readonly EventDbContext _dbContext;

    public TransactionBehavior(EventDbContext dbContext,
        ILogger<TransactionBehavior<TRequest, TResponse>> logger) {
        _dbContext = dbContext ?? throw new ArgumentException(nameof(EventDbContext));
        _logger = logger ?? throw new ArgumentException(nameof(ILogger));
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken) {
        var response = default(TResponse)!;
        var typeName = request.GetType().Name;

        try {
            if (_dbContext.HasActiveTransaction) {
                return await next();
            }

            var strategy = _dbContext.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () => {
                Guid transactionId;

                await using var transaction = await _dbContext.BeginTransactionAsync();
                if (transaction == null) throw new Exception();

                using (_logger.BeginScope(new List<KeyValuePair<string, object>> { new("TransactionContext", transaction.TransactionId) })) {
                    _logger.LogInformation("Begin transaction {TransactionId} for {CommandName} ({@Command})", transaction.TransactionId, typeName, request);

                    response = await next();

                    _logger.LogInformation("Commit transaction {TransactionId} for {CommandName}", transaction.TransactionId, typeName);

                    await _dbContext.CommitTransactionAsync(transaction);

                    transactionId = transaction.TransactionId;
                }
            });

            return response;
        } catch (Exception ex) {
            _logger.LogError(ex, "Error Handling transaction for {CommandName} ({@Command})", typeName, request);
            throw;
        }
    }
}