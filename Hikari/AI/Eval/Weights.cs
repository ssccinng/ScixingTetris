using System;
#if HIKARI_CLIENT
using MessagePack;
#endif

namespace Hikari.AI.Eval {
#if HIKARI_CLIENT
    [MessagePackObject(keyAsPropertyName: true)]
#endif
    [Serializable]
    public struct Weights {
        public int clear1;
        public int clear2;
        public int clear3;
        public int clear4;
        public int tSpin1;
        public int tSpin2;
        public int tSpin3;
        public int tMini1;
        public int tMini2;

        public int perfect;

        public int tHole;
        public int tstHole;
        public int finHole;
        public int donate;

        public int b2bContinue;
        public int b2bDestroy;
        public int ren;

        public int wastedT;
        public int holdT;

        public int bumpSum;
        public int bumpSumSq;
        public int maxHeightDiff;
        public int wellDepth;
        public int holes;
        public int holesSq;

        public int holeEdgeDiff0;
        public int holeEdgeDiff1;
        public int holeEdgeDiff2;
        public int holeEdgeDiffMany;

        public int maxHeight;
        public int top50;
        public int top75;
        public int danger;
        public int placementHeight;
        public int moveTime;

        public int coveredCells;
        public int coveredCellsSq;
        public int rowTransitions;
        public int colTransitions;
        public int10 wellX;

        public bool noTspin;

        public static Weights Default => new Weights {
            clear1 = -810,
            clear2 = -755,
            clear3 = -570,
            clear4 = 500,
            tSpin1 = 130,
            tSpin2 = 540,
            tSpin3 = 920,
            tMini1 = -680,
            tMini2 = -500,
            perfect = 2800,
            tHole = 200,
            tstHole = 200,
            finHole = 150,
            donate = 50,
            b2bContinue = 540,
            b2bDestroy = -35,
            ren = 120,
            wastedT = -250,
            holdT = 10,
            bumpSum = 20,
            bumpSumSq = -20,
            maxHeightDiff = 0,
            wellDepth = 50,
            holes = -150,
            holesSq = -1,
            holeEdgeDiff0 = 60,
            holeEdgeDiff1 = -150,
            holeEdgeDiff2 = 30,
            holeEdgeDiffMany = -40,
            maxHeight = -70,
            top50 = -50,
            top75 = -370,
            danger = -5,
            placementHeight = 0,
            moveTime = -4,
            coveredCells = -100,
            coveredCellsSq = -2,
            rowTransitions = -10,
            colTransitions = -10,
            wellX = new int10(-10, -20, 60, 2, 6, 6, 3, 46, -21, -10)
        };
    }
}