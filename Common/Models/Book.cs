using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Common.Models
{
    public class Book : MongoDocument
{
    public string Name { get; set; }
    public List<string> Author { get; set; } // Așteaptă o listă
    public int Year { get; set; }
    public string Genre { get; set; }
}

}