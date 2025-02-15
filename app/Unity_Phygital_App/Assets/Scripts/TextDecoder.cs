using System.Text.RegularExpressions;

public static class TextDecoder
{
    public static string DecodeText(string inputText)
    {
        // Sostituisci <p> con <br>
        inputText = Regex.Replace(inputText, "<p.*?>", "");

        // Sostituisci </p> con stringa vuota
        inputText = Regex.Replace(inputText, "</p>", "<br><size=40%><br><size=100%>");
        
        inputText = Regex.Replace(inputText, "\r", "");
        inputText = Regex.Replace(inputText, "\n", "");
        inputText = Regex.Replace(inputText, "/>", "");
        inputText = Regex.Replace(inputText, "/>", "");

        //sostituisci <em> con stringa vuota
        inputText = Regex.Replace(inputText, "<em>", "<i>");
        inputText = Regex.Replace(inputText, "</em>", "</i>");

        // Sostituisci <strong> con <b>
        inputText = Regex.Replace(inputText, "<strong.*?>", "<b>");

        // Sostituisci </strong> con </b>
        inputText = Regex.Replace(inputText, "</strong>", "</b>");

        return inputText;
    }
}
