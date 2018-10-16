using PlayingWithMarketo.Core.Models;

namespace PlayingWithMarketo.Core.Repositories
{
    public interface ITokenRepository
    {
        Token GetToken();
        void AddToken(Token token);
    }
}
