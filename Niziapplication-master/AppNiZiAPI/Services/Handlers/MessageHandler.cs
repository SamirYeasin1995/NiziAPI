using System;

namespace AppNiZiAPI.Models.Handlers
{
    public interface IMessageHandler
    {
        string BuildErrorMessage(Exception ex);
    }

    public class MessageHandler : IMessageHandler
    {
        /// <summary>
        /// Builds a feedback message for an error caught.
        /// </summary>
        public string BuildErrorMessage(Exception ex)
        {
      
            string callbackMessage = "";
            if (ex.InnerException != null)
                callbackMessage = ex.InnerException.Message;
            else if (ex.Message != null)
                callbackMessage = ex.Message;

           
            callbackMessage = callbackMessage.Split('.')[0];
            callbackMessage += ". ";

           
            if (callbackMessage.ToLower().Contains("datetime"))
                callbackMessage += "Please use format YYYY-MM-DD.";


            if (callbackMessage.ToLower().Contains("stacktrace"))
                callbackMessage = "An error occurred.";

            return callbackMessage;
        }
    }
}

