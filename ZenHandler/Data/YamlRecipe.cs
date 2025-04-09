using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZenHandler.Data
{
    public class Param
    {
        public string value { get; set; }
        public bool use { get; set; }
    }
    public class PPRecipeSpec
    {
        public string Ppid { get; set; }
        public string Version { get; set; }

        public Dictionary<string, Param> ParamMap { get; set; }
    }

    public class RootRecipe
    {
        public PPRecipeSpec RECIPE { get; set; }
    }

}
