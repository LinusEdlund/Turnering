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

  public async Task<IEnumerable<T>> LoadData<T, U, A, B>(string storedProcedure, Func<T, U, A, B, T> parameters, string split, string connectionId = "Default")
  {
    using IDbConnection connection = new MySqlConnection(_config.GetConnectionString(connectionId));

    return await connection.QueryAsync(storedProcedure, parameters, splitOn: split, commandType: CommandType.StoredProcedure);
  }

  public async Task<IEnumerable<T>> LoadDataParam<T, U, A, B>(string storedProcedure, Func<T, U, A, B, T> parameters, string split, object paramValues = null, string connectionId = "Default")
  {
    using IDbConnection connection = new MySqlConnection(_config.GetConnectionString(connectionId));

    return await connection.QueryAsync(storedProcedure, parameters, param: paramValues, splitOn: split, commandType: CommandType.StoredProcedure);
  }



}
