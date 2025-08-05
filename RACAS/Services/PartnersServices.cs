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
    public interface IPartnersServices
    {
        Task<List<Partners>> GetAllPartners();
        Task<string> InsertPartner(Partners param);
        Task<string> DeletePartner(int Id);
        Task<UserDetails> GetPartnerBranches(long Id);
    }
    public class PartnersServices : IPartnersServices
    {
        private readonly RACASContext dBContext;

        public PartnersServices(RACASContext _dbContext)
        {
            dBContext = _dbContext;
        }

        public async Task<List<Partners>> GetAllPartners()
        {
            var list = await dBContext.Partners.Where(x => x.RecordStatus!="Deleted").ToListAsync();
            return list;
        }

        public async Task<string> InsertPartner(Partners param)
        {
            try
            {
                if (param.Id > 0)
                {
                    var obj = await dBContext.Partners.FindAsync(param.Id);
                    if (obj != null)
                    {
                        obj.Vatid = param.Vatid;
                        obj.IdNumber = param.IdNumber;
                        obj.CompanyName = param.CompanyName;
                        obj.PostCode = param.PostCode;
                        obj.TownCity = param.TownCity;
                        obj.Country = param.Country;
                        obj.FullAddress = param.FullAddress;
                        obj.ContactPerson1 = param.ContactPerson1;
                        obj.ContactEmail1 = param.ContactEmail1;
                        obj.ContactPhone1 = param.ContactPhone1;
                        obj.ContactPerson2 = param.ContactPerson2;
                        obj.ContactEmail2 = param.ContactEmail2;
                        obj.ContactPhone2 = param.ContactPhone2;
                        obj.InsuranceCompany = param.InsuranceCompany;
                        obj.InsurancePolicyNumber = param.InsurancePolicyNumber;
                        obj.ContractDate = param.ContractDate;
                        obj.IsTaxablePerson = param.IsTaxablePerson;
                        obj.ContractSigned = param.ContractSigned;
                        obj.InsuranceContract = param.InsuranceContract;
                        obj.InsurancePolicyValidUntil = param.InsurancePolicyValidUntil;
                        obj.ModifiedDate = DateTime.UtcNow;
                        obj.ModifiedBy = CommonDataParam.LoginId;

                        dBContext.Partners.Update(obj);
                        await dBContext.SaveChangesAsync();
                    }
                }
                else
                {
                    param.CreatedDate = DateTime.UtcNow;
                    param.ModifiedDate = DateTime.UtcNow;
                    param.CreatedBy = CommonDataParam.LoginId;
                    param.ModifiedBy = CommonDataParam.LoginId;
                    param.RecordStatus = "Active";

                    dBContext.Partners.Add(param);
                    await dBContext.SaveChangesAsync();
                }


                return "success";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        public async Task<string> DeletePartner(int Id)
        {
            try
            {
                var obj = dBContext.Partners.FirstOrDefault(x => x.Id == Id);
                if (obj != null)
                {
                    obj.RecordStatus = "Deleted";
                    dBContext.Partners.Update(obj);
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

        public async Task<UserDetails> GetPartnerBranches(long Id)
        {
            UserDetails userDetails = new UserDetails();
            var obj = await dBContext.UserBranches.Where(x => x.UserId == Id && x.RecordType == "Partner").ToListAsync();
            userDetails.BranchList = await dBContext.Branches.ToListAsync();
            userDetails.UserBranchList = new List<UserBranchesModel>();
            if (obj?.Count > 0)
            {
                List<long> Ids = new List<long>();

                foreach (var item in obj)
                {
                    Ids.Add(item.BranchId);
                    var branch = userDetails.BranchList.Where(x => x.Id == item.BranchId).FirstOrDefault();
                    if (branch != null)
                    {
                        userDetails.UserBranchList.Add(new UserBranchesModel()
                        {
                            BranchCode = branch.BranchCode,
                            BranchName = branch.BranchName,
                            BranchId = branch.Id,
                            UserId = item.UserId,
                            Id = item.Id
                        });
                    }

                }
                userDetails.BranchList = userDetails.BranchList.Where(x => !Ids.Contains(x.Id)).ToList();

            }
            return userDetails;
        }
    }
}