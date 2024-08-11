using Entity.Tables;
using Microsoft.EntityFrameworkCore;

namespace Database
{
    public interface IMyDBContext
    {
        DbSet<Users> Users { get; set; }
        DbSet<Chats> Chats { get; set; }
        DbSet<GroupChats> GroupChats { get; set; }
    }
}