using RACAS.Model;
using RACAS.Models;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RACAS.Constants;
using Microsoft.EntityFrameworkCore;

namespace RACAS.Services
{
    public interface IUsersServices
    {
        Task<List<ModuleListModel>> GetModuleList();
        Task<User> UserLogin(string Email, string Password);
        Task<List<User>> GetList();
        Task<List<UserTypes>> GetUserType();
        Task<string> Insert(UserInputModel model);
        Task<string> EditUser(UserInputModel model);
        Task<User> GetUser(long UserId);
        Task<string> delete(long UserId);
        Task<string> deleteUserType(long UserTypeId);
        Task<List<string>> SelectedItems(long UserId);
        Task<string> InsertUserType(UserTypeModel model);
        Task<List<UserBranchesModel>> GetUserTypeBranches(int UserId, string Type);
        Task<string> AssignBranches(List<UserBranchesModel> model, string type);
        Task<string> UnAssignBranches(List<UserBranchesModel> model, string type);
    }
    public class UsersServices : IUsersServices
    {
        private readonly RACASContext dBContext;
        private readonly IWebHostEnvironment environment;
        public UsersServices(RACASContext _dbContext, IWebHostEnvironment _environment)
        {
            dBContext = _dbContext;
            environment = _environment;
        }
        public async Task<User> UserLogin(string UserName, string Password)
        {
            var obj = await dBContext.Users.FirstOrDefaultAsync(x => x.UserName == UserName && x.Password == Password && x.RecordStatus!="Deleted");

            return obj;
        }
        public async Task<List<ModuleListModel>> GetModuleList()
        {
            List<ModuleListModel> returnList = new List<ModuleListModel>();
            var list = dBContext.Modules.ToList();

            if (list?.Count > 0)
            {
                foreach (var m in list)
                {
                    var listAction = await dBContext.ModuleActions.Where(x => x.ModuleId == m.Id).ToListAsync();

                    returnList.Add(new ModuleListModel()
                    {
                        ModuleName = m.ModuleName,
                        Id = m.Id,
                        ActionList = listAction
                    }); ;
                }
            }

            return returnList;
        }
        public async Task<List<User>> GetList()
        {
            try
            {
                var list = await dBContext.Users.Where(x => x.RecordStatus!="Deleted").ToListAsync();
                return list;
            }
            catch (Exception ex)
            {

                return null;
            }

        }
        public async Task<List<UserBranchesModel>> GetUserTypeBranches(int UserId, string Type)
        {
            try
            {
                List<UserBranchesModel> userBranchesModels = new List<UserBranchesModel>();
                var list = await dBContext.UserBranches.Where(x => x.UserId == UserId && x.RecordType == Type).ToListAsync();
                if (list?.Count > 0)
                {

                    foreach (var item in list)
                    {
                        var branchObject = await dBContext.Branches.Where(x => x.Id == item.BranchId).FirstOrDefaultAsync();
                        userBranchesModels.Add(new UserBranchesModel()
                        {
                            Id = item.Id,
                            BranchCode = branchObject.BranchCode,
                            BranchName = branchObject.BranchName,
                            BranchId = item.BranchId,
                            UserId = item.UserId
                        });
                    }
                }
                return userBranchesModels;
            }
            catch (Exception ex)
            {

                return null;
            }

        }

