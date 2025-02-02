
// Type: com.digitalarcsystems.Traveller.CharacterPDF_Writer




using com.digitalarcsystems.Traveller.DataModel;
using TCG_CS_Engine.com.digitalarcsystems.Traveller;

#nullable disable
namespace com.digitalarcsystems.Traveller
{
  public class CharacterPDF_Writer
  {
    public static void makeCharacterPDF(
      string characterSheetPath,
      string exportPath,
      Character character,
      bool color = false,
      byte[] jpgHeadShot = null)
    {
      PDFUtilities.GenerateCharacterPDF(characterSheetPath, exportPath, character, jpgHeadShot);
    }
  }
}
