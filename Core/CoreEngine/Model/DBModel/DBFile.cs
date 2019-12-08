namespace CoreEngine.Model.DBModel
{
    public class DBFile
    {

        public string Id { get; set; }
        /// <summary>
        /// Can be duplicate
        public string FileName { get; set; }
        /// Should be unique
        /// </summary>
        public string FilePath { get; set; }
        public string FileHash { get; set; }
        public string Description { get; set; }
    }
}
