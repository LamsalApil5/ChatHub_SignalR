using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Tables
{
    public class Chats
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; }
        public string GroupChatId { get; set; }
        [Required]
        public string Massage { get; set; }
        [Required]
        public bool IsGroup {  get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        [ForeignKey("UserId")]
        public Users User { get; set; }
        [ForeignKey("GroupChatId")]
        public GroupChats groupChats { get; set; }

    }
}
