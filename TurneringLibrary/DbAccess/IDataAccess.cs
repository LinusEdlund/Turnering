using Dapper;

namespace TurneringLibrary.DbAccess;

public interface IDataAccess
{
  Task<IEnumerable<T>> LoadData<T, U>(string storedProcedure, U parameters, string connectionId = "Default");
  Task<IEnumerable<T>> LoadData<T, U, A, B>(string storedProcedure, Func<T, U, A, B, T> parameters, string split, string connectionId = "Default");
  Task<IEnumerable<T>> LoadDataParam<T, U, A, B>(string storedProcedure, Func<T, U, A, B, T> parameters, string split, object paramValues = null, string connectionId = "Default");
  Task SaveData<T>(string storedProcedure, T parameters, string connectionId = "Default");
}