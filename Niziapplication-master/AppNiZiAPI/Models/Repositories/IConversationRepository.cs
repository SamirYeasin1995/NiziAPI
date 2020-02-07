using System;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace AppNiZiAPI.Models.Repositories
{
   interface IConversationRepository
    {
        Task<List<Conversation>> GetConversation(int patient_id);
    }
}
