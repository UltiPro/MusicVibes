namespace MusicVibes.Models
{
    public class MusicFile
    {
        public string FilePath { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public string FileExtension { get; set; } = string.Empty;
        public string FileDuration { get; set; } = string.Empty;

        public MusicFile(string FilePath, string FileName, string FileExtension, string FileDuration)
        {
            this.FilePath = FilePath;
            this.FileName = FileName;
            this.FileExtension = FileExtension;
            this.FileDuration = FileDuration;
        }
    }
}