        public async Task<List<UserTypes>> GetUserType()
        {
            var userTypeList = await dBContext.UserTypes.Where(x => x.RecordStatus != "Deleted").ToListAsync();
            return userTypeList;
        }
        public async Task<string> Insert(UserInputModel model)
        {
            var param = new User()
            {
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.UserName,
                Password = model.Password,
                UserTypeId = model.UserTypeId,
                Id = model.UserId
            };

            try
            {


                if (param.Id > 0)
                {
                    var obj = await dBContext.Users.FindAsync(param.Id);
                    if (obj != null)
                    {
                        obj.Email = param.Email;
                        obj.FirstName = param.FirstName;
                        obj.LastName = param.LastName;
                        obj.UserName = param.UserName;
                        obj.Password = param.Password;
                        obj.UserTypeId = param.UserTypeId;
                        obj.ModifiedBy = CommonDataParam.LoginId;
                        obj.ModifiedDate = DateTime.UtcNow;
                        dBContext.Users.Update(obj);
                        await dBContext.SaveChangesAsync();


                        //var moduleList = dBContext.UserModules.Where(x => x.UserId == param.Id).ToList();
                        //if (moduleList?.Count > 0)
                        //{

                        //    dBContext.UserModules.RemoveRange(moduleList);
                        //    await dBContext.SaveChangesAsync();

                        //}



                    }

                    param = obj;
                }
                else
                {

                    param.CreatedBy = CommonDataParam.LoginId;
                    param.CreatedDate = DateTime.UtcNow;
                    param.ModifiedDate = DateTime.UtcNow;
                    param.ModifiedBy = CommonDataParam.LoginId;
                    param.RecordStatus = "Active";
                    dBContext.Users.Add(param);
                    await dBContext.SaveChangesAsync();

                }


                //if (model.UserRights?.Count > 0)
                //{
                //    foreach (string r in model.UserRights)
                //    {
                //        var arr = r.Split('_');
                //        int ParentId = 0;
                //        int ChildId = 0;
                //        if (arr?.Length > 1)
                //        {
                //            ParentId = Convert.ToInt32(arr[0]);
                //            ChildId = Convert.ToInt32(arr[1]);
                //        }
                //        else
                //        {
                //            ParentId = Convert.ToInt32(arr[0]);
                //        }


                //        var objmodule = dBContext.UserModules.OrderByDescending(x => x.Id).FirstOrDefault();
                //        long Id = 1;
                //        if (objmodule != null)
                //            Id = objmodule.Id + 1;

                //        UserModule obju = new UserModule()
                //        {
                //            Id = Id,
                //            RecordStatus = "Active",
                //            ActionId = ChildId,
                //            CreatedBy = CommonDataParam.LoginId,
                //            CreatedDate = DateTime.UtcNow,
                //            ModuleId = ParentId,
                //            UserId = param.Id,
                //            ModifiedBy = CommonDataParam.LoginId,
                //            ModifiedDate = DateTime.UtcNow
                //        };

                //        dBContext.UserModules.Add(obju);
                //        await dBContext.SaveChangesAsync();

                //    }
                //}


                return "success";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }
        public async Task<string> EditUser(UserInputModel model)
        {
          

            try
            {


               
                    var obj = await dBContext.Users.FindAsync(model.UserId);
                    if (obj != null)
                    {
                    obj.FirstName = model.FirstName;
                    obj.LastName = model.LastName;
                    obj.UserName = model.UserName;
                    obj.Email = model.Email;
                    obj.Password = model.Password;

                    obj.ModifiedBy = CommonDataParam.LoginId;
                        obj.ModifiedDate = DateTime.UtcNow;
                        dBContext.Users.Update(obj);
                        await dBContext.SaveChangesAsync();
                    }

                  

                return "success";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }
        public async Task<string> InsertUserType(UserTypeModel model)
        {
            var param = new UserTypes()
            {
                TypeName = model.TypeName,
                UserTypeId = model.USerTypeId
            };

            try
            {


                if (model != null)
                {

                   
                        var obj  = await dBContext.UserTypes.FirstOrDefaultAsync(x => x.UserTypeId == model.USerTypeId);
                    if (obj != null && obj.RecordStatus != "Deleted")
                        {
                            obj.TypeName = model.TypeName;
                            obj.UserTypeId = model.USerTypeId;
                            obj.ModifiedBy = CommonDataParam.LoginId;
                            obj.ModifiedDate = DateTime.UtcNow;
                            dBContext.UserTypes.Update(obj);
                            await dBContext.SaveChangesAsync();




                            var moduleList = dBContext.UserModules.Where(x => x.UserTypeId == model.USerTypeId).ToList();
                            if (moduleList?.Count > 0)
                            {

                                dBContext.UserModules.RemoveRange(moduleList);
                                await dBContext.SaveChangesAsync();

                            }
                            param = obj;
                        }

                      

                    else
                    {

                        var objuser = dBContext.UserTypes.OrderByDescending(x => x.Id).FirstOrDefault();
                        var maxUserTypeId = await dBContext.UserTypes.MaxAsync(x => (long?)x.UserTypeId) ?? 0; 


                        long Id = 1;
                     
                        if (objuser != null )
                        { Id = objuser.Id + 1; }
                        if (model.USerTypeId == 0 ) 
                        {
                            if (maxUserTypeId >= 7)
                            {
                                param.UserTypeId = objuser.UserTypeId + 1;
                            }
                            else
                            {
                                param.UserTypeId =  8;
                            }
                        }
                        param.Id = Id;
                        param.CreatedBy = CommonDataParam.LoginId;
                        param.CreatedDate = DateTime.UtcNow;
                        param.ModifiedBy = CommonDataParam.LoginId;
                        param.ModifiedDate = DateTime.UtcNow;
                        dBContext.UserTypes.Add(param);
                        await dBContext.SaveChangesAsync();

                    }

                    if (model.UserRights?.Count > 0)
                    {
                        foreach (string r in model.UserRights)
                        {
                            var arr = r.Split('_');
                            int ParentId = 0;
                            int ChildId = 0;
                            if (arr?.Length > 1)
                            {
                                ParentId = Convert.ToInt32(arr[0]);
                                ChildId = Convert.ToInt32(arr[1]);
                            }
                            else
                            {
                                ParentId = Convert.ToInt32(arr[0]);
                            }


                            var objmodule = dBContext.UserModules.OrderByDescending(x => x.Id).FirstOrDefault();
                            long Id = 1;
                            if (objmodule != null)
                                Id = objmodule.Id + 1;

                            UserModule obju = new UserModule()
                            {
                                Id = Id,
                                RecordStatus = "Active",
                                ActionId = ChildId,
                                CreatedBy = CommonDataParam.LoginId,
                                CreatedDate = DateTime.UtcNow,
                                ModuleId = ParentId,
                                UserTypeId = param.UserTypeId,
                                ModifiedBy = CommonDataParam.LoginId,
                                ModifiedDate = DateTime.UtcNow
                            };

                            dBContext.UserModules.Add(obju);
                            await dBContext.SaveChangesAsync();

                        }
                    }
                }

                return "success";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }


        public async Task<string> AssignBranches(List<UserBranchesModel> model, string type)
        {

            try
            {

                if (model?.Count > 0)
                {
                    //var objDel=await dBContext.UserBranches.Where(x => x.UserTypeId == model[0].UserTypeId && x.RecordType==type).ToListAsync();
                    //if (objDel.Count > 0)
                    //{
                    //    dBContext.UserBranches.RemoveRange(objDel);
                    //    dBContext.SaveChanges();
                    //}
                    long MaxId = 0;
                    var maxObj = await dBContext.UserBranches.OrderByDescending(x => x.Id).FirstOrDefaultAsync();
                    if (maxObj != null)
                        MaxId = maxObj.Id;

                    foreach (var item in model)
                    {
                        MaxId = MaxId + 1;

                        UserBranch branches = new UserBranch();
                        branches.Id = MaxId;
                        branches.BranchId = item.BranchId;
                        branches.CreatedBy = CommonDataParam.LoginId;
                        branches.CreatedDate = DateTime.UtcNow;
                        branches.UserId = item.UserId;
                        branches.ModifiedBy = CommonDataParam.LoginId;
                        branches.ModifiedDate = DateTime.UtcNow;
                        branches.RecordType = type;
                        dBContext.UserBranches.Add(branches);
                        await dBContext.SaveChangesAsync();
                    }
                }
                return "success";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }
        public async Task<string> UnAssignBranches(List<UserBranchesModel> model, string type)
        {

            try
            {

                if (model?.Count > 0)
                {
                    var objDel = await dBContext.UserBranches.Where(x => x.UserId == model[0].UserId && x.RecordType == type).ToListAsync();
                    if (objDel.Count > 0)
                    {
                        dBContext.UserBranches.RemoveRange(objDel);
                        dBContext.SaveChanges();
                    }

                }
                return "success";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }
        public async Task<User> GetUser(long UserId)
        {
            try
            {
                var list = await dBContext.Users.FindAsync(UserId);
                return list;
            }
            catch (Exception ex)
            {

                return null;
            }

        }
        public async Task<string> delete(long UserId)
        {
            try
            {
                var list = await dBContext.Users.FindAsync(UserId);
                if (list != null)
                {
                    list.RecordStatus = "Deleted";
                    dBContext.Users.Update(list);
                    await dBContext.SaveChangesAsync();

                }

                return "true";
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

        public async Task<List<string>> SelectedItems(long UserId)
        {
            try
            {
                List<string> result = new List<string>();
                var list = await dBContext.UserModules.Where(x => x.RecordStatus != "Deleted" && x.UserTypeId == UserId).ToListAsync();
                if (list?.Count > 0)
                {
                    foreach (var obj in list)
                    {
                        if (obj.ActionId == 0)
                        {
                            result.Add(obj.ModuleId.ToString());
                        }
                        else
                        {
                            result.Add(obj.ModuleId.ToString() + "_" + obj.ActionId.ToString());
                        }

                    }
                }
                return result;
            }
            catch (Exception ex)
            {

                return null;
            }

        }
    }
}
