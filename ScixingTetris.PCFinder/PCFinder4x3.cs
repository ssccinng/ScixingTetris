using ScixingTetrisCore;

namespace ScixingTetris.PCFinder;

public class PCFinder4x3
{
    private TetrisBitBoard _tetrisBitBoard;
    private MinoType[] _minoList;
    private PCType _pcType;
    private bool _allowHold;

    public PCFinder4x3(TetrisBitBoard tetrisBitBoard, MinoType[] minoList, PCType pcType, bool allowHold)
    {
        _tetrisBitBoard = tetrisBitBoard;
        _minoList = minoList;
        _pcType = pcType;
        _allowHold = allowHold;
    }

    public void Solve()
    {
        
    }

    public int Solve4x3(int stageId, MinoType Hold, int idx)
    {
        if (stageId == 3)
        {
            if (_tetrisBitBoard.Field[0] == 0)
            {
                // AllClear 成功
                return 1;
            }
            // 检测最后一列
            for (int i = 0; i < 4; i++)
            {
                if (!_tetrisBitBoard.IsCellFree(9, i))
                {
                    // 如果hold或者最后一块是i 则成功
                    break;
                }
                else
                {
                    // 否则是失败
                    // return 0;
                }

            }
        }
        
        // 只需要将各个块直接放置在各个位置即可，不需要搜索落点
        
        // 情况1 当前块直接使用
        for (int i = 0; i < 4; i++)
        {
            TetrisMinoStatus tetrisMinoStatus = new TetrisMinoStatus
            {
                TetrisMino = TetrisMino.GetTetrisMino(_minoList[idx]),
                
            };
        }
        // 情况2 使用hold块
        
        // 如果当前块满了 进入一个块
        return 1;
        // 求解4*3 需要回溯
        // return Solve4x3(stageId + 1);
    }
}