using ScixingTetrisCore;
using ScixingTetrisCore.Interface;

namespace ScixingTetris.PCFinder;

public enum PCType
{
    Fast,
    Attack
}

public class PCTreeNode
{
    public readonly int Layer;
    /// <summary>
    /// 场地
    /// </summary>
    public TetrisBitBoard Board;
    /// <summary>
    /// 子节点
    /// </summary>
    public List<PCTreeNode> Children = new();
}

public static class PCFinder
{
    public static FinderResult FindPC(TetrisBitBoard tetrisBitBoard, MinoType[] minoList, PCType pcType, bool allowHold)
    {
        FinderResult finderResult = new FinderResult();
        
        
        return finderResult;
    }
    /// <summary>
    /// 求解4*3方格
    /// </summary>
    /// <param name="tetrisBitBoard"></param>
    /// <param name="stage"></param>
    /// <returns></returns>
    public static bool Sovle4x3Area(TetrisBitBoard tetrisBitBoard, int stage)
    {
        return true;
    }

    public static bool IsAblePrefectClean(TetrisBitBoard  tetrisBitBoard)
    {
        int cnt = 0;
        
        return true;
    }
}