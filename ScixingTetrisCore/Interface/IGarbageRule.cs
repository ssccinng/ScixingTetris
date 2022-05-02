namespace ScixingTetrisCore
{
    public interface IGarbageRule
    {
        /// <summary>
        /// 一次性上涨的行数，0则不限制
        /// </summary>
        int Cap { get; set; }

    }
}