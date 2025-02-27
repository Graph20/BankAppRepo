﻿using BankApp.Repository;
using BankApp.Shared;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BankApp.Server.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ITransactionRepository _transactionRepository;
        public CustomerController(ICustomerRepository customerRepository, ITransactionRepository transactionRepository)
        {
            _customerRepository = customerRepository;
            _transactionRepository = transactionRepository;
        }

        [HttpPut("editcustomeronsignup")]
        public IActionResult EditCustomerOnSignup([FromBody] Customer customer)
        {

            var sparadCustomer = _customerRepository.UpdateNameOnSignUp(customer, User.GetIdentityId());

            return Ok(sparadCustomer);
        }

        [HttpGet("getunregistered")]
        public IActionResult GetUnregistered()
        {
            var customers = _customerRepository.GetUnregistered();
            return Ok(customers);
        }

        [HttpGet("getcustomer")]
        public IActionResult EditCustomerOnSignup()
        {

            var x = User.GetIdentityId();

            var dbCustomer = _customerRepository.GetById(User.GetIdentityId());

            return Ok(dbCustomer);
        }
        [HttpGet("getcustomer/{CustomerId:int}")]
        public IActionResult GetCustomer(int CustomerId)
        {
            var customer = _customerRepository.GetById(CustomerId, false);
            return Ok(customer);
        }

        [HttpPut("approveascustomer")]
        public IActionResult ApproveCustomer([FromBody] Customer customer)
        {
            var dbCustomer = _customerRepository.GetById(customer.CustomerId);
            dbCustomer.ApproveAsCustomer();
            _customerRepository.Save(customer);
            return Ok();
        }
        [HttpPut("approveasadmin")]
        public IActionResult ApproveAdmin([FromBody] Customer customer)
        {

            var dbCustomer = _customerRepository.GetById(customer.CustomerId);
            dbCustomer.ApproveAsAdmin();
            _customerRepository.Save(customer);
            return Ok();
        }


        [HttpGet("getcustomers")]
        public IActionResult GetCustomers()
        {
            var customers = _customerRepository.GetCustomers();
            return Ok(customers);
        }

        [HttpPut("addaccount/{CustomerId:int}/{AccountTypeId:int}")]
        public IActionResult AddAccount(int CustomerId, int AccountTypeId)
        {
            var customer = _customerRepository.GetById(CustomerId);
            customer.CreateNewAccount(AccountTypeId);
            _customerRepository.Save(customer);

            return Ok();
        }
       
        [HttpPut("depositwithdraw/{CustomerId:int}")]
        public IActionResult DepositAsCustomer(int CustomerId, [FromBody] TransactionDto tranDto)
        {
            try
            {
                var tran = new Transaction();

                if (tranDto.CustomerAccountId == null)
                {
                      tran = Transaction.CreateTransaction(tranDto.AccountId, tranDto.Amount, "Insättning av " + User.Identity.Name, "Deposit");
                }
                else
                {
                    var mycustomer = _customerRepository.GetById(CustomerId);
                    var otherCustomer = _customerRepository.GetByAccountId(tranDto.AccountId);

                    tran = Transaction.CreateTransaction(tranDto.AccountId, tranDto.Amount * -1, "Överföring till " + otherCustomer.Givenname + " " + otherCustomer.Surname, "Deposit");
                    _transactionRepository.Save(tran);

                    tran = Transaction.CreateTransaction(tranDto.CustomerAccountId.Value, tranDto.Amount, "Överföring från  " + mycustomer.Givenname + " " + mycustomer.Surname, "Deposit");
                    _transactionRepository.Save(tran);
                }

                if (tranDto.CustomerAccountId == null)
                {
                    var myAccount = _customerRepository.GetAccountById(tranDto.AccountId);
                    myAccount.SetBalance(tranDto.Amount);
                    _customerRepository.SaveAccount(myAccount);
                }
                else 
                {

                    var myAccount = _customerRepository.GetAccountById(tranDto.AccountId);
                    myAccount.SetBalance(tranDto.Amount * -1);
                    _customerRepository.SaveAccount(myAccount);

                    var otherAccount = _customerRepository.GetAccountById(tranDto.CustomerAccountId.Value);
                    otherAccount.SetBalance(tranDto.Amount);
                    _customerRepository.SaveAccount(otherAccount);
                }
                
              

                tranDto.Succeed = true;
                return Ok(tranDto);
            }catch(Exception ex)
            {
                tranDto.Succeed = false;
                tranDto.Error = "Det finns inget kund med det konto nummer som du försöker föra över pengar till";
                return Ok(tranDto);
            }
          

        }
        
    }
    
}
