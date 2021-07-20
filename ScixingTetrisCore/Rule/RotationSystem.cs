using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScixingTetrisCore.Interface;
namespace ScixingTetrisCore.Rule
{
    public class RotationSystem: IRotationSystem
    {
        public static readonly RotationSystem SRS = new()
        {
            KickTable = (minoType) => minoType switch
            {
                MinoType.SC_I => new[,]
                {
                    { (0, 0), (-2, 0), (+1, 0), (-2, -1), (+1, +2) },
                    { (0, 0), (-1, 0), (+2, 0), (-1, +2), (+2, -1) },
                    { (0, 0), (+2, 0), (-1, 0), (+2, +1), (-1, -2) },
                    { (0, 0), (+1, 0), (-2, 0), (+1, -2), (-2, +1) },
                },
                MinoType.SC_O => new[,]
                {
                    { (0, 0) },
                    { (0, 0) },
                    { (0, 0) },
                    { (0, 0) },
                },
                >= MinoType.SC_T and <= MinoType.SC_Z => new[,]
                {
                    { (0, 0), (-1, 0), (-1, +1), (0, -2), (-1, -2) },
                    { (0, 0), (+1, 0), (+1, -1), (0, +2), (+1, +2) },
                    { (0, 0), (+1, 0), (+1, +1), (0, -2), (+1, -2) },
                    { (0, 0), (-1, 0), (-1, -1), (0, +2), (-1, +2) },
                },
                _ => null
            },
        };
        public Func<MinoType, (int x, int y)[,]> KickTable;
        public Func<MinoType, (int x, int y)[,]> _180KickTable;

        public (bool isSuccess, int kickCnt) LeftRotation(ITetrisBoard tetrisBoard, ITetrisMinoStatus tetrisMinoStatus)
        {
            throw new NotImplementedException();
        }

        public (bool isSuccess, int kickCnt) RightRotation(ITetrisBoard tetrisBoard, ITetrisMinoStatus tetrisMinoStatus)
        {
            throw new NotImplementedException();
        }

        public (bool isSuccess, int kickCnt) _180Rotation(ITetrisBoard tetrisBoard, ITetrisMinoStatus tetrisMinoStatus)
        {
            throw new NotImplementedException();
        }
    }
}
