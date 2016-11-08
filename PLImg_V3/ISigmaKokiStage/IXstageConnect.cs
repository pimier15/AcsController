using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISigmaKokiStage
{
    public interface IXstageConnect
    {
        bool XStageConnect(string port);
        bool XstageRelease();
    }
}
