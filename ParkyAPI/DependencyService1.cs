using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyAPI
{
    public class DependencyService1
    {
        public DependencyService1(
            IOperationTransient transientOperation,
            IOperationScoped scopedOperation,
            IOperationSingleton singletonOperation,
            IOperationSingletonInstance instanceOperation)
        {
            transient = transientOperation;
            scoped = scopedOperation;
            singleton = singletonOperation;
            singletonInstance = instanceOperation;
        }

        private readonly IOperationTransient transient;
        private readonly IOperationScoped scoped;
        private readonly IOperationSingleton singleton;
        private readonly IOperationSingletonInstance singletonInstance;

        public void Write()
        {
            Console.WriteLine();
            Console.WriteLine("From DependencyService1");
            Console.WriteLine($"Transient - {transient.OperationId}");
            Console.WriteLine($"Scoped - {scoped.OperationId}");
            Console.WriteLine($"Singleton - {singleton.OperationId}");
            Console.WriteLine($"Singleton Instance - {singletonInstance.OperationId}");
        }
    }
}
