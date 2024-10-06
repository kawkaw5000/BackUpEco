using AdminSideEcoFridge.Models;
using Microsoft.Data.SqlClient; // Make sure to include this namespace for SqlParameter
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

public class UserSearcRepository
{
    private readonly EcoFridgeDbContext _db;

    public UserSearcRepository()
    {
        _db = new EcoFridgeDbContext();
    }

    public List<VwUsersRoleView> SearchUsers(string keyword)
    {
        var parameter = new SqlParameter("@Keyword", keyword ?? (object)DBNull.Value);

        var users = _db.VwUsersRoleViews
            .FromSqlRaw("EXEC SearchUsers @Keyword", parameter)
            .ToList();

        return users ?? new List<VwUsersRoleView>();
    }

}
