﻿using ERPProject.DataAccess.Abstract;
using ERPProject.DataAccess.Concrete.EntityFramework.DataManagement;
using ERPProject.Entity.Poco;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPProject.DataAccess.Concrete.EntityFramework
{
    public class EfRoleRepository : EfRepository<Role>,IRoleRepository
    {
        public EfRoleRepository(DbContext dbContext) : base(dbContext)
        {
        }
    }
}
