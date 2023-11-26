using Dapper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mango.Core.Dapper
{
    /// <summary>
    /// Dapper帮助接口
    /// </summary>
    public interface IDapperHelper
    {
        /// <summary>
        /// Query
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        IEnumerable<T> Query<T>(string sql, object param = null);

        /// <summary>
        /// QueryFirst
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        T QueryFirst<T>(string sql, object param = null);

        /// <summary>
        /// QueryFirstOrDefault
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        T QueryFirstOrDefault<T>(string sql, object param = null);

        /// <summary>
        /// QueryAsync
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="commandFlags"></param>
        /// <returns></returns>
        Task<IEnumerable<T>> QueryAsync<T>(string sql, object param = null, CommandFlags commandFlags = CommandFlags.None, CancellationToken cancellationToken = default);

        /// <summary>
        /// QueryFirstAsync
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="commandFlags"></param>
        /// <returns></returns>
        Task<T> QueryFirstAsync<T>(string sql, object param = null, CommandFlags commandFlags = CommandFlags.None, CancellationToken cancellationToken = default);

        /// <summary>
        /// QueryFirstOrDefaultAsync
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="commandFlags"></param>
        /// <returns></returns>
        Task<T> QueryFirstOrDefaultAsync<T>(string sql, object param = null, CommandFlags commandFlags = CommandFlags.None, CancellationToken cancellationToken = default);
    }
}
