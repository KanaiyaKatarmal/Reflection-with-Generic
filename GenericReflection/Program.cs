using System;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;

namespace GenericReflection
{

    public interface IRepository<T>
    {
        void Add(T entity);
        T GetById(int id);

    }

    public class UserRepository : IRepository<User>
    {
        public void Add(User entity) { /* Implementation */ }
        public User GetById(int id) { return new User { Id = 1 }; }

    }

    public class ProductRepository : IRepository<Product>
    {
        public void Add(Product entity) { /* Implementation */ }
        public Product GetById(int id) { return new Product { Id = 1 }; }
    }

    public interface IEntity
    {
        int Id { get; set; }
    }

    public class User : IEntity
    {
        public int Id { get; set; }
    }

    public class Product : IEntity
    {
        public int Id { get; set; }
    }
    class Program
    {
        static void Main()
        {
            Type repositoryType = typeof(IRepository<>);
            Type entityType = typeof(Product);

            Type closedRepositoryType = repositoryType.MakeGenericType(entityType);

            // Find and create an instance of a class implementing the IRepository<T> interface
            var repositoryImplementationType = Assembly.GetExecutingAssembly().GetTypes()
                .FirstOrDefault(type =>
                    !type.IsAbstract && !type.IsInterface &&
                    type.GetInterfaces()
                        .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == repositoryType && i.GetGenericArguments()[0] == entityType));

            if (repositoryImplementationType != null)
            {
                var repositoryInstance = Activator.CreateInstance(repositoryImplementationType);

                Console.WriteLine($"Repository instance created: {repositoryImplementationType.Name}");
            }
            else
            {
                Console.WriteLine("No suitable repository implementation found.");
            }

            moreExample();
        }

        static void moreExample()
        {
            Type genericTypeDefinition = typeof(List<>);
            Type[] typeArgs = { typeof(string) };
            Type closedGenericType = genericTypeDefinition.MakeGenericType(typeArgs);
            object instance = Activator.CreateInstance(closedGenericType);
            List<string> typedInstance = instance as List<string>;

            Console.WriteLine("Instance created: " + (typedInstance != null));
        }
    }
}
