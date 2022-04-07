namespace ACRCloudSdkCore
{
    public static unsafe partial class ACRCloudExtractTools
    {
        /// <summary>
        /// Set whether the debug output is printed.
        /// </summary>
        /// <param name="isDebug"><see langword="true"/> to display debug output</param>
        public static void SetDebug(bool isDebug)
        {
            NativeMethods.SetDebug(isDebug);
        }
    }
}
