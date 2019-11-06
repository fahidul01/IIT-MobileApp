namespace CoreEngine.Model.DBModel
{
    public class DBFile
    {
        public int Id { get; set; }
        /// <summary>
        /// Can be duplicate
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// Should be unique
        /// </summary>
        public string FilePath { get; set; }
        public string FileHash { get; set; }
    }
}
