using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamDiary
{
    public record Dream(string title, string description, DateTime date, string mood);
}
