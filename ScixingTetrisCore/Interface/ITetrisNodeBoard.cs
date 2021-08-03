using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScixingTetrisCore.Interface
{
    // AIboard需要尽可能的简化
    // 规则，方块序列由搜索树提供
    // board节点只负责提供场地信息，树节点会存储当前的节点信息
    // 搜索算法会根据board信息给出结果
    // 可能要优化软降到底这个过程
    // hold怎么办？board似乎需要一个hold的信息（？还是说是节点存 可能在节点会好一点


    /// <summary>
    /// AIBoard接口
    /// </summary>
    public interface ITetrisNodeBoard
    {
    }
}
