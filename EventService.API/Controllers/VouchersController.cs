﻿using EventService.Domain.Model;
using EventService.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventService.API.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class VouchersController : ControllerBase {
        private readonly EventContext _context;

        public VouchersController(EventContext context) {
            _context = context;
        }

        // GET: api/Vouchers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Voucher>>> GetVouchers() {
            return await _context.Vouchers.ToListAsync();
        }

        // GET: api/Vouchers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Voucher>> GetVoucher(long id) {
            var voucher = await _context.Vouchers.FindAsync(id);

            if (voucher == null) {
                return NotFound();
            }

            return voucher;
        }

        // PUT: api/Vouchers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVoucher(long id, Voucher voucher) {
            if (id != voucher.Id) {
                return BadRequest();
            }

            _context.Entry(voucher).State = EntityState.Modified;

            try {
                await _context.SaveChangesAsync();
            } catch (DbUpdateConcurrencyException) {
                if (!VoucherExists(id)) {
                    return NotFound();
                } else {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Vouchers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Voucher>> PostVoucher(Voucher voucher) {
            _context.Vouchers.Add(voucher);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVoucher", new { id = voucher.Id }, voucher);
        }

        // DELETE: api/Vouchers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVoucher(long id) {
            var voucher = await _context.Vouchers.FindAsync(id);
            if (voucher == null) {
                return NotFound();
            }

            _context.Vouchers.Remove(voucher);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool VoucherExists(long id) {
            return _context.Vouchers.Any(e => e.Id == id);
        }
    }
}
