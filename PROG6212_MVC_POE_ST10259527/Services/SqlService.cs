using Microsoft.EntityFrameworkCore;
using PROG6212_MVC_POE_ST10259527.Models;

namespace PROG6212_MVC_POE_ST10259527.Services
{
    public class SqlService
    {
        private readonly ApplicationDbContext _context;

        public SqlService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Add a user profile
        public async Task AddUserProfileAsync(UserProfileModel profile)
        {
            _context.UserProfiles.Add(profile);
            await _context.SaveChangesAsync();
        }

        // Get a user by lecturerID
        public async Task<UserProfileModel> GetUserByIDAsync(string lecturerID)
        {
            return await _context.UserProfiles.FirstOrDefaultAsync(u => u.RowKey == lecturerID);
        }

        // Get all users
        public async Task<List<UserProfileModel>> GetAllUsersAsync()
        {
            return await _context.UserProfiles.ToListAsync();
        }

        // Add a claim
        public async Task AddClaimAsync(ClaimsModel claim)
        {
            _context.Claims.Add(claim);
            await _context.SaveChangesAsync();
        }

        // Get all claims
        public async Task<List<ClaimsModel>> GetAllClaimsAsync()
        {
            return await _context.Claims.ToListAsync();
        }

        // Update the status of a claim
        public async Task UpdateClaimStatusAsync(ClaimsModel claim)
        {
            _context.Claims.Update(claim);
            await _context.SaveChangesAsync();
        }

        // Get a claim by ID
        public async Task<ClaimsModel> GetClaimByIdAsync(string claimId)
        {
            return await _context.Claims.FirstOrDefaultAsync(c => c.RowKey == claimId);
        }
    }
}
