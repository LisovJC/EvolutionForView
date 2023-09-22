using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evolution.Model
{
    public class SettingsModel
    {
        public bool RememberMeForAuth { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public bool AutoRun { get; set; }
        public bool AutoLogin { get; set; }
    }
}
