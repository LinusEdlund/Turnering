using Dapper;

namespace TurneringLibrary.DbAccess;

public interface IDataAccess
{
  Task<IEnumerable<T>> LoadData<T, U>(string storedProcedure, U parameters, string connectionId = "Default");
  Task SaveData<T>(string storedProcedure, T parameters, string connectionId = "Default");
  Task SaveData(string storedProcedure, DynamicParameters parameters, string connectionId = "Default");
}