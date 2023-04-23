

namespace BookStore.Application.Account.ManageUser
{
    public class GetUserDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        
        public GetRoleDto Role { get; set; } 
        
      
    }
}