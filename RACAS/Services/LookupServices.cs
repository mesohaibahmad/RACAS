using Microsoft.EntityFrameworkCore;
using RACAS.Constants;
using RACAS.Model;
using RACAS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RACAS.Services
{
    public interface ILookupServices
    {
        Task<LookupModel> GetLookups();
        Task<Countries> InsertCountry(Countries param);
        Task<Branches> InsertBranch(Branches param);
        Task<CausedBy> InsertCausedBy(CausedBy param);
        Task<Descriptions> InsertDescription(Descriptions param);




        Task<string> delete(int Id);
        Task<string> deleteBranch(int Id);
        Task<string> deleteCausedBy(int Id);
        Task<string> deleteDescription(int Id);
        Task<string> deleteUserType(long UserTypeId);



    }
    public class LookupServices : ILookupServices
    {
        private readonly RACASContext dBContext;

        public LookupServices(RACASContext _dbContext)
        {
            dBContext = _dbContext;
        }

        public async Task<LookupModel> GetLookups()
        {
            try
            {
                LookupModel objectModel = new LookupModel();
                objectModel.CountryList = await dBContext.Countries.ToListAsync();
                objectModel.BranchList = await dBContext.Branches.ToListAsync();
                objectModel.CausedByList = await dBContext.CausedBy.ToListAsync();
                objectModel.DescriptionList = await dBContext.Descriptions.ToListAsync();
                objectModel.UserTypeList = await dBContext.UserTypes.ToListAsync();




                return objectModel;
            }
            catch (Exception ex)
            {
                return null;
            }
          
        }

        public async Task<Countries> InsertCountry(Countries param)
        {

            if (param.Id > 0)
            {
                var obj = await dBContext.Countries.FindAsync(param.Id);
                if (obj != null)
                {
                    obj.CountryCode = param.CountryCode.Trim();
                    obj.CountryName = param.CountryName.Trim();
                    dBContext.Countries.Update(obj);
                    await dBContext.SaveChangesAsync();
                }

                return param;
            }
            else
            {
                param.CountryCode = param.CountryCode.Trim();
                param.CountryName = param.CountryName.Trim();
                dBContext.Countries.Add(param);
                await dBContext.SaveChangesAsync();

                return param;
            }

        }

        public async Task<Branches> InsertBranch(Branches param)
        {

            if (param.Id > 0)
            {
                var obj = await dBContext.Branches.FindAsync(param.Id);
                if (obj != null)
                {
                    obj.BranchCode = param.BranchCode.Trim();
                    obj.BranchName = param.BranchName.Trim();
                    obj.Division = param.Division.Trim();
                    obj.CountryId = param.CountryId;
                    dBContext.Branches.Update(obj);
                    await dBContext.SaveChangesAsync();
                }

                return param;
            }
            else
            {
                param.BranchCode = param.BranchCode.Trim();
                param.BranchName = param.BranchName.Trim();
                param.Division = param.Division.Trim();
                param.CountryId = param.CountryId;
                dBContext.Branches.Add(param);
                await dBContext.SaveChangesAsync();

                return param;
            }

        }

        public async Task<CausedBy> InsertCausedBy(CausedBy param)
        {

            if (param.Id > 0)
            {
                var obj = await dBContext.CausedBy.FindAsync(param.Id);
                if (obj != null)
                {
                    obj.Causes = param.Causes.Trim();
                    dBContext.CausedBy.Update(obj);
                    await dBContext.SaveChangesAsync();
                }

                return param;
            }
            else
            {
                param.Causes = param.Causes.Trim();
                dBContext.CausedBy.Add(param);
                await dBContext.SaveChangesAsync();

                return param;
            }

        }

        public async Task<Descriptions> InsertDescription(Descriptions param)
        {

            if (param.Id > 0)
            {
                var obj = await dBContext.Descriptions.FindAsync(param.Id);
                if (obj != null)
                {
                    obj.Description = param.Description.Trim();
                    dBContext.Descriptions.Update(obj);
                    await dBContext.SaveChangesAsync();
                }

                return param;
            }
            else
            {
                param.Description = param.Description.Trim();
                dBContext.Descriptions.Add(param);
                await dBContext.SaveChangesAsync();

                return param;
            }

        }

        

        public async Task<string> delete(int Id)
        {
            try
            {
                var obj = dBContext.Countries.FirstOrDefault(x => x.Id == Id);
                if (obj != null)
                {
                    dBContext.Countries.Remove(obj);
                    await dBContext.SaveChangesAsync();
                    return "success";
                }
                return "success";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        public async Task<string> deleteBranch(int Id)
        {
            try
            {
                var obj = dBContext.Branches.FirstOrDefault(x => x.Id == Id);
                if (obj != null)
                {
                    dBContext.Branches.Remove(obj);
                    await dBContext.SaveChangesAsync();
                    return "success";
                }
                return "success";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        public async Task<string> deleteCausedBy(int Id)
        {
            try
            {
                var obj = dBContext.CausedBy.FirstOrDefault(x => x.Id == Id);
                if (obj != null)
                {
                    dBContext.CausedBy.Remove(obj);
                    await dBContext.SaveChangesAsync();
                    return "success";
                }
                return "success";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        public async Task<string> deleteDescription(int Id)
        {
            try
            {
                var obj = dBContext.Descriptions.FirstOrDefault(x => x.Id == Id);
                if (obj != null)
                {
                    dBContext.Descriptions.Remove(obj);
                    await dBContext.SaveChangesAsync();
                    return "success";
                }
                return "success";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        public async Task<string> deleteUserType(long UserTypeId)
        {
            try
            {
                var list = await dBContext.UserTypes.FindAsync(UserTypeId);
                if (list != null)
                {
                    list.RecordStatus = "Deleted";
                    dBContext.UserTypes.Update(list);
                    await dBContext.SaveChangesAsync();


                    //var moduleList = dBContext.UserModules.Where(x => x.UserTypeId == UserTypeId).ToList();
                    //if (moduleList?.Count > 0)
                    //{
                    //    dBContext.UserModules.RemoveRange(moduleList);
                    //    await dBContext.SaveChangesAsync();
                    //}
                }

                return "true";
            }
            catch (Exception ex)
            {

                return ex.Message;
            }

        }
    }
}
