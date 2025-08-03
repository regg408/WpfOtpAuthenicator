namespace Main.Common
{
    /// <summary>
    /// OTP資料結構
    /// </summary>
    internal class OtpAccount
    {
        /// <summary>
        /// 名稱
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 發行來源
        /// </summary>
        public string Issuer { get; set; }
        /// <summary>
        /// 密鑰
        /// </summary>
        public string Secret { get; set; }
    }
}
