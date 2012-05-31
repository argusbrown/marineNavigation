namespace Communication
{
    /// <summary>
    /// C0 - ASCII control codes
    /// </summary>
    /// <remarks>
    /// These are the only ones I need so far.
    /// </remarks>
    public enum ControlCode : byte
    {
        /// <summary>
        /// Start of Text
        /// </summary>
        STX = 0x02,
        /// <summary>
        /// End of Test
        /// Used as a "break" character
        /// </summary>
        ETX = 0x03,
        /// <summary>
        /// Data Link Escape
        /// Cause the following octets to be interpreted as raw data
        /// </summary>
        DLE = 0x10
    }
}