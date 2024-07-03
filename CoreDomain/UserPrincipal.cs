namespace CoreDomain
{
    public interface IUserPrincipal
    {
        Guid UserId { get; }
    }

    public class UserPrincipal : IUserPrincipal
    {
        public Guid UserId { get; set; }
    }
}
