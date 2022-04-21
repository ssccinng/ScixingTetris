using ScixingTetrisCore;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace KingofSwl.Client
{
    public enum LinliuType
    {
        Left,
        Right,
        SoftDrop,
        SonicDrop,
        HardDrop,
        LeftR,
        RightR,
        _180R,
        Hold,
        Reset,
    }
    public class TestControl
    {
        private TetrisGameBoard _tetrisGameBoard;
        public bool[] VirKey = new bool[10];
        public long[] KeyPressTime = new long[10];
        public int[] KeyRunCnt = new int[10];
        public void SetKeyStatus(LinliuType linliuType, bool value)
        {
            if (value == VirKey[(int)linliuType]) return;
            if (!value)
            {

                KeyRunCnt[(int)linliuType] = 0;
            }
            else
            {
                switch (linliuType)
                {
                    case LinliuType.Left:
                        SetKeyStatus(LinliuType.Right, false);
                        break;
                    case LinliuType.Right:
                        SetKeyStatus(LinliuType.Left, false);
                        break;
                    case LinliuType.SoftDrop:
                        break;
                    case LinliuType.SonicDrop:
                        break;
                    case LinliuType.LeftR:
                        break;
                    case LinliuType.RightR:
                        break;
                    case LinliuType._180R:
                        break;
                    case LinliuType.Hold:
                        break;
                    default:
                        break;
                }
                KeyPressTime[(int)linliuType] = stopWatch.ElapsedMilliseconds;
            }
            VirKey[(int)linliuType] = value;

        }



        private bool _left;
        public bool Left
        {
            get => _left; set
            {
                _left = value;
                if (_left == false) leftCnt = 0;
            }
        }
        int rightCnt = 0;

        private bool _right;
        public bool Right
        {
            get => _right; set
            {
                _right = value;
                if (_right == false) rightCnt = 0;
            }
        }
        int leftCnt = 0;
        public event Action NextF;
        private Timer _timer;
        private Task _timer1;
        private Timer _gtimer;
        private Stopwatch stopWatch;
        public TestControl(TetrisGameBoard tetrisGameBoard)
        {
            _tetrisGameBoard = tetrisGameBoard;
            stopWatch = Stopwatch.StartNew();
            //Stopwatch.StartNew();
            //_timer1 = new Thread(test);
            //_timer1.Start();
            //_timer1 = Task.Run(() => {while (true) ; });
            _timer = new Timer(new TimerCallback(_ => test()), null, 0, 17);
            _gtimer = new Timer(new TimerCallback(_ => NextF?.Invoke()), null, 0, 16);
        }
        public int das = 70;
        public int arr = 10;
        public int ss = 10;
        int lf = 0;
        int rf = 0;


        public  void test()
        {
            var nt = stopWatch.ElapsedMilliseconds;
            for (int i = 0; i < 10; i++)
            {
                long needCnt = 10;
                if (VirKey[i])
                {
                    //var needCnt = Math.Max(0, (nt - KeyPressTime[i] - das)) / arr + 1;
                    if (i == (int)LinliuType.SoftDrop)
                    {
                        if (ss != 0)
                        {
                            needCnt = Math.Max(0, (nt - KeyPressTime[i] )) / ss + 1;
                        }
                        else
                        {
                            if (KeyRunCnt[i] > 1)
                            {

                                KeyRunCnt[i] = 0;
                            }
                            
                        }
                        
                    }
                    else
                    {
                        if (arr != 0)
                        {
                            needCnt = Math.Max(0, (nt - KeyPressTime[i] - das)) / arr + 1;
                        }
                        else
                        {
                            
                            if (KeyRunCnt[i] > 1)
                            {
                                
                                KeyRunCnt[i] = 0;
                            }
                            else
                            {
                                needCnt = 1;
                                if (nt - KeyPressTime[i] - das > 0) needCnt = 10;
                            }
                        }
                    }
                    while (KeyRunCnt[i] < needCnt)
                    {
                        switch ((LinliuType)i)
                        {
                            case LinliuType.Left:
                                _tetrisGameBoard.MoveLeft();
                                break;
                            case LinliuType.Right:
                                _tetrisGameBoard.MoveRight();
                                break;
                            case LinliuType.SoftDrop:
                                if (ss == 0)
                                {
                                    _tetrisGameBoard.SonicDrop();

                                }
                                else
                                {
                                    _tetrisGameBoard.SoftDrop();

                                }
                                break;
                            case LinliuType.SonicDrop:
                                _tetrisGameBoard.SonicDrop();
                                break;
                            case LinliuType.LeftR:
                                _tetrisGameBoard.LeftRotation();
                                break;
                            case LinliuType.RightR:
                                _tetrisGameBoard.RightRotation();

                                break;
                            case LinliuType._180R:
                                break;
                            case LinliuType.Hold:
                                break;
                            default:
                                break;
                        }
                        KeyRunCnt[i]++;
                    }
                }
            }

            //        if (Left)
            //        {
            //            //_tetrisGameBoard.MoveLeft();
            //            if (leftCnt == 0 || (leftCnt > das && (leftCnt - das) / arr > lf))
            //            {
            //                _tetrisGameBoard.MoveLeft();
            //                lf = (leftCnt - das) / arr;
            //            }
            //            leftCnt += 17;
            //            //NextF?.Invoke();
            //        }
            //if (Right)
            //{
            //    //_tetrisGameBoard.MoveLeft();
            //    if (rightCnt == 0 || (rightCnt > das && (rightCnt - das) / arr > rf))
            //    {
            //        _tetrisGameBoard.MoveRight();
            //        rf = (leftCnt - das) / arr;
            //    }
            //    rightCnt += 17;
            //    //NextF?.Invoke();
            //}

        }
        private static void NOP(double durationSeconds)
        {
            var durationTicks = Math.Round(durationSeconds * Stopwatch.Frequency);
            var sw = Stopwatch.StartNew();

            while (sw.ElapsedTicks < durationTicks)
            {

            }
        }
    }
}
