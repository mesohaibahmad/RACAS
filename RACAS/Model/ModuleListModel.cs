using RACAS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RACAS.Model
{
    public class ModuleListModel
    {
        public int Id { get; set; }
        public string ModuleName { get; set; }
        public string ArabicModuleName { get; set; }
        public List<ModuleAction> ActionList { get; set; }
    }
}
