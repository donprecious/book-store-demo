using MlkPwgen;

namespace SharedKernel.Utility;


public static class RandomNumbers
{
    public static string GenerateTrackingNumber(int length)
    {
        var baseChar = 
                       "0123456789";
        var result =  PasswordGenerator.Generate(length, baseChar);
      
        return result;
    }
}