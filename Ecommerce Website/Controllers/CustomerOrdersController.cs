using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ecommerce_Website.Models;
using System.Data;
using Microsoft.AspNetCore.Authorization;

namespace Ecommerce_Website.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles ="Customer")]
    [ApiController]
    public class CustomerOrdersController : ControllerBase
    {
        private readonly HomeShopDbContext _context;

        public CustomerOrdersController(HomeShopDbContext context)
        {
            _context = context;
        }

        // GET: api/CustomerOrders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerOrder>>> GetCustomerOrders()
        {
          if (_context.CustomerOrders == null)
          {
              return NotFound();
          }
            return await _context.CustomerOrders.ToListAsync();
        }

        // GET: api/CustomerOrders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerOrder>> GetCustomerOrder(int id)
        {
          if (_context.CustomerOrders == null)
          {
              return NotFound();
          }
            var customerOrder = await _context.CustomerOrders.FindAsync(id);

            if (customerOrder == null)
            {
                return NotFound();
            }

            return customerOrder;
        }

        [HttpGet("CustomerId")]
        public async Task<ActionResult<IEnumerable<CustomerOrder>>> GetCustomerOrdersById(int custId)
        {
            if (_context.CustomerOrders == null)
            {
                return NotFound();
            }
            var customerOrders = await _context.CustomerOrders.ToListAsync();

            var selectOrders = (from orders in customerOrders
                                where orders.CustomerId == custId
                                select orders).ToList();

            if (selectOrders == null)
            {
                return NotFound();
            }

            return selectOrders;
        }

        // PUT: api/CustomerOrders/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomerOrder(int id, CustomerOrder customerOrder)
        {
            if (id != customerOrder.CustomerOrderId)
            {
                return BadRequest();
            }

            _context.Entry(customerOrder).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerOrderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/CustomerOrders
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CustomerOrder>> PostCustomerOrder(CustomerOrder customerOrder)
        {
          if (_context.CustomerOrders == null)
          {
              return Problem("Entity set 'HomeShopDbContext.CustomerOrders'  is null.");
          }
            _context.CustomerOrders.Add(customerOrder);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCustomerOrder", new { id = customerOrder.CustomerOrderId }, customerOrder);
        }

        // DELETE: api/CustomerOrders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomerOrder(int id)
        {
            if (_context.CustomerOrders == null)
            {
                return NotFound();
            }
            var customerOrder = await _context.CustomerOrders.FindAsync(id);
            if (customerOrder == null)
            {
                return NotFound();
            }

            _context.CustomerOrders.Remove(customerOrder);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CustomerOrderExists(int id)
        {
            return (_context.CustomerOrders?.Any(e => e.CustomerOrderId == id)).GetValueOrDefault();
        }
    }
}
