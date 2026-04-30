namespace Bfar.XCutting.Foundation.Constants
{
    public static class SplitterConstants
    {
        public static string[] DotSplitter { get; }
        public static string[] DashSplitter { get; }
        public static string[] CommaSplitter { get; }
        public static string[] SemicolonSplitter { get; }
        public static string[] ColonSplitter { get; }
        public static string[] NewlineSplitter { get; }
        public static string[] DoubleSpaceSplitter { get; }
        public static string[] SpaceSplitter { get; }
        public static string[] TabSplitter { get; }
        public static string[] TabAndColonSplitter { get; }
        public static string[] SlashSplitter { get; }
        public static string[] AtSignSplitter { get; }
        public static string[] DateTimeSplitter { get; }
        static SplitterConstants()
        {
            DotSplitter = [StringConstants.Dot];
            DashSplitter = [StringConstants.Dash];
            CommaSplitter = [StringConstants.Comma];
            SemicolonSplitter = [StringConstants.SemiColon];
            NewlineSplitter = [StringConstants.CarriageReturnNewLine];
            DoubleSpaceSplitter = [StringConstants.DoubleSpace];
            SpaceSplitter = [StringConstants.Space];
            TabSplitter = [StringConstants.Tab];
            ColonSplitter = [StringConstants.Colon];
            TabAndColonSplitter = [StringConstants.Colon, StringConstants.Tab];
            SlashSplitter = [StringConstants.Slash];
            AtSignSplitter = [StringConstants.AtSign];
            DateTimeSplitter = [StringConstants.Dash, StringConstants.Slash, StringConstants.Colon, StringConstants.T, StringConstants.Space];

        }

    }
}
