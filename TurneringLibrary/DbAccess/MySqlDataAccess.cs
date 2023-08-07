using Dapper;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System.Data;

namespace TurneringLibrary.DbAccess;
public class MySqlDataAccess : IDataAccess
{

  private readonly IConfiguration _config;

  public MySqlDataAccess(IConfiguration config)
  {
    _config = config;
  }

  public async Task<IEnumerable<T>> LoadData<T, U>(string storedProcedure, U parameters, string connectionId = "Default")
  {
    using IDbConnection connection = new MySqlConnection(_config.GetConnectionString(connectionId));

    return await connection.QueryAsync<T>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
  }

  public async Task SaveData<T>(string storedProcedure, T parameters, string connectionId = "Default")
  {
    using IDbConnection connection = new MySqlConnection(_config.GetConnectionString(connectionId));

    await connection.ExecuteAsync(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
  }

  public async Task SaveData(string storedProcedure, DynamicParameters parameters, string connectionId = "Default")
  {
    using IDbConnection connection = new MySqlConnection(_config.GetConnectionString(connectionId));

    await connection.ExecuteAsync(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
  }


}
