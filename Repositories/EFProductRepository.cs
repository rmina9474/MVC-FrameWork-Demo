﻿using Microsoft.EntityFrameworkCore;
using Reina.MacCredy.Models;

namespace Reina.MacCredy.Repositories
{
    public class EFProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;
        public EFProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            try
            {
                return await _context.Products
                    .Include(p => p.Category) // Include thông tin về category
                    .Include(p => p.Reviews) // Include reviews for displaying ratings
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                // Log the error (in a real app you'd use a proper logger)
                Console.WriteLine($"Database error in GetAllAsync: {ex.Message}");

                // Return an empty list rather than throwing an exception
                return new List<Product>();
            }
        }
        public async Task<Product> GetByIdAsync(int id)
        {
            // return await _context.Products.FindAsync(id);
            // lấy thông tin kèm theo category
            var product = await _context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
                throw new KeyNotFoundException($"Product with ID {id} not found");
            return product;
        }
        public async Task AddAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }
    }
}
