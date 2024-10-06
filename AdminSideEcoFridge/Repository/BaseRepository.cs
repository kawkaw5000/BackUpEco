using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdminSideEcoFridge.Contracts;
using Microsoft.EntityFrameworkCore;
using AdminSideEcoFridge.Utils;
using AdminSideEcoFridge.Models;


namespace AdminSideEcoFridge.Repository
{
    public class BaseRepository<T> : IBaseRepository<T>

        where T : class
    {
        public DbContext _db;
        public DbSet<T> _table;

        public BaseRepository()
        {
            _db = new EcoFridgeDbContext();
            _table = _db.Set<T>();
        }

        public ErrorCode Create(T t)
        {
            try
            {
                _table.Add(t);
                _db.SaveChanges();
                return ErrorCode.Success;

            }
            catch (Exception ex)
            {

                return ErrorCode.Error;
            }
        }

        public ErrorCode Delete(object id)
        {
            try
            {
                var user = Get(id);
                _table.Remove(user);
                _db.SaveChanges();
                return ErrorCode.Success;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ErrorCode.Error;
            }
        }


        public ErrorCode Update(object id, T t)
        {
            try
            {
                var oldObj = Get(id);
                _db.Entry(oldObj).CurrentValues.SetValues(t);
                _db.SaveChanges();

                return ErrorCode.Success;
            }
            catch (Exception ex)
            {

                return ErrorCode.Error;
            }
        }

        public T Get(object id)
        {
            return _table.Find(id);
        }

        public List<T> GetAll()
        {
            return _table.ToList();
        }

        
    }
}
