namespace GoalManager.Infrastructure.Data;

using System;
using System.Data;
using System.Threading.Tasks;

using Dapper;

public static class SqlConnectionFactoryDapperExtensions
{
    public static Task<IEnumerable<TItem>> QuerySpAsync<TItem>(
        this ISqlConnectionFactory sqlConnectionFactory,
        string storedProcedureName,
        object? param = null,
        IDbTransaction? transaction = null,
        int? commandTimeout = null)
    {
        return sqlConnectionFactory.QueryAsync<TItem>(storedProcedureName, param, transaction, commandTimeout, CommandType.StoredProcedure);
    }

    public static Task<IEnumerable<TItem>> QueryCommandAsync<TItem>(
        this ISqlConnectionFactory sqlConnectionFactory,
        string commandText,
        object? param = null,
        IDbTransaction? transaction = null,
        int? commandTimeout = null)
    {
        return sqlConnectionFactory.QueryAsync<TItem>(commandText, param, transaction, commandTimeout, CommandType.Text);
    }

    public static Task<TItem?> QueryFirstOrDefaultSpAsync<TItem>(
        this ISqlConnectionFactory sqlConnectionFactory,
        string storedProcedureName,
        object? param = null,
        IDbTransaction? transaction = null,
        int? commandTimeout = null)
    {
        return sqlConnectionFactory.QueryFirstOrDefaultAsync<TItem>(storedProcedureName, param, transaction, commandTimeout, CommandType.StoredProcedure);
    }

    public static Task<TItem?> QueryFirstOrDefaultCommandAsync<TItem>(
        this ISqlConnectionFactory sqlConnectionFactory,
        string commandText,
        object? param = null,
        IDbTransaction? transaction = null,
        int? commandTimeout = null)
    {
        return sqlConnectionFactory.QueryFirstOrDefaultAsync<TItem>(commandText, param, transaction, commandTimeout, CommandType.Text);
    }

    public static Task<TResult> QueryCommandMultipleAsync<TResult>(
        this ISqlConnectionFactory sqlConnectionFactory,
        string commandText,
        object? param,
        Func<SqlMapper.GridReader, Task<TResult>> readerFunc)
    {
        return sqlConnectionFactory.QueryCommandMultipleAsync(commandText, param, null, sqlConnectionFactory.GetCommandTimeout(), CommandType.Text, readerFunc);
    }

    public static async Task<TResult> QueryCommandMultipleAsync<TResult>(
        this ISqlConnectionFactory sqlConnectionFactory,
        string commandText,
        object? param,
        IDbTransaction? transaction,
        int? commandTimeout,
        CommandType? commandType,
        Func<SqlMapper.GridReader, Task<TResult>> readerFunc)
    {
        await using var connection = sqlConnectionFactory.CreateConnection();
        await connection.OpenAsync().ConfigureAwait(false);
        await using var multi = await connection.QueryMultipleAsync(commandText, param, transaction, commandTimeout.GetValueOrDefault(sqlConnectionFactory.GetCommandTimeout()), commandType).ConfigureAwait(false);

        return await readerFunc(multi).ConfigureAwait(false);
    }

    private static Task<IEnumerable<TItem>> QueryAsync<TItem>(
        this ISqlConnectionFactory sqlConnectionFactory,
        string commandText,
        object? param = null,
        IDbTransaction? transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null)
    {
        ArgumentNullException.ThrowIfNull(sqlConnectionFactory);
        ArgumentException.ThrowIfNullOrWhiteSpace(commandText);

        return QueryInternalAsync<TItem>(sqlConnectionFactory, commandText, param, transaction, commandTimeout, commandType);
    }

    private static Task<TItem?> QueryFirstOrDefaultAsync<TItem>(
        this ISqlConnectionFactory sqlConnectionFactory,
        string commandText,
        object? param = null,
        IDbTransaction? transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null)
    {
        ArgumentNullException.ThrowIfNull(sqlConnectionFactory);
        ArgumentException.ThrowIfNullOrWhiteSpace(commandText);

        return QueryFirstOrDefaultInternalAsync<TItem>(sqlConnectionFactory, commandText, param, transaction, commandTimeout, commandType);
    }

    private static async Task<IEnumerable<TItem>> QueryInternalAsync<TItem>(
        ISqlConnectionFactory sqlConnectionFactory,
        string commandText,
        object? param,
        IDbTransaction? transaction,
        int? commandTimeout,
        CommandType? commandType)
    {
        await using var connection = sqlConnectionFactory.CreateConnection();
        await connection.OpenAsync().ConfigureAwait(false);
        return await connection.QueryAsync<TItem>(commandText, param, transaction, commandTimeout.GetValueOrDefault(sqlConnectionFactory.GetCommandTimeout()), commandType).ConfigureAwait(false);
    }

    private static async Task<TItem?> QueryFirstOrDefaultInternalAsync<TItem>(
        ISqlConnectionFactory sqlConnectionFactory,
        string commandText,
        object? param,
        IDbTransaction? transaction,
        int? commandTimeout,
        CommandType? commandType)
    {
        await using var connection = sqlConnectionFactory.CreateConnection();
        await connection.OpenAsync().ConfigureAwait(false);
        return await connection.QueryFirstOrDefaultAsync<TItem>(commandText, param, transaction, commandTimeout.GetValueOrDefault(sqlConnectionFactory.GetCommandTimeout()), commandType).ConfigureAwait(false);
    }
}
