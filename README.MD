# ScixingTetris

## 一个以c#编写，通用的方块运行平台

Board接口相关

TetrisBoard 
输出自身场地
判断自身场地某点是不是空的
宽高，可被展示的高度(死亡高度(? )


TetrisNodeBoard: TetrisBoard 搜索树节点场地
极简属性，仅需要场地，

TetrisGameBoard: TetrisBoard
支持上下左右旋转hold等操作 是实际游戏的board
