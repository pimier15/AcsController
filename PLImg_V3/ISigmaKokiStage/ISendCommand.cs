using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISigmaKokiStage
{
    public interface ISendCommand
    {
        void Write(string command);
        string Query(string command);
        string Read();
    }
}
