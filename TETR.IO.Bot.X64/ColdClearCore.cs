using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
namespace TETR.IO.Bot.X64
{
    public struct CCBook { }
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct CCWeights
    {
        public Int32 back_to_back { get; set; }
        public Int32 bumpiness { get; set; }
        public Int32 bumpiness_sq { get; set; }
        public Int32 row_transitions { get; set; }
        public Int32 height { get; set; }
        public Int32 top_half { get; set; }
        public Int32 top_quarter { get; set; }
        public Int32 jeopardy { get; set; }
        public Int32 cavity_cells { get; set; }
        public Int32 cavity_cells_sq { get; set; }
        public Int32 overhang_cells { get; set; }
        public Int32 overhang_cells_sq { get; set; }
        public Int32 covered_cells { get; set; }
        public Int32 covered_cells_sq { get; set; }
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        [JsonInclude]
        public Int32[] tslot;// = new Int32[4] { get; set; }
        //public Int32[] Tslot => tslot;
        public Int32 well_depth { get; set; }
        public Int32 max_well_depth { get; set; }
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        [JsonInclude]
        public Int32[] well_column; // = new Int32[10] { get; set; }
        //public Int32[] Well_column => well_column;
        public Int32 b2b_clear { get; set; }
        public Int32 clear1 { get; set; }
        public Int32 clear2 { get; set; }
        public Int32 clear3 { get; set; }
        public Int32 clear4 { get; set; }
        public Int32 tspin1 { get; set; }
        public Int32 tspin2 { get; set; }
        public Int32 tspin3 { get; set; }
        public Int32 mini_tspin1 { get; set; }
        public Int32 mini_tspin2 { get; set; }
        public Int32 perfect_clear { get; set; }
        public Int32 combo_garbage { get; set; }
        public Int32 move_time { get; set; }
        public Int32 wasted_t { get; set; }
        public byte use_bag { get; set; }
        public byte timed_jeopardy { get; set; }
        public byte stack_pc_damage { get; set; }

        
       
    }
    public enum CCMovementMode
    {
        CC_0G,
        CC_20G,
        CC_HARD_DROP_ONLY
    }

    public enum CCSpawnRule
    {
        CC_ROW_19_OR_20,
        CC_ROW_21_AND_FALL,
    }
    public enum CCMovement
    {
        CC_LEFT, CC_RIGHT,
        CC_CW, CC_CCW,
        /* Soft drop all the way down */
        CC_DROP
    }
    public enum CCTspinStatus
    {
        CC_NONE_TSPIN_STATUS,
        CC_MINI,
        CC_FULL,
    }
    public enum CCPiece
    {
        //CC_I, CC_T, CC_O, CC_S, CC_Z, CC_L, CC_J
        CC_I, CC_O, CC_T, CC_L, CC_J, CC_S, CC_Z
    }

    public enum CCPcPriority
    {
        CC_PC_OFF,
        CC_PC_FASTEST,
        CC_PC_ATTACK
    }

    public enum CCBotPollStatus
    {
        CC_MOVE_PROVIDED,
        CC_WAITING,
        CC_BOT_DEAD
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct CCPlanPlacement
    {
        CCPiece piece;
        CCTspinStatus tspin;

        /* Expected cell coordinates of placement, (0, 0) being the bottom left */
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] expected_x;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] expected_y;

        /* Expected lines that will be cleared after placement, with -1 indicating no line */
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        Int32[] cleared_lines;
    }



    [StructLayout(LayoutKind.Sequential, Pack = 4)]

    public struct CCMove
    {
        /* Whether hold is required */

        //public bool  hold;
        public byte hold;
        //public char hold1;
        /* Expected cell coordinates of placement, (0, 0) being the bottom left */
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] expected_x;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] expected_y;
        ///* Number of moves in the path */

        public byte movement_count;
        //public char movement_count1;
        ///* Movements */
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public CCMovement[] movements;

        ///* Bot Info */

        public uint nodes;

        public uint depth;

        public uint original_rank;

    }
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct CCOptions
    {
        public CCMovementMode mode { get; set; }
        public CCSpawnRule spawn_rule { get; set; }
        public CCPcPriority PcPriority { get; set; }


        public UInt32 min_nodes { get; set; }
        public UInt32 max_nodes { get; set; }
        public UInt32 threads { get; set; }
        public byte use_hold { get; set; }
        //public char use_hold1 { get; set; }
        public byte speculate { get; set; }
        //public char speculate1 { get; set; }
    }

    public struct CCAsyncBot { }
    public static class ColdClearCore
    {

        // [///DllImport("cold_clear.dll", CallingConvention = CallingConvention.Cdecl)]
        [DllImport("cold_clear.dll")]
        public static extern IntPtr cc_launch_async(CCOptions options, CCWeights weights, CCBook book, CCPiece[] queue, UInt32 count);

        [DllImport("cold_clear.dll")]
        public static extern IntPtr cc_launch_with_board_async( CCOptions options, CCWeights weights, byte[] field,
    UInt32 bag_remain, CCPiece hold/*？？？*/, byte b2b, UInt32 combo, CCPiece[] queue, UInt32 count);
        //public static extern IntPtr cc_launch_async(IntPtr options, IntPtr weights);
        [DllImport("cold_clear.dll")]
        public static extern void cc_destroy_async(IntPtr bot);
        [DllImport("cold_clear.dll")]
        public static extern void cc_reset_async(IntPtr bot, byte[] field, byte b2b, UInt32 combo);
        [DllImport("cold_clear.dll")]
        public static extern void cc_add_next_piece_async(IntPtr bot, CCPiece piece);
        [DllImport("cold_clear.dll")]
        public static extern void cc_request_next_move(IntPtr bot, UInt32 incoming);
        //[DllImport("cold_clear.dll")]
        //public static extern int cc_poll_next_move(IntPtr bot, IntPtr move);
        [DllImport("cold_clear.dll")]
        public static extern CCBotPollStatus cc_poll_next_move(IntPtr bot, ref CCMove move, CCPlanPlacement[] plan, IntPtr plan_length);


        [DllImport("cold_clear.dll")]
        public static extern CCBotPollStatus cc_block_next_move(
            IntPtr bot,
            IntPtr move,
            CCPlanPlacement[] plan,
            IntPtr plan_length
        );
        [DllImport("cold_clear.dll")]
        public static extern bool cc_is_dead_async(IntPtr bot);
        //[DllImport("cold_clear.dll")]
        //public static extern void cc_default_options(IntPtr options);

        //[DllImport("cold_clear.dll")]
        //public static extern void cc_default_weights(IntPtr weights);
        
        [DllImport("cold_clear.dll")]
        public static extern void cc_default_options(ref CCOptions options);

        [DllImport("cold_clear.dll")]
        public static extern void cc_default_weights(ref CCWeights weights);

        [DllImport("cold_clear.dll")]
        public static extern void cc_fast_weights(ref CCWeights weights);


    }
}
