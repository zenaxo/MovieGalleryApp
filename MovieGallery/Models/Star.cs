using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieGallery.Models
{
    public class Star
    {
        public int rating { get; set; }
        public int total { get; set; }
    }
}