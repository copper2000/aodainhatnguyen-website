using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;

namespace Model.DAO
{
    public class UserDAO
    {
        AodaiDbContext db = null;
        public UserDAO()
        {
            db = new AodaiDbContext();
        }
        public long Insert (User entity)
        {
            db.Users.Add(entity);
            db.SaveChanges();
            return entity.ID;
        }

        public bool Update(User entity)
        {
            try
            {
                var user = db.Users.Find(entity.ID);                
                user.Name = entity.Name;                
                user.Address = entity.Address;
                user.Email = entity.Email;
                user.ModifiedBy = entity.ModifiedBy;
                user.ModifiedDate = DateTime.Now;
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
      
        public User GetById(string userName)
        {
            return db.Users.SingleOrDefault(c => c.UserName == userName);
        }
        
        public User ViewDetail(int id)
        {
            return db.Users.Find(id);
        }

        public int Login(string userName, string password)
        {
            var result = db.Users.SingleOrDefault(u => u.UserName == userName);
            if(result == null)
            {
                return 0;
            } else
            {
                if (result.Status == false)
                {
                    return -1;
                }
                else
                {
                    if(result.Password == password)
                    {
                        return 1;
                    } else
                    {
                        return -2;
                    }
                }
            }
        }

        public bool Delete(int id)
        {
            try
            {
                var user = db.Users.Find(id);
                db.Users.Remove(user);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
