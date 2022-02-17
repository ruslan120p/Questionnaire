using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire
{
    internal interface IProfileSaver
    {
        Profile Profile { get; set; }

        string PathToSave { get; set; }

        void Save();
    }
}
