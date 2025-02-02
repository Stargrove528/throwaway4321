
// Type: com.digitalarcsystems.Traveller.UploadProgressEventArgs




using System;

#nullable disable
namespace com.digitalarcsystems.Traveller
{
  public class UploadProgressEventArgs : EventArgs
  {
    public long BytesUploaded { get; set; }

    public long TotalBytes { get; set; }

    public string LastUploadedName { get; set; }

    public string Error { get; set; }

    public UploadProgressEventArgs(
      long bytesUploaded,
      long totalBytes,
      string lastUploadedName,
      string error)
    {
      this.BytesUploaded = bytesUploaded;
      this.TotalBytes = totalBytes;
      this.LastUploadedName = lastUploadedName;
      this.Error = error;
    }
  }
}
