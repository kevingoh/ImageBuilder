using System;
using System.Text; 

namespace DeployUsingARMTemplate
{
public class GetUniqueName {

// Generates a random string with a given size.    
    private readonly Random _random = new Random();  
    public string RandomString(int size, bool lowerCase = true)  
    {  
      var builder = new StringBuilder(size);  

      // char is a single Unicode character  
      char offset = lowerCase ? 'a' : 'A';  
      const int lettersOffset = 26; // A...Z or a..z: length = 26  
  
      for (var i = 0; i < size; i++)  
      {  
        var @char = (char)_random.Next(offset, offset + lettersOffset);  
        builder.Append(@char);  
      }  
  
      return lowerCase ? builder.ToString().ToLower() : builder.ToString();  
    }  

}
}