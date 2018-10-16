using PlayingWithMarketo.Core.Models;
using PlayingWithMarketo.Core.Repositories;
using System;
using System.Linq;

namespace PlayingWithMarketo.Persistance
{
    public class TokenRepository : ITokenRepository
    {
        private readonly IMarketoDbContext _context;

        public TokenRepository(IMarketoDbContext context)
        {
            _context = context;
        }

        public Token GetToken()
        {
            return
                _context.Tokens.FirstOrDefault(t => t.ExpiresAt > DateTime.UtcNow);
        }

        public void AddToken(Token token)
        {
            _context.Tokens.Add(token);
        }
    }
}
