using System.Text.Json;
using System.Web;
using System.Xml.Serialization;
using Microsoft.Extensions.Logging;

namespace SharedKernel.Utility;

public static class XmlUtil
{
    public static (bool success, T record) DeserializePNRXml<T>(string payload, ILogger _logger) where T : class
    {
        try
        {
            _logger.LogInformation("attempting to deserialize: "+ payload);
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (StringReader reader = new StringReader(payload))
            {
               
                var result = (T)serializer.Deserialize(reader)!;
                _logger.LogInformation("deserializing xml ");
                _logger.LogInformation(JsonSerializer.Serialize(result));
                return (true, result);
            }
        }
        catch (Exception e)
        {

            _logger.LogError(e, "failed to deserialize record using xml ");

            _logger.LogError(JsonSerializer.Serialize(e));

           
        }
        
        return (false, null );
    }

    public static (bool success, T record) DeserializePNRJson<T>(string payload, ILogger _logger) where T : class
    {
        try
        {
            _logger.LogInformation("attempting to deserialize json: " + payload);
          var result =  JsonSerializer.Deserialize<T>(payload);
          _logger.LogInformation("json deserialization complete: " + result);
          return (true, result);
        }
        catch (Exception e)
        {

            _logger.LogError(e, "failed to deserialize record using json ");

            _logger.LogError(JsonSerializer.Serialize(e));
            

        }

        return (false, null);
    }
    
    public static (bool success, T record) DeserializePNRXml<T>(string payload) where T : class
    {
            
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (StringReader reader = new StringReader(payload))
            {

                var result = (T)serializer.Deserialize(reader)!;
             
                return (true, result);
            }
      
    }
    public static string DecodeXML(string xmlString, ILogger _logger)
    {
        _logger.LogInformation("attempting to decode  encoded raw xml string " + xmlString);
        var decodedXml = HttpUtility.HtmlDecode(xmlString);
        _logger.LogInformation("decoded raw xml string complete : " + decodedXml);
        return decodedXml;
    }

    public static string DecodeXML(string xmlString)
    {
       
        var decodedXml = HttpUtility.HtmlDecode(xmlString);
        return decodedXml;
    }
}