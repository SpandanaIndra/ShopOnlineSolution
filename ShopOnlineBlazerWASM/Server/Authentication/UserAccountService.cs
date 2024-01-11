namespace ShopOnlineBlazerWASM.Server.Authentication
{
    public class UserAccountService
    {
        private List<UserAccount> _users;
        public UserAccountService()
        {
            _users = new List<UserAccount>()
            {
                new UserAccount{UserName="Admin",Password="admin",Role="Administrator"},
                new UserAccount{UserName="spandana",Password="spandana",Role="User"}
            };
        }
        public UserAccount? GetUserAccountByUSerName(string username)
        {
            return _users.FirstOrDefault(user => user.UserName == username);
        }
    }
}
