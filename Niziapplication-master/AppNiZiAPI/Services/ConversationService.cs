using AppNiZiAPI.Models;
using AppNiZiAPI.Models.Handler;
using AppNiZiAPI.Models.Handlers;
using AppNiZiAPI.Models.Repositories;
using AppNiZiAPI.Services.Helpers;
using AppNiZiAPI.Services.Serializer;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace AppNiZiAPI.Services
{
    public interface IConversationService
    {
        IMessageHandler FeedbackHandler { get; }

        Task<Dictionary<ServiceDictionaryKey, object>> TryGetConversationById(int id);
    }
    class ConversationService : IConversationService
    {
        public IMessageHandler FeedbackHandler { get; }

        private readonly IConversationRepository _conversationRepository;
        private readonly IMessageSerializer _messageSerializer;
        private readonly IQueryHelper _queryHelper;

        public ConversationService(IConversationRepository conversationRepository,
            IMessageHandler feedbackHandler, IMessageSerializer messageSerializer,
            IQueryHelper queryHelper)
        {
            _conversationRepository = conversationRepository;
            this.FeedbackHandler = feedbackHandler;
            _messageSerializer = messageSerializer;
            _queryHelper = queryHelper;
        }


        async public Task<Dictionary<ServiceDictionaryKey, object>> TryGetConversationById(int id)
        {
            Dictionary<ServiceDictionaryKey, object> dictionary = new Dictionary<ServiceDictionaryKey, object>();

            try
            {
                List<Conversation> conversations = await _conversationRepository.GetConversation(id);

                if (conversations.Count <= 0)
                {
                    dictionary.Add(ServiceDictionaryKey.ERROR, $"No conversations for the given user: {id}.");
                    dictionary.Add(ServiceDictionaryKey.HTTPSTATUSCODE, HttpStatusCode.NotFound);
                    return dictionary;
                }

                dynamic data = _messageSerializer.Serialize(conversations);
                dictionary.Add(ServiceDictionaryKey.VALUE, data);
            }
            catch (Exception ex)
            {
                dictionary.AddErrorMessage(ServiceDictionaryKey.ERROR, ex, FeedbackHandler);
            }

            return dictionary;
        }
    }
}