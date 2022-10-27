using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models
{
    public class ToDoList
    {
        [Key]
        public int todoID { get; set; }
        public int todoShoppingCartID { get; set; }
        public DateTime todoSaveDate { get; set; }
        public int todoUserID { get; set; }
        public int todoState { get; set; }

    }
}
