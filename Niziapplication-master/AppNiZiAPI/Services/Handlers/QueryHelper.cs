using AppNiZiAPI.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace AppNiZiAPI.Models.Handler
{
    public interface IQueryHelper
    {
        int ExtractIntegerFromRequestQuery(string variableName, HttpRequest req);

        bool IsValidId(Dictionary<ServiceDictionaryKey, object> dictionary, string idText);

        bool IsValidInteger(Dictionary<ServiceDictionaryKey, object> dictionary, string text);
    }

    public class QueryHelper : IQueryHelper
    {
        public int ExtractIntegerFromRequestQuery(string variableName, HttpRequest req)
        {
            int data = 0;

            string dataFromQuery = req.Query[variableName];

            if (dataFromQuery != null)
                if (Int32.TryParse(dataFromQuery, out int integerValue))
                    data = integerValue;

            return data;
        }

        public bool IsValidInteger(Dictionary<ServiceDictionaryKey, object> dictionary, string text)
        {
            bool success = Int32.TryParse(text, out int integerValue);

            if (!success)
                dictionary.Add(ServiceDictionaryKey.ERROR, "Malformed ID passed. Please pass a single number.");

            return success;
        }

        public bool IsValidId(Dictionary<ServiceDictionaryKey, object> dictionary, string idText)
        {
            if (!IsValidInteger(dictionary, idText))
                return false;

            int id = Int32.Parse(idText);

            if (id <= 0)
            {
                dictionary.Add(ServiceDictionaryKey.ERROR, "Id is invalid. Please insert a single positive number.");
                return false;
            }

            return true;
        }
    }
}
