using Microsoft.Extensions.DependencyModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Mango.Core.Helper
{
    public class AssemblyHelper
    {
        /// <summary>
        /// 加载所有程序集
        /// </summary>
        /// <returns></returns>
        public static List<Assembly> GetAllAssemblies()
        {
            var result = new List<Assembly>();
            var assemblies = DependencyContext.Default.CompileLibraries
                .ToList();
            foreach (var assembly in assemblies)
            {
                result.Add(Assembly.Load(assembly.Name));
            }
            return result;
        }

        /// <summary>
        /// 条件加载程序集
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static List<Assembly> GetAssemblies(Func<CompilationLibrary, bool> predicate)
        {
            var result = new List<Assembly>();
            var assemblies = DependencyContext.Default.CompileLibraries
                .Where(predicate)
                .ToList();
            foreach (var assembly in assemblies)
            {
                result.Add(Assembly.Load(assembly.Name));
            }
            return result;
        }
    }
}
