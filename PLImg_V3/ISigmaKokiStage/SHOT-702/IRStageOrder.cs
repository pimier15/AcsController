using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISigmaKokiStage
{
    public interface IRStageOrder
    {
        string MovePosSetCommand { get; set; }
        string MoveSettedPosCommand { get; set; }
        string OriginCommand { get; set; }
        string StatusCommand { get; set; }
        string ForceStopCommand { get; set; }

        void MoveAbsPos( int pos );
        void Wait2Arrive( int targetPos );
        void Wait2ArriveEpsilon( int targetPos, double epsilon );
        void Origin( );
        void SetRSpeed( int speed, int acc );
        string Status( );
        void ForceStop( );
    }
}
