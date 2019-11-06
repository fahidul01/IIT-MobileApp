namespace CoreEngine.Model.DBModel
{
    public class DBFile
    {
        public string Id { get; set; }
        /// <summary>
        /// Can be duplicate
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// Should be unique
        /// </summary>
        public string FilePath { get; set; }
    }
}
