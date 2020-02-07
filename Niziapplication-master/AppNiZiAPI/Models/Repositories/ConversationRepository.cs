using AppNiZiAPI.Models.Repositories;
using AppNiZiAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace AppNiZiAPI
{
    public class ConversationRepository : IConversationRepository
    {
        async Task<List<Conversation>> IConversationRepository.GetConversation(int patient_id)
        {
                SqlConnection conn = new SqlConnection(Environment.GetEnvironmentVariable("sqldb_connection"));
                List<Conversation> conversations = new List<Conversation>();
                using (conn)
                {
                    conn.Open();
                    var text = $"SELECT * FROM Conversation";

                    using (SqlCommand cmd = new SqlCommand(text, conn))
                    {
                        SqlDataReader reader = await cmd.ExecuteReaderAsync();
                        while (reader.Read())
                        {
                            Conversation conversation = new Conversation
                            {
                                Id = (int)reader["id"],
                                Title = (string)reader["title"],
                                Summary = (string)reader["summary"],
                                Date = Convert.ToDateTime(reader["date"]).Date
                            };
                            conversations.Add(conversation);
                        }
                    }
                    conn.Close();
                }
                return conversations;   
        }
    }
}