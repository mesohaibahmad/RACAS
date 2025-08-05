using Microsoft.EntityFrameworkCore;
using RACAS.Model;
using RACAS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RACAS.Services
{
    public interface IBranchServices
    {
        Task<List<Branches>> GetBranches();
        // Task<Category> GetCategories();
        Task<List<Branches>> UnAssignBranches(List<long> CategoryIds);
      //  Task<List<UserCategoryModel>> GetBranches(int UserId);
      // Task<Branch> Insert(Branch param);
      //  Task<bool> delete(int Id);
    }
    public class BranchServices : IBranchServices
    {
        private readonly RACASContext dBContext;

        public BranchServices(RACASContext _dbContext)
        {
            dBContext = _dbContext;
        }

        public async Task<List<Branches>> GetBranches()
        {
            var list = dBContext.Branches.ToList();
            return list;
        }
        public async Task<List<Branches>> UnAssignBranches(List<long> CategoryIds)
        {
            var list = dBContext.Branches.Where(x => !CategoryIds.Contains(x.Id)).ToList();
            return list;
        }
        //public async Task<List<UserCategoryModel>> GetBranches(int UserId)
        //{
        //    var obj = dBContext.Branches.Where(x => x.Id == UserId).ToList();
        //    List<long> catids = null;
        //    List<UserCategoryModel> list = null;
        //    if (obj != null)
        //    {
        //        catids = obj.Select(x => x.Id).ToList();
        //        var listObj = dBContext.Branches.Where(x => catids.Contains(x.Id)).ToList();
        //        if (listObj != null)
        //        {
        //            list = new List<UserCategoryModel>();
        //            foreach (var cat in listObj)
        //            {

                       
        //            }
        //        }
        //    }
        //    return list;
        //}

        //public async Task<List<UserCategory>> GetUserCategories(int UserId)
        //{
        //    var lists = dBContext.UserCategories.ToList();
        //    return lists;
        //}
        //public async Task<Category> Insert(Branch param)
        //{
        //    try
        //    {
        //        if (param.CategoryId > 0)
        //        {
        //            var obj = await dBContext.Categories.FindAsync(param.CategoryId);
        //            if (obj != null)
        //            {
        //                obj.ModifiedBy = 1;
        //                obj.ModifiedDate = DateTime.UtcNow;
        //                dBContext.Categories.Update(obj);
        //                await dBContext.SaveChangesAsync();
        //            }

        //            return param;
        //        }
        //        else
        //        {
        //            var objcat = dBContext.Categories.OrderByDescending(x => x.CategoryId).FirstOrDefault();
        //            int CategoryId = 1;
        //            if (objcat != null)
        //                CategoryId = objcat.CategoryId + 1;

        //            param.CategoryId = CategoryId;
        //            param.CreatedBy = 1;
        //            param.CreatedDate = DateTime.UtcNow;
        //            param.RecordStatus = "Active";
        //            dBContext.Categories.Add(param);
        //            await dBContext.SaveChangesAsync();

        //            return param;
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        return null;
        //    }

        //}
        //public async Task<bool> delete(int Id)
        //{
        //    try
        //    {
        //        var obj = dBContext.Categories.FirstOrDefault(x => x.CategoryId == Id);
        //        if (obj != null)
        //        {
        //            obj.RecordStatus = "Deleted";
        //            dBContext.Categories.Update(obj);
        //            await dBContext.SaveChangesAsync();
        //            return true;
        //        }
        //        return false;
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }

        //}
    }
}
