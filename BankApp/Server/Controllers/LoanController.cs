﻿using BankApp.Repository;
using BankApp.Shared;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankApp.Server.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class LoanController : ControllerBase
    {
        private readonly ILoanRepository _loanRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly ICustomerRepository _customerRepository;
        public LoanController(ILoanRepository loanRepository,ICustomerRepository customerRepository,
            ITransactionRepository transactionRepository)
        {
            _loanRepository = loanRepository;
            _transactionRepository = transactionRepository;
            _customerRepository = customerRepository;
        }


        [HttpPost("addloan/{CustomerId:int}")]
        public IActionResult AddLoan(int CustomerId,[FromBody] Loan loan)
        {
            var dbLoan =_loanRepository.Save(loan);

            var tran = Transaction.CreateTransaction(loan.AccountId, loan.Amount, "Insättning banklån " + dbLoan.LoanId.ToString(), "Deposit");

            _transactionRepository.Save(tran);

            var account = _customerRepository.GetAccountById(loan.AccountId);
            account.SetBalance(loan.Amount);

            _customerRepository.SaveAccount(account);
            return Ok(loan);
        }

    }
}
