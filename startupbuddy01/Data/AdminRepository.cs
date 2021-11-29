using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace startupbuddy01.Data
{
    public class AdminRepository
    {
        private readonly SbDataContext _db;

        public AdminRepository(SbDataContext db)
        {
            _db = db;
        }
        public IQueryable<AdminUserModel> GetAll()
        {
            return _db.Users;            
        }

        public AdminUserModel Add(AdminUserModel param)
        {
            _db.Users.Add(param);
            _db.SaveChanges();
            return param;
        }


        public AdminUserModel Update(AdminUserModel param)
        {
            var d = _db.Users.Find(param.id);
            d.name = param.name;
            d.age = param.age;
            d.email = param.email;
            d.gender = param.gender;
            d.photo = param.photo;
            d.position = param.position;
            _db.SaveChanges();
            return param;
        }

        public bool Delete(AdminUserModel param)
        {
            try
            {
                var d = _db.Users.Find(param.id);
                _db.Users.Remove(d);
                _db.SaveChanges();
                return true;
            }catch(Exception e)
            {
                return false;
            }
        }

        public bool Delete(int id)
        {
            try
            {
                var d = _db.Users.Find(id);
                _db.Users.Remove(d);
                _db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }


        public IQueryable<TY> FilterFunction<TY, Y>(Func<IQueryable<TY>, List<Y>, IQueryable<TY>> filterFunction, IQueryable<TY> dta, List<Y> param)
            where TY : class
            where Y : class
        {
            return filterFunction(dta, param);
        }

        public IQueryable<TY> SortedFunction<TY>(Func<IQueryable<TY>, string, string, IQueryable<TY>> sortFunction, IQueryable<TY> dta, string property, string sortDirection) where TY : class
        {
            return sortFunction(dta, property, sortDirection);
        }
    }
}