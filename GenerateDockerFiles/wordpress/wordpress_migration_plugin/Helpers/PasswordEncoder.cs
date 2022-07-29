using System;
using System.Diagnostics;

namespace DeployUsingARMTemplate
{
public class PassEncoder 
{
public static void PasswordChecker (string password)  
        {  
            try  
            {  
                Console.Write(password);  
                string EnteredVal = "";  
                do  
                {  
                    ConsoleKeyInfo key = Console.ReadKey(true);  
                    // Backspace Should Not Work  
                    if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)  
                    {  
                        EnteredVal += key.KeyChar;  
                        Console.Write("*");  
                    }  
                    else  
                    {  
                        if (key.Key == ConsoleKey.Backspace && EnteredVal.Length > 0)  
                        {  
                            EnteredVal = EnteredVal.Substring(0, (EnteredVal.Length - 1));  
                            Console.Write("\b \b");  
                        }  
                        else if (key.Key == ConsoleKey.Enter)  
                        {  
                            if (string.IsNullOrWhiteSpace(EnteredVal))  
                            {  
                                Console.WriteLine("");  
                                Console.WriteLine("Empty value not allowed.");  
                                PasswordChecker(password);  
                                break;  
                            }  
                            else  
                            {  
                                Console.WriteLine("");  
                                break;  
                            }  
                        }  
                    }  
                } while (true);  
            }  
            catch (Exception ex)  
            {  
                throw ex;  
            }  
        }  
    }  
}