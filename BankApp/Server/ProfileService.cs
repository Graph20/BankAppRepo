﻿using AspBankApp.Repository;
using BankApp.Repository;
using BankApp.Shared;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Services;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

public class ProfileService : IProfileService
{
    private readonly ICustomerRepository _customerRepository;

    public ProfileService(ICustomerRepository ICustomerRepository)
    {
        _customerRepository = ICustomerRepository;
    }

    public async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        var nameClaim = context.Subject.FindAll(JwtClaimTypes.Name);
        context.IssuedClaims.AddRange(nameClaim);

        var emailIdentifier = context.Subject.Claims.Where(x => x.Type == "name").FirstOrDefault();
        var iDIdentifier = context.Subject.Claims.Where(x => x.Type == "sub").FirstOrDefault();

        Customer customerExistInDb = null;
        if (emailIdentifier != null)
        {
            customerExistInDb = _customerRepository.GetByEmailIdentifier(emailIdentifier.Value);

            if(customerExistInDb == null)
            {
                _customerRepository.CreatCustomer(emailIdentifier.Value, iDIdentifier.Value);
                customerExistInDb = _customerRepository.GetByEmailIdentifier(emailIdentifier.Value);
            }
            else
            {
                if (customerExistInDb.IsAdmin)
                {
                    var adminRole = new Claim(ClaimTypes.Role, "admin");

                    context.IssuedClaims.Add(adminRole);
                }
                else if (customerExistInDb.IsCustomer)
                {
                    var adminRole = new Claim(ClaimTypes.Role, "customer");

                    context.IssuedClaims.Add(adminRole);
                }
                else 
                {
                    var pendingRole = new Claim(ClaimTypes.Role, "pending");

                    context.IssuedClaims.Add(pendingRole);
                }
            }
        }

        //var adminRole = new Claim(ClaimTypes.Role, "admin");
      
       // context.IssuedClaims.Add(adminRole);
       


        await Task.CompletedTask;
    }

    public async Task IsActiveAsync(IsActiveContext context)
    {
        await Task.CompletedTask;
    }
}