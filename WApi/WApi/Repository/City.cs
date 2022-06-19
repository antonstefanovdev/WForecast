using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WApi.Repository
{
    [Table(name: "City")]
    public class City
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public City(
            int id,
            string name
            )
        {
            Id = id;
            Name = name;
        }
    }
}
