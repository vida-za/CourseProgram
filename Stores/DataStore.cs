using CourseProgram.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace CourseProgram.Stores
{
    public class DataStore
    {
        private readonly Dictionary<Type, object> _arrays = new Dictionary<Type, object>();
        private readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();

        public DataStore()
        {
            _arrays[typeof(Address)] = new List<Address>();
            _arrays[typeof(Bud)] = new List<Bud>();
            _arrays[typeof(Cargo)] = new List<Cargo>();
            _arrays[typeof(Client)] = new List<Client>();
            _arrays[typeof(DriverCategories)] = new List<DriverCategories>();
            _arrays[typeof(Driver)] = new List<Driver>();
            _arrays[typeof(Haul)] = new List<Haul>();
            _arrays[typeof(Machine)] = new List<Machine>();
            _arrays[typeof(Nomenclature)] = new List<Nomenclature>();
            _arrays[typeof(Order)] = new List<Order>();
            _arrays[typeof(Route)] = new List<Route>();
            _arrays[typeof(Worker)] = new List<Worker>();
        }

        private IList<T>? GetList<T>()
        {
            return _arrays.TryGetValue(typeof(T), out var array) ? (IList<T>)array : null;
        }

        public IList<T> GetArrayData<T>() where T : IModel
        {
            _lock.EnterReadLock();
            try
            {
                var array = GetList<T>();
                return (IList<T>)array ?? throw new KeyNotFoundException($"Array data for {typeof(T)} not found!");
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        public IEnumerable<object> GetAllArrays()
        {
            _lock.EnterReadLock();
            try
            {
                return _arrays.Values.ToArray();
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        public void AddData<T>(T data) where T : IModel, IEquatable<T>
        {
            _lock.EnterWriteLock();
            try
            {
                var list = GetList<T>();
                if (list == null)
                    throw new KeyNotFoundException($"Array data for {typeof(T)} not found!");

                if (!list.Contains(data))
                    list.Add(data);
                else
                {
                    int index = list.IndexOf(data);
                    list[index] = data;
                }
            }
            finally 
            { 
                _lock.ExitWriteLock(); 
            }
        }

        public void ReplaceData<T>(T data) where T : IModel, IEquatable<T>
        {
            _lock.EnterWriteLock();
            try
            {
                var list = GetList<T>();
                if (list == null)
                    throw new KeyNotFoundException($"Array data for {typeof(T)} not found!");


                int index = list.IndexOf(data);
                if (index >= 0)
                    list[index] = data;
                else
                    throw new InvalidOperationException($"Item of type {typeof(T)} not found in DataStore!");
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        public void RemoveData<T>(T data) where T : IModel, IEquatable<T>
        {
            _lock.EnterWriteLock();
            try
            {
                var list = GetList<T>();
                if (list == null)
                    throw new KeyNotFoundException($"Array data for {typeof(T)} not found!");

                if (!list.Remove(data))
                    throw new InvalidOperationException($"Item of type {typeof(T)} not found in DataStore!");
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        public T? FindData<T>(T data) where T : IModel, IEquatable<T>
        {
            _lock.EnterReadLock();
            try
            {
                var list = GetList<T>();
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (data.Equals(item))
                            return item;
                    }
                }

                return default;
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }
    }
}