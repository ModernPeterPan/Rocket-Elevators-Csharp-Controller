using System;
using System.Collections.Generic;

namespace Commercial_Controller
{
    public class BestElevatorInformations
    {
        public Elevator bestElevator;
        public int bestScore;
        public int referenceGap;
        public BestElevatorInformations(Elevator _bestElevator, int _bestScore, int _referenceGap)
        {
            this.bestElevator = _bestElevator;
            this.bestScore = _bestScore;
            this.referenceGap = _referenceGap;
        }
    }
}