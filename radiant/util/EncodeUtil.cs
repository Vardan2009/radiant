// Radiant Encoding Utility

public class EncodeUtil
{
    public static string ComputeHash(string input)
    {
        // Very simple hash function example (not for production use)
        int hash = 0;
        foreach (char c in input)
        {
            hash = (hash * 31) + c; // Basic hash function
        }
        return hash.ToString();
    }
}
