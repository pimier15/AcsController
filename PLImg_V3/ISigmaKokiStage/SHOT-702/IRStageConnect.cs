using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISigmaKokiStage
{
    public interface IRStageConnect
    {
        bool RStageConnect( string port );
        bool RstageRelease( );
    }
}
