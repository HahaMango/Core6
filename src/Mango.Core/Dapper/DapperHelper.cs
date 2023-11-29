using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mango.Core.Dapper
{
    /// <summary>
    /// Dapper帮助类
    /// </summary>
    public class DapperHelper : IDapperHelper
    {
        private readonly string _connectionString;
        private readonly Type _databaseType;

        /// <summary>
        /// 使用连接字符串初始化
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="databaseType"></param>
        public DapperHelper(string connectionString,Type databaseType)
        {
            if(databaseType == null)
            {
                throw new ArgumentNullException(nameof(databaseType));
            }
            _connectionString = connectionString;
            _databaseType = databaseType;
        }

        /// <summary>
        /// Query
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public IEnumerable<T> Query<T>(string sql, object? param = null)
        {
            using (var cn = InstantiateConnection())
            {
                return cn.Query<T>(sql, param);
            }
        }

        /// <summary>
        /// QueryFirst
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public T QueryFirst<T>(string sql, object? param = null)
        {
            using (var cn = InstantiateConnection())
            {
                return cn.QueryFirst<T>(sql, param);
            }
        }

        /// <summary>
        /// QueryFirstOrDefault
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public T? QueryFirstOrDefault<T>(string sql, object? param = null)
        {
            using (var cn = InstantiateConnection())
            {
                return cn.QueryFirstOrDefault<T>(sql, param);
            }
        }

        /// <summary>
        /// QueryAsync
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="commandFlags"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object? param = null, CommandFlags commandFlags = CommandFlags.None, CancellationToken cancellationToken = default)
        {
            var command = new CommandDefinition(sql, param, null, null, null, commandFlags, cancellationToken);
            using (var cn = InstantiateConnection())
            {
                return await cn.QueryAsync<T>(command);
            }
        }

        /// <summary>
        /// QueryFirstAsync
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="commandFlags"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<T> QueryFirstAsync<T>(string sql, object? param = null, CommandFlags commandFlags = CommandFlags.None, CancellationToken cancellationToken = default)
        {
            var command = new CommandDefinition(sql, param, null, null, null, commandFlags, cancellationToken);
            using (var cn = InstantiateConnection())
            {
                return await cn.QueryFirstAsync<T>(command);
            }
        }

        /// <summary>
        /// QueryFirstOrDefaultAsync
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="commandFlags"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<T?> QueryFirstOrDefaultAsync<T>(string sql, object? param = null, CommandFlags commandFlags = CommandFlags.None, CancellationToken cancellationToken = default)
        {
            var command = new CommandDefinition(sql, param, null, null, null, commandFlags, cancellationToken);
            using (var cn = InstantiateConnection())
            {
                return await cn.QueryFirstOrDefaultAsync<T>(command);
            }
        }

        private IDbConnection InstantiateConnection()
        {
            var i = _databaseType.GetInterfaces();
            if(!i.Any(item => item.Name == typeof(IDbConnection).Name))
            {
                throw new InvalidOperationException($"类型{_databaseType.Name}不实现IDbConnection接口");
            }
            var ci = _databaseType.GetConstructor(new Type[] { typeof(string) });
            if(ci == null)
            {
                throw new NullReferenceException($"类型{_databaseType.Name}不存在一个string的形参构造函数");
            }
            return (IDbConnection)ci.Invoke(new object[] { _connectionString });
        }
    }
}
