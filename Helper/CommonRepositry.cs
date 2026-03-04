using Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Helper
{
    public class CommonRepositry
    {
        private readonly HrmdbContext _HRMDBContext;
        public CommonRepositry(HrmdbContext hRMDBContext)
        {
            _HRMDBContext = hRMDBContext;
        }

        public IQueryable<PermissionMst> PermissionList()
        {
            return _HRMDBContext.PermissionMsts.AsQueryable();
        }
        public IQueryable<RolePermissionMst> RolePermissionList()
        {
            return _HRMDBContext.RolePermissionMsts.AsQueryable();
        }
        public IQueryable<TokenMst> TokenList()
        {
            return _HRMDBContext.TokenMsts.AsQueryable();
        }
        public IQueryable<UserMst> UserList()
        {
            return _HRMDBContext.UserMsts.AsQueryable();
        }
        public IQueryable<RoleMst> RoleList()
        {
            return _HRMDBContext.RoleMsts.AsQueryable();
        }
        public IQueryable<DepartmentMst> DepartmentList()
        {
            return _HRMDBContext.DepartmentMsts.AsQueryable();
        }
        public IQueryable<DesignationMst> DesignationList()
        {
            return _HRMDBContext.DesignationMsts.AsQueryable();
        }

        public IQueryable<TicketMst>TicketList()
        {
            return _HRMDBContext.TicketMsts.AsQueryable();
        }
    }
}
