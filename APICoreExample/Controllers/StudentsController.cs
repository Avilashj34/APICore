using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APICoreExample.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICoreExample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private  AppDbContext AppDbContext { get; set; }
        public StudentsController(AppDbContext AppDbContext)
        {
            this.AppDbContext = AppDbContext;
        }
        #region PostCustomer
        [HttpPost]
        [Route("addcustomer")]
        public async Task<IActionResult> AddCustomer(Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Not a Valid Model");
            }
            using var transcation = AppDbContext.Database.BeginTransaction();
            try
            {
                await AppDbContext.Customers.AddAsync(customer);
                await AppDbContext.SaveChangesAsync();
                transcation.Commit();
            }
            catch (Exception e)
            {
                await transcation.RollbackAsync();
                return BadRequest(new { Message = e.Message, Success = false, TrancationId = transcation.TransactionId });
            }
            return Ok("Added Customer");
        }
        #endregion

        #region GetbyName
        [HttpGet] 
        [Route("get/{fname:aplha}")]  //aplha => used for acceptance of only one word name
        public IActionResult GetByCustomerName(string fname)
        {
            using (var transaction = AppDbContext.Database.BeginTransaction())
            try
            {
                var customer =  AppDbContext.Customers.Where(c => c.FirstName == fname).FirstOrDefault();
                if (customer == null)
                {
                    return NotFound();
                }
                return Ok(customer);
            }
            catch (Exception e)
            {
                transaction.Rollback();
                return BadRequest(new { Message = e.Message, Success = false, TrancationId = transaction.TransactionId });
            }
        }
        #endregion

        #region GetAll()
        [HttpGet]
        [Route("getall}")]  //aplha => used for acceptance of only one word name
        public async Task<IActionResult> GetAllCustomer()
        {
            using var transaction = AppDbContext.Database.BeginTransaction();
            try
            {
                var customer = await AppDbContext.Customers.Select(c=> new 
                {
                    c.FirstName,
                    c.LastName,
                    c.Age
                }).ToListAsync();
                if (customer == null)
                {
                    return BadRequest("No record found");
                }
                return Ok(new { customerData = customer, Transaction = transaction.TransactionId, success = true });
            }
            catch (Exception e)
            {
                transaction.Rollback();
                return BadRequest(new { e.Message, Success = false, TrancationId = transaction.TransactionId });
            }
        }
        #endregion

        #region UpdateCustomer
        [HttpPut]
        [Route("update/customer")]
        public async Task<IActionResult> UpdateCustomerData(Customer customer)
        {
            using var transaction = AppDbContext.Database.BeginTransaction();
            try
            {
                var user = AppDbContext.Customers.FirstOrDefaultAsync(c=> c.Id == customer.Id);
                if(user == null)
                {
                    return BadRequest("Customer Doesn't exists");
                }
                AppDbContext.Entry(user).CurrentValues.SetValues(customer);
                await AppDbContext.SaveChangesAsync();
                return Ok("Updated Customer Data");
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                return BadRequest(new { e.Message, Success = false, TrancationId = transaction.TransactionId });
            }
        }
        #endregion

        #region DeleteCutomer
        [HttpDelete]
        [Route("delete/customer")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            using var transaction = AppDbContext.Database.BeginTransaction();
            try
            {
                var customer = await AppDbContext.Customers.FirstOrDefaultAsync(c => c.Id == id);
                if (customer == null)
                {
                    return BadRequest("User with "+id +" doesn't exist");
                }
                AppDbContext.Customers.Remove(customer);
                return Ok("Deleted Data");
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                return BadRequest(new { e.Message, Success = false, TrancationId = transaction.TransactionId });
            }
        }
        #endregion
    }
}
