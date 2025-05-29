using System.Data.Common;

using Microsoft.Data.SqlClient;

namespace GoalManager.Infrastructure.Data;

public interface ISqlConnectionFactory
{
    DbConnection CreateConnection();

    int GetCommandTimeout();
}

public sealed class SqlConnectionFactory(string connectionString, int commandTimeOut) : ISqlConnectionFactory
{
    private readonly string _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
    private readonly int _commandTimeOut = commandTimeOut <= 0 ? throw new ArgumentOutOfRangeException(nameof(commandTimeOut), commandTimeOut, "commandTimeout should be equal to or bigger than 0") : commandTimeOut;

    public DbConnection CreateConnection()
    {
        return new SqlConnection(_connectionString);
    }

    public int GetCommandTimeout()
    {
        return _commandTimeOut;
    }
}
