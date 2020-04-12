using System;
using System.Collections.Generic;

namespace PageManager.Gui.Classes {
    class Config {
        public string AccessToken { get; set; }
        public int DataLimit { get; set; }
        public bool PrettyJson { get; set; }
        public bool LoadDataAtStartup { get; set; }
    }
}
