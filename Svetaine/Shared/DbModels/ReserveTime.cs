using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Svetaine.Shared.DbModels
{
    public class ReserveTime
    {
        [Key]
        public int Id { get; set; }
        public string CostumerPhone { get; set; } = "";
        public string CostumerName { get; set; } = "";
        public string ServiceType { get; set; } = "";
        public float Price { get; set; } = 0f;
        public int Day { get; set; } = 2000_00_00;

        public int TimeIndex { get; set; } = 0;
        public int TimeSize { get; set; } = 0;
        public User User { get; set; }

    }
}
